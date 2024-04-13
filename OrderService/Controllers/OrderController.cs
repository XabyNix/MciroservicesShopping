using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models.DTO;
using OrderService.Repository;

namespace OrderService.Controllers;

[Route("api/order")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;

    public OrderController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    [HttpGet("{id:Guid}")]
    public IActionResult GetOrder(Guid id)
    {
        var order = _orderRepository.GetOrder(id);
        if (order == null) return NotFound();
        return Ok(order);
    }
    
    [HttpGet]
    public IActionResult GetOrders()
    {
        var orders = _orderRepository.GetOrders();
        if (orders == null) return NotFound();

        var ordersDto = orders.Select(o => new OrderDto()
        {
            OrderId = o.OrderId,
            UserId = o.UserId,
            Status = o.Status,
            Total = o.Total,
            Items = o.Items.Select(i => new OrderItemDto()
            {
                OrderId = i.OrderId,
                ProductId = i.ProductId,
                Description = i.Description,
                Name = i.Name,
                Price = i.Price,
            })
        });
        
        
        return Ok(ordersDto);
    }
    
    [HttpGet("user/{userId:guid}")]
    public IActionResult GetOrderByUser(Guid userId)
    {
        var orders = _orderRepository.GetOrdersByUser(userId);
        if (orders == null) return NotFound();
        return Ok(orders);
    }
    
}