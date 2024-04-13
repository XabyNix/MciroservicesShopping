using OrderService.Database;
using OrderService.DTO;
using OrderService.Grpc;
using OrderService.Models;

namespace OrderService.Services;

public class OrderProcess : IOrderProcess
{
    private readonly IConfiguration _configurations;
    private readonly CatalogService _catalogService;

    public OrderProcess(IConfiguration configurations, CatalogService catalogService)
    {
        _catalogService = catalogService;
        _configurations = configurations;
    }

    public void Validate(CheckoutEvent receivedEvent)
    {
        /*var ids = ExtractIdsFromEventAsString(receivedEvent);
        var request = new CheckItemRequest() { Items = {ids} };
        var isItemsValid = await _catalogService.CheckItemExistence(request);*/
        var order = new Order()
        {   
            Items = receivedEvent.OrderItems,
            status = Order.OrderStatus.Pending,
            Total = receivedEvent.Total,
        };
        
        MyDatabase.Push(CalculateTotal(order));

    }

    private static Order CalculateTotal(Order order)
    {
        if (order.DiscountPerc != 0)
        {
            var total = order.Items.Sum(i => i.Price);
            order.Total = (order.DiscountPerc / total) * 100;
        }
        return order;

    }
    
    
    /*private static IEnumerable<string> ExtractIdsFromEventAsString(CheckoutEvent eventBody)
    {
        return eventBody.OrderItems.Select(i => i.ProductId.ToString());
    }*/
    
}