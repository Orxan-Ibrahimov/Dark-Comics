using DarkComics.Helpers.Enums;
using DarkComics.Models.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DarkComics.ViewModels
{
    public class ComicViewModel
    {
        public List<Product> RandomComics { get; set; }
        public List<Product> Comics { get; set; }
        public List<Serie> Series { get; set; }
        public Product Comic { get; set; }
        public Serie Serie { get; set; }
        public List<Character> Characters { get; set; }
        public List<SelectListItem> CharacterList { get; set; }
        public List<SelectListItem> TeamOrNot { get; set; }
        public List<int?> ChosenCharacters { get; set; }
        public List<SelectListItem> SerieList { get; set; }
        public String Cover { get; set; }
        public String Backface { get; set; }


    }
}
