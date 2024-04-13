using Common.Protos;
using Grpc.Core;
using OrderService.Repository;

namespace OrderService.Grpc;

public class GrpcOrderService : OrderServiceGrpc.OrderServiceGrpcBase
{
    private readonly IOrderRepository _orderRepository;

    public GrpcOrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public override Task<OrderResponseGrpc> GetOrderDetail(OrderRequestGrpc request, ServerCallContext context)
    {
        var order = _orderRepository.GetOrder(new Guid(request.OrderId));

        //mapping into Grpc model
        var orderResponse = new OrderResponseGrpc
        {
            OrderId = order.OrderId.ToString(),
            UserId = order.UserId.ToString(),
            Total = order.Total,
            Status = order.Status.ToString(),
            Items =
            {
                order.Items.Select(i => new OrderItemGrpc
                {
                    OrderId = i.OrderId.ToString(),
                    ProductId = i.ProductId.ToString(),
                    Description = i.Description,
                    Name = i.Name,
                    Price = i.Price
                })
            }
        };
        return Task.FromResult(orderResponse);
    }

    public override Task<OrderItemResponseGrpc> GetOrderItems(OrderRequestGrpc request, ServerCallContext context)
    {
        var order = _orderRepository.GetOrder(new Guid(request.OrderId));
        var items = new OrderItemResponseGrpc
        {
            OrderItems =
            {
                order.Items.Select(i => new OrderItemGrpc
                {
                    OrderId = i.OrderId.ToString(),
                    ProductId = i.ProductId.ToString(),
                    Description = i.Description ?? string.Empty,
                    Name = i.Name,
                    Price = i.Price,
                    Quantity = i.Quantity
                })
            }
        };

        Console.WriteLine($"Items from order {order.OrderId} are {order.Items.Count()}");
        return Task.FromResult(items);
    }
}