using DarkComics.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Models.Entity
{
    public class CharacterNews : BaseEntity
    {
        public int? CharacterId { get; set; }
        public Character Character { get; set; }
        public int? NewsId { get; set; }
        public News News { get; set; }
    }
}
