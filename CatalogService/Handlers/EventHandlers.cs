using CatalogService.AsyncMessaging;
using CatalogService.Grpc.Services;
using CatalogService.Models;
using CatalogService.Repositories.Interfaces;
using Common.Models;

namespace CatalogService.Handlers;

public class EventHandlers : IEventHandlers
{
    private readonly ICatalogRepository _catalogRepository;
    private readonly IEventProducer _eventProducer;
    private readonly OrderService _orderService;
    private readonly IReservedItemRepository _reservedItemRepository;

    public EventHandlers(IReservedItemRepository reservedItemRepository, ICatalogRepository catalogRepository,
        IEventProducer eventProducer,
        OrderService orderService)
    {
        _reservedItemRepository = reservedItemRepository;
        _catalogRepository = catalogRepository;
        _eventProducer = eventProducer;
        _orderService = orderService;
    }

    public async Task Handle(OrderCreatedEvent orderCreatedEvent)
    {
        try
        {
            var itemToReserve = orderCreatedEvent.Items.Select(i => new ReservedItem
            {
                ProductId = i.ProductId,
                QuantityReserved = i.Quantity
            });
            _reservedItemRepository.ReserveItems(itemToReserve);
            await _catalogRepository.SaveChanges();
            _eventProducer.PublishEvent(
                new ItemsReservedEvent(orderCreatedEvent.OrderId));
        }
        catch (Exception e)
        {
            _eventProducer.PublishEvent(
                new ItemReservationFailedEvent(orderCreatedEvent.OrderId));
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Handle(OrderConfirmEvent orderConfirmEvent)
    {
        try
        {
            var items = await _orderService.GetOrderItems(orderConfirmEvent.OrderId);

            //Remove the quantity from the catalog database
            var itemsProfiled = items.OrderItems.Select(i => new Item
            {
                Id = new Guid(i.ProductId),
                Quantity = i.Quantity,
                Description = i.Description,
                Price = i.Price,
                ProductName = i.Name
            });

            _catalogRepository.UpdateManyQuantity(itemsProfiled);

            //Remove the reserved items from the reserved database 
            var reservedItems = items.OrderItems.Select(i => new ReservedItem
            {
                ProductId = new Guid(i.ProductId),
                QuantityReserved = i.Quantity
            });
            _reservedItemRepository.FreeReservedItem(reservedItems);
            await _catalogRepository.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Handle(PaymentRejectedEvent paymentRejectedEvent)
    {
        try
        {
            var items = await _orderService.GetOrderItems(paymentRejectedEvent.OrderId);
            var reservedItems = items.OrderItems.Select(i => new ReservedItem
            {
                ProductId = new Guid(i.ProductId),
                QuantityReserved = i.Quantity
            });

            _reservedItemRepository.FreeReservedItem(reservedItems);
            await _catalogRepository.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}