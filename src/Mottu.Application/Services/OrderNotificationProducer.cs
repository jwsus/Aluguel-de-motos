using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Mottu.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Mottu.Application.Services
{
    using RabbitMqModel = RabbitMQ.Client.IModel;
    public class OrderNotificationProducer : IDisposable
    {
        private readonly IConnection _connection;
        private readonly RabbitMqModel _channel;
        private const string QueueName = "order_notifications";

        public OrderNotificationProducer()
        {
            var factory = new ConnectionFactory() { 
              HostName = "localhost", // ou "127.0.0.1"
              Port = 5672,
              UserName = "guest", // Conforme configurado no docker-compose
              Password = "guest" 
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: QueueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        public void NotifyOrderCreated(Order order)
        {
            var message = JsonSerializer.Serialize(order);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                  routingKey: QueueName,
                                  basicProperties: null,
                                  body: body);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
