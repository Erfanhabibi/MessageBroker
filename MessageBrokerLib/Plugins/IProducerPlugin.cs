namespace MessageBrokerLib.Plugins
{
    public interface IProducerPlugin : IPlugin
    {
        void Produce(string content);
    }
}
