using CartService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CartService.Models;
using System.Text.Json;
using CartService.Profiles;
using AutoMapper;
using Google.Protobuf;

namespace CartService;

[Route("api/cart")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICartRepository _cartRepository;
    private readonly HttpClient _client;
    private readonly IMessageService _messageService;
    private readonly Services.CartService _catalogService;

    public CartController(ICartRepository cartRepository, IHttpClientFactory client, IMapper mapper, IMessageService messageService)
    {
        _mapper = mapper;
        _cartRepository = cartRepository;
        _client = client.CreateClient();
        _messageService = messageService;
        _catalogService = new Services.CartService();
    }


    [HttpGet]
    public ActionResult<IEnumerable<Cart>> GetCarts()
    {
        var carts = _cartRepository.GetCarts();
        if (carts == null)
        {
            return NotFound();
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

    [HttpPost("items")]
    public async Task<ActionResult<CartItem>> AddItemToCart(CartItemRequest requestItem)
    {
        if (!ModelState.IsValid) return BadRequest();

        try
        {
            var catalogItem = await _catalogService.GetItem(requestItem.CartId);
            
            CartItemResponse returnItem = new()
            {
                ItemId = new Guid(catalogItem.ItemId),
                CartId = requestItem.CartId,
                Name = catalogItem.ProductName,
                Price = catalogItem.Price,
                Quantity = requestItem.Quantity
            };
            
            return Ok(returnItem);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.InnerException);
        }

        return Empty;
    }

    [HttpGet("items")]
    public IActionResult GetItemFromCart(Guid cartId)
    {
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
