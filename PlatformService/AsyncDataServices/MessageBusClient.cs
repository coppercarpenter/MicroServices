using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlatformService.DTOs;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    internal class MessageBusClient : IMessageBusClient
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration config, ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("MessageBusClient");
            _config = config;
            var factory = new ConnectionFactory()
            {
                HostName = config["RabbitMQHost"],
                Port = int.Parse(config["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQConnectionShutDown;


                _logger.LogInformation("Connected to message Bus");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

        }

        private void RabbitMQConnectionShutDown(object sender, ShutdownEventArgs args)
        {
            _logger.LogInformation("Connection Shutdown");
        }

        public void PublishNewPlatform(PlatformPublishedDto platform)
        {
            var message = JsonSerializer.Serialize(platform);
            if (_connection.IsOpen)
            {
                _logger.LogInformation("Connection open, sending message ");
                SendMessage(message);
            }
            else
            {
                _logger.LogError("Connection is Close");
            }
        }


        public void Dispose()
        {
            _logger.LogInformation("MessageBus disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);

            _logger.LogInformation($"message sent : {message}");

        }
    }
}
