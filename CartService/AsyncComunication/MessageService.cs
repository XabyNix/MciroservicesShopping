using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CartService;

public class MessageService : IMessageService
{
    private readonly IConnection _connection;

    public MessageService()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri("localhost:5671"),
        };
        if (_connection == null || !_connection.IsOpen) _connection = factory.CreateConnection();
    }


    public void Send(string message)
    {

        Console.WriteLine("--> Sending message");
        var channel = _connection.CreateModel();
        channel.ExchangeDeclare(exchange: "user", ExchangeType.Topic);
        var props = channel.CreateBasicProperties();

        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish("user", "cart.add", basicProperties: props, body);

        Console.WriteLine("--> Sent Message");

    }
}
