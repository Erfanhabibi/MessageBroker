using MessageBrokerLib;

namespace ProducerLib;

public class Producer : IProducer
{
    private readonly IMessageBroker _broker;

    public Producer(IMessageBroker broker)
    {
        _broker = broker;
    }

    public void Produce(string content)
    {
        var message = new Message { Content = content };
        _broker.SendMessage(message);
        Console.WriteLine($"[Producer] Sent message: {content}");
    }
}
