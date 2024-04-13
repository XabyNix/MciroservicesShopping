using System;
using System.Collections.Generic;
using CatalogService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService;

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
        if (fullCatalog == null)
        {
            return NotFound();
        }
        System.Console.WriteLine("--> Getting Catalog");
        return Ok(fullCatalog);
    }


    [HttpGet("item/{id:guid}", Name = "GetCatalogItem")]
    public ActionResult<Item> GetItem(Guid id)
    {
        var item = _catalog.GetCatalogItem(id);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    /*[HttpGet("item/cart/{CartId:Guid}")]
    public ActionResult<IEnumerable<Item>> GetItemFromCart(Guid CartId)
    {
        var ItemsInsideCart = _catalog.GetItemFromCart(CartId);
        if (ItemsInsideCart is null)
        {
            return NotFound();
        }
        return Ok(ItemsInsideCart);
    }*/

    [HttpGet("item/{name}")]
    public ActionResult<Item> GetCatalogItemByName(string name)
    {
        if (name == null)
        {
            return BadRequest(name);
        }

        var item = _catalog.GetCatalogItemByName(name);

        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [HttpPost]
    public IActionResult AddItem([FromBody] Item item)
    {
        if (item == null)
        {
            return BadRequest(item);
        }
        bool exists = _catalog.ItemExists(item.Id);
        if (exists)
        {
            return Conflict();
        }
        _catalog.AddItem(item);

        return CreatedAtRoute("GetCatalogItem", new { id = item.Id }, item);

    }

    [HttpPut]
    public IActionResult UpdateItem(Item item)
    {
        if (item == null)
        {
            return BadRequest();
        }
        _catalog.UpdateItem(item);
        return CreatedAtRoute("GetCatalogItem", new { id = item.Id }, item);
    }




}
