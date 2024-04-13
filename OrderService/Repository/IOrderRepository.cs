using Common.Models.Order;
using OrderService.Models;

namespace OrderService.Repository;

public interface IOrderRepository
{
    
    Order CreateOrder(Order order);
    Order GetOrder(Guid orderId);
    IEnumerable<Order> GetOrders();
    IEnumerable<Order> GetOrdersByUser(Guid userId);
    void RemoveOrder(Guid orderId);
    void ConfirmOrderStatus(Guid orderId);
}