<div id="top"></div>

<!-- PROJECT SHIELDS -->
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]


<!-- PROJECT LOGO -->
<br />
<div align="center">

<h3 align="center">DepScan - simple project dependency scanning</h3>

  <p align="center">
    Scan your project contents and find out which countries your dependencies originate from.
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

![DepScanWin Screen Shot][product-screenshot]

DepScan tool can provide a simple solution to quickly scan your project directory for 3rd party dependencies and gather geolocation info about dependency owner and maintainers and which country they are related to. It uses famous awesome / made-in projects by [IonicaBizau](https://github.com/IonicaBizau) and other contributors to collect dependency metadata, country of origin and other related GitHub data.
<br >
<br >
The windows executable expects an input XML file (dictionary) with dependency information and produces text report with matched dependency data. You may find already prepared dictionary files per country under `dictionary` directory.
You may also use command line PHP script `php-scrapper`, bundled within this project to customize your XML input and selectively scrap data for specific countries.

![DepScanWin intro dialog][product-screenshot2]

### Supported countries
Currently, the following countries are supported either by MD or JSON format provided by other contributor repositories.
If you need to prepare your own XML dictionary refer to php-scrapper CLI tool.

#### MD format compatible
[Brazil](https://raw.githubusercontent.com/felipefialho/awesome-made-by-brazilians/master/README.md)
[Russia](https://raw.githubusercontent.com/igoradamenko/awesome-made-by-russians/master/README.md)
[India](https://raw.githubusercontent.com/jeswinsimon/awesome-made-by-indians/master/README.md)
[Japan](https://raw.githubusercontent.com/mvximenko/awesome-made-by-japanese/main/README.md)
[Ukraine](https://raw.githubusercontent.com/chernivtsijs/made-in-ukraine/master/README.md)
[Germany](https://raw.githubusercontent.com/mvximenko/awesome-made-by-germans/main/README.md)
[Angola](https://raw.githubusercontent.com/joaroque/awesome-made-by-angolans/main/README.md)
[China](https://raw.githubusercontent.com/JN-H/awesome-made-by-chinese/master/README.md)
[Albania](https://raw.githubusercontent.com/redjanym/awesome-made-by-albanians/master/README.md)
[Mexico](https://raw.githubusercontent.com/kinduff/awesome-made-by-mexicans/main/README.md)

#### JSON format compatible
[Turkey](https://raw.githubusercontent.com/IonicaBizau/made-in-turkey/master/package.json)
[Romania](https://raw.githubusercontent.com/IonicaBizau/made-in-romania/master/package.json)
[Serbia](https://raw.githubusercontent.com/IonicaBizau/made-in-serbia/master/package.json)
[Russia](https://raw.githubusercontent.com/IonicaBizau/made-in-russia/master/package.json)
[Brazil](https://raw.githubusercontent.com/IonicaBizau/made-in-brazil/master/package.json)
[Belarus](https://raw.githubusercontent.com/IonicaBizau/made-in-belarus/master/package.json)
[Italy](https://raw.githubusercontent.com/IonicaBizau/made-in-italy/master/package.json)
[Chile](https://raw.githubusercontent.com/IonicaBizau/made-in-chile/master/package.json)
[Bosnia](https://raw.githubusercontent.com/IonicaBizau/made-in-bosnia/master/package.json)
[Colombia](https://raw.githubusercontent.com/IonicaBizau/made-in-colombia/master/package.json)
[Poland](https://raw.githubusercontent.com/IonicaBizau/made-in-poland/master/package.json)
[India](https://raw.githubusercontent.com/IonicaBizau/made-in-india/master/package.json)
[Netherlands](https://raw.githubusercontent.com/IonicaBizau/made-in-netherlands/master/package.json)
[Bulgaria](https://raw.githubusercontent.com/IonicaBizau/made-in-bulgaria/master/package.json)
[Bolivia](https://raw.githubusercontent.com/IonicaBizau/made-in-bolivia/master/package.json)
[Ukraine](https://raw.githubusercontent.com/IonicaBizau/made-in-ukraine/master/package.json)
[Venezuela](https://raw.githubusercontent.com/IonicaBizau/made-in-venezuela/master/package.json)
[Uruguay](https://raw.githubusercontent.com/IonicaBizau/made-in-uruguay/master/package.json)
[Spain](https://raw.githubusercontent.com/IonicaBizau/made-in-spain/master/package.json)
[Slovenia](https://raw.githubusercontent.com/IonicaBizau/made-in-slovenia/master/package.json)
[Slovakia](https://raw.githubusercontent.com/IonicaBizau/made-in-slovakia/master/package.json)
[Portugal](https://raw.githubusercontent.com/IonicaBizau/made-in-portugal/master/package.json)
[Peru](https://raw.githubusercontent.com/IonicaBizau/made-in-peru/master/package.json)
[Paraguay](https://raw.githubusercontent.com/IonicaBizau/made-in-paraguay/master/package.json)
[Moldova](https://raw.githubusercontent.com/IonicaBizau/made-in-moldova/master/package.json)
[Malta](https://raw.githubusercontent.com/IonicaBizau/made-in-malta/master/package.json)
[Lithuania](https://raw.githubusercontent.com/IonicaBizau/made-in-lithuania/master/package.json)
[Latvia](https://raw.githubusercontent.com/IonicaBizau/made-in-latvia/master/package.json)
[Guyana](https://raw.githubusercontent.com/IonicaBizau/made-in-guyana/master/package.json)
[Greece](https://raw.githubusercontent.com/IonicaBizau/made-in-greece/master/package.json)
[France](https://raw.githubusercontent.com/IonicaBizau/made-in-france/master/package.json)
[Finland](https://raw.githubusercontent.com/IonicaBizau/made-in-finland/master/package.json)
[Estonia](https://raw.githubusercontent.com/IonicaBizau/made-in-estonia/master/package.json)
[Ecuador](https://raw.githubusercontent.com/IonicaBizau/made-in-ecuador/master/package.json)
[Denmark](https://raw.githubusercontent.com/IonicaBizau/made-in-denmark/master/package.json)
[Croatia](https://raw.githubusercontent.com/IonicaBizau/made-in-croatia/master/package.json)
[Belgium](https://raw.githubusercontent.com/IonicaBizau/made-in-belgium/master/package.json)
[Austria](https://raw.githubusercontent.com/IonicaBizau/made-in-austria/master/package.json)
[Argentina](https://raw.githubusercontent.com/IonicaBizau/made-in-argentina/master/package.json)
[Albania](https://raw.githubusercontent.com/IonicaBizau/made-in-albania/master/package.json)

### Supported scan options

* Follow compressed files (jar, ear, war, zip)
* Scan Javascript project metadata files (NPM)
* Scan Python project metadata files (requirements)
* Scan Java project metadata files (POM, Gradle)
* Scan PHP project metadata files (Composer)
* Scan all textual files with extended regex - this option will target all textual files instead of tech stack specific like java, nodejs etc.
* Scan for extra package maintainer details by their github usernames, nicknames, emails etc.

<p align="right">(<a href="#top">back to top</a>)</p>



### Built With

* [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)
* [PHP](https://www.php.net/)


<!-- GETTING STARTED -->
## Getting Started

Checkout this project and open it with C# suported IDE.
You may use VSCode or VisualStudio Community.

### Prerequisites

If you plan to use php-scrapper to customize your search then install php system wide or use php docker image.
Before you run php script create config.php file from template php-scrapper/config.php.dist and update configuration variables to suite your needs. For an example you may need to insert your own GitHub API token which is required by php to communicate with GitHub API.

### Installation

1. Get a free GitHub API Key if you want to prepare your custom input data with dependency information targeting specific country or list of countries.
2. Clone the repo
   ```sh
   git clone https://github.com/tezvi/depscan.git
   ```
3. Update php script `php-scrapper/scrapper.php` and insert your own GitHub API key and modify country list if needed and then run the script.
   ```sh
   php -f php-scrapper/scrapper.php
   ```
4. Open DepScanWin in Visual Studio or other supported IDE and build project. Execute DepScanWin from project's bin directory.


<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

Project Link: [https://github.com/tezvi/depscan](https://github.com/tezvi/depscan)

<p align="right">(<a href="#top">back to top</a>)</p>


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[license-shield]: https://img.shields.io/github/license/tezvi/depscan.svg?style=for-the-badge
[license-url]: https://github.com/tezvi/depscan/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/andrej-v-11481925/
[product-screenshot]: docs/scanscreen.png
[product-screenshot2]: docs/mainscreen.png
