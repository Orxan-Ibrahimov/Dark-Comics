using DarkComics.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Models.Entity
{
    public class Comment : BaseEntity
    {
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }       
        public List<PostComment> PostComments { get; set; }
    }
}
