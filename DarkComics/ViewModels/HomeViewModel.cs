using DarkComics.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.ViewModels
{
    public class HomeViewModel
    {
        public List<Product> Comics { get; set; }
        public List<Product> FilteringComics { get; set; }
        public List<Product> BestComics { get; set; }
        public List<Character> Characters { get; set; }
        public List<Serie> Series { get; set; }

    }
}
