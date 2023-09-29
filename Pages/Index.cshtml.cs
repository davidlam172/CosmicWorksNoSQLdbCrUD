﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using CosmicWorksTest2.Models;
using CosmicWorksTest2.Services;

namespace CosmicWorksTest2.Pages;

public class IndexPageModel : PageModel
{
    private readonly ICosmosService _cosmosService;

    public IEnumerable<Product>? Products { get; set; }

    public IndexPageModel(ICosmosService cosmosService)
    {
        _cosmosService = cosmosService;
    }

    public async Task OnGetAsync()
    {
        Products ??= await _cosmosService.RetrieveActiveProductsAsync();
    }
}