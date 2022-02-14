using DarkComics.DAL;
using DarkComics.Models.Entity;
using DarkComics.ViewComponents;
using DarkComics.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Controllers
{
    public class SearchController : Controller
    {
        private readonly DarkComicDbContext _context;
        public SearchController(DarkComicDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [AutoValidateAntiforgeryTokenAttribute]
        public IActionResult Search(SearchViewModel searchViewModel)
        {
            if (string.IsNullOrEmpty(searchViewModel.Name))
            {
                return NotFound();
            }

            SearchViewModel search = new SearchViewModel
            {
                Products = _context.Products.Include(p => p.ComicDetail).ThenInclude(cd => cd.Serie).Include(p => p.ProductCharacters).
              ThenInclude(pc => pc.Character).Where(p => p.IsActive == true && p.Name.ToLower().Contains(searchViewModel.Name.ToLower())).ToList()
            };
            return View(search);
        }


        public IActionResult SearchNews(int? id, int pageIndex = 1, int pageSize = 4)
        {
            if (id == null)
            {
                return NotFound();
            }

            // List<News> news = _context.News.Include(n => n.CharacterNews).ThenInclude(cn => cn.Character).Include(n => n.TagNews).
            //ThenInclude(tn => tn.Tag).Where(n=>n.TagNews.Exists(t=>t.TagId == id)).OrderByDescending(n => n.Id).ToList();
            List<TagNews> tagNews = _context.TagNews.Include(tn => tn.Tag).Include(tn => tn.News).ThenInclude(n => n.CharacterNews).
                 Where(tn => tn.TagId == id).ToList();

            if (tagNews == null)
            {
                return NotFound();
            }

            PaginationViewModel<TagNews> paginationViewModel = new PaginationViewModel<TagNews>(tagNews, pageSize, pageIndex);
            
            return View(paginationViewModel);
        }

    }
}
