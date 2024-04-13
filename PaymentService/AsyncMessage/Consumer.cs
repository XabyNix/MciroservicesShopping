using System.Text;
using System.Text.Json;
using Common.Models;
using PaymentService.Handlers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PaymentService.AsyncMessage;

public class Consumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private IModel _channel;
    private IConnection _connection;
    private string _queueName;


    public Consumer(IConfiguration configuration, IServiceScopeFactory scopeFactory)
    {
        _configuration = configuration;
        _scopeFactory = scopeFactory;
        InitRabbitMq();
    }

    private void InitRabbitMq()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_configuration["Aws:RabbitMQHost"] ?? string.Empty),
            UserName = _configuration["Aws:Username"],
            Password = _configuration["Aws:Password"]
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _queueName = _channel.QueueDeclare(exclusive: false).QueueName;

        _channel.QueueBind(_queueName, "order", "order.reserved");
    }

    //OrderCreatedEvent consumer
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var serializedBody = Encoding.UTF8.GetString(body);
            var itemsReservedEvent = JsonSerializer.Deserialize<ItemsReservedEvent>(serializedBody)!;

            Console.WriteLine($"--> Received: {itemsReservedEvent.GetType().Name}");
            using (var scope = _scopeFactory.CreateScope())
            {
                var paymentHandler = scope.ServiceProvider.GetService<IPaymentHandler>()!;
                await paymentHandler.Handle(itemsReservedEvent);
            }
        };
        _channel.BasicConsume(_queueName, true, consumer);
        return Task.CompletedTask;
    }
}