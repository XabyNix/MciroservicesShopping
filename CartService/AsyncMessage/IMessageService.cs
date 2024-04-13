using Common.Models;

namespace CartService.AsyncMessage;

public interface IMessageService
{
    public void PublishMessage(CheckoutEvent message);
}
