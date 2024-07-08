using System.Text;
using System.Text.Json;
using Common.Models;
using OrderService.Handlers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderService.AsyncComunication;

public class Consumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private IModel _channel;
    private IConnection _connection;

    private string _orderAbortQueue;
    private string _orderCheckoutQueue;
    private string _orderConfirmedQueue;


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
            Uri = new Uri(_configuration["Aws:RabbitMQHost"]),
            UserName = _configuration["Aws:Username"],
            Password = _configuration["Aws:Password"]
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare("order", ExchangeType.Topic, true);

        _orderCheckoutQueue = _channel.QueueDeclare(exclusive: false).QueueName;
        _orderConfirmedQueue = _channel.QueueDeclare(exclusive: false).QueueName;
        _orderAbortQueue = _channel.QueueDeclare(exclusive: false).QueueName;

        _channel.QueueBind(_orderCheckoutQueue, "order", "cart.checkout");
        _channel.QueueBind(_orderConfirmedQueue, "order", "order.confirm");
        _channel.QueueBind(_orderAbortQueue, "order", "order.reserved.error");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        EventConsumer<OrderConfirmEvent>(_orderConfirmedQueue);
        EventConsumer<CheckoutEvent>(_orderCheckoutQueue);
        EventConsumer<ItemReservationFailedEvent>(_orderAbortQueue);
        return Task.CompletedTask;
    }

    private void EventConsumer<T>(string queueToConsume) where T : BasicEvent
    {
        var eventConsumer = new EventingBasicConsumer(_channel);
        eventConsumer.Received += (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var serializedBody = Encoding.UTF8.GetString(body);
            var deserializedEvent = JsonSerializer.Deserialize<T>(serializedBody)!;
            Console.WriteLine($"--> Received: {typeof(T).Name}");
            using (var scope = _scopeFactory.CreateScope())
            {
                var eventHandlers = scope.ServiceProvider.GetService<IEventHandlers>();
                switch (deserializedEvent)
                {
                    case CheckoutEvent checkoutEvent:
                        eventHandlers.Handle(checkoutEvent);
                        break;
                    case OrderConfirmEvent orderConfirmEvent:
                        eventHandlers.Handle(orderConfirmEvent);
                        break;
                    case ItemReservationFailedEvent reservationFailedEvent:
                        eventHandlers.Handle(reservationFailedEvent);
                        break;
                }
            }
        };
        _channel.BasicConsume(queueToConsume, true, eventConsumer);
    }
}