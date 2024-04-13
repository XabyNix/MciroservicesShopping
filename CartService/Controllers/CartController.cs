using System.Data;
using CartService.AsyncMessage;
using CartService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CartService.Models;
using CartService.Models.Dto;
using Common.Models;
using Common.Models.Order;

namespace CartService.Controllers;

[Route("api/cart")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartRepository _cartRepository;
    private readonly IMessageService _messageService;

    public CartController(ICartRepository cartRepository, IMessageService messageService)
    {
        _cartRepository = cartRepository;
        _messageService = messageService;
        
    }
    
        /*---------------Endpoints---------------*/

    [HttpGet]
    public ActionResult<IEnumerable<CartResponseDto>> GetCarts()
    {
        var carts = _cartRepository.GetCarts();

        var cartDto = carts.Select(c => new CartResponseDto()
        {
            Id = c.Id,
            CreatedBy = c.CreatedBy,
            CreatedAt = c.CreatedAt,
            Items = c.Items,
        });
        
        return Ok(cartDto);
    }

    [HttpGet("{id:guid}", Name = "GetCart")]
    public ActionResult<Cart> GetCart(Guid id)
    {
        var cart = _cartRepository.GetCart(id);
        var cartDto = new CartResponseDto()
        {
            CreatedAt = cart.CreatedAt,
            CreatedBy = cart.CreatedBy,
            Items = cart.Items.Select(i=> new CartItemDto()
            {
                Name = i.Name,
                Price = i.Price,
                CartId = i.CartId,
                ProductId = i.ProductId, 
            }),
        };

        return Ok(cartDto);
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
    public IActionResult AddCart([FromBody] CartRequestDto cartRequest)
    {
        if (cartRequest.CreatedBy == Guid.Empty)
        {
            return BadRequest();
        }
        
        var cart = new Cart()
        {
            CreatedAt = DateTime.Now,
            CreatedBy = cartRequest.CreatedBy,
        };

        try
        {
            _cartRepository.AddCart(cart);
        }
        catch (DuplicateNameException e)
        {
            return Conflict(e.Message);
        }

        
        return CreatedAtRoute("GetCart", new { cart.Id }, new CartResponseDto());
    }
    

    [HttpGet("Checkout")]
    public IActionResult Checkout([FromQuery]Guid cartId)
    {
        if (cartId == Guid.Empty)
        {
            return BadRequest("You need to provide the <id> of the cart you want to checkout");
        }
        
        //Get the cart from cartService via repository call
        var cart = _cartRepository.GetCart(cartId);
        
        //Map into a DTO
        var cartDto = new CartDto()
        {
            CreatedAt = cart.CreatedAt,
            CreatedBy = cart.CreatedBy,
            Items = cart.Items.Select(i => new CartItemDto()
            {
                Name = i.Name,
                Price = i.Price,
                ProductId = i.ProductId,
                Description = i.Description,
            })
        };
        //Create the event to send into message queue
        var message = new CheckoutEvent(cartDto);

        _messageService.PublishMessage(message);
        return Ok("Order added to queue and will be processed!");
    }
    
    
    

}
