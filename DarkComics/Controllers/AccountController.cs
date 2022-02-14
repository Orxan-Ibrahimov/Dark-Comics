using DarkComics.DAL;
using DarkComics.Helpers.Enums;
using DarkComics.Helpers.Methods;
using DarkComics.Models.Entity;
using DarkComics.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DarkComics.Controllers
{
    public class AccountController : Controller
    {
        private readonly DarkComicDbContext _context;
        public UserManager<AppUser> _userManager { get; }
        public SignInManager<AppUser> _signInManager { get; }
        public AccountController(DarkComicDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }                  

            Random rand = new Random(); 
            register.SecurityCode = rand.Next(99999,1000000).ToString();

            string data = JsonSerializer.Serialize(register);
            HttpContext.Response.Cookies.Append("user", data);
            string message = "Security code for registration";
            MailOpertions.SendMessage(register.Email, message,register.SecurityCode);            

            return View("CompleteRegister");
        }

        public IActionResult CompleteRegister()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CompleteRegister(SecurityCodeVM security)
        {
            if (!ModelState.IsValid)
            {
                return View(security);
            }

            RegisterViewModel register = JsonSerializer.Deserialize<RegisterViewModel>(Request.Cookies["user"]);            

            HttpContext.Response.Cookies.Delete("user");

            if (register == null)
                return NotFound();

            if (security.Code != register.SecurityCode)
                return BadRequest();

            AppUser user = new AppUser
            {
                Fullname = register.FullName,
                UserName = register.Username,
                Email = register.Email,
                Image = "default.jpg"
            };

            IdentityResult result = await _userManager.CreateAsync(user, register.Password);


            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View(register);
            }
            await _userManager.AddToRoleAsync(user, Role.User.ToString());

            return RedirectToAction("Index", "Home");
        }
       
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Comic",login);
            }

            var user = await _userManager.FindByNameAsync(login.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password is not correct");
            }
            var result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is not correct");
                return RedirectToAction("Index", "Comic", login);
            }
            return RedirectToAction("Index", "Comic");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();            
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string username,ChangePasswordViewModel changePassword)
        {
            AppUser user = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            IdentityResult result = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Subscribe()
        {

            AppUser user = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user.IsSubscriber)
                user.IsSubscriber = false;
            else
                user.IsSubscriber = true;

            _context.Users.Update(user);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
