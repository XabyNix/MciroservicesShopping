using Common.Protos;
using Grpc.Net.Client;

namespace CatalogService.Grpc.Services;

public class OrderService
{
    private readonly OrderServiceGrpc.OrderServiceGrpcClient _client;

    public OrderService(IConfiguration configuration)
    {
        var channel = GrpcChannel.ForAddress(configuration["GrpcServices:Order"] ??
                                             throw new NullReferenceException(
                                                 "CatalogService: Unable to connect to OrderService"));
        _client = new OrderServiceGrpc.OrderServiceGrpcClient(channel);
    }

    public async Task<OrderResponseGrpc> GetOrderDetails(Guid orderId)
    {
        return await _client.GetOrderDetailAsync(new OrderRequestGrpc { OrderId = orderId.ToString() });
    }

    public async Task<OrderItemResponseGrpc> GetOrderItems(Guid orderId)
    {
        var items = await _client.GetOrderItemsAsync(new OrderRequestGrpc { OrderId = orderId.ToString() });
        return items;
    }
}