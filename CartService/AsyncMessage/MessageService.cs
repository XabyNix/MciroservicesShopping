using System.Text;
using System.Text.Json;
using CartService.AsyncMessage;
using Common.Models;
using RabbitMQ.Client;

namespace CartService;

public class MessageService : IMessageService
{
    private readonly IModel _channel;
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;

    public MessageService(IConfiguration configuration)
    {
        _configuration = configuration;
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
            Console.WriteLine("--> Connected to MessageBus");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void PublishMessage(CheckoutEvent message)
    {
        var serializedMessage = JsonSerializer.Serialize(message);
        if (_connection.IsOpen)
        {
            Send(serializedMessage);
            Console.WriteLine($"--> Event {nameof(message)} sent");
        }
        else
        {
            Console.WriteLine("--> MessageBus closed");
        }
    }

    private void Send(string serializedMessage)
    {
        var body = Encoding.UTF8.GetBytes(serializedMessage);
        _channel.BasicPublish("order", "cart.checkout", null, body);
    }
}