using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Intranet.API.Domain;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Intranet.API.Extensions;
using Intranet.Shared.Factories;
using Intranet.API.Helpers;
using Intranet.API.ViewModels;
using Intranet.Shared.Extensions;

namespace Intranet.API.Controllers
{
    /// <summary>
    /// Manage news items.
    /// </summary>
    [Produces("application/json")]
    [Route("/api/v1/[controller]")]
    public class FaqController : Controller, IRestControllerAsync<FaqViewModel>
    {
        private readonly IntranetApiContext _context;

        public FaqController(IntranetApiContext context)
        {
            _context = context;
        }

        #region DELETE
        [Authorize("IsAdmin")]
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var faq = await _context.Faqs
                    .Include(f => f.Category)
                        .ThenInclude(c => c.Faqs)
                    .SingleOrDefaultAsync(f => f.Id == id);

                if (faq.IsNull())
                {
                    return NotFound();
                }

                // If the FAQ is the only one attached to a category then delete the category as well
                if (faq.Category.HasNoRelatedEntities(faq))
                {
                    _context.Remove(faq.Category);
                }

                _context.Remove(faq);

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region GET
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var faqs = await _context.Faqs
                    .Include(f => f.Category)
                    .Include(f => f.FaqKeywords)
                        .ThenInclude(fk => fk.Keyword)
                    .ToListAsync();

                var faqViewModels = faqs?
                    .Select(f => new FaqViewModel(f))
                    .ToList() ?? new List<FaqViewModel>();

                return Ok(faqViewModels);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var faq = await _context.Faqs
                    .Include(f => f.Category)
                    .Include(f => f.FaqKeywords)
                        .ThenInclude(fk => fk.Keyword)
                    .SingleOrDefaultAsync(c => c.Id == id);

                if (faq.IsNull())
                {
                    return NotFound();
                }

                return Ok(new FaqViewModel(faq));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region POST
        [Authorize("IsAdmin")]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] FaqViewModel faqVM)
        {
            try
            {
                var category = await _context.Categories.SingleOrDefaultAsync(c => c.Title.Equals(faqVM.Category.Title, StringComparison.OrdinalIgnoreCase));

                if (category.IsNotNull())
                {
                    faqVM.Category = category;
                }
                else
                {
                    faqVM.Category = new Category
                    {
                        Title = faqVM.Category.Title,
                        Url = UrlHelper.URLFriendly(faqVM.Category.Title),
                    };
                }

                var faq = new Faq
                {
                    Answer = faqVM.Answer,
                    Category = faqVM.Category,
                    Question = faqVM.Question,
                    Url = UrlHelper.URLFriendly(faqVM.Question),
                };

                var keywords = KeywordHelper.GetKeywordsFromString(faqVM.Keywords);
                var allKeywordEntities = GetAllKeywordEntitiesInternal(faqVM, keywords);
                KeywordHelper.SetKeywords<Faq, FaqKeyword>(keywords, faq, allKeywordEntities);

                await _context.AddAsync(faq);

                await _context.SaveChangesAsync();

                return Ok(new FaqViewModel(faq));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region PUT
        [Authorize("IsAdmin")]
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] FaqViewModel faqVM)
        {
            try
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
                            Url = UrlHelper.URLFriendly(faqVM.Category.Title),
                        };
                    }
                }

                entity.Answer = faqVM.Answer;
                entity.Question = faqVM.Question;

                var keywords = KeywordHelper.GetKeywordsFromString(faqVM.Keywords);
                var allKeywordEntities = GetAllKeywordEntitiesInternal(faqVM, keywords);
                KeywordHelper.SetKeywords<Faq, FaqKeyword>(keywords, entity, allKeywordEntities);

                await _context.SaveChangesAsync();

                return Ok(new FaqViewModel(entity));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Private Helpers
        private List<Keyword> GetAllKeywordEntitiesInternal(FaqViewModel faq, IEnumerable<string> keywords)
        {
            return _context.Keywords?
            .Include(k => k.FaqKeywords)
                .ThenInclude(fk => fk.Faq)?
            .Where(k => keywords.Contains(k.Name, StringComparer.OrdinalIgnoreCase) || k.FaqKeywords.Any(nk => nk.FaqId.Equals(faq.Id)))
            .ToList();
        }
        #endregion
    }
}
