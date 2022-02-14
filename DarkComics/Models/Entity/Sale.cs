using DarkComics.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Models.Entity
{
    public class Sale : BaseEntity
    {
        [Required]
        public string Client { get; set; }
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Home { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<SaleItem> SaleItems { get; set; }
    }
}
