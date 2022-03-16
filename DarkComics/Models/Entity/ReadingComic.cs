using DarkComics.Models.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Models.Entity
{
    public class ReadingComic:BaseEntity
    {
        [Required, NotMapped]
        public List<IFormFile> Photos { get; set; }
        [BindNever]
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }

        public ComicDetail ComicDetail { get; set; }
        public int? ComicDetailId { get; set; }
    }
}
