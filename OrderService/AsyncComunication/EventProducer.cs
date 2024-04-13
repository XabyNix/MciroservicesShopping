using System.Text;
using System.Text.Json;
using Common.Models;
using RabbitMQ.Client;

namespace OrderService.AsyncComunication;

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

    public void PublishEvent(OrderCreatedEvent orderCreatedEvent)
    {
        if (_connection.IsOpen)
        {
            var serializedEvent = JsonSerializer.Serialize(orderCreatedEvent);
            Send(serializedEvent);
            Console.WriteLine($"Event {nameof(orderCreatedEvent)} sent");
        }
        else
        {
            Console.WriteLine("Connection closed cannot send event");
        }
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
        _channel.ExchangeDeclare("order", ExchangeType.Topic);
    }

    private void Send(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish("order", "order.created", null, body);
    }
}