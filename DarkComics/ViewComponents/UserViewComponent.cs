using DarkComics.DAL;
using DarkComics.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.ViewComponents
{
    public class UserViewComponent : ViewComponent
    {
        private readonly DarkComicDbContext _context;
        public UserViewComponent(DarkComicDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            UserViewModel userViewModel = new UserViewModel
            {
                User = _context.Users.FirstOrDefault(u=>u.UserName == User.Identity.Name)
            };

            return View(await Task.FromResult(userViewModel));
        }
    }
}
