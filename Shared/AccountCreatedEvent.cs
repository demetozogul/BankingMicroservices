using Microsoft.VisualBasic;

namespace Shared;

public class AccountCreatedEvent
{
public string? AccountId{get; set;}
public string? CustomerName {get;set;}
public string? CustomerPhone {get;set;}
public DateTime CreatedAt{get;set;}
}
