using System.Text;
using RabbitMQ.Client;

namespace CartService;

public class MessageService : IMessageService
{
    private readonly ConnectionFactory factory;
    private readonly IConnection _connection;

    public MessageService()
    {
        factory = new ConnectionFactory
        {
            Uri = new Uri("amqps://b-670600e9-2e2d-4f9d-9be6-6c409e3dd5d9.mq.eu-west-1.amazonaws.com:5671"),
            UserName = "orazio",
            Password = "ciaociao0011"
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
