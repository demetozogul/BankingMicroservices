Banking Microservices with RabbitMQ (Event-Driven Architecture)

This project demonstrates a simple event-driven microservices architecture using RabbitMQ.
Three independent services communicate only through events, not direct API calls.

Architecture

Account Service (Publisher) → Creates accounts and publishes account.created events

Notification Service (Consumer) → Listens for account creation events and simulates sending notifications

Audit Service (Consumer) → Listens to all account.* events and logs them

Topic Exchange structure:

account.exchange (topic)

 ├── notification.account.created   (routing: account.created)
 
 └── audit.account.created          (routing: account.*)


RabbitMQ Setup
Exchange

account.exchange → topic

Queues
Queue	Routing Key
notification.account.created	account.created
audit.account.created	account.*

How to Run
1-Start RabbitMQ

UI: http://localhost:15672

(username: guest / password: guest)

2-Run the services

Each in separate terminals:

cd AccountService
dotnet run

cd NotificationService
dotnet run

cd AuditService
dotnet run

3-Once AccountService runs, it publishes an event

NotificationService and AuditService consume and process the event.

Shared Event Model
public class AccountCreatedEvent
{
    public string AccountId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPhone { get; set; }
    public DateTime CreatedAt { get; set; }
}

