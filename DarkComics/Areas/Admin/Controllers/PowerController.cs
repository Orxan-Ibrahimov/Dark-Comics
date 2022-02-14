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
    public class PowerController : Controller
    {
        private readonly DarkComicDbContext _db;
        public PowerController(DarkComicDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            PowerViewModel powerViewModel = new PowerViewModel
            {
                Powers = _db.Powers.ToList()
            };

            return View(powerViewModel);
        }

        public ActionResult Create()
        {
            PowerViewModel power = new PowerViewModel
            {
                Power = new Power()
            };
            return View(power);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ActionResult Create(PowerViewModel powerViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(powerViewModel);
            }

            _db.Powers.Add(powerViewModel.Power);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult MakeActiveOrDeactive(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Power power = _db.Powers.FirstOrDefault(p => p.Id == id);

            if (power == null)
            {
                return NotFound();
            }

            if (power.IsActive == true)
                power.IsActive = false;
            else
                power.IsActive = true;

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PowerViewModel powerViewModel = new PowerViewModel
            {
                Power = _db.Powers.FirstOrDefault(p => p.Id == id)
        };
            if (powerViewModel.Power == null)
            {
                return BadRequest();
            }

            return View(powerViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, PowerViewModel powerViewModel)
        {
            if (id == null || (id != powerViewModel.Power.Id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return View(powerViewModel);

            _db.Powers.Update(powerViewModel.Power);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
