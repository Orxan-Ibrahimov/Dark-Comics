using DarkComics.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Models.Entity
{
    public class TagNews : BaseEntity
    {
        public int? TagId { get; set; }
        public Tag Tag { get; set; }
        public int? NewsId { get; set; }
        public News News { get; set; }
    }
}
