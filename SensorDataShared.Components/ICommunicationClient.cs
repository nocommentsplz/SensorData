using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SensorData.SharedComponents
{
    public interface ICommunicationClient<TCommandObject>
        where TCommandObject : class, ICommandObject, new()
    {
        int Send(TCommandObject commandObject);
        TCommandObject SendReceive(TCommandObject commandObject);
    }
}
