using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;
using Mottu.Domain.Entities;
using Mottu.Application.Common.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Mottu.Api.Services
{
    using RabbitMqModel = RabbitMQ.Client.IModel; // Alias para RabbitMQ.Client.IModel

    public class OrderNotificationConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly RabbitMqModel _channel;
        private const string QueueName = "order_notifications";
        private readonly IServiceScopeFactory _scopeFactory;

        public OrderNotificationConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
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

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var order = JsonSerializer.Deserialize<Order>(message);

                // Processar a notificação do pedido
                await ProcessOrderNotification(order, cancellationToken);
            };

            _channel.BasicConsume(queue: QueueName,
                                  autoAck: true,
                                  consumer: consumer);

            return Task.CompletedTask;
        }

        private async Task ProcessOrderNotification(Order order, CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                // Obter entregadores aptos
                var deliverymen = dbContext.Deliverymans
                                           .ToList();

                // Armazenar a notificação no banco de dados
                foreach (var deliveryman in deliverymen)
                {
                    var notification = new Notification
                    {
                        Id = Guid.NewGuid(),
                        DeliverymanId = deliveryman.Id,
                        OrderId = order.Id,
                        Message = $"Novo pedido disponível: {order.Id}",
                        CreatedAt = DateTime.UtcNow
                    };

                    dbContext.Notifications.Add(notification);
                }

                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
