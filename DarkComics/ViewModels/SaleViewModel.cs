using DarkComics.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.ViewModels
{
    public class SaleViewModel
    {
        public List<Sale> Sales { get; set; }
        public Sale Sale { get; set; }
    }
}
