using System.Net.Sockets;

namespace SensorData.SharedComponents
{
    public interface IIndividualCommandObjectProcessor<TCommandObject>
        where TCommandObject : class, ICommandObject, new()
    {
        void ProcessCommand(TCommandObject commandObject, Socket socket);
    }
}