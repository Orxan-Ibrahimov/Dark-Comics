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
    public class NewsViewComponent : ViewComponent
    {
        private readonly DarkComicDbContext _context;
        public NewsViewComponent(DarkComicDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            NewsViewModel newsViewModel = new NewsViewModel
            {
                NewsList = _context.News.Include(n => n.CharacterNews).ThenInclude(cn => cn.Character).Include(n => n.TagNews).
              ThenInclude(tn => tn.Tag).ToList()
            };

           
            return View(await Task.FromResult(newsViewModel));
        }
    }
}
