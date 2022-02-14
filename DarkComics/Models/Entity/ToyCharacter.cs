
using DarkComics.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Models.Entity
{
    public class ToyCharacter : BaseEntity
    {
        public Character Character { get; set; }
        public int? CharacterId { get; set; }
        public Toy Toy { get; set; }
        public int? ToyId { get; set; }
    }
}
