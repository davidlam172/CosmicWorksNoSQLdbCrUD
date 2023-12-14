using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using CosmicWorksTest2.Models;
using Microsoft.Azure.Cosmos;
using CosmicWorksTest2.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace CosmicWorksTest2.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ICosmosService _cosmosService;
        //private readonly TokenService _tokenService;

        public LoginModel(ICosmosService cosmosService)
        {
            _cosmosService = cosmosService;
            LoggedInSuccessful = true;
            //_tokenService = tokenService;
        }
        [BindProperty]
        public string username { get; set; }
        [BindProperty]
        public string password { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }
        public bool LoggedInSuccessful { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            CosmosUser user = _cosmosService.GetUserByUsername(username).Result;

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                //var token = _tokenService.GenerateJwtToken(username);
                //Response.Headers.Add("Authorization", $"Bearer {token}");
                return RedirectToPage("/index");
            }
            else
            {
                LoggedInSuccessful = false;
                // Authentication failed.
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                //return Unauthorized();
                return Page();
            }
        }
    }
}
