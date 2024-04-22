using System;
using System.Collections.Generic;
using CatalogService.Models;
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
    public IActionResult AddItem([FromBody] Item item)
    {
        if (item == null) return BadRequest(item);
        var exists = _catalog.ItemExists(item.Id);
        if (exists) return Conflict();
        _catalog.AddItem(item);

        return CreatedAtRoute("GetCatalogItem", new { id = item.Id }, item);
    }

    [HttpPut]
    public IActionResult UpdateItem(Item item)
    {
        if (item == null) return BadRequest();
        _catalog.UpdateItem(item);
        return CreatedAtRoute("GetCatalogItem", new { id = item.Id }, item);
    }
}