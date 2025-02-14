using MessageBrokerLib.Broker;
using MessageBrokerLib.Logging;

namespace MessageBrokerLib.Plugins
{
    [Plugin("ConsumerPlugin")]
    public class ConsumerPlugin : IConsumerPlugin
    {
        private IMessageBroker _broker;
        private int _threadCount;

        public ConsumerPlugin()
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
}
