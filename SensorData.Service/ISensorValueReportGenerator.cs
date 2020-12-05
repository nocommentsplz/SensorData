using SensorData.Service.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.Service
{
    public interface ISensorValueReportGenerator
    {
        void GenerateReport();
        SensorValueReportSettings Settings { set; }
    }
}
