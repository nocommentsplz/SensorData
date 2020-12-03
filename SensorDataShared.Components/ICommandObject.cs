namespace SensorData.SharedComponents
{
    public interface ICommandObject
    {
        byte[] RawCommandDataBuffer { get; set; }
    }
}