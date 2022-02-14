using DarkComics.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.ViewModels
{
    public class SearchViewModel
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
