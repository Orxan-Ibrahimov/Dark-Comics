using DarkComics.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Models.Entity
{
    public class City : BaseEntity
    {
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        public List<Character> Characters { get; set; }
    }
}
