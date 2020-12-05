using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SensorData.Service
{
    sealed class SensorValueCollection
    {
        private static readonly Lazy<SensorValueCollection>
            lazy =
            new Lazy<SensorValueCollection>
                (() => new SensorValueCollection());

        internal static SensorValueCollection Instance { get { return lazy.Value; } }

        ConcurrentDictionary<byte, ConcurrentQueue<byte>> _collection;
        private SensorValueCollection()
        {
            _collection = new ConcurrentDictionary<byte, ConcurrentQueue<byte>>();
        }

        internal void AddSensorValue(byte sensorId, byte sensorValue)
        {
            ConcurrentQueue<byte> sensorValues = _collection.GetOrAdd(sensorId, new ConcurrentQueue<byte>());
            sensorValues.Enqueue(sensorValue);
        }

        internal IEnumerable<byte> GetSensorValues(byte sensorId)
        {
            ConcurrentQueue<byte> sensorValues = _collection.GetOrAdd(sensorId, new ConcurrentQueue<byte>());
            List<byte> copy = new List<byte>();
            copy.AddRange(sensorValues);
            return copy;
        }

        internal IEnumerable<byte> GetSensors()
        {
            List<byte> copy = new List<byte>();
            copy.AddRange(_collection.Keys);
            return copy;
        }
    }
}