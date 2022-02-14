using DarkComics.Models.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Models.Entity
{
    public class News : BaseEntity
    {
        [Required(ErrorMessage ="You must Write News Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "You must Write News Body")]
        public string Text { get; set; }
        [Required(ErrorMessage = "You must Write News Short Description")]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "You must Write Blogger")]
        public string Blogger { get; set; }
        public string Image { get; set; }
        [Required,NotMapped]
        public IFormFile Photo { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public List<CharacterNews> CharacterNews { get; set; }
        public List<TagNews> TagNews { get; set; }
        public List<PostComment> PostComments { get; set; }

    }
}
