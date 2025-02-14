using MessageBrokerLib.Broker;

namespace MessageBrokerLib.Plugins
{
    public interface IPlugin
    {
        void Initialize(IMessageBroker broker);
        void Configure(Dictionary<string, object> settings);
    }
}
