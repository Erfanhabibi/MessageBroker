namespace MessageBrokerLib;

public interface IMessageBroker
{
    void SendMessage(Message message);
    Message? ReceiveMessage();
    List<Message> GetAllMessages();
}
