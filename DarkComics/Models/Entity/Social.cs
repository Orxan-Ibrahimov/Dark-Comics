using DarkComics.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Models.Entity
{
    public class Social : BaseEntity
    {
        public string Icon { get; set; }
        public string Link { get; set; }
        public int? FooterId { get; set; }
        public Footer Footer { get; set; }
    }
}
