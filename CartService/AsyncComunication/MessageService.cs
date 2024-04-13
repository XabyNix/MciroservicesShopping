using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CartService.DTO.InternalComunication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CartService;

public class MessageService : IMessageService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IConfiguration _configuration;

    public MessageService(IConfiguration configuration)
    {
        _configuration = configuration;
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQHost"],
            Port = int.Parse(_configuration["RabbitMQPort"]!),
        };
        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "cart", ExchangeType.Topic);
            Console.WriteLine("--> Connected to MessageBus");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        /*if (_connection == null || !_connection.IsOpen) _connection = factory.CreateConnection();*/
    }
    public void PublishMessage(PlaceOrderEvent message)
    {
        var serializedMessage = JsonSerializer.Serialize(message);
        if (_connection.IsOpen)
        {
            Console.WriteLine("--> MessageBus open, trying to send message...");
            Send(serializedMessage);
        }
        else
        {
            Console.WriteLine("--> MessageBus closed");
        }
    }
    
    private void Send(string serializedMessage)
    {
        var props = _channel.CreateBasicProperties();
        var body = Encoding.UTF8.GetBytes(serializedMessage);
        _channel.BasicPublish("cart", "cart.checkout", basicProperties: props, body);
        
        Console.WriteLine("--> Message sent");
    }
}
