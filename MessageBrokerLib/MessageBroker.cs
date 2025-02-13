using System.Collections.Concurrent;
using System.Text.Json;

namespace MessageBrokerLib;

public class MessageBroker : IMessageBroker
{
    private readonly ConcurrentQueue<Message> _messageQueue = new();
    private readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "messages.json");

    public MessageBroker()
    {
        LoadMessagesFromFile();
    }

    public void SendMessage(Message message)
    {
        _messageQueue.Enqueue(message);
        SaveMessagesToFile();
    }

    public Message? ReceiveMessage()
    {
        if (_messageQueue.TryDequeue(out var message))
        {
            SaveMessagesToFile();
            return message;
        }
        return null;
    }

    public List<Message> GetAllMessages()
    {
        return _messageQueue.ToList();
    }

    private void SaveMessagesToFile()
    {
        var messages = _messageQueue.ToList();
        File.WriteAllText(_filePath, JsonSerializer.Serialize(messages));
    }

    private void LoadMessagesFromFile()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            var messages = JsonSerializer.Deserialize<List<Message>>(json) ?? new List<Message>();
            foreach (var message in messages)
            {
                _messageQueue.Enqueue(message);
            }
        }
    }
}
