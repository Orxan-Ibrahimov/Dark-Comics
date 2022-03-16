using DarkComics.Models.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.ViewModels
{
    public class ReadViewModel
    {
        public ComicDetail ComicDetail { get; set; }
        public ReadingComic Reading { get; set; }
        public int? ChosenComicId { get; set; }
        public List<SelectListItem> ComicList { get; set; }
        public List<Product> Comics { get; set; }
        
    }
}
