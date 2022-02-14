using DarkComics.Models.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.ViewModels
{
    public class FooterViewModel
    {
        public Footer Footer { get; set; }
        public List<Footer> Footers { get; set; }
        public List<SelectListItem> FooterList { get; set; }
        public Social SocialLink { get; set; }
        public List<Social> SocialLinks { get; set; }
        public Contact Contact { get; set; }
    }
}
