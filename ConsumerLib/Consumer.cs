using MessageBrokerLib;

namespace ConsumerLib;

public class Consumer : IConsumer
{
    private readonly IMessageBroker _broker;

    public Consumer(IMessageBroker broker)
    {
        _broker = broker;
    }

    public Message? Consume()
    {
        return _broker.ReceiveMessage();
    }
}
