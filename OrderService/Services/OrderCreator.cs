using Common.Models.Order;
using OrderService.Models;
using OrderService.Repository;

namespace OrderService.Services;

public class OrderCreator : IOrderCreator
{
    private readonly IOrderRepository _orderRepository;

    public OrderCreator(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public Guid Create(CartDto cart)
    {
        var orderToInsert = new Order()
        {
            Items = cart.Items.Select(i => new OrderItem()
            {
                ProductId = i.ProductId,
                Name = i.Name,
                Description = i.Description,
                Price = i.Price,
                Quantity = i.Quantity,
            }).ToList(),
            Status = OrderStatus.Pending,
            UserId = cart.CreatedBy,
            Total = cart.Items.Sum(i=> i.Price),
        };

        var createdOrder = _orderRepository.CreateOrder(orderToInsert);
        return createdOrder.OrderId;
    }

    public void RollbackOrder(Guid orderId)
    {
        _orderRepository.RemoveOrder(orderId);
    }

    public void Confirm(Guid orderId)
    {
        try
        {
            _orderRepository.ConfirmOrderStatus(orderId);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}