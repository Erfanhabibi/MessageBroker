namespace MessageBroker.Broker;

public interface IMessageBroker
{
    void SendMessage(Message message);
    Message? ReceiveMessage();

    void SaveMessageInFile( Message message);
    void LoadMessageFromFile();
    void RemoveMessageFromFile();
}
