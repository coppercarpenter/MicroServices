using CommandsService.EventProcessing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommandsService.AsyncDataService
{
    public class MessageBusSubscriber : BackgroundService
    {
        #region Private Fields

        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly IEventProcessor _event;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        #endregion Private Fields

        #region Private Methods

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQHost"],
                Port = int.Parse(_config["RabbitMQPort"])
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "");

            _logger.LogInformation("Listing on the message bus");

            _connection.ConnectionShutdown += RabbitMQConnectionShutdown;
        }

        private void RabbitMQConnectionShutdown(object sender, ShutdownEventArgs args)
        {
            _logger.LogInformation("Connection Closed");
        }

        #endregion Private Methods

        #region Constructor

        public MessageBusSubscriber(IConfiguration config, IEventProcessor eventProcessor, ILoggerFactory logger)
        {
            _config = config;
            _event = eventProcessor;
            _logger = logger.CreateLogger("MessageBusSubscriber");

            InitializeRabbitMQ();
        }

        #endregion Constructor

        #region Methods

        public override void Dispose()
        {
            if (_connection.IsOpen)
            {
                _connection.Close();
                _connection.Close();
            }
            base.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (moduleHandle, e) =>
            {
                _logger.LogInformation("Event Received");

                var body = e.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                _event.ProecssEvent(notificationMessage);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        #endregion Methods
    }
}