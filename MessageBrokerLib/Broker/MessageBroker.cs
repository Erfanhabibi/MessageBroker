using System.Text.Json;
using MessageBrokerLib.Logging;


namespace MessageBrokerLib.Broker;

public class MessageBroker : IMessageBroker
{
    private readonly Queue<Message> _messageQueue = new();
    private readonly object _lock = new();
    private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage", "messages.json");

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
            Logger.Log($"Message sent: {message}", LogLevel.Info);
        }
    }

    public Message? ReceiveMessage()
    {
        lock (_lock)
        {
            if (_messageQueue.Count == 0)
            {
                Logger.Log("No messages to receive.", LogLevel.Warning);
                return null;
            }

            var message = _messageQueue.Dequeue();
            RemoveMessageFromFile();
            Logger.Log($"Message received: {message}", LogLevel.Info);
            return message;
        }
    }

    public List<Message> GetAllMessages()
    {
        lock (_lock)
        {
            Logger.Log("Retrieved all messages.", LogLevel.Info);
            return _messageQueue.ToList();
        }
    }

    public void SaveMessageInFile(Message message)
    {
        lock (_lock)
        {
            try
            {
                var directory = Path.GetDirectoryName(_filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    Logger.Log($"Created directory: {directory}", LogLevel.Info);
                }

                var json = JsonSerializer.Serialize(_messageQueue, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
                Logger.Log("Messages saved to file as JSON.", LogLevel.Info);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error saving message to file: {ex.Message}", LogLevel.Error);
            }
        }
    }

    public void LoadMessageFromFile()
    {
        lock (_lock)
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Logger.Log("No message file found to load.", LogLevel.Warning);
                    return;
                }

                var json = File.ReadAllText(_filePath);
                var messages = JsonSerializer.Deserialize<List<Message>>(json);

                if (messages != null)
                {
                    foreach (var message in messages)
                    {
                        _messageQueue.Enqueue(message);
                    }
                    Logger.Log($"{messages.Count} messages loaded from file.", LogLevel.Info);
                }
            }
            catch (JsonException ex)
            {
                Logger.Log($"Error deserializing messages: {ex.Message}", LogLevel.Error);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error loading messages from file: {ex.Message}", LogLevel.Error);
            }
        }
    }

    public void RemoveMessageFromFile()
    {
        lock (_lock)
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Logger.Log("Message file does not exist.", LogLevel.Warning);
                    return;
                }

                var messages = File.ReadAllLines(_filePath).ToList();
                if (messages.Count > 0)
                {
                    messages.RemoveAt(0);
                    var json = JsonSerializer.Serialize(_messageQueue, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(_filePath, json);
                    Logger.Log("First message removed from file.", LogLevel.Info);
                }
                else
                {
                    Logger.Log("Message file is empty.", LogLevel.Warning);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error removing message from file: {ex.Message}", LogLevel.Error);
            }
        }
    }
}
