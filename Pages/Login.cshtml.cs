using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using CosmicWorksTest2.Models;
using Microsoft.Azure.Cosmos;
using CosmicWorksTest2.Services;

namespace CosmicWorksTest2.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ICosmosService _cosmosService;

        //[BindProperty]
        //public CosmosUser CosmosUser { get; set; }

        public LoginModel(ICosmosService cosmosService)
        {
            _cosmosService = cosmosService;
        }
        [BindProperty]
        public string username { get; set; }
        [BindProperty]
        public string password { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            CosmosUser user = _cosmosService.GetUserByUsername(username).Result;

            // Verify the password. 
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.password))
            {
                // Authentication succeeded. You can set a session or cookie to remember the user's authentication.
                // Typically, you would implement user sessions or identity-based authentication here.

                return RedirectToPage("/Index"); // Redirect to the protected page (e.g., the homepage).
            }
            else
            {
                // Authentication failed.
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }
    }
}
