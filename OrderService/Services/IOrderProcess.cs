using OrderService.DTO;

namespace OrderService.Services;

public interface IOrderProcess
{
    public void Validate(CheckoutEvent receivedEvent);
}