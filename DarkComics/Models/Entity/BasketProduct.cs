using DarkComics.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Models.Entity
{
    public class BasketProduct:BaseEntity
    {
        public int? Count { get; set; }
    }
}
