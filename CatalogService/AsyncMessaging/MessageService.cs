using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CatalogService.Models;
using Microsoft.Extensions.Hosting;
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
            HostName = "localhost",
            Port = 5671
        };

        if (_connection == null || !_connection.IsOpen) _connection = factory.CreateConnection();
    }


    private void Recive()
    {

        var channel = _connection.CreateModel();
        channel.ExchangeDeclare(exchange: "cart", ExchangeType.Topic);

        var queue = channel.QueueDeclare().QueueName;

        channel.QueueBind(queue, "cart", "cart.checkout");
        System.Console.WriteLine("--> Queue name: " + queue);
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            var message = JsonSerializer.Deserialize<Item>(body);
            System.Console.WriteLine($"--> Message received: {message}");
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
