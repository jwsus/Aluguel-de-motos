using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;

namespace SeuProjeto.Services
{
    public class RabbitMQService : IDisposable
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQService(IConfiguration configuration)
        {
            _configuration = configuration;
            InitializeRabbitMQ();
        }

        public void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory() 
            { 
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672 // Porta padrão do RabbitMQ
            };
            Console.WriteLine(factory);
            // Tentativa de conexão com espera
            var maxRetryAttempts = 10;
            var delay = TimeSpan.FromSeconds(5);

            for (int i = 0; i < maxRetryAttempts; i++)
            {
                try
                {
                    _connection = factory.CreateConnection();
                    _channel = _connection.CreateModel();
                    Console.WriteLine("Conexão com RabbitMQ estabelecida com sucesso.");
                    break;
                }
                catch (BrokerUnreachableException)
                {
                    Console.WriteLine($"Tentativa {i + 1} de {maxRetryAttempts}: RabbitMQ não está acessível. Esperando {delay.Seconds} segundos antes de tentar novamente.");
                    System.Threading.Thread.Sleep(delay);
                }
            }

            if (_connection == null || _channel == null)
            {
                throw new Exception("Não foi possível estabelecer a conexão com o RabbitMQ após múltiplas tentativas.");
            }
        }

        public void SendMessage(string message)
        {
            _channel.QueueDeclare(queue: "testQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = System.Text.Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                 routingKey: "testQueue",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($" [x] Sent {message}");
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
