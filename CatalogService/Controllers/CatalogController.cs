using CatalogService.Models;
using CatalogService.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Repositories.Interfaces;

[Route("api/catalog")]
[ApiController]
public class CatalogController : ControllerBase
{
    private readonly ICatalogRepository _catalog;

    public CatalogController(ICatalogRepository catalog)
    {
        _catalog = catalog;
    }


    [HttpGet]
    public ActionResult<IEnumerable<Item>> GetCatalog()
    {
        var fullCatalog = _catalog.GetCatalog();
        if (fullCatalog == null) return NotFound();
        Console.WriteLine("--> Getting Catalog");
        return Ok(fullCatalog);
    }


    [HttpGet("item/{id:guid}", Name = "GetCatalogItem")]
    public ActionResult<Item> GetItem(Guid id)
    {
        var item = _catalog.GetCatalogItem(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpGet("item/{name}")]
    public ActionResult<Item> GetCatalogItemByName(string name)
    {
        if (name == null) return BadRequest(name);

        var item = _catalog.GetItemsByName(name);

        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public IActionResult AddItem([FromForm] ItemDto itemRequest)
    {
        Console.WriteLine($"Received POST AddItem: {itemRequest}");
        if (itemRequest == null) return BadRequest(itemRequest);
        var item = new Item
        {
            Quantity = itemRequest.Quantity,
            Description = itemRequest.Description,
            Price = itemRequest.Price,
            ProductName = itemRequest.ProductName
        };

        var exists = _catalog.ItemExists(item.Id);
        if (exists) return Conflict();
        _catalog.AddItem(item);

        return CreatedAtRoute("GetCatalogItem", new { id = item.Id }, item);
    }

    [HttpDelete("delete")]
    public IActionResult RemoveItems([FromBody] IEnumerable<ItemsDeleteRequest> request)
    {
        var modelItems = request.Select(i => new Item
        {
            ProductName = i.ProductName,
            Description = i.Description,
            Price = i.Price,
            Id = i.Id
        });

        _catalog.RemoveItems(modelItems);
        _catalog.SaveChanges();
        Console.WriteLine("--> Items removed");
        return Ok();
    }

    [HttpPut]
    public IActionResult UpdateItem(Item item)
    {
        if (item == null) return BadRequest();
        _catalog.UpdateItem(item);
        return CreatedAtRoute("GetCatalogItem", new { id = item.Id }, item);
    }
}