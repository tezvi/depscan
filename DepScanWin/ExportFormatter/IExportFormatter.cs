using System.IO;

namespace DepScan.ExportFormatter
{
    internal interface IExportFormatter
    {
        void Export(string filePath, ResultManager resultManager);

        void OutputScanResult(ResultManager.ScannedFile scannedFile, TextWriter writer);
    }
}
