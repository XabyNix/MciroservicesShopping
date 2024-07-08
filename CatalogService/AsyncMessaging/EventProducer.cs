using System.Text;
using System.Text.Json;
using Common.Models;
using RabbitMQ.Client;

namespace CatalogService.AsyncMessaging;

public class EventProducer : IEventProducer
{
    private readonly IConfiguration _configuration;
    private IModel _channel;
    private IConnection _connection;

    public EventProducer(IConfiguration configuration)
    {
        _configuration = configuration;
        InitRabbitMq();
    }

    public void PublishEvent<T>(T itemsReservedEvent) where T : BasicEvent
    {
        if (_connection.IsOpen)
        {
            var serializedEvent = JsonSerializer.Serialize(itemsReservedEvent);
            switch (itemsReservedEvent)
            {
                case ItemsReservedEvent:
                    Send(serializedEvent, "order.reserved");
                    break;
                case ItemReservationFailedEvent:
                    Send(serializedEvent, "order.reserved.error");
                    break;
                default:
                    throw new Exception($"The type '{nameof(T)}' is not recognized in PublishEvent");
            }

            Console.WriteLine($"Event {typeof(T).Name} sent");
        }
        else
        {
            Console.WriteLine("Connection closed cannot send event");
        }
    }

    private void InitRabbitMq()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_configuration["Aws:RabbitMQHost"]),
                UserName = _configuration["Aws:Username"],
                Password = _configuration["Aws:Password"]
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare("order", ExchangeType.Topic);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void Send(string message, string routingKey)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish("order", routingKey, null, body);
    }
}