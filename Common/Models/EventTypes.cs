namespace Common.Models;

public enum EventTypes
{
    CheckoutEvent,
    OrderCreated,
    ItemsReserved,
    
    PaymentConfirmed,
    PaymentRejected,
    OrderConfirmed,
    OrderRejected
}