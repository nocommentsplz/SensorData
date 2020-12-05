using System.Collections.Generic;

namespace SensorData.Service
{
    public interface ISensorDataServerRegistry
    {
        IEnumerable<SensorDataServerRegistrar> RegisteredServerCreators { get; }
    }
}