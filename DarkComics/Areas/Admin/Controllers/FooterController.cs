using DarkComics.DAL;
using DarkComics.Models.Entity;
using DarkComics.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Areas.Admin.Controllers
{
    [Area("admin")]
    public class FooterController : Controller
    {
        private readonly DarkComicDbContext _db;
        public FooterController(DarkComicDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            FooterViewModel footerViewModel = new FooterViewModel
            {
                Footers = _db.Footer.Include(f=>f.SocialLinks).ToList()
            };
            return View(footerViewModel);
        }

        public IActionResult Create()
        {
            FooterViewModel footerViewModel = new FooterViewModel
            {
                Footer = new Footer()
                //Footers = _db.Footer.ToList(),
                //FooterList = new List<SelectListItem>()
            };

            //foreach (var footer in footerViewModel.Footers)
            //{
            //    footerViewModel.FooterList.AddRange(new List<SelectListItem>{
            //        new SelectListItem() { Text = footer.Title, Value = footer.Id.ToString() }
            //    });
            //};
            return View(footerViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ActionResult Create(FooterViewModel footerViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(footerViewModel);
            }

            _db.Footer.Add(footerViewModel.Footer);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Update(int? id)
        {
            if (id == null)
                return NotFound();

            FooterViewModel footerViewModel = new FooterViewModel
            {
                Footer = new Footer()
                //Footers = _db.Footer.ToList(),
                //FooterList = new List<SelectListItem>()
            };

            if (footerViewModel.Footer == null)
            {
                return BadRequest();
            }

            //foreach (var footer in footerViewModel.Footers)
            //{
            //    footerViewModel.FooterList.AddRange(new List<SelectListItem>{
            //        new SelectListItem() { Text = footer.Title, Value = footer.Id.ToString() }
            //    });
            //};



            return View(footerViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, FooterViewModel footerViewModel)
        {
            if (id == null || (id != footerViewModel.Footer.Id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return View(footerViewModel);

            _db.Footer.Update(footerViewModel.Footer);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }       
    }
}
