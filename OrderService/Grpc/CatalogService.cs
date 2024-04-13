using Grpc.Net.Client;

namespace OrderService.Grpc;

public class CatalogService
{
    private readonly IConfiguration _configuration;
    private readonly Catalog.CatalogClient _client;

    public CatalogService(IConfiguration configuration)
    {
        _configuration = configuration;
        
        var channel = GrpcChannel.ForAddress(_configuration["CatalogServiceGrpc"]!);
        _client = new Catalog.CatalogClient(channel);
        
    }

    public async Task<CheckItemResponse> CheckItemExistence(CheckItemRequest request)
    {
        var response = await _client.CheckItemsAsync(request);
        return response;
    }
}