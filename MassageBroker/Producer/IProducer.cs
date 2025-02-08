namespace MessageBroker.Producer;

public interface IProducer
{
    void Produce(string content);
}

