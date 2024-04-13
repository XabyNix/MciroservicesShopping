using Common.Models;
using Common.Models.Order;

namespace OrderService.Services;

public interface IOrderCreator
{

    Guid Create(CartDto receivedEvent);
    void RollbackOrder(Guid orderId);
    void Confirm(Guid orderId);

}