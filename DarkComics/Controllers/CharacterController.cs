using DarkComics.DAL;
using DarkComics.Models.Entity;
using DarkComics.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Controllers
{
    public class CharacterController : Controller
    {
        private readonly DarkComicDbContext _context;
        public CharacterController(DarkComicDbContext context)
        {
            _context = context;
        }
        // GET: CharacterController
        public ActionResult Index()
        {
            CharacterViewModel characterViewModel = new CharacterViewModel
            {
                Characters = _context.Characters.Include(c => c.City).Where(c => c.IsActive == true).ToList()
        };

            int count = characterViewModel.Characters.Count();

            HttpContext.Response.Cookies.Append("characters", count.ToString());

            return View(characterViewModel);
        }

        // GET: CharacterController/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CharacterViewModel characterView = new CharacterViewModel
            {
                Character = _context.Characters.Include(c => c.City).Include(c => c.ProductCharacters).ThenInclude(c => c.Product).ThenInclude(p => p.ComicDetail).
                Include(c => c.CharacterPowers).ThenInclude(cp => cp.Power).Include(c => c.ToyCharacters).ThenInclude(tc => tc.Toy).
                Include(c => c.ToyCharacters).ThenInclude(tc => tc.Toy).Include(c => c.CharacterNews).ThenInclude(cn => cn.News).Where(c => c.IsActive == true).
                FirstOrDefault(c => c.Id == id)
            };
            if (characterView.Character == null)
            {
                return NotFound();
            }


            return View(characterView);
        }

        // GET: CharacterController/Create
        public ActionResult Search(int? id,int pageIndex = 1,int pageSize = 4)
        {

            if (id == null)
            {
                return NotFound();
            }

            Character character = _context.Characters.Include(c => c.ToyCharacters).ThenInclude(tc => tc.Toy).Include(c => c.CharacterPowers).
                ThenInclude(cp => cp.Power).Include(c => c.City).Include(c => c.ProductCharacters).ThenInclude(pc => pc.Product).
                Include(c => c.CharacterNews).ThenInclude(cn => cn.News).FirstOrDefault(c => c.Id == id);
           
            var products = character.ProductCharacters.OrderByDescending(p => p.ProductId).ToList();
            if (products == null)
            {
                return NotFound();
            }

            PaginationViewModel<ProductCharacter> paginationViewModel = new PaginationViewModel<ProductCharacter>(products, pageSize, pageIndex);
          
            return View(paginationViewModel);
        }

        public IActionResult LoadCharacters(int skip, int take)
        {
            CharacterViewModel characterViewModel = new CharacterViewModel
            {
                Characters = _context.Characters.Where(c => c.IsActive == true).OrderBy(c => c.Id).Skip(skip).Take(take).ToList()
            };


            return View("_LoadCharacters", characterViewModel);
        }
    }
}
