using MessageBrokerLib.Broker;
using MessageBrokerLib.Logging;

namespace ConsumerLib;

public class Consumer : IConsumer
{
    private readonly IMessageBroker _broker;
    private readonly int _threadCount;

    public Consumer(IMessageBroker broker, int threadCount)
    {
        _broker = broker;
        _threadCount = threadCount;
    }

    public void Consume()
    {
        var tasks = new List<Task>();

        for (int i = 0; i < _threadCount; i++)
        {
            tasks.Add(Task.Run(() => ConsumeMessage()));
        }

        Task.WaitAll(tasks.ToArray());
    }

    private void ConsumeMessage()
    {
        while (true)
        {
            var message = _broker.ReceiveMessage();
            if (message != null)
            {
                Logger.Log($"[Consumer] Received message: {message.Content}", LogLevel.Info);
            }
            else
            {
                break;
            }
        }
    }
}
