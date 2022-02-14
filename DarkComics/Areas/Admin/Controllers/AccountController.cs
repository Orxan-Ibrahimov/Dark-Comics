using DarkComics.Helpers.Enums;
using DarkComics.Models.Entity;
using DarkComics.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Areas.Admin.Controllers
{
    [Area("admin")]
    public class AccountController : Controller
    {
        public UserManager<AppUser> _userManager { get; }
        public SignInManager<AppUser> _signInManager { get; }
        public RoleManager<IdentityRole> _userRole { get; }
        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> userRole, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _userRole = userRole;
            _signInManager = signInManager;
            //_db = db;
        }
        public async Task<IActionResult> Register()
        {
            await CreateAsync();
            AdminRegisterViewModel admin = new AdminRegisterViewModel();
            admin.Roles = new List<RoleViewModel>();

            foreach (var role in Enum.GetNames(typeof(Role)))
            {
                admin.Roles.Add(new RoleViewModel { Name = role });
            };

            return View(admin);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(AdminRegisterViewModel register)
        {
            AdminRegisterViewModel admin = new AdminRegisterViewModel();
            admin.Roles = new List<RoleViewModel>();

            foreach (var role in Enum.GetNames(typeof(Role)))
            {
                admin.Roles.Add(new RoleViewModel { Name = role });
            }

            if (!ModelState.IsValid)
            {
                return View(admin);
            }
            var dbUser = await _userManager.FindByNameAsync(register.Username);
            if (dbUser != null)
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
                return View(admin);
            }

            await _userManager.AddToRoleAsync(user, register.Role);

            return RedirectToAction(nameof(Login), "Account");
        }

        public async Task CreateAsync()
        {
            await _userRole.CreateAsync(new IdentityRole(Role.SuperAdmin.ToString()));
            await _userRole.CreateAsync(new IdentityRole(Role.Admin.ToString()));
            await _userRole.CreateAsync(new IdentityRole(Role.User.ToString()));
        }
        public IActionResult Login()
        {
            AdminLoginViewModel adminLogin = new AdminLoginViewModel();
            return View(adminLogin);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(AdminLoginViewModel adminLogin)
        {
            if (!ModelState.IsValid)
            {
                return View(adminLogin);
            }

            var user = await _userManager.FindByNameAsync(adminLogin.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password is not correct");
                return View(adminLogin);
            }
            if ((await _userManager.GetRolesAsync(user))[0] == Role.SuperAdmin.ToString() ||
                (await _userManager.GetRolesAsync(user))[0] == Role.Admin.ToString())
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, adminLogin.Password, false, false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Username or Password is not correct");
                    return View(adminLogin);
                }
                return RedirectToAction(nameof(Index), "Character");
            }
            else
            {
                ModelState.AddModelError("", "Username or Password is not correct");
                return View(adminLogin);
            }
        }
        public async Task<IActionResult> LogOutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Character");
        }
    }
}
