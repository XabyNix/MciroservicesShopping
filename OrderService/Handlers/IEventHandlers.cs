using Common.Models;

namespace OrderService.Handlers;

public interface IEventHandlers
{
    void Handle(CheckoutEvent checkoutEvent);
    void Handle(OrderConfirmEvent orderConfirmEvent);
    void Handle(ItemReservationFailedEvent reservationFailedEvent);
}