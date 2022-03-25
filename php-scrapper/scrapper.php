<?php
/**
 * PHP script for parsing GitHub repository metadata based on awesome/made-in projects that catalogues
 * various Open source projects per country.
 *
 * Expected output of this script is file output.xml with all repository related metadata.
 *
 * How to:
 *  - Edit bellow lines to add your GitHub token key. Bare in mind that GitHub API will limit your API usage per hour.
 *  - Additional executions will try to reload previous output.xml and continue where it left of.
 *  - Modify urls list to suite your needs.
 *
 * @author Andrej Vitez <contact@andrejvitez.com>
 */
pcntl_async_signals(true);

const USER_AGENT          = 'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.51 Safari/537.36';
const PATTERN_MD_LINE     = '/\[(?<title>(.+?))\]\((?<url>.+?)\)/';
const PATTERN_GITHUB_LINE = '/github\.com\/(?<user>[^\/]+)(\/(?<project>[^\/]+))?/i';
const CURL_MAX_RETRIES    = 3;

class DepEntry implements JsonSerializable
{
    public string $url             = '';
    public string $name            = '';
    public string $title           = '';
    public string $repoName        = '';
    public string $description     = '';
    public string $owner           = '';
    public array  $maintainers     = [];
    public array  $languageTags    = [];
    public int    $watchersCount   = 0;
    public int    $forksCount      = 0;
    public int    $openIssuesCount = 0;
    public bool   $archived        = false;
    public bool   $hasIssues       = false;
    public string $license         = '';
    public string $countryOrigin   = '';
    public string $updatedAt       = '';

    public function jsonSerialize()
    {
        return get_object_vars($this);
    }
}

class Maintainer implements JsonSerializable
{
    public string $name     = '';
    public string $user     = '';
    public string $url      = '';
    public string $location = '';
    public string $email    = '';

    public function jsonSerialize()
    {
        return get_object_vars($this);
    }
}

class NotFoundException extends Exception
{
}

class AccessDeniedException extends Exception
{
}

class Registry
{
    /**
     * @var DepEntry[]
     */
    public array $deps = [];

    public function exists(DepEntry $entry)
    {
        return array_key_exists($entry->repoName, $this->deps);
    }

    public function add(DepEntry $entry)
    {
        $this->deps[$entry->repoName] = $entry;
    }

    public function count()
    {
        return count($this->deps);
    }
}

function logger(string $message)
{
    echo date('Y-m-d H:i:s') . '> ' . $message . PHP_EOL;
}

function extractRepoFromUrl(string $url)
{
    if (!preg_match(PATTERN_GITHUB_LINE, $url, $matches)) {
        return ['', ''];
    }

    return [$matches['user'], $matches['project'] ?? $matches['user']];
}

/**
 * @throws AccessDeniedException
 * @throws NotFoundException
 */
function httpGet(string $url, int &$errorRetries = 0)
{
    logger('Fetching url: ' . $url);
    $ch = curl_init();
    curl_setopt($ch, CURLOPT_URL, $url);
    curl_setopt($ch, CURLOPT_USERPWD, GITHUB_AUTH);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
    curl_setopt($ch, CURLOPT_USERAGENT, USER_AGENT);

    try {
        $responseData = curl_exec($ch);
        $statusCode   = (int) curl_getinfo($ch, CURLINFO_HTTP_CODE);
        $errorMsg     = curl_error($ch);

        if (!$statusCode && $errorMsg && $errorRetries <= CURL_MAX_RETRIES) {
            return httpGet($url, ++$errorRetries);
        }

        if ($statusCode === 302 || $statusCode === 301) {
            $json = json_decode($responseData);
            if (isset($json->url)) {
                logger('Redirecting to ' . $url);

                return httpGet($json->url);
            } else {
                throw new RuntimeException('Failed to determine redirect URL. Exiting.');
            }
        }

        $responseData = handleCurlError($statusCode, $responseData, $url, $errorMsg);
    }
    finally {
        if (is_resource($ch)) {
            curl_close($ch);
        }
    }

    return $responseData;
}

/**
 * @throws NotFoundException
 * @throws AccessDeniedException
 */
