using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

//ExchangeDeclare
channel.ExchangeDeclare(
    exchange: "account.exchange",
    type: "topic",
    durable: true
);

//QueueDeclare
channel.QueueDeclare("audit.account.created", durable: true, exclusive: false, autoDelete: false);

//QueueBind (with wildcard)
channel.QueueBind(
    queue: "audit.account.created",
    exchange: "account.exchange",
    routingKey: "account.*"
);

//Consumer
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var json = Encoding.UTF8.GetString(body);

    var ev = JsonSerializer.Deserialize<AccountCreatedEvent>(json);

    Console.WriteLine("Audit Service:");
    Console.WriteLine($"Log → New account created: {ev?.AccountId} at {ev?.CreatedAt}");
};

// event listener
channel.BasicConsume(queue: "audit.account.created", autoAck: true, consumer: consumer);

Console.WriteLine("Audit Service listening...");
Console.ReadLine();

