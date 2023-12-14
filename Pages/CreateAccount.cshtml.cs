using CosmicWorksTest2.Models;
using CosmicWorksTest2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CosmicWorksTest2.Pages
{
    public class CreateAccountModel : PageModel
    {
        private readonly ICosmosService _cosmosService;

        public CreateAccountModel(ICosmosService cosmosService)
        {
            _cosmosService = cosmosService;
            CreatedSuccess = true;
        }
        [BindProperty]
        public CosmosUser CosmosUser { get; set; }

        public void OnGet()
        {
        }
        public bool CreatedSuccess { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            bool success = await _cosmosService.CreateAccount(CosmosUser);
            if (success)
            {
                return RedirectToPage("/Login");
            }
            else
            {
                CreatedSuccess = false;
                return Page();
            }
        }
    }
}
