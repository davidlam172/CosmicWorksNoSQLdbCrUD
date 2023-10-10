using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CosmicWorksTest2.Models;
using CosmicWorksTest2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class DeleteProductModel : PageModel
{
    private readonly ICosmosService _cosmosService;

    public DeleteProductModel(ICosmosService cosmosService)
    {
        _cosmosService = cosmosService;
    }


    [BindProperty]
    public string CategoryID { get; set; }
    [BindProperty]
    public string ProductID { get; set; }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _cosmosService.DeleteProductAsync(ProductID, CategoryID);
        return RedirectToPage("/Products");
    }
}
