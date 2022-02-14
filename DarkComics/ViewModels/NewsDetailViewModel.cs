﻿using DarkComics.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.ViewModels
{
    public class NewsDetailViewModel
    {
        public News News { get; set; }
        public List<PostComment> PostComments { get; set; }

    }
}
