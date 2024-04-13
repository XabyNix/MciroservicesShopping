using Common.Models;

namespace CatalogService.Handlers;

public interface IEventHandlers
{
    Task Handle(OrderCreatedEvent orderCreatedEvent);
    Task Handle(OrderConfirmEvent orderConfirmEvent);
    Task Handle(PaymentRejectedEvent paymentRejectedEvent);
}