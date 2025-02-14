using Microsoft.AspNetCore.Mvc;
using MessageBrokerLib.Broker;

namespace MessageBrokerApi.Controllers;

[ApiController]
[Route("api/messages")]
public class MessageBrokerController : ControllerBase
{
    private readonly IMessageBroker _broker;

    public MessageBrokerController(IMessageBroker broker)
    {
        _broker = broker;
    }

    [HttpPost("send")]
    public IActionResult SendMessage([FromBody] Message message)
    {
        _broker.SendMessage(message);
        return Ok(new { message = "Message sent successfully!" });
    }

    [HttpGet("receive")]
    public IActionResult ReceiveMessage()
    {
        var message = _broker.ReceiveMessage();
        return message != null ? Ok(message) : NoContent();
    }

    [HttpGet("all")]
    public IActionResult GetAllMessages()
    {
        var messages = _broker.GetAllMessages();
        return Ok(messages);
    }
}
