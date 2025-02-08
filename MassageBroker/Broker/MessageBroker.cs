using System.Text.Json;

namespace MessageBroker.Broker;




public class MessageBroker : IMessageBroker
{
    private readonly Queue<Message> _messageQueue = new Queue<Message>();

    public void SendMessage(Message message)
    {
        lock (_messageQueue)
        {
            _messageQueue.Enqueue(message);
            Console.WriteLine($"[Message Broker] Message sent: {message}");
        }
    }

    private readonly Object _lock = new();

    public Message? ReceiveMessage()
    {
        lock (_lock)
        { 
            if (_messageQueue.Count == 0)
            {
                Console.WriteLine("[Message Broker] No messages to receive");
                return null;
            }
            var message = _messageQueue.Dequeue();
            Console.WriteLine($"[Message Broker] Message received: {message}");
            return message;
        }
    }


    public void SaveMessageInFile(string filePath)
    {
        lock (_lock)
        {
            var messages = _messageQueue.ToList();
            var json = JsonSerializer.Serialize(messages);
                
            File.WriteAllText(filePath, json);
            Console.WriteLine($"[Message Broker] Messages saved to {filePath}");
        }
    }

    public void LoadMessageFromFile(string filePath)
    {
        lock (_lock)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var messages = JsonSerializer.Deserialize<List<Message>>(json);

                if (messages != null)
                {
                    foreach (var message in messages)
                    {
                        _messageQueue.Enqueue(message);
                    }
                    Console.WriteLine($"[Message Broker] Messages loaded from {filePath}");
                }
            }
        }
    }
}
