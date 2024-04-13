using Common.Models.Order;

namespace Common.Models;

public interface BasicEvent
{
}

//Orders
public record CheckoutEvent(CartDto Cart) : BasicEvent;

public record OrderCreatedEvent(Guid OrderId, IEnumerable<CartItemDto> Items) : BasicEvent;

public record OrderConfirmEvent(Guid OrderId) : BasicEvent;

//ItemsReservation
public record ItemsReservedEvent(Guid OrderId) : BasicEvent;

public record ItemReservationFailedEvent(Guid OrderId) : BasicEvent;

//Payments
public record PaymentSuccessfulEvent(Guid OrderId) : BasicEvent;

public record PaymentRejectedEvent(Guid OrderId) : BasicEvent;