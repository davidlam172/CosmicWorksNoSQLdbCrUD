using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CosmicWorksTest2.Pages.Private;

public class PrivatePage1Model : PageModel
{
    public string Message { get; private set; }

    public void OnGet()
    {
        Message = "A private page inside the Private folder.";
    }
}