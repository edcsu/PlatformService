using PlatformService.Business.Platform.Config;
using PlatformService.Business.Platform.ViewModels;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MessageBusClient> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string exchangeName = "trigger";
        public MessageBusClient(IConfiguration configuration, 
            ILogger<MessageBusClient> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var rabbitmqConfig = _configuration.GetRabbitMQConfig();
            var factory = new ConnectionFactory()
            {
                HostName = rabbitmqConfig.Host,
                Port = rabbitmqConfig.Port,
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                _logger.LogInformation("Connected to rabbitmq messagebus");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to rabbitmq");
                throw;
            }
        }

        public void PublishNewPlatform(PlatformPublished platformPublished)
        {
            var message = JsonSerializer.Serialize(platformPublished);

            if (_connection.IsOpen)
            {
                _logger.LogInformation("RabbitMQ connection is open");
                SendMessage(message);
            }
            else
            {
                _logger.LogError("RabbitMQ connection isnot open");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _logger.LogInformation("Publishing to RabbitMQ");
            _channel.BasicPublish(exchange: exchangeName, 
                routingKey: string.Empty,
                basicProperties: null,
                body: body);
            _logger.LogInformation("Published Message:{Message} to exchange:{Exchange}", message, exchangeName);
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("RabbitMQ connection shutdown");
        }

        public void Dispose()
        {
            _logger.LogInformation("MessageBus disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
            }
        }
    }
}
