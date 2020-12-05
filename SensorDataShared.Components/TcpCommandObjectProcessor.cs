using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SensorData.SharedComponents
{
    public abstract class TcpCommandObjectProcessor<TCommandObject> : ICommandObjectProcessor<TCommandObject>
        where TCommandObject : class, ITcpCommandObject, new()
    {
        public virtual void ProcessCommand(TCommandObject commandObject, Socket socket)
        {
            IIndividualCommandObjectProcessor<TCommandObject> indivitualCommandObjectProcessor
                = GetIndividualCommandObjectProcessor(commandObject);

            indivitualCommandObjectProcessor.ProcessCommand(commandObject, socket);
        }

        protected abstract IIndividualCommandObjectProcessor<TCommandObject> GetIndividualCommandObjectProcessor(TCommandObject commandObject);
    }
}
