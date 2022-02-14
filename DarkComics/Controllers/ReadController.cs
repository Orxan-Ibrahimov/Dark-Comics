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
               Comics = _context.Products.Include(p=>p.ComicDetail).ThenInclude(cd=>cd.Serie).Include(p=>p.ProductCharacters).
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
            ComicDetail comicDetail = _context.ComicDetails.Include(cd => cd.ReadingComics).Include(cd => cd.Serie).Include(cd => cd.Products).
                ThenInclude(p => p.ProductCharacters).ThenInclude(p => p.Character).FirstOrDefault(cd => cd.Id == id);
         

            if (comicDetail == null)
            {
                return NotFound();
            }

            ReadViewModel readViewModel = new ReadViewModel
            {
                ComicDetail = comicDetail
            };
            return View(readViewModel);
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
