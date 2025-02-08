using MessageBroker.Broker;


namespace MessageBroker.Consumer;

public class Consumer(IMessageBroker broker) : IConsumer
{
    private readonly IMessageBroker _broker = broker;

    public void Consume()
    {
        var message = _broker.ReceiveMessage();
        if (message != null)
        {
            Console.WriteLine($"[Consumer] received message: {message.Content}");
        }
        else
        {
            Console.WriteLine("[Consumer] no message received");
        }
    }
}
