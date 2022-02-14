using DarkComics.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DarkComics.Models.Entity
{
    public class Contact : BaseEntity
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]       
        public string Message { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
