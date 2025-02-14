using MessageBrokerLib.Broker;
using MessageBrokerLib.Logging;

namespace ProducerLib;

public class Producer : IProducer
{
    private readonly IMessageBroker _broker;
    private readonly int _threadCount;

    public Producer(IMessageBroker broker, int threadCount)
    {
        _broker = broker;
        _threadCount = threadCount;
    }

    public void Produce(string content)
    {
        var tasks = new List<Task>();

        for (int i = 0; i < _threadCount; i++)
        {
            tasks.Add(Task.Run(() => ProduceMessage(content)));
        }

        Task.WaitAll(tasks.ToArray());
    }

    private void ProduceMessage(string content)
    {
        var message = new Message { Content = content };
        const int maxRetryAttempts = 3;
        int retryCount = 0;
        bool isSent = false;

        while (retryCount < maxRetryAttempts && !isSent)
        {
            try
            {
                _broker.SendMessage(message);
                isSent = true;
                Logger.Log($"[Producer] Sent message: {content}", LogLevel.Info);
            }
            catch (Exception ex)
            {
                retryCount++;
                Logger.Log($"Attempt {retryCount} - Failed to send message: {ex.Message}", LogLevel.Warning);
                if (retryCount == maxRetryAttempts)
                {
                    Logger.Log($"[Producer] Failed to send message after {maxRetryAttempts} attempts.", LogLevel.Error);
                }
            }
        }
    }
}
