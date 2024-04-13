using System.Text;
using System.Text.Json;
using Common.Models;
using RabbitMQ.Client;

namespace PaymentService.AsyncMessage;

public class EventProducer : IEventProducer
{
    private const string PaymentRejectedRoutingKey = "order.payment.rejected";
    private const string OrderConfirmRoutingKey = "order.confirm";
    private readonly IConfiguration _configuration;
    private IModel _channel;
    private IConnection _connection;

    public EventProducer(IConfiguration configuration)
    {
        _configuration = configuration;
        InitRabbitMq();
    }

    public void PublishEvent<T>(T eventToSend) where T : BasicEvent
    {
        if (_connection.IsOpen)
        {
            var serializedEvent = JsonSerializer.Serialize(eventToSend);
            switch (eventToSend)
            {
                case OrderConfirmEvent:
                    Send(serializedEvent, OrderConfirmRoutingKey);
                    Console.WriteLine($"Event {typeof(T).Name} sent");
                    break;
                case PaymentRejectedEvent:
                    Send(serializedEvent, PaymentRejectedRoutingKey);
                    Console.WriteLine($"Event {typeof(T).Name} sent");
                    break;
                default:
                    throw new Exception($"The type '{typeof(T).Name}' is not recognized in PublishEvent");
            }
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


    private void Send(string message, string routingKey)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish("order", routingKey, null, body);
    }
}