function handleCurlError(int $statusCode, $responseData, string $url, string $errorMsg)
{
    if ($statusCode >= 500) {
        throw new RuntimeException(sprintf('GitHub server returned status %d with error "%s"', $statusCode, $errorMsg));
    }

    if (!$responseData || $statusCode >= 400) {
        if ($statusCode === 403 && strpos($responseData, 'API rate limit exceeded') !== false) {
            logger('GitHub API rate limit reached, waiting for an hour to restore quota...');
            sleep(61 * 60);

            return httpGet($url);
        } else if ($statusCode === 404) {
            throw new NotFoundException('Github repository not found');
        } else if (in_array($statusCode, [403, 451]) && strpos($responseData, 'Repository access blocked') !== false) {
            throw new AccessDeniedException('Access to repository is not granted');
        }

        throw new RuntimeException(
            sprintf('Unable to fetch URL %s, status: %s, error: %s', $url, $statusCode, $errorMsg)
        );
    }

    return $responseData;
}

/**
 * @throws NotFoundException
 * @throws AccessDeniedException
 */
function githubApiRequest($path)
{
    $githubData = httpGet('https://api.github.com/' . $path);
    if (false === $githubData) {
        throw new RuntimeException(sprintf('Unable to fetch github url %s', $path));
    }

    $data = json_decode($githubData);
    if (!$data || json_last_error()) {
        throw new RuntimeException(
            sprintf(
                'Unable to parse json for %s with error %s',
                $path,
                json_last_error_msg()
            )
        );
    }

    return $data;
}

/**
 * @throws NotFoundException
 * @throws AccessDeniedException
 */
function addGithubProjectInfo(string $repoName, DepEntry $dep)
{
    $data                 = githubApiRequest('repos/' . $repoName);
    $dep->description     = $data->description ?? '';
    $dep->hasIssues       = (bool) $data->has_issues;
    $dep->forksCount      = (int) $data->forks_count;
    $dep->watchersCount   = (int) $data->watchers_count;
    $dep->openIssuesCount = (int) $data->open_issues;
    $dep->archived        = (bool) $data->archived;
    $dep->license         = empty($data->license->name) ? '' : $data->license->name;
    $dep->updatedAt       = $data->updated_at ?? '';
}

/**
 * @throws NotFoundException
 * @throws AccessDeniedException
 */
function addGithubLanguages(string $repoName, DepEntry $dep)
{
    $data = githubApiRequest('repos/' . $repoName . '/languages');
    foreach (array_keys(get_object_vars($data)) as $lang) {
        if (!in_array($lang, $dep->languageTags)) {
            $dep->languageTags[] = $lang;
        }
    }
}

function trimObjectStringFields($dep)
{
    foreach (get_object_vars($dep) as $var => $value) {
        if (is_string($value)) {
            $dep->{$var} = trim($value);
        }
        if (is_array($value)) {
            foreach ($value as $item) {
                if (!is_object($item)) {
                    continue;
                }
                trimObjectStringFields($item);
            }
        }
    }
}

function getUserFromGithubUrl(string $url)
{
    if (preg_match('/github\.com\/([^\/]+)$/', $url, $matches)) {
        return $matches[1];
    }

    return null;
}

function readMdRepositories(Registry $registry, array $mdUrls)
{
    foreach ($mdUrls as $country => $mdUrl) {
        logger('Loading MD Url ' . $mdUrl);
        $rawData = httpGet($mdUrl);

        $rows = explode(PHP_EOL, $rawData);
        foreach ($rows as $lineIdx => $row) {
            if (!preg_match('/\*\*\[/', $row)) {
                continue;
            }

            $parts = explode(' by ', $row);
            $res1  = preg_match(PATTERN_MD_LINE, $parts[0] ?? '', $repoMatch);
            $res2  = preg_match_all(PATTERN_MD_LINE, $parts[1] ?? '', $maintainersMatch, PREG_SET_ORDER);

            if (!$res1 || !$res2) {
                logger('Unable to parse row at line ' . ($lineIdx + 1));
                continue;
            }

            try {
                processEntry($repoMatch, $maintainersMatch, $country, $registry);
            }
            catch (Throwable $exception) {
                logger(sprintf('Failed to process MD entry \'%s\'', $repoMatch['url']));
                throw $exception;
            }
        }
    }
}

