using SensorData.Service.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SensorData.Service
{
    class SensorValueReportGenerator : ISensorValueReportGenerator
    {
        public SensorValueReportSettings Settings { private get; set; }
        private static object _generateLock = new object();
        public void GenerateReport()
        {
            lock (_generateLock)
            {
                List<SensorValueReportEntity> report = new List<SensorValueReportEntity>();

                IEnumerable<byte> sensors = SensorValueCollection.Instance.GetSensors();
                foreach (byte sensorId in sensors)
                {
                    IEnumerable<byte> sensorValues = SensorValueCollection.Instance.GetSensorValues(sensorId);
                    if (sensorValues.Any())
                    {
                        report.Add(new SensorValueReportEntity()
                        { sensor_id = sensorId, latest = sensorValues.Last(), average = Convert.ToByte(sensorValues.Average(x => x)) });
                    }
                }

                if (report.Any())
                {
                    string fileName = Settings.FileFormat.Replace("{date_time}", DateTime.Now.ToString("yy-MM-dd-hh-mm-ss"));
                    string path = Path.Combine(Settings.DirectoryPath, fileName);
                    try
                    {
                        string reportText = JsonSerializer.Serialize(report, new JsonSerializerOptions() { WriteIndented = true });
                        File.WriteAllText(path, reportText);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }
    }

    class SensorValueReportEntity
    {
        public byte sensor_id { get; set; }
        public byte average { get; set; }
        public byte latest { get; set; }
    }
}
