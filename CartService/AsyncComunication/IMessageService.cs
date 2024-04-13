using CartService.DTO.InternalComunication;
using CartService.InternalComunication;

namespace CartService;

public interface IMessageService
{
    public void PublishMessage(PlaceOrderEvent message);
}
