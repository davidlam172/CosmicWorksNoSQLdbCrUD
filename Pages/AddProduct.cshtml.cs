using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CosmicWorksTest2.Models; // Import the Product model
using CosmicWorksTest2.Services;

public class AddProductModel : PageModel
{
    private ICosmosService _cosmosService;

    public IEnumerable<Product>? Products { get; set; }

    public AddProductModel(ICosmosService cosmosService)
    {
        _cosmosService = cosmosService;
    }

    public async Task OnGetAsync()
    {
        Products ??= await _cosmosService.RetrieveActiveProductsAsync();
    }
}