using DarkComics.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Models.Entity
{
    public class PostComment : BaseEntity
    {
        public int? NewsId { get; set; }
        public News News { get; set; }
        public int? CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
