

namespace MessageBrokerLib.Broker;

public interface IMessageBroker
{
    void SendMessage(Message message);
    Message? ReceiveMessage();
    List<Message> GetAllMessages();
    void SaveMessageInFile(Message message);
    void LoadMessageFromFile();
    void RemoveMessageFromFile();
}
