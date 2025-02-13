using MessageBrokerLib;

namespace ConsumerLib;

public class Consumer : IConsumer
{
    private readonly IMessageBroker _broker;

    public Consumer(IMessageBroker broker)
    {
        _broker = broker;
    }

    public void Consume()
    {
        var message = _broker.ReceiveMessage();
        if (message != null)
        {
            Console.WriteLine($"[Consumer] Received message: {message.Content}");
        }
        else
        {
            Console.WriteLine("[Consumer] No message received");
        }
    }
}
