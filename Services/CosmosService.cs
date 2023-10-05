using CosmicWorksTest2.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CosmicWorksTest2.Services;

public class CosmosService : ICosmosService
{
    private readonly CosmosClient _client;

    public CosmosService()
    {
        _client = new CosmosClient(
            connectionString: "AccountEndpoint=https://dbaccountname.documents.azure.com:443/;AccountKey=yDcpXQRYGndF7pvVDtaOtrM7rAoJuSID7MOlYpCFBIkJ090Zg3LpoVLkabjbgFFgiqC6MxBiLuVCACDbRsuquw==;"
        );
    }
    private Container container
    {
        get => _client.GetDatabase("cosmicworks").GetContainer("products");
    }
    public async Task<IEnumerable<Product>> RetrieveAllProductsAsync()
    {
        var queryable = container.GetItemLinqQueryable<Product>();
        using FeedIterator<Product> feed = queryable
    //.Where(p => p.price < 2000m)
    .OrderByDescending(p => p.price)
    .ToFeedIterator();
        List<Product> results = new();
        while (feed.HasMoreResults)
        {
            var response = await feed.ReadNextAsync();
            foreach (Product item in response)
            {
                results.Add(item);
            }
        }
        return results;
    }
    public async Task<IEnumerable<Product>> RetrieveActiveProductsAsync()
    {
        string sql = """
SELECT
    p.id,
    p.categoryId,
    p.categoryName,
    p.sku,
    p.name,
    p.description,
    p.price,
    p.tags
FROM products p
""";
        //        string sql = """
        //SELECT
        //    p.id,
        //    p.categoryId,
        //    p.categoryName,
        //    p.sku,
        //    p.name,
        //    p.description,
        //    p.price,
        //    p.tags
        //FROM products p
        //JOIN t IN p.tags
        //WHERE t.name = @tagFilter
        //""";
        var query = new QueryDefinition(
        query: sql
    )
        .WithParameter("@tagFilter", "Tag-75");
        using FeedIterator<Product> feed = container.GetItemQueryIterator<Product>(
        queryDefinition: query
    );
        List<Product> results = new();

        while (feed.HasMoreResults)
        {
            FeedResponse<Product> response = await feed.ReadNextAsync();
            foreach (Product item in response)
            {
                results.Add(item);
            }
        }

        return results;
    }
    public async Task AddProductAsync(Product product)
    {
        try
        {
            // Generate a unique identifier for the product
            string productId = Guid.NewGuid().ToString();

            // Create a new instance of Product with the 'id' property set
            var newProduct = new Product(
            id: productId,
            categoryId: product.categoryId,
            categoryName: product.categoryName,
            sku: product.sku,
            name: product.name,
            description: product.description,
            price: product.price
            //id: "TESTID",
            //categoryId: "TEST",
            //categoryName: "TEST",
            //sku: "TEST",
            //name: "TEST",
            //description: "TEST",
            //price: 0
            );

        // Use the correct partition key value
        string partitionKey = newProduct.categoryId;

        // Add the new product to the CosmosDB container
        ItemResponse<Product> response = await container.CreateItemAsync(newProduct, new PartitionKey(partitionKey));
    }
    catch (Exception ex)
    {
        // Handle exceptions, log errors, or perform error handling as needed
        Console.WriteLine($"Error adding product: {ex.Message}");
        throw;
    }
    }

}
