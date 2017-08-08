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
using Intranet.Web.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Intranet.Web.Common.Helpers;
using Intranet.Web.Domain.Helpers;

namespace Intranet.Web.Controllers
{
    public class FaqsController : Controller
    {
        private readonly IntranetApiContext _context;

        public FaqsController(IntranetApiContext context)
        {
            _context = context;    
        }

        #region GET
        // GET: Faqs
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                    .Include(c => c.Faqs)
                        .ThenInclude(f => f.FaqKeywords)
                            .ThenInclude(fk => fk.Keyword)
                    .ToListAsync() ?? new List<Category>();

            return View(categories);
        }

        // GET: Faqs/Details/5
        [Authorize("IsAdmin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = await _context.Faqs
                .Include(f => f.Category)
                .Include(f => f.FaqKeywords)
                    .ThenInclude(fk => fk.Keyword)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (faq.IsNull())
            {
                return NotFound();
            }

            return View(new FaqViewModel(faq));
        }
        #endregion

        #region CREATE
        // POST: Faqs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // GET: Faqs/Create
        [Authorize("IsAdmin")]
        public IActionResult Create([FromQuery] string category = null)
        {
            if (category.IsNull())
            {
                ViewData["Categories"] = new SelectList(_context.Categories, nameof(Category.Title), nameof(Category.Title));
            }
            else
            {
                ViewData["Categories"] = new SelectList(_context.Categories, nameof(Category.Title), nameof(Category.Title), category);
            }

            var vm = new FaqViewModel();
            vm.Category = new Category();
            return View(vm);
        }

        [Authorize("IsAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Category,Question,Answer,Keywords")] FaqViewModel faqVM)
        {
            if (ModelState.IsValid)
            {
                var category = await _context
                    .Categories
                    .SingleOrDefaultAsync(c => c.Title.Equals(faqVM.Category.Title, StringComparison.OrdinalIgnoreCase));

                if (category.IsNotNull())
                {
                    faqVM.Category = category;
                }
                else
                {
                    faqVM.Category = new Category
                    {
                        Title = faqVM.Category.Title,
                        Url = CustomUrlHelper.URLFriendly(faqVM.Category.Title),
                    };
                }

                var faq = new Faq
                {
                    Answer = faqVM.Answer,
                    Category = faqVM.Category,
                    Question = faqVM.Question,
                    Url = CustomUrlHelper.URLFriendly(faqVM.Question),
                };

                var keywords = KeywordHelpers.GetKeywordsFromString(faqVM.Keywords);
                var allKeywordEntities = GetAllKeywordEntitiesInternal(faqVM, keywords);
                KeywordHelpers.SetKeywords<Faq, FaqKeyword>(keywords, faq, allKeywordEntities);

                await _context.AddAsync(faq);

                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { Id = faq.Id });
            }
            ViewData["Categories"] = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Title));
            return View(faqVM);
        }
        #endregion

        #region EDIT
        // GET: Faqs/Edit/5
        [Authorize("IsAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = await _context.Faqs
                .Include(f => f.Category)
                .Include(f => f.FaqKeywords)
                    .ThenInclude(fk => fk.Keyword)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (faq == null)
            {
                return NotFound();
            }

            ViewData["Categories"] = new SelectList(_context.Categories, nameof(Category.Title), nameof(Category.Title), faq.Category.Title);

            return View(new FaqViewModel(faq));
        }

        // POST: Faqs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize("IsAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Category,Question,Answer,Url")] FaqViewModel faqVM)
        {
            if (id != faqVM?.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var entity = await _context.Faqs
                    .Include(f => f.Category)
                    .SingleOrDefaultAsync(c => c.Id == id);

                if (entity.IsNull())
                {
                    return NotFound();
                }

                if (!entity.Category.Title.Equals(faqVM.Category.Title, StringComparison.OrdinalIgnoreCase))
                {
                    var category = await _context.Categories.SingleOrDefaultAsync(c => c.Title.Equals(faqVM.Category.Title, StringComparison.OrdinalIgnoreCase));

                    if (category.IsNotNull())
                    {
                        entity.Category = category;
                    }
                    else
                    {
                        entity.Category = new Category
                        {
                            Title = faqVM.Category.Title,
                            Url = CustomUrlHelper.URLFriendly(faqVM.Category.Title),
                        };
                    }
                }

                entity.Answer = faqVM.Answer;
                entity.Question = faqVM.Question;

                var keywords = KeywordHelpers.GetKeywordsFromString(faqVM.Keywords);
                var allKeywordEntities = GetAllKeywordEntitiesInternal(faqVM, keywords);
                KeywordHelpers.SetKeywords<Faq, FaqKeyword>(keywords, entity, allKeywordEntities);

                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { Id = entity.Id });
            }
            return View(faqVM);
        }
        #endregion

        #region DELETE
        // GET: Faqs/Delete/5
        [Authorize("IsAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = await _context.Faqs
                .Include(f => f.Category)
                .Include(f => f.FaqKeywords)
                    .ThenInclude(fk => fk.Keyword)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (faq == null)
            {
                return NotFound();
            }

            return View(new FaqViewModel(faq));
        }

        // POST: Faqs/Delete/5
        [Authorize("IsAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faq = await _context.Faqs
                    .Include(f => f.FaqKeywords)
                        .ThenInclude(fk => fk.Keyword)
                    .Include(f => f.Category)
                        .ThenInclude(c => c.Faqs)
                    .SingleOrDefaultAsync(f => f.Id == id);

            if (faq.IsNull())
            {
                return NotFound();
            }

            _context.Remove(faq);

            // If the category has no more related entities then delete the category as well
            if (await CategoryHelpers.HasNoRelatedEntities(faq.Category, _context, ignore: faq))
            {
                _context.Remove(faq.Category);
            }

            // If the keywords has no more related entities then delete the category as well
            foreach (var keyword in faq.FaqKeywords.Select(fk => fk.Keyword))
            {
                if (await KeywordHelpers.HasNoRelatedEntities(keyword, _context, ignore: faq.FaqKeywords))
                {
                    _context.Remove(keyword);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion
        
        #region Private Helpers
        private List<Keyword> GetAllKeywordEntitiesInternal(FaqViewModel faq, IEnumerable<string> keywords)
        {
            if (keywords.IsNull())
            {
                return new List<Keyword>();
            }

            return _context.Keywords?
            .Include(k => k.FaqKeywords)
                .ThenInclude(fk => fk.Faq)?
            .Where(k => keywords.Contains(k.Name, StringComparer.OrdinalIgnoreCase) || k.FaqKeywords.Any(nk => nk.FaqId.Equals(faq.Id)))
            .ToList();
        }

        private bool FaqExists(int id)
        {
            return _context.Faqs.Any(e => e.Id == id);
        }
        #endregion
    }
}
