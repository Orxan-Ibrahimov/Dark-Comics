using DarkComics.DAL;
using DarkComics.Helpers.Methods;
using DarkComics.Models.Entity;
using DarkComics.ViewModels;
using DarkComics.ViewModels.Basket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DarkComics.Controllers
{
    public class BasketController : Controller
    {
        private readonly DarkComicDbContext _context;
        public BasketController(DarkComicDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var cookie = HttpContext.Request.Cookies["basket"];
            var temporaryList = new List<BasketProduct>();
            if (!string.IsNullOrEmpty(cookie))
                temporaryList = JsonSerializer.Deserialize<List<BasketProduct>>(cookie);

            List<Product> products = _context.Products.Include(p => p.ComicDetail).ThenInclude(cd => cd.Serie).Include(p => p.ProductCharacters).
             ThenInclude(pc => pc.Character).Where(p => p.IsActive == true).ToList();
            BasketViewModel basketView = BasketMethod.ShowBasket(products, cookie);



            return View(basketView);
        }

        public IActionResult AddBasket(int id)
        {
            var dbProduct = _context.Products.ToList().FirstOrDefault(b => b.Id == id && b.IsActive == true);
            if (dbProduct == null)
            {
                return NotFound();
            }

            List<BasketProduct> temporaryList = new List<BasketProduct>();
            var myCookie = Request.Cookies["basket"];
            temporaryList = BasketMethod.AddBasket(dbProduct, myCookie);
            myCookie = JsonSerializer.Serialize(temporaryList);
            Response.Cookies.Append("basket", myCookie);


            // Show Cookie

            List<Product> products = _context.Products.Include(p => p.ComicDetail).ThenInclude(cd => cd.Serie).Include(p => p.ProductCharacters).
               ThenInclude(pc => pc.Character).Where(p => p.IsActive == true && p.Quantity > 0).ToList();
            BasketViewModel basketView = BasketMethod.ShowBasket(products, myCookie);

            return View("_Basket", basketView);

        }

        public IActionResult DeleteProduct(int id)
        {
            var dbProduct = _context.Products.ToList().FirstOrDefault(b => b.Id == id && b.IsActive == true);
            if (dbProduct == null)
            {
                return NotFound();
            }
            string cookie = Request.Cookies["basket"];
            var temporaryList = JsonSerializer.Deserialize<List<BasketProduct>>(cookie);
            var product = temporaryList.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                temporaryList.Remove(product);
            }

            Response.Cookies.Append("basket", JsonSerializer.Serialize<List<BasketProduct>>(temporaryList));

            List<Product> products = _context.Products.Include(p => p.ComicDetail).ThenInclude(cd => cd.Serie).Include(p => p.ProductCharacters).
             ThenInclude(pc => pc.Character).Where(p => p.IsActive == true).ToList();
            cookie = JsonSerializer.Serialize(temporaryList);
            BasketViewModel basketView = BasketMethod.ShowBasket(products, cookie);

            return View("_Basket", basketView);

        }

        public IActionResult DecreaseProduct(int id)
        {
            var dbProduct = _context.Products.ToList().FirstOrDefault(b => b.Id == id && b.IsActive == true);
            if (dbProduct == null)
            {
                return NotFound();
            }
            var cookie = Request.Cookies["basket"];
            var temporaryList = new List<BasketProduct>();

            if (!string.IsNullOrEmpty(cookie))
            {
                temporaryList = JsonSerializer.Deserialize<List<BasketProduct>>(cookie);
                if (temporaryList != null)
                {
                    var product = temporaryList.FirstOrDefault(p => p.Id == id);
                    if (product != null && product.Count > 0)
                    {
                        product.Count--;
                        Response.Cookies.Append("basket", JsonSerializer.Serialize(temporaryList));
                    }
                }

            }
            cookie = JsonSerializer.Serialize(temporaryList);
            List<Product> products = _context.Products.Include(p => p.ComicDetail).ThenInclude(cd => cd.Serie).Include(p => p.ProductCharacters).
              ThenInclude(pc => pc.Character).Where(p => p.IsActive == true).ToList();
            BasketViewModel basketView = BasketMethod.ShowBasket(products, cookie);

            return View("_Basket", basketView);

        }

        public IActionResult IncreaseProduct(int id)
        {
            var dbProduct = _context.Products.ToList().FirstOrDefault(b => b.Id == id && b.IsActive == true);
            if (dbProduct == null)
            {
                return NotFound();
            }
            var cookie = Request.Cookies["basket"];
            var temporaryList = new List<BasketProduct>();

            if (!string.IsNullOrEmpty(cookie))
            {
                temporaryList = JsonSerializer.Deserialize<List<BasketProduct>>(cookie);
                if (temporaryList != null)
                {
                    var product = temporaryList.FirstOrDefault(p => p.Id == id);
                    if (product != null && dbProduct.Quantity - product.Count > 0)
                    {
                        product.Count++;
                        Response.Cookies.Append("basket", JsonSerializer.Serialize(temporaryList));
                    }
                }

            }
            cookie = JsonSerializer.Serialize(temporaryList);
            List<Product> products = _context.Products.Include(p => p.ComicDetail).ThenInclude(cd => cd.Serie).Include(p => p.ProductCharacters).
              ThenInclude(pc => pc.Character).Where(p => p.IsActive == true).ToList();
            BasketViewModel basketView = BasketMethod.ShowBasket(products, cookie);

            return View("_Basket", basketView);
        }

        public IActionResult Order()
        {
            OrderViewModel order = new OrderViewModel();
            return View(order);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Order(OrderViewModel order)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

           var basket = JsonSerializer.Deserialize<List<BasketProduct>>(Request.Cookies["basket"]);
            order.SaleData.SaleItems = new List<SaleItem>();
            foreach (var basketItem in basket)
            {
                SaleItem saleItem = new SaleItem
                {
                    Product = _context.Products.Include(p=>p.ComicDetail).ThenInclude(cd=>cd.Serie).FirstOrDefault(p=>p.Id == basketItem.Id),                   
                    Count = basketItem.Count
                };
                saleItem.Price = saleItem.Product.Price - (saleItem.Product.Price * saleItem.Product.ComicDetail.Serie.Discount / 100);
                saleItem.Product.Quantity -= saleItem.Count;
                order.SaleData.SaleItems.Add(saleItem);
                _context.SaleItems.Add(saleItem);
                saleItem.Product.IsActive = false;
            }
            _context.Sales.Add(order.SaleData);

          
            _context.SaveChanges();
            Response.Cookies.Delete("basket");
            return RedirectToAction(nameof(Index),"Home");
        }
    }
}
