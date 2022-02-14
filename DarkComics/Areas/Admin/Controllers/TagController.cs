using DarkComics.DAL;
using DarkComics.Models.Entity;
using DarkComics.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly DarkComicDbContext _db;
        public TagController(DarkComicDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            TagViewModel tagViewModel = new TagViewModel
            {
                Tags = _db.Tags.Include(c => c.TagNews).ThenInclude(tn=>tn.News).ToList()
            };

            return View(tagViewModel);
        }

        public ActionResult Create()
        {
            TagViewModel tagViewModel = new TagViewModel
            {
                Tag = new Tag()
            };

            return View(tagViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ActionResult Create(TagViewModel tagViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(tagViewModel);
            }

            _db.Tags.Add(tagViewModel.Tag);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Remove(int? id)
        {
            if (id == null)
                return NotFound();

            Tag tag = _db.Tags.Include(c => c.TagNews).ThenInclude(tn => tn.News).FirstOrDefault(c => c.Id == id);

            if (tag == null)
            {
                return NotFound();
            }

            _db.Tags.Remove(tag);

            foreach (var tagNews in tag.TagNews)
            {
                _db.TagNews.Remove(tagNews);
            }

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TagViewModel tagViewModel = new TagViewModel
            {
                Tag = _db.Tags.FirstOrDefault(t => t.Id == id)
            };
            if (tagViewModel.Tag == null)
            {
                return BadRequest();
            }

            return View(tagViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, TagViewModel tagViewModel)
        {
            if (id == null || (id != tagViewModel.Tag.Id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return View(tagViewModel);

            _db.Tags.Update(tagViewModel.Tag);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
