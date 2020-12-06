using SensorData.SharedComponents.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.Service.Settings
{
    public class SensorValueReportSettings
    {
        public string FileFormat { get; set; }
        public int ReportInConsoleIntervalInSeconds { get; set; }
        public int ReportInFileIntervalInSeconds { get; set; }
        public string DirectoryPath { get; set; }
    }
}
