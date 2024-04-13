using Common.Models;

namespace PaymentService.AsyncMessage;

public interface IEventProducer
{
    public void PublishEvent<T>(T eventToSend) where T : BasicEvent;
}