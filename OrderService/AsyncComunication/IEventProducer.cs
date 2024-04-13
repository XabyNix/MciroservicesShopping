using Common.Models;

namespace OrderService.AsyncComunication;

public interface IEventProducer
{
    public void PublishEvent(OrderCreatedEvent orderCreatedEvent);
}