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
    }

    [BindProperty]
    public Product Product { get; set; }

    [BindProperty]
    public string NewCategoryId { get; set; } // New category ID
    public void OnGet()
    {
        // handles the initial GET request
    }

    public string ExistingCategoryId { get; set; }
    public async Task<IActionResult> OnPostAsync()
    {

        await _cosmosService.EditProductAsync(Product, NewCategoryId);
        return RedirectToPage("/Products");
    }
}
