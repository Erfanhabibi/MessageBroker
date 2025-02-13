namespace MessageBrokerLib;

public class Message
{
    // Id for the message
    public Guid Id { get; set; } = Guid.NewGuid();

    // Message content
    public string Content { get; set; } = string.Empty;

    // Time when the message was sent
    public DateTime Timestamp { get; set; } = DateTime.Now;

    // Override ToString method for easy display
    public override string ToString()
    {
        return $"Id: {Id} | Content: {Content} | Timestamp: {Timestamp}";
    }
}
