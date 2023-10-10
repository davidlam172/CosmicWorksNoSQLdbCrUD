using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CosmicWorksTest2.Models;
using CosmicWorksTest2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class EditProductModel : PageModel
{
    private readonly ICosmosService _cosmosService;

    public EditProductModel(ICosmosService cosmosService)
    {
        _cosmosService = cosmosService;
        ProductExists = true;
    }

    [BindProperty]
    public Product Product { get; set; }

    [BindProperty]
    public string NewCategoryId { get; set; }
    public void OnGet()
    {
        // handles the initial GET request
    }

    public bool ProductExists { get; set; }
    public async Task<IActionResult> OnPostAsync()
    {
        ProductExists = await _cosmosService.CheckProductExistsAsync(Product.id, Product.categoryId);
        if (ProductExists)
        {
            await _cosmosService.EditProductAsync(Product, NewCategoryId);
            return RedirectToPage("/Products");
        }
        else
        {
            return Page();
        }
    }
}
