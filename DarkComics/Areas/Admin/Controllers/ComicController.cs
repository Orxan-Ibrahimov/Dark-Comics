using DarkComics.DAL;
using DarkComics.Helpers.Enums;
using DarkComics.Helpers.Methods;
using DarkComics.Models.Entity;
using DarkComics.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DarkComics.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ComicController : Controller
    {
        private readonly DarkComicDbContext _db;
        private readonly IWebHostEnvironment _env;
        public ComicController(DarkComicDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        // GET: ComicController
        public ActionResult Index()
        {
            ComicViewModel comicViewModel = new ComicViewModel
            {
                Series = _db.Series.Include(p => p.ComicDetails).ThenInclude(cd => cd.Products).ThenInclude(p => p.ProductCharacters).
               ThenInclude(pc => pc.Character).ToList()
            };

            return View(comicViewModel);
        }

        // GET: ComicController/Details/5
        public ActionResult ComicIndex(int? id)
        {
            if (id == null)
                return NotFound();

            List<Product> products = _db.Products.Include(p => p.ComicDetail).ThenInclude(cd => cd.Serie).Include(p => p.ProductCharacters).
               ThenInclude(pc => pc.Character).Where(p => p.Category == Category.Comic && p.ComicDetail.SerieId == id).ToList();

            ComicViewModel comicView = new ComicViewModel
            {
                RandomComics = products,
                Serie = _db.Series.FirstOrDefault(s=>s.Id == id)
            };

            if (comicView.RandomComics == null || comicView.Serie == null)
                return BadRequest();

            return View(comicView);
        }

        public ActionResult CreateSerie()
        {
            ComicViewModel comicViewModel = new ComicViewModel
            {
                CharacterList = new List<SelectListItem>(),
                TeamOrNot = new List<SelectListItem>(),
                Characters = _db.Characters.ToList()

            };

            return View(comicViewModel);
        }        
                
        public IActionResult ComicDetail(int? id)
        {
            if (id == null)
                return NotFound();

          Product comic = _db.Products.Include(p => p.ComicDetail).ThenInclude(cd => cd.Serie).Include(p => p.ProductCharacters).
               ThenInclude(pc => pc.Character).Where(p => p.Category == Category.Comic).FirstOrDefault(c=>c.Id == id);

            if (comic == null)
                return NotFound();

            ComicViewModel comicViewModel = new ComicViewModel
            {
                Comic = comic
            };

            return View(comicViewModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ActionResult CreateSerie(ComicViewModel comicViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(comicViewModel);
            }

            string path = $"assets/img/comics/serie-${comicViewModel.Serie.Id}";
            if (comicViewModel.Serie.Id == null)
            {
                if (_db.Series.Max(s => s.Id) == null)
                    path = $"assets/img/comics/serie-1";
                else
                    path = $"assets/img/comics/serie-{_db.Series.Max(s => s.Id) + 1}";
            }



            comicViewModel.Serie.Cover = RenderImage(comicViewModel.Serie.CoverPhoto, path);
            comicViewModel.Serie.Backface = RenderImage(comicViewModel.Serie.BackfacePhoto, path);


            if (string.IsNullOrEmpty(comicViewModel.Serie.Cover) || string.IsNullOrEmpty(comicViewModel.Serie.Backface))
            {
                ModelState.AddModelError("Image", "Image was incorrect");
                return View(comicViewModel);
            }

            _db.Series.Add(comicViewModel.Serie);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }      

        public ActionResult CreateComic(int? id)
        {
           
            ComicViewModel comicViewModel = new ComicViewModel
            {
                Serie = _db.Series.Include(p => p.ComicDetails).ThenInclude(cd => cd.Products).ThenInclude(p => p.ProductCharacters).
                ThenInclude(pc => pc.Character).Where(s => s.IsDeleted == false).FirstOrDefault(s => s.Id == id),
                CharacterList = new List<SelectListItem>(),
                Characters = _db.Characters.ToList()
        };

            if (comicViewModel.Serie == null)
                return NotFound();

            foreach (var character in comicViewModel.Characters)
            {
                comicViewModel.CharacterList.AddRange(new List<SelectListItem>{
                    new SelectListItem() { Text = character.Name, Value = character.Id.ToString() }
                });
            }
            return View(comicViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ActionResult CreateComic(int? id, ComicViewModel comicView)
        {
            if (id == null)
            {
                return NotFound();
            }
            Serie serie = _db.Series.FirstOrDefault(s => s.Id == id);

            if (serie == null)
            {
                return NotFound();
            }

            comicView.Comic.Category = Category.Comic;           
            comicView.Comic.ComicDetail.Serie = serie;
            comicView.Comic.ComicDetail.IsCover = true;


            if (!ModelState.IsValid)
            {
                return View(comicView);
            }

            string path = $"assets/img/product-{comicView.Comic.Id}";
            if (comicView.Comic.Id == null)
            {
                if (_db.Products.Max(p => p.Id) == null)
                  path = $"assets/img/product-1";
                else
                    path = $"assets/img/product-{_db.Products.Max(p => p.Id) + 1}";
            }

            comicView.Comic.Image = RenderImage(comicView.Comic.Photo,path);           
            comicView.Comic.ComicDetail.Backface = RenderImage(comicView.Comic.ComicDetail.BackfacePhoto,path);

            if (string.IsNullOrEmpty(comicView.Comic.Image))
            {
                ModelState.AddModelError("Image", "Image was incorrect");
                return View(comicView);
            }
           
            _db.Products.Add(comicView.Comic);
            _db.SaveChanges();

            // Create ProductCharacter
            int? productId = _db.Products.Max(c => c.Id);
            Product product = _db.Products.Find(productId);

            foreach (var chosencharacter in comicView.ChosenCharacters)
            {
                Character character = _db.Characters.FirstOrDefault(p => p.Id == chosencharacter);
                ProductCharacter productCharacter = new ProductCharacter();

                productCharacter.ProductId = productId;
                productCharacter.CharacterId = character.Id;
                _db.ProductCharacters.Add(productCharacter);
                _db.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }

        // GET: EventController/Edit/5
        public IActionResult UpdateSerie(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Serie serie = _db.Series.Include(s => s.ComicDetails).ThenInclude(cd => cd.Products).FirstOrDefault(s => s.Id == id);
            if (serie == null)
            {
                return NotFound();
            }

            ComicViewModel comicViewModel = new ComicViewModel
            {
               Serie = serie
            };
            comicViewModel.Cover = comicViewModel.Serie.Cover;
            comicViewModel.Backface = comicViewModel.Serie.Backface;

            return View(comicViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult UpdateSerie(int? id, ComicViewModel comicViewModel)
        {
            if (id == null || (id != comicViewModel.Serie.Id))
            {
                return NotFound();
            }

            string oldPath = $"assets/img/comics/serie-{comicViewModel.Serie.Id}";


            //if user don't choose image program enter here
            if (comicViewModel.Serie.CoverPhoto == null)
            {
                comicViewModel.Serie.Cover = comicViewModel.Cover;
                ModelState["Serie.CoverPhoto"].ValidationState = ModelValidationState.Valid;
            }
            else
                comicViewModel.Serie.Cover = RenderImage(comicViewModel.Serie.CoverPhoto, comicViewModel.Cover, oldPath);


            if (comicViewModel.Serie.BackfacePhoto == null)
            {
                comicViewModel.Serie.Backface = comicViewModel.Backface;
                ModelState["Serie.BackfacePhoto"].ValidationState = ModelValidationState.Valid;
            }
            else
                comicViewModel.Serie.Backface = RenderImage(comicViewModel.Serie.BackfacePhoto, comicViewModel.Backface, oldPath);


            if (string.IsNullOrEmpty(comicViewModel.Serie.Cover) || string.IsNullOrEmpty(comicViewModel.Serie.Backface))
            {
                ModelState.AddModelError("Image", "Image was incorrect");
                return View(comicViewModel);
            }

            if (!ModelState.IsValid)
                return View(comicViewModel.Serie);


            _db.Series.Update(comicViewModel.Serie);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));

        }

        // GET: EventController/Edit/5
        public IActionResult UpdateComic(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           ComicViewModel comicViewModel = new ComicViewModel
            {
                Comic = _db.Products.Include(p => p.ComicDetail).ThenInclude(cd => cd.Serie).Include(p => p.ProductCharacters).
                ThenInclude(pc => pc.Character).Where(p => p.Category == Category.Comic && p.IsActive == true).FirstOrDefault(c=>c.Id == id),
               CharacterList = new List<SelectListItem>(),
               Characters = _db.Characters.ToList(),
               Series = _db.Series.Include(p => p.ComicDetails).ThenInclude(cd => cd.Products).ThenInclude(p => p.ProductCharacters).
               ThenInclude(pc => pc.Character).Where(s => s.IsDeleted == false).ToList(),
               SerieList = new List<SelectListItem>()

           };

            if (comicViewModel.Comic == null)
            {
                return BadRequest();
            }

            comicViewModel.Cover = comicViewModel.Comic.Image;
            comicViewModel.Backface = comicViewModel.Comic.ComicDetail.Backface;
           
            

            foreach (var character in comicViewModel.Characters)
            {
                comicViewModel.CharacterList.AddRange(new List<SelectListItem>{
                    new SelectListItem() { Text = character.Name, Value = character.Id.ToString() }
                });
            }

            foreach (var serie in comicViewModel.Series)
            {
                comicViewModel.SerieList.AddRange(new List<SelectListItem>{
                    new SelectListItem() { Text = serie.Name, Value = serie.Id.ToString() }
                });
            }
            return View(comicViewModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult UpdateComic(int? id, ComicViewModel comicViewModel)
        {
            if (id == null || comicViewModel.Comic.Id != id)
            {
                return NotFound();
            }

            string oldPath = $"assets/img/product-{comicViewModel.Comic.Id}";

            //if user don't choose image program enter here
            if (comicViewModel.Comic.Photo == null)
            {
                comicViewModel.Comic.Image = comicViewModel.Cover;
                ModelState["Comic.Photo"].ValidationState = ModelValidationState.Valid;
            }
            else
                comicViewModel.Comic.Image = RenderImage(comicViewModel.Comic.Photo, comicViewModel.Cover, oldPath);

            if (comicViewModel.Comic.ComicDetail.BackfacePhoto == null)
            {
                comicViewModel.Comic.ComicDetail.Backface = comicViewModel.Backface;
                ModelState["Comic.ComicDetail.BackfacePhoto"].ValidationState = ModelValidationState.Valid;
            }
            else
                comicViewModel.Comic.ComicDetail.Backface = RenderImage(comicViewModel.Comic.ComicDetail.BackfacePhoto, comicViewModel.Backface, oldPath);

            if (string.IsNullOrEmpty(comicViewModel.Comic.ComicDetail.Backface) || string.IsNullOrEmpty(comicViewModel.Comic.Image))
            {
                ModelState.AddModelError("Image", "Image was incorrect");
                return View(comicViewModel);
            }

            if (!ModelState.IsValid)
                return View(comicViewModel);

            comicViewModel.Comic.ComicDetailId = comicViewModel.Comic.ComicDetail.Id;
            comicViewModel.Comic.Category = Category.Comic;

            ////_db.ComicDetails.Update(comicViewModel.Comic.ComicDetail);
            _db.Products.Update(comicViewModel.Comic);

            //_db.SaveChanges();

            // Create ProductCharacter
            List<ProductCharacter> productCharacters = _db.ProductCharacters.Where(pc => pc.ProductId == comicViewModel.Comic.Id).ToList();

            foreach (var productCharacter in productCharacters)
            {

                if (!comicViewModel.ChosenCharacters.Contains(productCharacter.CharacterId))
                    _db.ProductCharacters.Remove(productCharacter);
            }

            foreach (var chosenCharacter in comicViewModel.ChosenCharacters)
            {

                if (!productCharacters.Exists(cn => cn.CharacterId == chosenCharacter))
                {
                    ProductCharacter productCharacter = new ProductCharacter
                    {
                        ProductId = comicViewModel.Comic.Id,
                        CharacterId = chosenCharacter
                    };
                    _db.ProductCharacters.Add(productCharacter);
                }
            }

            _db.SaveChanges();
            return RedirectToAction(nameof(Index), comicViewModel);

        }

        public IActionResult ReadingComics()
        {
            ReadViewModel readViewModel = new ReadViewModel()
            {
                ComicList = new List<SelectListItem>(),
                Comics = _db.Products.ToList()
            };

            foreach (var comic in readViewModel.Comics)
            {
                readViewModel.ComicList.AddRange(new List<SelectListItem>{
                    new SelectListItem() { Text = comic.Name, Value = comic.Id.ToString() }
                });
            }
            return View(readViewModel);
        }

        [HttpPost]
        public IActionResult ReadingComics(ReadViewModel readViewModel)
        {
            if (!ModelState.IsValid)
                return NotFound();

            Product product = _db.Products.Include(p=>p.ComicDetail).FirstOrDefault(p => p.Id == readViewModel.ChosenComicId);
            if(product == null)
                return NotFound();

            string path = $"assets/img/readingComics/{product.Name}".Replace("#", "").Replace(" ","");
            

            foreach (var photo in readViewModel.Reading.Photos)
            {

                string filename = RenderImage(photo, path);
                ReadingComic read = new ReadingComic()
                {
                    ComicDetailId = product.ComicDetailId,
                    Image = filename
                };
                //readViewModel.Reading.ComicDetailId = product.ComicDetailId;
                //readViewModel.Reading.Image = filename;
                //_db.ReadingComics.Add(readViewModel.Reading);
                _db.ReadingComics.Add(read);
                _db.SaveChanges();

            }

            return RedirectToAction("Index");
        }
               
        
        public ActionResult MakeActiveOrDeactive(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = _db.Products.Include(p => p.ComicDetail).ThenInclude(cd => cd.Serie).Include(p => p.ProductCharacters).
               ThenInclude(pc => pc.Character).Where(p => p.Category == Category.Comic).FirstOrDefault(c => c.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            if (product.IsActive == true)
                product.IsActive = false;

            else if(product.Quantity > 0)
            {
                product.IsActive = true;

                List<AppUser> users = _db.Users.Where(u => u.IsSubscriber == true).ToList();
                foreach (var user in users)
                {
                    MailOpertions.SendMessage(user.Email, product.MailHeading, product.MailMessage, true);
                }
            }

            _db.SaveChanges();

            ComicViewModel comicView = new ComicViewModel
            {
                RandomComics = _db.Products.Include(p => p.ComicDetail).ThenInclude(cd => cd.Serie).Include(p => p.ProductCharacters).
             ThenInclude(pc => pc.Character).Where(p => p.Category == Category.Comic && p.ComicDetail.SerieId == product.ComicDetail.SerieId).ToList(),
                Serie = _db.Series.FirstOrDefault(s => s.Id == product.ComicDetail.Serie.Id)
            };

            return View(nameof(ComicIndex),comicView);
        }

        public ActionResult MakeActiveOrDeactiveSerie(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Serie serie = _db.Series.Include(p => p.ComicDetails).ThenInclude(cd => cd.Products).ThenInclude(p => p.ProductCharacters).
                ThenInclude(pc => pc.Character).FirstOrDefault(s => s.Id == id);

            if (serie == null)
            {
                return NotFound();
            }

            if (serie.IsDeleted == false)
            {
                serie.IsDeleted = true;
                foreach (var comic in serie.ComicDetails)
                {
                    comic.Products.FirstOrDefault().IsActive = false;
                }
            }

            else
                serie.IsDeleted = false;

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public string RenderImage(IFormFile photo,string path)
        {
            if (!photo.ContentType.Contains("image"))
            {
                return null;
            }
            if (photo.Length / 1024 > 10000)
            {
                return null;
            }

            string filename = Guid.NewGuid().ToString() + '-' + photo.FileName;
            string environment = _env.WebRootPath;
            string newSlider = Path.Combine(environment, path);
            

            if (!Directory.Exists(newSlider))
            {
                Directory.CreateDirectory(newSlider);
            }
            newSlider = Path.Combine(newSlider, filename);

            using (FileStream file = new FileStream(newSlider, FileMode.Create))
            {
                photo.CopyTo(file);
            }

            return filename;

        }
        public string RenderImage(IFormFile photo, string oldFilename,string oldPath)
        {
            oldFilename = Path.Combine(_env.WebRootPath, oldPath, oldFilename);
            FileInfo oldFile = new FileInfo(oldFilename);
            if (System.IO.File.Exists(oldFilename))
            {
                oldFile.Delete();
            };
            
            return RenderImage(photo,oldPath);
        }   
    }
}