function readJsonRepositories(Registry $registry, array $jsonUrls)
{
    foreach ($jsonUrls as $country => $jsUrl) {
        logger('Loading JSON Url ' . $jsUrl);
        $rawData = httpGet($jsUrl);

        $rows = explode(PHP_EOL, $rawData);
        foreach ($rows as $lineIdx => $row) {
            if (!preg_match('/^\s+\"\[/', $row)) {
                continue;
            }

            $res = preg_match_all(PATTERN_MD_LINE, $row, $matches, PREG_SET_ORDER);

            if (!$res || count($matches) < 2) {
                continue;
            }

            $repoMatch        = end($matches);
            $maintainersMatch = [array_shift($matches)];

            try {
                processEntry($repoMatch, $maintainersMatch, $country, $registry);
            }
            catch (Throwable $exception) {
                logger(sprintf('Failed to process JSON entry \'%s\'', $repoMatch['url']));
                throw $exception;
            }
        }
    }
}

/**
 * @throws NotFoundException
 */
function processEntry($repoMatch, $maintainersMatch, string $country, Registry $registry)
{
    $dep        = new DepEntry();
    $dep->title = trim($repoMatch['title'], "@*\r\n\t");

    [$owner, $project] = extractRepoFromUrl($repoMatch['url']);

    if (!$owner || !$project) {
        logger('Unable to parse owner and project from url ' . $repoMatch['url']);

        return;
    }

    $dep->name          = $project;
    $dep->owner         = $owner;
    $dep->url           = $repoMatch['url'];
    $dep->repoName      = $owner . '/' . $project;
    $dep->countryOrigin = ucfirst($country);

    if ($registry->exists($dep)) {
        logger(sprintf('Repository %s is already collected', $dep->repoName));

        return;
    }

    logger('Processing repo ' . $dep->repoName);

    foreach ($maintainersMatch as $match) {
        $maintainer       = new Maintainer();
        $maintainer->url  = $match['url'];
        $maintainer->name = str_replace(['*', '@'], '', $match['title']);
        $maintainer->user = getUserFromGithubUrl($match['url']) ?? '';
        if ($maintainer->user) {
            try {
                $userData             = githubApiRequest('users/' . $maintainer->user);
                $maintainer->email    = $userData->email ?? '';
                $maintainer->location = $userData->location ?? '';
            }
            catch (NotFoundException $exception) {
                logger(sprintf('Owner user not found : \'%s\'', $maintainer->user));
            }
            catch (AccessDeniedException $exception) {
                logger(sprintf('Not allowed to access user : \'%s\'', $maintainer->user));
            }
        }
        $dep->maintainers[] = $maintainer;
    }

    if (false !== strpos($dep->url, 'github.com')) {
        try {
            addGithubProjectInfo($dep->repoName, $dep);
            addGithubLanguages($dep->repoName, $dep);
        }
        catch (NotFoundException|AccessDeniedException $exception) {
            logger(sprintf($exception->getMessage() . ': skipping \'%s\'', $repoMatch['url']));
        }
    } else {
        logger('Skipping github meta loading because repo is not hosted on github');
    }
    trimObjectStringFields($dep);

    $registry->add($dep);
}

function outputObjectToXml($object, DOMNode $node, DOMDocument $dom)
{
    foreach (get_object_vars($object) as $name => $value) {
        if (!is_array($value)) {
            if (is_bool($value)) {
                $value = $value ? 'true' : 'false';
            }
            $node->appendChild($dom->createElement(ucfirst($name), encodeSpecialChars($value)));
            continue;
        }
        $childrenNode = $dom->createElement(ucfirst($name));
        $node->appendChild($childrenNode);

        if (isset($value[0]) && is_object($value[0])) {
            foreach ($value as $item) {
                $childNode = $dom->createElement(rtrim(ucfirst($name), 's'));
                $childrenNode->appendChild($childNode);
                outputObjectToXml($item, $childNode, $dom);
            }
        } else {
            $childrenNode->nodeValue = encodeSpecialChars(implode('; ', $value));
        }
    }
}

function encodeSpecialChars(string $value): string
{
    return htmlspecialchars($value, ENT_QUOTES | ENT_SUBSTITUTE | ENT_DISALLOWED);
}

function getDepsXml(Registry $registry): DOMDocument
{
    $dom               = new DOMDocument('1.0', 'UTF-8');
    $dom->formatOutput = true;
    $rootEl            = $dom->createElement('Items');

    foreach ($registry->deps as $dep) {
        $depEl = $dom->createElement('Dependency');
        $depEl->setAttribute('name', encodeSpecialChars($dep->repoName));
        outputObjectToXml($dep, $depEl, $dom);
        $rootEl->appendChild($depEl);
    }

    $dom->appendChild($rootEl);

    return $dom;
}

