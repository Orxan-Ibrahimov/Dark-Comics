using DarkComics.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Models.Entity
{
    public class Toy : BaseEntity
    {
        [Required, StringLength(maximumLength: 100)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> DeactivatedDate { get; set; }
        public List<ToyCharacter> ToyCharacters { get; set; }
    }
}
