using Common.Models;
using PaymentService.AsyncMessage;

namespace PaymentService.Handlers;

public class PaymentHandler : IPaymentHandler
{
    private readonly IEventProducer _eventProducer;

    public PaymentHandler(IEventProducer eventProducer)
    {
        _eventProducer = eventProducer;
    }

    public async Task Handle(ItemsReservedEvent orderCreatedEvent)
    {
        try
        {
            await Process();

            _eventProducer.PublishEvent(new OrderConfirmEvent(orderCreatedEvent.OrderId));
        }
        catch (Exception e)
        {
            _eventProducer.PublishEvent(new PaymentRejectedEvent(orderCreatedEvent.OrderId));
            Console.WriteLine(e.Message);
        }
    }

    private async Task Process()
    {
        await Task.Delay(1000);
        //throw new Exception("Payment Refused");
    }
}