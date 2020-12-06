using SensorData.Service.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.Service
{
    public interface ISensorValueReportGenerator
    {
        List<SensorValueReportEntity> GenerateReport();
        void SaveReportToFile();
        SensorValueReportSettings Settings { set; }
    }

    public class SensorValueReportEntity
    {
        public byte sensor_id { get; set; }
        public byte average { get; set; }
        public byte latest { get; set; }
    }
}
