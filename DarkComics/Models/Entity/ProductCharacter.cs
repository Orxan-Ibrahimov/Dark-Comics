using DarkComics.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Models.Entity
{
    public class ProductCharacter : BaseEntity
    {
        public Product Product { get; set; }
        public int? ProductId { get; set; }
        public Character Character { get; set; }
        public int? CharacterId { get; set; }
    }
}
