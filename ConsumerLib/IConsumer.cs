using MessageBrokerLib;

namespace ConsumerLib;

public interface IConsumer
{
    Message? Consume();
}
