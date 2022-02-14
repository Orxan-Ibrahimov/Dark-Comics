using DarkComics.DAL;
using DarkComics.Models.Entity;
using DarkComics.ViewModels.Basket;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DarkComics.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly DarkComicDbContext _context;
        public UserManager<AppUser> _userManager { get; }

        public HeaderViewComponent(DarkComicDbContext context, UserManager<AppUser> user)
        {
            _context = context;
            _userManager = user;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            BasketViewModel basketVM = new BasketViewModel
            {
                TotalCount = 0,
                TotalPrice = 0,
                ProductDetails = new List<BasketItemViewModel>(),
                User = _userManager
                
            };
            var cookie = HttpContext.Request.Cookies["basket"];

            if (cookie != null)
            {
                var tempList = JsonSerializer.Deserialize<List<BasketProduct>>(cookie);

                if (tempList.FirstOrDefault() != null)
                {
                    foreach (var temporaryProduct in tempList)
                    {
                        if (temporaryProduct != null)
                        {
                            var basketItem = _context.Products.FirstOrDefault(p => p.Id == temporaryProduct.Id && p.IsActive == true);

                            BasketItemViewModel basketItemViewModel = new BasketItemViewModel
                            {

                                Product = basketItem,
                                Count = temporaryProduct.Count
                            };
                            basketVM.ProductDetails.Add(basketItemViewModel);
                            basketVM.TotalCount++;
                            basketVM.TotalPrice += Convert.ToDecimal(basketItem.Price * basketItemViewModel.Count);
                        }
                    }
                }
            }

            return View(await Task.FromResult(basketVM));
        }

    }
}
