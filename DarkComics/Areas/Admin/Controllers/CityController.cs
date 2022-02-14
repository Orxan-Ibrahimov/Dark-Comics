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
    public class CityController : Controller
    {
        private readonly DarkComicDbContext _db;
        public CityController(DarkComicDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            CityViewModel cityViewModel = new CityViewModel
            {
                Cities = _db.Cities.Include(c => c.Characters).ToList()
            };

            return View(cityViewModel);
        }

        public ActionResult Create()
        {
            CityViewModel city = new CityViewModel
            {
                City = new City()
            };
          
            return View(city);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ActionResult Create(CityViewModel cityViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(cityViewModel);
            }
            _db.Cities.Add(cityViewModel.City);
            _db.SaveChanges();
           
            return RedirectToAction("Index");
        }

        public ActionResult MakeActiveOrDeactive(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

          City city = _db.Cities.Include(c => c.Characters).FirstOrDefault(c => c.Id == id);

            if (city == null)
            {
                return NotFound();
            }

            if (city.IsActive == true)
                city.IsActive = false;
            else
                city.IsActive = true;

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CityViewModel cityViewModel = new CityViewModel
            {
                City = _db.Cities.FirstOrDefault(c=>c.Id == id)
            };
            if (cityViewModel.City == null)
            {
                return BadRequest();
            }        
      
            return View(cityViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, CityViewModel cityViewModel)
        {
            if (id == null || (id != cityViewModel.City.Id))
            {
                return NotFound();
            }
           
            if (!ModelState.IsValid)
                return View(cityViewModel);          

            _db.Cities.Update(cityViewModel.City);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


    }
}
