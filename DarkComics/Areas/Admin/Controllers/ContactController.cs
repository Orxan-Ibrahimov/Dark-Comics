using DarkComics.DAL;
using DarkComics.Helpers.Methods;
using DarkComics.Models.Entity;
using DarkComics.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ContactController : Controller
    {
        private readonly DarkComicDbContext _db;
        public ContactController(DarkComicDbContext db)
        {
            _db = db; 
        }
        public IActionResult Index(int? id, int pageIndex = 1, int pageSize = 4)
        {         
            ContactViewModel contactViewModel = new ContactViewModel
            {
                Messages = _db.Contact.Where(c => c.IsActive).ToList()
            };

            PaginationViewModel<Contact> paginationViewModel = new PaginationViewModel<Contact>(contactViewModel.Messages, pageSize, pageIndex);


            return View(paginationViewModel);
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
                return NotFound();

            ContactViewModel contactViewModel = new ContactViewModel
            {
                Message = _db.Contact.Where(c => c.IsActive).FirstOrDefault(c=>c.Id == id)
            };

            if (contactViewModel.Message == null)
                return BadRequest();

            return View(contactViewModel);
        }

        public IActionResult RemoveMessage(int? id)
        {
            if (id == null)
                return NotFound();

            ContactViewModel contactViewModel = new ContactViewModel
            {
                Message = _db.Contact.Where(c => c.IsActive).FirstOrDefault(c => c.Id == id)
            };

            if (contactViewModel.Message == null)
                return BadRequest();

            contactViewModel.Message.IsActive = false;
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Answer(int? id)
        {
            if (id == null)
                return NotFound();

           Contact user = _db.Contact.Where(c => c.IsActive).FirstOrDefault(c => c.Id == id);

            if (user == null)
                return BadRequest();

            ContactViewModel contactViewModel = new ContactViewModel
            {
                Message = new Contact
                {
                    Email = user.Email
                }
            };            

            return View(contactViewModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Answer(ContactViewModel contactViewModel)
        {
            if (!ModelState.IsValid)
                return View(contactViewModel);

            MailOpertions.SendMessage(contactViewModel.Message.Email,contactViewModel.Message.Subject,contactViewModel.Message.Message,true);

            return RedirectToAction(nameof(Index));
        }
    }
}
