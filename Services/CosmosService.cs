using CosmicWorksTest2.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Reflection.Metadata;

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
            string productId = Guid.NewGuid().ToString();

            var newProduct = new Product(
            id: productId,
            categoryId: product.categoryId,
            categoryName: product.categoryName,
            sku: product.sku,
            name: product.name,
            description: product.description,
            price: product.price
            );

            string partitionKey = newProduct.categoryId;

            ItemResponse<Product> response = await container.CreateItemAsync(newProduct, new PartitionKey(partitionKey));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding product: {ex.Message}");
            throw;
        }
    }

    public async Task EditProductAsync(Product updatedProduct, String NewCategoryId)
    {
        try
        {
            ItemResponse<Product> existingProductResponse = await container.ReadItemAsync<Product>(
                partitionKey: new PartitionKey(updatedProduct.categoryId),
                id: updatedProduct.id.Replace(" ", "")
            );

            if (existingProductResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Product existingProduct = existingProductResponse.Resource;

                Product updated = new Product(
                    id: existingProduct.id,
                    categoryId: existingProduct.categoryId,
                    categoryName: updatedProduct.categoryName ?? existingProduct.categoryName,
                    sku: updatedProduct.sku ?? existingProduct.sku,
                    name: updatedProduct.name ?? existingProduct.name,
                    description: updatedProduct.description ?? existingProduct.description,
                    price: updatedProduct.price != -1 ? updatedProduct.price : existingProduct.price
                );


                ItemResponse<Product> response = await container.ReplaceItemAsync(
                    partitionKey: new PartitionKey(existingProduct.categoryId),
                    id: existingProduct.id,
                    item: updated
                );
            }
            else
            {
                Console.WriteLine("Product not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error editing product: {ex.Message}");
            throw;
        }
    }
    public async Task DeleteProductAsync(string ProductID, string CategoryID)
    {
        try
        {
            await container.DeleteItemAsync<Product>(ProductID.Replace(" ", ""), new PartitionKey(CategoryID));
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine("Item not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting item: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> CheckProductExistsAsync(string productId, string categoryId)
    {
        try
        {
            ItemResponse<Product> existingProductResponse = await container.ReadItemAsync<Product>(
                partitionKey: new PartitionKey(categoryId),
                id: productId.Replace(" ", "")
            );

            return existingProductResponse.StatusCode == System.Net.HttpStatusCode.OK;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking product existence: {ex.Message}");
            throw;
        }
    }




}
