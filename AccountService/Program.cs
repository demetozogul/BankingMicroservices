

using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Shared;

var accountEvent = new AccountCreatedEvent
{
    AccountId= Guid.NewGuid().ToString(),
    CustomerName="Demeter",
    CustomerPhone="05376676767",
    CreatedAt=DateTime.Now,
};

//message turn to json
var json =JsonSerializer.Serialize(accountEvent);
var body = Encoding.UTF8.GetBytes(json);

//RabbitMQ connection
var factory = new ConnectionFactory(){HostName="localhost"};

using var connection=factory.CreateConnection();
using var channel = connection.CreateModel();

// Exchange Publish

channel.ExchangeDeclare(exchange: "account.exchange", type: "topic", durable: true);

//Message publish
channel.BasicPublish(
    exchange: "account.exchange",
    routingKey: "account.created",
    basicProperties: null,
    body: body
);

Console.WriteLine("Account Created event published:");
Console.WriteLine(json);

