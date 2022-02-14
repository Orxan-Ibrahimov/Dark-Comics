using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Models.Entity
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string Fullname { get; set; }
        [Required]
        public bool IsAgree { get; set; }
        public bool IsSubscriber { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public string Image { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
