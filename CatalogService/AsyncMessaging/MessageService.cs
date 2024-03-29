using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CatalogService.AsyncMessaging;

public class MessageService : IHostedService
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


    public void Recive()
    {

        var channel = _connection.CreateModel();
        channel.ExchangeDeclare(exchange: "user", ExchangeType.Topic);

        var queue = channel.QueueDeclare().QueueName;

        channel.QueueBind(queue, "user", "cart.add");
        System.Console.WriteLine("--> Queue name: " + queue);
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            System.Console.WriteLine($"--> Message recived: {message}");
        };

        channel.BasicConsume(queue, autoAck: true, consumer);

    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Recive();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        System.Console.WriteLine("--> Shuttingdown from StopAsync");
        return Task.CompletedTask;
    }
}
