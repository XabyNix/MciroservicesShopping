using CatalogService.Repositories.Interfaces;
using Grpc.Core;

namespace CatalogService.Services;

public class CatalogService : Catalog.CatalogBase
{
    private readonly ICatalogRepository _repository;

    public CatalogService(ICatalogRepository repository)
    {
        _repository = repository;
    }

    public override Task<ItemResponse> GetItem(ItemRequest request, ServerCallContext context)
    {
        Console.WriteLine("Received: " + request.ItemId);
        if (request.ItemId == string.Empty)
            throw new RpcException(new Status(StatusCode.InvalidArgument,
                "Bro you have to insert the id cause its missing"));

        var itemGuid = new Guid(request.ItemId);
        var itemFound = _repository.GetCatalogItem(itemGuid);

        if (itemFound == null)
            throw new RpcException(new Status(StatusCode.NotFound,
                "Unable to find an item with the id:" + request.ItemId));
        var responseItem = new ItemResponse
        {
            ItemId = itemFound.Id.ToString(),
            Description = itemFound.Description,
            Price = itemFound.Price,
            ProductName = itemFound.ProductName
        };

        return Task.FromResult(responseItem);
    }

    public override Task<CheckItemResponse> CheckItems(CheckItemRequest request, ServerCallContext context)
    {
        var itemsIdsToCheck = request.Items.Select(i => new Guid(i)).ToList();

        var itemsExisting = _repository.GetItemsFromIdList(itemsIdsToCheck).ToList();

        var okCheck = itemsIdsToCheck.Count == itemsExisting.Count;

        return Task.FromResult(new CheckItemResponse { Ok = okCheck });
    }
}