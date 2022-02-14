using DarkComics.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Models.Entity
{
    public class Tag : BaseEntity
    {
        [Required(ErrorMessage = "You must Write tag's Title")]
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<TagNews> TagNews { get; set; }
    }
}
