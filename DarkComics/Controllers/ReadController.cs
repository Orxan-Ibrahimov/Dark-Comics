using DarkComics.DAL;
using DarkComics.Helpers.Enums;
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
    public class ReadController : Controller
    {
        private readonly DarkComicDbContext _context;
        public ReadController(DarkComicDbContext context)
        {
            _context = context;
        }
        // GET: ReadController
        public ActionResult Index()
        {
            ReadComicViewModel readComicViewModel = new ReadComicViewModel
            {
               Comics = _context.Products.Include(p=>p.ComicDetail).ThenInclude(cd=>cd.ReadingComics).Include(p=>p.ProductCharacters).
               ThenInclude(pc=>pc.Character).Where(p=>p.Category == Category.Comic && p.ComicDetail.IsCover == true && p.IsActive == true).ToList()
            };
            return View(readComicViewModel);
        }

        // GET: ReadController/Details/5
        public ActionResult Read(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product product = _context.Products.Include(cd => cd.ProductCharacters).Include(cd => cd.ComicDetail).
                ThenInclude(p => p.ReadingComics).Include(cd => cd.SaleItems).FirstOrDefault(cd => cd.Id == id);
         

            if (product == null)
            {
                return NotFound();
            }

            ReadComicViewModel readComicViewModel = new ReadComicViewModel
            {
                Comic = product
            };
            return View(readComicViewModel);
        }

        // GET: ReadController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReadController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReadController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReadController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReadController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReadController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
