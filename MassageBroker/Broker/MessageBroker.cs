using System.Text.Json;

namespace MessageBroker.Broker;


public class MessageBroker : IMessageBroker
{
    private readonly Queue<Message> _messageQueue = new();
    private readonly object _lock = new();

    private readonly string _filePath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\Storage\messages.txt");


    public MessageBroker()
    {
        LoadMessageFromFile();
    }

    public void SendMessage(Message message)
    {
        lock (_lock)
        {
            _messageQueue.Enqueue(message);
            SaveMessageInFile(message);
            Console.WriteLine($"[Message Broker] Message sent: {message}");
        }
    }

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
            RemoveMessageFromFile();
            Console.WriteLine($"[Message Broker] Message received: {message}");
            return message;
        }
    }

    public void SaveMessageInFile(Message message)
    {
        lock (_lock)
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(message);
            File.AppendAllText(_filePath, json + Environment.NewLine);
        }
    }

    public void LoadMessageFromFile()
    {
        lock (_lock)
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("[Message Broker] No message file found to load.");
                return;
            }

            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                try
                {
                    var message = JsonSerializer.Deserialize<Message>(line);
                    if(message != null)
                    {
                        _messageQueue.Enqueue(message);
                        Console.WriteLine($"[Message Broker] Message loaded: {message}");
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"[Message Broker] Error deserializing message: {ex.Message}");
                }

            }

        }
    }

    public void RemoveMessageFromFile()
    {
        lock (_lock)
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("[Message Broker] Message file does not exist.");
                return;
            }
            var lines = File.ReadAllLines(_filePath);
            if (lines.Length > 0)
            {
                var newLines =  lines.Skip(1).ToList();
                File.WriteAllLines(_filePath, newLines);
                Console.WriteLine("[Message Broker] first line removed from file.");
            }

        }
            
    }

}


