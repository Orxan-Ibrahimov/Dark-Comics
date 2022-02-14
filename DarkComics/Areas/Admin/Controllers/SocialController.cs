using DarkComics.DAL;
using DarkComics.Models.Entity;
using DarkComics.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DarkComics.Areas.Admin.Controllers
{
    [Area("admin")]
    public class SocialController : Controller
    {
        private readonly DarkComicDbContext _db;
        public SocialController(DarkComicDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            FooterViewModel footerViewModel = new FooterViewModel
            {
                SocialLinks = _db.SocialLinks.Include(s => s.Footer).ToList()
            };
            return View(footerViewModel);
        }

        public IActionResult Create()
        {
            FooterViewModel footerViewModel = new FooterViewModel
            {
                SocialLink = new Social(),
                Footers = _db.Footer.ToList(),
                FooterList = new List<SelectListItem>()
            };

            foreach (var footer in footerViewModel.Footers)
            {
                footerViewModel.FooterList.AddRange(new List<SelectListItem>{
                    new SelectListItem() { Text = footer.Title, Value = footer.Id.ToString() }
                });
            };
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

            _db.SocialLinks.Add(footerViewModel.SocialLink);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Update(int? id)
        {
            if (id == null)
                return NotFound();

            FooterViewModel footerViewModel = new FooterViewModel
            {
                SocialLink = _db.SocialLinks.FirstOrDefault(s => s.Id == id),
                Footers = _db.Footer.ToList(),
                FooterList = new List<SelectListItem>()
            };

            if (footerViewModel.SocialLink == null)
            {
                return BadRequest();
            }

            foreach (var footer in footerViewModel.Footers)
            {
                footerViewModel.FooterList.AddRange(new List<SelectListItem>{
                    new SelectListItem() { Text = footer.Title, Value = footer.Id.ToString() }
                });
            };

            

            return View(footerViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, FooterViewModel footerViewModel)
        {
            if (id == null || (id != footerViewModel.SocialLink.Id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return View(footerViewModel);

            _db.SocialLinks.Update(footerViewModel.SocialLink);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

       
        public IActionResult Remove(int? id)
        {
            if (id == null)
                return NotFound();

            Social social = _db.SocialLinks.Include(s => s.Footer).FirstOrDefault(s => s.Id == id);

            if (social == null)
            {
                return BadRequest();
            }

            _db.SocialLinks.Remove(social);           

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
