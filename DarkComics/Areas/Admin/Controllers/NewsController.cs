using DarkComics.DAL;
using DarkComics.Models.Entity;
using DarkComics.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DarkComics.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly DarkComicDbContext _db;
        private readonly IWebHostEnvironment _env;
        public NewsController(DarkComicDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public IActionResult Index()
        {
            NewsViewModel newsViewModel = new NewsViewModel
            {
                NewsList = _db.News.Include(n => n.CharacterNews).ThenInclude(cn => cn.Character).Include(n=>n.PostComments).
                ThenInclude(pc => pc.Comment).Include(n => n.TagNews).ThenInclude(tn => tn.Tag).ToList() 
            };
            return View(newsViewModel);
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
                return NotFound();

            NewsDetailViewModel newsDetailViewModel = new NewsDetailViewModel
            {
                News = _db.News.Include(n => n.CharacterNews).ThenInclude(cn => cn.Character).Include(n => n.TagNews).
                ThenInclude(tn => tn.Tag).Include(n=>n.PostComments).ThenInclude(pc=>pc.Comment).ThenInclude(c=>c.User).FirstOrDefault(n=>n.Id == id),
                
            };

            if (newsDetailViewModel.News == null)
                return NotFound();           

            return View(newsDetailViewModel);
        }

        public ActionResult Create()
        {

            NewsViewModel newsViewModel = new NewsViewModel
            {
                NewsList = _db.News.Include(n => n.CharacterNews).ThenInclude(cn => cn.Character).Include(n => n.TagNews).ThenInclude(tn => tn.Tag).ToList(),
                Tags = _db.Tags.ToList(),
                TagList = new List<SelectListItem>(),
                Characters = _db.Characters.ToList(),
                CharacterList = new List<SelectListItem>()
            };

            foreach (var tag in newsViewModel.Tags)
            {
                newsViewModel.TagList.AddRange(new List<SelectListItem>{
                    new SelectListItem() { Text = tag.Title, Value = tag.Id.ToString() }
                });
            }

            foreach (var character in newsViewModel.Characters)
            {
                newsViewModel.CharacterList.AddRange(new List<SelectListItem>{
                    new SelectListItem() { Text = character.Name, Value = character.Id.ToString() }
                });
            }


            return View(newsViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ActionResult Create(NewsViewModel newsViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(newsViewModel);
            }

            newsViewModel.News.Image = RenderImage(newsViewModel.News, newsViewModel.News.Photo);           

            if (string.IsNullOrEmpty(newsViewModel.News.Image))
            {
                ModelState.AddModelError("Image", "Image was incorrect");
                return View(newsViewModel);
            }

            _db.News.Add(newsViewModel.News);
            _db.SaveChanges();

            int? newsId = _db.News.Max(n => n.Id);
            News News = _db.News.Find(newsId);

            foreach (var character in newsViewModel.ChosenCharacters)
            {
                Character characterDb = _db.Characters.FirstOrDefault(p => p.Id == character);
                CharacterNews characterNews = new CharacterNews();

                characterNews.NewsId = newsId;
                characterNews.CharacterId = characterDb.Id;
                _db.CharacterNews.Add(characterNews);
                _db.SaveChanges();
            }

            foreach (var chosenTag in newsViewModel.ChosenTags)
            {
                Tag tag = _db.Tags.FirstOrDefault(t => t.Id == chosenTag);
                TagNews tagNews = new TagNews();

                tagNews.NewsId = newsId;
                tagNews.TagId = tag.Id;
                _db.TagNews.Add(tagNews);
                _db.SaveChanges();
            }
            return RedirectToAction("Index", newsViewModel);
        }

        // GET: EventController/Edit/5
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NewsViewModel newsViewModel = new NewsViewModel
            {
                News= _db.News.Include(n => n.CharacterNews).ThenInclude(cn => cn.Character).Include(n => n.TagNews).ThenInclude(tn => tn.Tag).FirstOrDefault(n=>n.Id == id),
                Tags = _db.Tags.ToList(),
                TagList = new List<SelectListItem>(),
                Characters = _db.Characters.ToList(),
                CharacterList = new List<SelectListItem>()

            };
            newsViewModel.Image = newsViewModel.News.Image;
           
            if (newsViewModel.News == null)
            {
                return BadRequest();
            }

            foreach (var tag in newsViewModel.Tags)
            {
                newsViewModel.TagList.AddRange(new List<SelectListItem>{
                    new SelectListItem() { Text = tag.Title, Value = tag.Id.ToString() }
                });
            }

            foreach (var character in newsViewModel.Characters)
            {
                newsViewModel.CharacterList.AddRange(new List<SelectListItem>{
                    new SelectListItem() { Text = character.Name, Value = character.Id.ToString() }
                });
            }

            return View(newsViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, NewsViewModel newsViewModel)
        {
            if (id == null || (id != newsViewModel.News.Id))
            {
                return NotFound();
            }


            //if user don't choose image program enter here
            if (newsViewModel.News.Photo == null)
            {
                newsViewModel.News.Image = newsViewModel.Image;
                ModelState["News.Photo"].ValidationState = ModelValidationState.Valid;
            }
            else
                newsViewModel.News.Image = RenderImage(newsViewModel.News, newsViewModel.News.Photo, newsViewModel.Image);

            if (string.IsNullOrEmpty(newsViewModel.News.Image))
            {
                ModelState.AddModelError("Image", "Image was incorrect");
                return View(newsViewModel);
            }

            if (!ModelState.IsValid)
                return View(newsViewModel.News);


            List<TagNews> tagNews = _db.TagNews.Where(tn => tn.NewsId == newsViewModel.News.Id).ToList();
            List<CharacterNews> characterNews = _db.CharacterNews.Where(cn => cn.NewsId == newsViewModel.News.Id).ToList();

            foreach (var oldCharacterNews in characterNews)            {

                if (!newsViewModel.ChosenCharacters.Contains(oldCharacterNews.CharacterId))
                    _db.CharacterNews.Remove(oldCharacterNews);
            }

            foreach (var chosenCharacterNews in newsViewModel.ChosenCharacters)
            {

                if (!characterNews.Exists(cn=>cn.CharacterId == chosenCharacterNews))
                {
                    CharacterNews newCharacterNews = new CharacterNews
                    {
                        NewsId = newsViewModel.News.Id,
                        CharacterId = chosenCharacterNews
                    };
                    _db.CharacterNews.Add(newCharacterNews);                    
                }
            }


            foreach (var oldTagNews in tagNews)
            {
                if (!newsViewModel.ChosenTags.Contains(oldTagNews.TagId))
                    _db.TagNews.Remove(oldTagNews);
            }

            foreach (var chosenTagNews in newsViewModel.ChosenTags)
            {

                if (!tagNews.Exists(tn => tn.TagId == chosenTagNews))
                {
                    TagNews newTagNews = new TagNews
                    {
                        NewsId = newsViewModel.News.Id,
                        TagId = chosenTagNews
                    };
                    _db.TagNews.Add(newTagNews);
                }
            }


            _db.News.Update(newsViewModel.News);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index), newsViewModel);

        }

        public ActionResult DeleteCharacter(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CharacterNews characterNews = _db.CharacterNews.FirstOrDefault(c => c.CharacterId == id);
            if (characterNews == null)
            {
                return NotFound();
            }

            NewsDetailViewModel newsDetailViewModel = new NewsDetailViewModel
            {
                News = _db.News.Include(n => n.CharacterNews).ThenInclude(cn => cn.Character).Include(n => n.TagNews).ThenInclude(tn => tn.Tag).FirstOrDefault(n => n.Id == characterNews.NewsId)
            };
                      
            if (newsDetailViewModel.News == null)
                return NotFound();

            _db.CharacterNews.Remove(characterNews);
            _db.SaveChanges();

            return View(nameof(Detail), newsDetailViewModel);
        }

        public ActionResult DeleteTag(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }           

            TagNews tagNews = _db.TagNews.FirstOrDefault(c => c.TagId == id);
            if (tagNews == null)
            {
                return NotFound();
            }

            NewsDetailViewModel newsDetailViewModel = new NewsDetailViewModel
            {
                News = _db.News.Include(n => n.CharacterNews).ThenInclude(cn => cn.Character).Include(n => n.TagNews).ThenInclude(tn => tn.Tag).FirstOrDefault(n => n.Id == tagNews.NewsId)
            };

            if (newsDetailViewModel.News == null)
                return NotFound();

            _db.TagNews.Remove(tagNews);
            _db.SaveChanges();

            return View(nameof(Detail), newsDetailViewModel);
        }

        public IActionResult MakeActiveOrDeactiveMessage(int? id)
        {
            if (id == null)
                return NotFound();

            Comment comment = _db.Comments.Include(c=>c.PostComments).ThenInclude(pc=>pc.News).FirstOrDefault(c => c.Id == id);

            if (comment.IsActive)
                comment.IsActive = false;
            else
                comment.IsActive = true;

            _db.SaveChanges();
            NewsDetailViewModel newsDetailViewModel = new NewsDetailViewModel
            {
                News = _db.News.Include(n => n.CharacterNews).ThenInclude(cn => cn.Character).Include(n => n.TagNews).
               ThenInclude(tn => tn.Tag).Include(n => n.PostComments).ThenInclude(pc => pc.Comment).ThenInclude(c=>c.User).FirstOrDefault(n => n.Id == comment.PostComments[0].NewsId)
            };

            return View(nameof(Detail),newsDetailViewModel);
        }


        public string RenderImage(News news, IFormFile photo)
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
            string newSlider = Path.Combine(environment, "assets", "img","news", $"news-{news.Id}");
            if (news.Id == null)
            {
                if (_db.News.Max(c => c.Id) == null)
                    newSlider = Path.Combine(environment, "assets", "img", "news", $"news-1");
                else
                newSlider = Path.Combine(environment, "assets", "img", "news", $"news-{_db.News.Max(c => c.Id + 1)}");
            }
                



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

        public string RenderImage(News news, IFormFile photo, string oldFilename)
        {
            oldFilename = Path.Combine(_env.WebRootPath, "assets", "img", "news", $"news-{news.Id}", oldFilename);
            FileInfo oldFile = new FileInfo(oldFilename);
            if (System.IO.File.Exists(oldFilename))
            {
                oldFile.Delete();
            };

            return RenderImage(news, photo);
        }
    }
}
