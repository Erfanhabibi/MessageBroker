
using MessageBrokerLib.Broker;
using MessageBrokerLib.Logging;

namespace MessageBrokerLib.Plugins
{
    [Plugin("ProducerPlugin")]
    public class ProducerPlugin : IProducerPlugin
    {
        private IMessageBroker _broker;
        private int _threadCount;

        public ProducerPlugin()
        {
            _threadCount = 1; // مقدار پیش‌فرض یا هر مقداری که مناسب است
        }

        public void Initialize(IMessageBroker broker)
        {
            _broker = broker;
        }

        public void Configure(Dictionary<string, object> settings)
        {
            if (settings.ContainsKey("ThreadCount"))
            {
                _threadCount = Convert.ToInt32(settings["ThreadCount"]);
            }
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
}
