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
    public class Serie : BaseEntity
    {
        public string Name { get; set; }        
        public string Cover { get; set; }
        [Required, NotMapped]
        public IFormFile CoverPhoto { get; set; }
        public string Backface { get; set; }
        [Required, NotMapped]
        public IFormFile BackfacePhoto { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DeletedDate { get; set; }
        public bool IsTeam { get; set; }
        public int Discount { get; set; }
        public bool IsDeleted { get; set; }
        public List<ComicDetail> ComicDetails { get; set; }

    }
}
