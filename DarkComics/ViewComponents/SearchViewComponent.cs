using DarkComics.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.ViewComponents
{
    public class SearchViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            SearchViewModel searchViewModel = new SearchViewModel ();

            return View(await Task.FromResult(searchViewModel));
        }
    }
}
