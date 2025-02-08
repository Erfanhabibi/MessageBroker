namespace MessageBroker;

public class Message
{
    //id for the message
    public Guid Id { get; set; } = Guid.NewGuid();

    //message text
    public string Content { get; set; } = string.Empty;

    //time the message was send
    public DateTime Timestamp { get; set; } = DateTime.Now;

    //override ToString method
    public override string ToString()
    {
        return $"Id: {Id} | Content: {Content} | Timestamp: {Timestamp}";
    }
}
