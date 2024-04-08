using CartService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CartService.Models;

namespace CartService.Controllers;

[Route("api/cart")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartRepository _cartRepository;
    //private readonly IMessageService _messageService;
    private readonly Services.CartService _catalogService;

    public CartController(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
        //_messageService = messageService;
        _catalogService = new Services.CartService(); //gRPC for calling catalog service
    }
    
        /*---------------Endpoints---------------*/

    [HttpGet]
    public ActionResult<IEnumerable<Cart>> GetCarts()
    {
        var carts = _cartRepository.GetCarts();
        if (carts == null)
        {
            return NoContent();
        }
        return Ok(carts);
    }

    [HttpGet("{id:guid}", Name = "GetCart")]
    public ActionResult<Cart> GetCart(Guid id)
    {
        var cart = _cartRepository.GetCart(id);
        if (cart == null)
        {
            return NotFound();
        }
        return Ok(cart);
    }

    [HttpGet("user/{id:guid}")]
    public ActionResult<Cart> GetCartByUser(Guid id)
    {
        var cart = _cartRepository.GetCartByUser(id);
        if (cart == null)
        {
            return NotFound();
        }
        return Ok(cart);
    }

    [HttpPost]
    public IActionResult AddCart([FromBody] Cart cart)
    {
        if (cart == null) return BadRequest();
        _cartRepository.AddCart(cart);
        return CreatedAtRoute("GetCart", new { cart.Id }, cart);
    }

    [HttpPut]
    public IActionResult UpdateCart([FromBody] Cart cart)
    {
        _cartRepository.UpdateCart(cart);
        return CreatedAtRoute("GetCart", new { cart.Id }, cart);
    }

    [HttpPost("item")]
    public async Task<ActionResult<CartItem>> AddItemToCart(CartItemRequest requestItem)
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
            };
            await _cartRepository.AddItemToCart(cartItem);
        }
        
        var returnItem = new CartItemResponse()
        {
            ItemId = new Guid(catalogItem.ItemId),
            CartId = requestItem.CartId,
            Name = catalogItem.ProductName,
            Price = catalogItem.Price,
            Quantity = requestItem.Quantity
        };
        
        return Ok(returnItem);
    }
    

    [HttpGet("item")]
    public IActionResult GetItemFromCart(Guid cartId)
    {
        if (cartId == Guid.Empty) return BadRequest("Id cannot be empty");
        var items = _cartRepository.GetItemFromCart(cartId);
        if (items is null) return NotFound();
        return Ok(items);
    }

    [HttpDelete]
    public IActionResult RemoveItemFromCart(Guid ItemId, Guid CartId)
    {
        var item = _cartRepository.RemoveItemFromCart(ItemId, CartId);

        CartItem itemDTO = new()
        {
            CartId = item.CartId,
            ProductId = item.ProductId,
            Price = item.Price,
            Name = item.Name,
            Quantity = item.Quantity,
        };
        return Ok(itemDTO);
    }


}
