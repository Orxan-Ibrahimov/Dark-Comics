using DarkComics.Helpers.Enums;
using DarkComics.Models.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.ViewModels.Basket
{
    public class BasketViewModel
    {
        public UserManager<AppUser> User { get; set; }
        public AppUser AppUser { get; set; }
        public List<BasketItemViewModel> ProductDetails { get; set; }
        public int? TotalCount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
