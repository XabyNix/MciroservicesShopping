using System.Text;
using System.Text.Json;
using OrderService.Database;
using OrderService.DTO;
using OrderService.Models;
using OrderService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderService.AsyncComunication;

public class MessageService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private IConnection _connection;
    private IModel _channel;
    private string _queueName;
    private readonly IServiceScopeFactory _scopeFactory;


    public MessageService(IConfiguration configuration, IServiceScopeFactory scopeFactory)
    {
        _configuration = configuration;
        _scopeFactory = scopeFactory;
        InitRabbitMQ();
    }

    private void InitRabbitMQ()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQHost"],
            Port = int.Parse(_configuration["RabbitMQPort"] ?? throw new ArgumentNullException("")),
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        //_channel.ExchangeDeclare("");
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(_queueName, "cart", "cart.checkout");
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var serializedBody = Encoding.UTF8.GetString(body);
            
            var receivedEvent = JsonSerializer.Deserialize<CheckoutEvent>(serializedBody);
            Console.WriteLine("--> Received: ");
            Console.WriteLine(serializedBody);

            using (var scope = _scopeFactory.CreateScope())
            {
                var orderProcess = scope.ServiceProvider.GetRequiredService<IOrderProcess>();
                orderProcess.Validate(receivedEvent);
            }
        };
        _channel.BasicConsume(_queueName, true, consumer);
        return Task.CompletedTask;
    }

    
}