function readExistingLog(Registry $registry)
{
    if (!file_exists(OUTPUT_FILE) || filesize(OUTPUT_FILE) < 1) {
        return;
    }

    logger('Found previous output file, reading entries...');
    $xml = simplexml_load_file(OUTPUT_FILE);
    if (!$xml) {
        throw new RuntimeException('Unable to read old output file');
    }

    foreach ($xml->Dependency as $depXml) {
        $dep              = new DepEntry();
        $dep->url         = (string) $depXml->Url;
        $dep->name        = (string) $depXml->Name;
        $dep->title       = (string) $depXml->Title;
        $dep->repoName    = (string) $depXml->RepoName;
        $dep->description = (string) $depXml->Description;
        $dep->owner       = (string) $depXml->Owner;

        foreach ($depXml->Maintainers->Maintainer as $maintainerXml) {
            $maintainer           = new Maintainer();
            $maintainer->url      = (string) $maintainerXml->Url;
            $maintainer->name     = (string) $maintainerXml->Name;
            $maintainer->location = (string) $maintainerXml->Location;
            $maintainer->email    = (string) $maintainerXml->Email;
            $maintainer->user     = (string) $maintainerXml->User;
            $dep->maintainers[]   = $maintainer;
        }

        $dep->languageTags = explode(';', (string) $depXml->LanguageTags);
        foreach ($dep->languageTags as &$langTag) {
            $langTag = trim($langTag);
        }

        $dep->watchersCount   = (int) $depXml->WatchersCount;
        $dep->forksCount      = (int) $depXml->ForksCount;
        $dep->openIssuesCount = (int) $depXml->OpenIssuesCount;
        $dep->archived        = ((string) $depXml->Archived) === 'true';
        $dep->hasIssues       = ((string) $depXml->HasIssues) === 'true';
        $dep->license         = (string) $depXml->Licence;
        $dep->countryOrigin   = (string) $depXml->CountryOrigin;
        $dep->updatedAt       = (string) $depXml->UpdatedAt;

        $registry->add($dep);
    }

    logger(sprintf('Loaded %d dependencies from previous output file', $registry->count()));
}

function saveOutput(Registry $registry)
{
    if (!$registry->count()) {
        return;
    }

    $outputDir  = __DIR__ . DIRECTORY_SEPARATOR;
    $outputPath = $outputDir . DIRECTORY_SEPARATOR . 'output.xml';
    getDepsXml($registry)->save($outputPath . '.tmp');
    if (!rename($outputPath . '.tmp', $outputPath)) {
        throw new RuntimeException('Unable to publish output filr to ' . $outputPath);
    }
}

const OUTPUT_FILE = __DIR__ . DIRECTORY_SEPARATOR . 'output.xml';
$depRegistry = new Registry();

$sigIntHandler = function($signal) use ($depRegistry) {
    switch ($signal) {
        case SIGTERM:
        case SIGINT:
        case SIGHUP:
            logger(sprintf('Caught %s signal, saving partial data', $signal));
            saveOutput($depRegistry);
            exit(0);
    }
};
pcntl_signal(SIGTERM, $sigIntHandler);
pcntl_signal(SIGHUP, $sigIntHandler);
pcntl_signal(SIGINT, $sigIntHandler);

$configFile = __DIR__ . DIRECTORY_SEPARATOR . 'config.php';
if (!file_exists($configFile)) {
    logger(
        sprintf(
            'Unable to read configuration file "%s". Please create config file from config.php.dist template.',
            $configFile
        )
    );
    exit(1);
}

require_once $configFile;
global $mdUrls, $jsonUrls;

if (!isset($mdUrls, $jsonUrls) || !defined('GITHUB_AUTH')) {
    logger('Missing configuration variables, please check your config.php file.');
    exit(1);
}

try {
    $stats = [];
    readExistingLog($depRegistry);

    readMdRepositories($depRegistry, $mdUrls);
    $stats['md'] = $depRegistry->count();

    readJsonRepositories($depRegistry, $jsonUrls);
    $stats['json'] = $depRegistry->count() - $stats['md'];

    logger('Collected: ' . json_encode($stats));
    saveOutput($depRegistry);
}
catch (Throwable $exception) {
    logger('Fatal error: ' . $exception);
    saveOutput($depRegistry);
    exit(1);
}
