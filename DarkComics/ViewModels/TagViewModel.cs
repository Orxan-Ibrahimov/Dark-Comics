using DarkComics.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.ViewModels
{
    public class TagViewModel
    {
        public List<Tag> Tags { get; set; }
        public Tag Tag { get; set; }
    }
}
