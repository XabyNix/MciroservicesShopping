using CartService.Models;
using CartService.Models.Dto;
using CartService.Repositories.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Controllers;

[Route("api/items")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly Services.CartService _catalogService;
    private readonly IItemRepository _itemRepository;

    public ItemController(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
        _catalogService = new Services.CartService(); //gRPC for calling catalog service
    }

    [HttpPost]
    public async Task<ActionResult<CartItem>> AddItemToCart([FromBody] CartItemRequest requestItem)
    {
        var catalogItem = await _catalogService.GetItem(requestItem.ItemId);


        for (var i = 0; i < requestItem.Quantity; i++)
        {
            var cartItem = new CartItem
            {
                ProductId = new Guid(catalogItem.ItemId),
                CartId = requestItem.CartId,
                Name = catalogItem.ProductName,
                Price = catalogItem.Price,
                Description = catalogItem.Description,
                Quantity = requestItem.Quantity
            };
            await _itemRepository.AddItemToCart(cartItem);
        }

        var returnItem = new CartItemDto
        {
            ProductId = new Guid(catalogItem.ItemId),
            CartId = requestItem.CartId,
            Name = catalogItem.ProductName,
            Price = catalogItem.Price,
            Quantity = requestItem.Quantity
        };
        CreatedAtRoute("GetCart", new { returnItem.CartId }, returnItem);
        return Ok(returnItem);
    }


    [HttpDelete]
    public IActionResult RemoveItemFromCart(Guid itemId, Guid cartId)
    {
        var item = _itemRepository.RemoveItemFromCart(itemId, cartId);

        CartItem itemDto = new()
        {
            CartId = item.CartId,
            ProductId = item.ProductId,
            Price = item.Price,
            Name = item.Name,
            Quantity = item.Quantity
        };
        return Ok(itemDto);
    }
}