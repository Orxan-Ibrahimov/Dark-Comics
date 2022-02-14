using DarkComics.DAL;
using DarkComics.Models.Entity;
using DarkComics.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Controllers
{
    public class NewsController : Controller
    {
        private readonly DarkComicDbContext _context;
        public NewsController(DarkComicDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? id, int pageIndex = 1, int pageSize = 4)
        {
            List<News> news = _context.News.Include(n => n.CharacterNews).ThenInclude(cn => cn.Character).Include(n => n.TagNews).
             ThenInclude(tn => tn.Tag).Include(n => n.PostComments).ThenInclude(pc => pc.Comment).ThenInclude(c => c.User).OrderByDescending(n => n.Id).ToList();


            if (news == null)
            {
                return NotFound();
            }

            PaginationViewModel<News> paginationViewModel = new PaginationViewModel<News>(news, pageSize, pageIndex);


            return View(paginationViewModel);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            News news = _context.News.Include(n => n.CharacterNews).ThenInclude(cn => cn.Character).Include(n => n.TagNews).
                ThenInclude(tn => tn.Tag).Include(n=>n.PostComments).ThenInclude(pc=>pc.Comment).ThenInclude(c => c.User).FirstOrDefault(n=>n.Id == id);

            if (news == null)
            {
                return NotFound();
            }

            NewsViewModel newsViewModel = new NewsViewModel
            {
                News = news,
                AppUser = _context.Users.FirstOrDefault(u=>u.UserName == User.Identity.Name)
            };
            return View(newsViewModel);
        }

        public IActionResult MessageSend(int? id, string message)
        {
            if (id == null || string.IsNullOrEmpty(message))
            {
                return View();
            }

            Comment comment = new Comment();
            comment.Message = message;
            comment.User = _context.Users.FirstOrDefault(u=>u.UserName == User.Identity.Name);  

            _context.Comments.Add(comment);

            PostComment postComment = new PostComment
            {
                Comment = comment,
                NewsId = id
            };
            _context.PostComments.Add(postComment);
            _context.SaveChanges();

            NewsViewModel news = new NewsViewModel
            {               
                PostComment = postComment
            };
            return PartialView("_MessageSend", news);
        }

    }
}
