using Common.Models;

namespace PaymentService.Handlers;

public interface IPaymentHandler
{
    Task Handle(ItemsReservedEvent orderCreatedEvent);
}