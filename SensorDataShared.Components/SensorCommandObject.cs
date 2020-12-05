using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.SharedComponents
{
    public class SensorCommandObject : ApiCommandObject
    {
        public SensorCommandObject() : base() { }

        public SensorCommandObject(byte command) : base(command) { }

        public SensorCommandObject(byte command, byte sensorId) : base(command, sensorId) { }

        public SensorCommandObject(byte command, byte sensorId, byte[] data) : base(command, sensorId, data) { }
    }
}
