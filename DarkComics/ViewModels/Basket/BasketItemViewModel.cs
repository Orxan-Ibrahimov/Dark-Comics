using System;
using System.Collections.Generic;
using DarkComics.Models.Entity;
using System.Threading.Tasks;

namespace DarkComics.ViewModels.Basket
{
    public class BasketItemViewModel
    {
        public Product Product { get; set; }
        public int? Count { get; set; }
    }
}
