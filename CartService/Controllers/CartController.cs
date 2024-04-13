using System.Data;
using CartService.DTO;
using CartService.DTO.InternalComunication;
using CartService.InternalComunication;
using CartService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CartService.Models;

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
    public ActionResult<IEnumerable<Cart>> GetCarts()
    {
        var carts = _cartRepository.GetCarts();
        
        return Ok(carts);
    }

    [HttpGet("{id:guid}", Name = "GetCart")]
    public ActionResult<Cart> GetCart(Guid id)
    {
        var cart = _cartRepository.GetCart(id);
        var itemsDto = cart.Items.Select(i => new CartItemResponse()
        {
            Name = i.Name,
            Price = i.Price,
            CartId = i.CartId,
            ProductId = i.ProductId,
        });
        var cartDto = new CartResponseDto()
        {
            CreatedAt = cart.CreatedAt,
            CreatedBy = cart.CreatedBy,
            Items = itemsDto.ToList(),
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
    public IActionResult AddCart([FromQuery] CartRequestDto cartRequest)
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
    

    [HttpGet("process")]
    public IActionResult Process([FromQuery]Guid cartId)
    {
        //If cartId is empty return a bad request with a message
        if (cartId == Guid.Empty)
        {
            return BadRequest("You need to provide the <id> of the cart you want to checkout");
        }
        
        //Get the cart from cartService via repository call
        var cart = _cartRepository.GetCart(cartId);
        
        //Map into a DTO
        var itemsDto = cart.Items.Select(i => new CartItemResponse()
        {
            Name = i.Name,
            Price = i.Price,
            CartId = i.CartId,
            ProductId = i.ProductId,
        });
        
        //Create the event to send into message queue
        var message = new PlaceOrderEvent()
        {
            //TODO:Add Discount 
            CommandType = CommandTypes.PlaceOrder,
            OrderItems = itemsDto,
        };

        _messageService.PublishMessage(message);
        return Ok("Order added to queue and will be processed!");
    }
    
    
    

}
