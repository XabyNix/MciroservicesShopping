using System.Text;
using System.Text.Json;
using CatalogService.Handlers;
using Common.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CatalogService.AsyncMessaging;

public class Consumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _scopeFactory;
    private IModel _channel;
    private IConnection _connection;

    private string _orderConfirmedQueue;
    private string _orderCreatedQueue;
    private string _rejectedPaymentQueue;


    public Consumer(IConfiguration configuration, IServiceProvider scopeFactory)
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

        _orderCreatedQueue = _channel.QueueDeclare(exclusive: false).QueueName;
        _rejectedPaymentQueue = _channel.QueueDeclare(exclusive: false).QueueName;
        _orderConfirmedQueue = _channel.QueueDeclare(exclusive: true).QueueName;

        _channel.QueueBind(_orderCreatedQueue, "order", "order.created");
        _channel.QueueBind(_rejectedPaymentQueue, "order", "order.payment.rejected");
        _channel.QueueBind(_orderConfirmedQueue, "order", "order.confirm");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        EventConsumer<OrderCreatedEvent>(_orderCreatedQueue);
        EventConsumer<PaymentRejectedEvent>(_rejectedPaymentQueue);
        EventConsumer<OrderConfirmEvent>(_orderConfirmedQueue);
        return Task.CompletedTask;
    }

    private void EventConsumer<T>(string queueName) where T : BasicEvent
    {
        var orderConfirmedConsumer = new EventingBasicConsumer(_channel);
        orderConfirmedConsumer.Received += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var serializedBody = Encoding.UTF8.GetString(body);
            var clearEvent = JsonSerializer.Deserialize<T>(serializedBody);
            Console.WriteLine($"--> Received: {typeof(T).Name}");
            using (var scope = _scopeFactory.CreateScope())
            {
                var eventHandler = scope.ServiceProvider.GetService<IEventHandlers>();
                switch (clearEvent)
                {
                    case OrderCreatedEvent orderCreatedEvent:
                        await eventHandler.Handle(orderCreatedEvent);
                        break;
                    case PaymentRejectedEvent paymentRejectedEvent:
                        await eventHandler.Handle(paymentRejectedEvent);
                        break;
                    case OrderConfirmEvent orderConfirmEvent:
                        await eventHandler.Handle(orderConfirmEvent);
                        break;
                }
            }
        };
        _channel.BasicConsume(queueName, true, orderConfirmedConsumer);
    }
}