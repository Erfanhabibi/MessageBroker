namespace MessageBroker.Broker;

public interface IMessageBroker
{
    void SendMessage(Message message);
    Message? ReceiveMessage();

    void SaveMessageInFile(string filePath);
    void LoadMessageFromFile(string filePath);
}
