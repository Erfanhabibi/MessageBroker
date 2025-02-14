

namespace MessageBrokerLib.Plugins
{
    public interface IConsumerPlugin : IPlugin
    {
        void Consume();
    }
}
