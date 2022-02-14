using DarkComics.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Models.Entity
{
    public class CharacterPower : BaseEntity
    {
        public Power Power { get; set; }
        public int? PowerId { get; set; }
        public Character Character { get; set; }
        public int? CharacterId { get; set; }
    }
}
