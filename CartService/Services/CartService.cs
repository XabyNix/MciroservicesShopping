using Grpc.Net.Client;

namespace CartService.Services;

public class CartService
{
    private readonly Catalog.CatalogClient _client;
    public CartService()
    {
        var channel = GrpcChannel.ForAddress("https://localhost:5001");
        _client = new Catalog.CatalogClient(channel);
    }

    public async Task<ItemResponse> GetItem(Guid id)
    {
        var reply = await _client.GetItemAsync(new ItemRequest() { ItemId = "3fa85f64-5717-4562-b3fc-2c963f66afa6" });
        return reply;
    }
}