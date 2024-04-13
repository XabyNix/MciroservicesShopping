using Common.Models;
using OrderService.AsyncComunication;
using OrderService.Services;

namespace OrderService.Handlers;

public class EventHandlers : IEventHandlers
{
    private readonly IEventProducer _eventProducer;
    private readonly IOrderCreator _orderCreator;

    public EventHandlers(IOrderCreator orderCreator, IEventProducer eventProducer)
    {
        _orderCreator = orderCreator;
        _eventProducer = eventProducer;
    }

    public void Handle(CheckoutEvent checkoutEvent)
    {
        try
        {
            var orderId = _orderCreator.Create(checkoutEvent.Cart);
            Console.WriteLine($"--> Order {orderId} Created");
            _eventProducer.PublishEvent(new OrderCreatedEvent(orderId, checkoutEvent.Cart.Items));
        }
        catch (Exception e)
        {
            Console.WriteLine("--> Unable to create order" + e.Message);
            throw new Exception(e.Message, e.InnerException);
        }
    }

    public void Handle(OrderConfirmEvent orderConfirmEvent)
    {
        _orderCreator.Confirm(orderConfirmEvent.OrderId);
        Console.WriteLine($"--> Order {orderConfirmEvent.OrderId} confirmed");
    }

    public void Handle(ItemReservationFailedEvent reservationFailedEvent)
    {
        _orderCreator.RollbackOrder(reservationFailedEvent.OrderId);
        Console.WriteLine($"--> Order {reservationFailedEvent.OrderId} rejected");
    }
}