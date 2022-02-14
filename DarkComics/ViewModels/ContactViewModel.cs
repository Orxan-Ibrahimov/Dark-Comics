using DarkComics.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.ViewModels
{
    public class ContactViewModel
    {
        public List<Contact> Messages { get; set; }
        public Contact Message { get; set; }
    }
}
