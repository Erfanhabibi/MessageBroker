using MessageBrokerLib.Broker;
using MessageBrokerLib.Plugins;
using Microsoft.AspNetCore.Mvc;

namespace MessageBrokerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PluginsController : ControllerBase
    {
        private readonly IMessageBroker _broker;

        public PluginsController(IMessageBroker broker)
        {
            _broker = broker;
        }

        [HttpGet("load")]
        public IActionResult LoadPlugins()
        {
            var assemblyLocation = Path.GetDirectoryName(typeof(PluginLoader).Assembly.Location);
            var pluginsPath = Path.Combine(assemblyLocation, "Plugins");

            var plugins = PluginLoader.LoadPlugins(pluginsPath);

            foreach (var plugin in plugins)
            {
                plugin.Initialize(_broker);

                var settings = new Dictionary<string, object>
                {
                    { "ThreadCount", 5 }
                };
                plugin.Configure(settings);

                if (plugin is IProducerPlugin producer)
                {
                    producer.Produce("Sample message");
                }

                if (plugin is IConsumerPlugin consumer)
                {
                    consumer.Consume();
                }
            }

            return Ok("Plugins loaded and executed.");
        }
    }
}
