using Common.Models;

namespace CatalogService.AsyncMessaging;

public interface IEventProducer
{
    public void PublishEvent<T>(T orderCreatedEvent) where T : BasicEvent;
}