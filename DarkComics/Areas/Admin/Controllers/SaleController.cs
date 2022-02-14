using DarkComics.DAL;
using DarkComics.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Areas.Admin.Controllers
{
    [Area("admin")]
    public class SaleController : Controller
    {
        private readonly DarkComicDbContext _db;
        public SaleController(DarkComicDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            SaleViewModel saleViewModel = new SaleViewModel
            {
                Sales = _db.Sales.Include(s => s.SaleItems).ThenInclude(si => si.Product).ToList()
            };
            return View(saleViewModel);
        }

        public IActionResult Detail(int? id)
        {
            SaleViewModel saleViewModel = new SaleViewModel
            {
                Sale = _db.Sales.Include(s => s.SaleItems).ThenInclude(si => si.Product).ThenInclude(p=>p.ComicDetail)
                .ThenInclude(cd=>cd.Serie).FirstOrDefault(s=>s.Id == id)
            };
            return View(saleViewModel);
        }
    }
}
