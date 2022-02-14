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
    public class ComicDetail : BaseEntity
    {
        
        public string Backface { get; set; }
        [Required,NotMapped]
        public IFormFile BackfacePhoto { get; set; }
        public bool IsCover { get; set; }
        public int? PageCount { get; set; }       
        public int? SerieId { get; set; }
        //public int? IsActive { get; set; }
        public Serie Serie { get; set; }
        public List<ReadingComic> ReadingComics { get; set; }
        public List<Product> Products { get; set; }

    }
}
