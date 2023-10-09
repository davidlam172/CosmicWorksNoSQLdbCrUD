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

    public void OnGet()
    {
        // This method handles the initial GET request
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _cosmosService.EditProductAsync(Product);
        return RedirectToPage("/Products");
    }
}
