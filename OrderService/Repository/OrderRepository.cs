using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;

namespace OrderService.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    public Order CreateOrder(Order order)
    {
        var createdOrder = _context.Order.Add(order);

        _context.SaveChanges();
        return createdOrder.Entity;
    }

    public Order GetOrder(Guid orderId)
    {
        var order = _context.Order.Include(o => o.Items).FirstOrDefault(o => o.OrderId == orderId);
        return order;
    }

    public IEnumerable<Order> GetOrders()
    {
        var orders = _context.Order.Include(o => o.Items);
        return orders;
    }

    public IEnumerable<Order> GetOrdersByUser(Guid userId)
    {
        return _context.Order.Where(o => o.UserId == userId)
            .Include(o => o.Items);
    }

    public void RemoveOrder(Guid orderId)
    {
        var orderToRemove = _context.Order.FirstOrDefault(o => o.OrderId == orderId);
        _context.Order.Remove(orderToRemove);
        _context.SaveChangesAsync();
    }

    public void ConfirmOrderStatus(Guid orderId)
    {
        var order = _context.Order.Single(o => o.OrderId == orderId);
        order.Status = OrderStatus.Confirmed;
        _context.Order.Update(order);
        _context.SaveChanges();
    }
}