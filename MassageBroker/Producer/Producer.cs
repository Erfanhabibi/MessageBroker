using MessageBroker.Broker;


namespace MessageBroker.Producer;


public class Producer(IMessageBroker broker) : IProducer
{
    private readonly IMessageBroker _broker = broker;

    public void Produce(string content)
    {
        var message= new Message { Content = content };
        _broker.SendMessage(message);
        Console.WriteLine($"[Producer] sent message: {content}");
    }
}
