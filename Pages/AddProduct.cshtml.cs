using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CosmicWorksTest2.Models;
using CosmicWorksTest2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        // This method handles the initial GET request
    }

    public async Task<IActionResult> OnPostAsync()
    {
        //if (ModelState.IsValid)
        //{
            // Call your CosmosService to add the product to the database
            // Example:
            await _cosmosService.AddProductAsync(Product);

            // Redirect to a confirmation page or the product list page
            return RedirectToPage("/Products");
        //}

        // If the model state is not valid, return to the form page
        //return Page();
    }
}
