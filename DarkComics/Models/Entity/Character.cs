using DarkComics.Models.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DarkComics.Models.Entity
{
    public class Character : BaseEntity
    {
        [Required(ErrorMessage = "Name was incorrect"), StringLength(maximumLength: 25)]
        public string Name { get; set; }
        [Required]
        public string HeroName { get; set; }
        [Required]
        public string FirstAppearance { get; set; }
        [BindNever]
        public string FirstImage { get; set; }
        [Required,NotMapped]
        public IFormFile FirstPhoto { get; set; }
        [BindNever]
        public string Logo { get; set; }
        [Required, NotMapped]
        public IFormFile LogoPhoto { get; set; }
        [BindNever]
        public string SecondImage { get; set; }
        [Required,NotMapped]
        public IFormFile SecondPhoto { get; set; }
        [BindNever]
        public string Profile { get; set; }
        [Required,NotMapped]
        public IFormFile ProfilePhoto { get; set; }
        public bool IsActive { get; set; }
        public DateTime DeactivatedDate { get; set; }
        [Required]
        public string NickName { get; set; }       
        [Required]
        public string Creator { get; set; }
        [Required,StringLength(maximumLength:20)]
        public string Height { get; set; }
        [Required]
        public int? Weight { get; set; }
        [Required, StringLength(maximumLength: 20)]
        public string EyeColor { get; set; }
        [Required, StringLength(maximumLength: 20)]
        public string HairStyle { get; set; }
        [Required, StringLength(maximumLength: 30)]
        public string Education { get; set; }
        [Required]
        public int? Fighting { get; set; }
        [Required]
        public int? Durability { get; set; }
        [Required]
        public int? Energy { get; set; }
        [Required]
        public int? Strength { get; set; }
        [Required]
        public int? Speed { get; set; }
        [Required]
        public int? Intelligence { get; set; }
        public string LayoutImage { get; set; }
        [Required, NotMapped]
        public IFormFile LayoutPhoto { get; set; }
        [AllowHtml,Required,DataType("text")]        
        public string AboutCharacter { get; set; }
        [AllowHtml, Required, DataType("text")]
        public string ShortDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        //public List<Product> Products { get; set; }
        public List<ToyCharacter> ToyCharacters { get; set; }
        public List<CharacterPower> CharacterPowers { get; set; }
        public List<ProductCharacter> ProductCharacters { get; set; }
        public List<CharacterNews> CharacterNews { get; set; }
        public City City { get; set; }
        public int? CityId { get; set; }
    }
}
