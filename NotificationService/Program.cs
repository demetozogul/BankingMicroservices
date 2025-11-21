using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

//ExchangeDeclare (idempotent)
channel.ExchangeDeclare(
    exchange: "account.exchange",
    type: "topic",
    durable: true
);
//QueueDeclare (idempotent)
channel.QueueDeclare("notification.account.created", durable: true, exclusive: false, autoDelete: false);

//QueueBind 
channel.QueueBind(
    queue: "notification.account.created",
    exchange: "account.exchange",
    routingKey: "account.created"
);

//consumer
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var json = Encoding.UTF8.GetString(body);

    var ev = JsonSerializer.Deserialize<AccountCreatedEvent>(json);

    Console.WriteLine("Notification Service:");
    Console.WriteLine($"SMS {ev?.CustomerPhone} - '{ev?.CustomerName}' hesabınız açıldı!");
};
// event listener
channel.BasicConsume(queue: "notification.account.created", autoAck: true, consumer: consumer);

Console.WriteLine("Notification Service listening...");
Console.ReadLine();
