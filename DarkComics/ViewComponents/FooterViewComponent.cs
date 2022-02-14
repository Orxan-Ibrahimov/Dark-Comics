using DarkComics.DAL;
using DarkComics.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly DarkComicDbContext _context;
        public FooterViewComponent(DarkComicDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            FooterViewModel footerViewModel = new FooterViewModel
            {
                Footer = _context.Footer.Include(f => f.SocialLinks).FirstOrDefault()
            };

            return View(await Task.FromResult(footerViewModel));
        }
    }
}
