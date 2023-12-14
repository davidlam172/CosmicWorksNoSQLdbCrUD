using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CosmicWorksTest2.Models;
using CosmicWorksTest2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
[Authorize]
public class AddProductModel : PageModel
{
    private readonly ICosmosService _cosmosService;

    public AddProductModel(ICosmosService cosmosService)
    {
        _cosmosService = cosmosService;
    }

    [BindProperty]
    public Product Product { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
            await _cosmosService.AddProductAsync(Product);
            return RedirectToPage("/Products");
    }
}