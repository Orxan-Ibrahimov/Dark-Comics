using DarkComics.Models.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.ViewModels
{
    public class NewsViewModel
    {
        public List<News> NewsList { get; set; }
        public News News { get; set; }
        public AppUser AppUser { get; set; }
        public Comment Comment { get; set; }
        public PostComment PostComment { get; set; }
        public List<Tag> Tags { get; set; }
        public List<SelectListItem> TagList { get; set; }
        public List<Character> Characters { get; set; }
        public List<SelectListItem> CharacterList { get; set; }
        public List<int?> ChosenCharacters { get; set; }
        public List<int?> ChosenTags { get; set; }

        public string Image { get; set; }       
    }
}
