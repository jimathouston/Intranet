using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Intranet.Web.Domain.Data;
using Intranet.Web.Domain.Models.Entities;
using Intranet.Web.ViewModels;
using Intranet.Web.Common.Factories;
using Microsoft.AspNetCore.Http;
using Intranet.Web.Extensions;
using Intranet.Web.Domain.Helpers;
using Intranet.Web.Common.Helpers;
using Intranet.Services.FileStorageService;
using Intranet.Web.Common.Extensions;

namespace Intranet.Web.Controllers
{
    public class NewsController : Controller
    {
        private readonly IntranetApiContext _context;
        private readonly IDateTimeFactory _dateTimeFactory;
        private readonly IFileStorageService _fileStorageService;

        public NewsController(IntranetApiContext context,
                              IDateTimeFactory dateTimeFactory,
                              IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
            _context = context;
            _dateTimeFactory = dateTimeFactory;
        }

        #region GET
        // GET: News
        public async Task<IActionResult> Index()
        {
            try
            {
                var news = await _context.News
                    .Include(n => n.HeaderImage)
                    .Include(n => n.User)
                    .Include(n => n.NewsTags)
                        .ThenInclude(nt => nt.Tag)
                    .ToListAsync();

                var newsViewModel = news
                    .Select(n => new NewsViewModel(n))
                    .ToList();

                return View(newsViewModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var news = await _context.News
                    .Include(n => n.HeaderImage)
                    .Include(n => n.User)
                    .Include(n => n.NewsTags)
                        .ThenInclude(nt => nt.Tag)
                    .SingleOrDefaultAsync(n => n.Id == id);

                if (news == null)
                {
                    return NotFound();
                }

                var newsViewModel = new NewsViewModel(news);

                return View(newsViewModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: News/2017/05/20/title-of-the-news
        [Route("[controller]/{year:int}/{month:int}/{day:int}/{url}")]
        [HttpGet]
        public async Task<IActionResult> Details(int year, int month, int day, string url)
        {
            try
            {
                var date = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);

                var news = await _context.News
                    .Include(n => n.HeaderImage)
                    .Include(n => n.User)
                    .Include(n => n.NewsTags)
                        .ThenInclude(nt => nt.Tag)
                    .SingleOrDefaultAsync(n => n.Created.Date == date.Date && n.Url == url);

                if (news == null)
                {
                    return NotFound();
                }

                var newsViewModel = new NewsViewModel(news);

                return View(newsViewModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region CREATE
        // GET: News/Create
        public IActionResult Create()
        {
            return View(new NewsViewModel());
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Text,Published,HeaderImage,Tags")] NewsViewModel news)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(news);
                }

                var image = Request.Form.Files.SingleOrDefault(f => f.ContentType.Contains("image"));

                if (image.IsNotNull())
                {
                    var filename = await _fileStorageService.SetImageAsync(image);
                    news.HeaderImage = new Image { FileName = filename };
                }

                var username = HttpContext.User.GetUsername();
                var displayName = HttpContext.User.GetDisplayName();

                if (_context.Users.Find(username) == null)
                {
                    var user = new User
                    {
                        Username = username,
                        DisplayName = displayName,
                    };

                    _context.Users.Add(user);
                }

                var newNews = new News
                {
                    Title = news.Title,
                    Text = news.Text,
                    UserId = username,
                    Published = news.Published,
                    Url = CustomUrlHelper.URLFriendly(news.Title),
                };

                if (!String.IsNullOrWhiteSpace(news.HeaderImage?.FileName))
                {
                    newNews.HeaderImage = new Image
                    {
                        FileName = news.HeaderImage.FileName
                    };
                }

                newNews.Created = _dateTimeFactory.DateTimeOffsetUtc;

                var isTitleAndDateUniqe = !_context.News.Any(n => n.Created.Date == newNews.Created.Date && n.Url == newNews.Url);

                if (!isTitleAndDateUniqe)
                {
                    ModelState.AddModelError("error", "There has already been created a news with that title today!");
                    HttpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                    return View(news);
                }

                var tags = TagHelpers.GetTagsFromString(news.Tags);
                var allTagEntities = GetAllTagEntitiesInternal(news, tags);
                TagHelpers.SetTags<News, NewsTag>(tags, newNews, allTagEntities);

                _context.News.Add(newNews);
                await _context.SaveChangesAsync();

                var newsViewModel = new NewsViewModel(newNews);

                return RedirectToAction("Details", new { id = newNews.Id });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region EDIT
        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var news = await _context.News
                    .Include(n => n.HeaderImage)
                    .Include(n => n.User)
                    .Include(n => n.NewsTags)
                        .ThenInclude(nt => nt.Tag)
                    .SingleOrDefaultAsync(n => n.Id.Equals(id));

                if (news == null)
                {
                    return NotFound();
                }

                var newsViewModel = new NewsViewModel(news);

                return View(newsViewModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Text,Published,HeaderImage,Tags")] NewsViewModel news)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(news);
                }

                var username = HttpContext.User.GetUsername();
                var isAdmin = HttpContext.User.IsAdmin();

                var entity = _context.News
                    .Include(n => n.User)
                    .SingleOrDefault(n => n.Id == id);

                if (entity == null)
                {
                    return NotFound();
                }

                if (entity.UserId?.Equals(username) != true && !isAdmin)
                {
                    ModelState.AddModelError("Error", "As a non admin you can only update your own news.");
                    HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return View(news);
                }

                // If the news changes to Published for the first time, set creation date
                if (!entity.HasEverBeenPublished && news.Published)
                {
                    entity.Created = _dateTimeFactory.DateTimeOffsetUtc;
                }
                else if (news.Published)
                {
                    entity.Updated = _dateTimeFactory.DateTimeOffsetUtc;
                }

                entity.Title = news.Title;
                entity.Text = news.Text;
                entity.Published = news.Published;

                var image = Request.Form.Files.SingleOrDefault(f => f.ContentType.Contains("image"));

                if (image.IsNotNull())
                {
                    var filename = await _fileStorageService.SetImageAsync(image);
                    news.HeaderImage = new Image { FileName = filename };
                }

                if (entity.HeaderImage.IsNotNull() && !String.IsNullOrWhiteSpace(news.HeaderImage?.FileName))
                {
                    entity.HeaderImage.FileName = news.HeaderImage?.FileName;
                }
                else if (!String.IsNullOrWhiteSpace(news.HeaderImage?.FileName))
                {
                    entity.HeaderImage = new Image
                    {
                        FileName = news.HeaderImage.FileName
                    };
                }

                var tags = TagHelpers.GetTagsFromString(news.Tags);
                var allTagEntities = GetAllTagEntitiesInternal(news, tags);
                TagHelpers.SetTags<News, NewsTag>(tags, entity, allTagEntities);

                _context.SaveChanges();

                return RedirectToAction("Details", new { Id = entity.Id });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region DELETE
        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.User)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (news == null)
            {
                return NotFound();
            }

            return View(new NewsViewModel(news));
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var username = HttpContext.User.GetUsername();
                var isAdmin = HttpContext.User.IsAdmin();

                var news = _context.News.Find(id);

                if (news == null)
                {
                    return NotFound();
                }

                if (news.UserId?.Equals(username) != true && !isAdmin)
                {
                    ModelState.AddModelError("Error", "As a non admin you can only remove your own news.");
                    return View(new NewsViewModel(news));
                }

                _context.News.Remove(news);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }
        #endregion

        #region Private Helpers
        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }

        private List<Tag> GetAllTagEntitiesInternal(NewsViewModel news, IEnumerable<string> tags)
        {
            if (tags.IsNull())
            {
                return new List<Tag>();
            }

            return _context.Tags?
            .Include(k => k.NewsTags)
                .ThenInclude(nt => nt.News)?
            .Where(k => tags.Contains(k.Name, StringComparer.OrdinalIgnoreCase) || k.NewsTags.Any(nt => nt.NewsId.Equals(news.Id)))
            .ToList();
        }
        #endregion
    }
}
