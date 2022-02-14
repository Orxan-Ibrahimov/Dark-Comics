using DarkComics.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Models.Entity
{
    public class Footer : BaseEntity
    {
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public List<Social> SocialLinks { get; set; }
    }
}
