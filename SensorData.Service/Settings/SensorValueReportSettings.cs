using SensorData.SharedComponents.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.Service.Settings
{
    public class SensorValueReportSettings
    {
        public string FileFormat { get; set; }
        public int IntervalInSeconds { get; set; }
        public string DirectoryPath { get; set; }
    }
}
