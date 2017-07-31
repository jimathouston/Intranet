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
using Intranet.API.Domain.Helpers;

namespace Intranet.API.Controllers
{
    /// <summary>
    /// Manage news items.
    /// </summary>
    [Produces("application/json")]
    [Route("/api/v1/[controller]")]
    public class PolicyController : Controller, IRestControllerAsync<PolicyViewModel>
    {
        private readonly IntranetApiContext _context;

        public PolicyController(IntranetApiContext context)
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
                var policy = await _context.Policies
                    .Include(f => f.PolicyKeywords)
                        .ThenInclude(fk => fk.Keyword)
                    .Include(f => f.Category)
                        .ThenInclude(c => c.Policies)
                    .SingleOrDefaultAsync(f => f.Id == id);

                if (policy.IsNull())
                {
                    return NotFound();
                }

                _context.Remove(policy);

                // If the category has no more related entities then delete the category as well
                if (await CategoryHelpers.HasNoRelatedEntities(policy.Category, _context, ignore: policy))
                {
                    _context.Remove(policy.Category);
                }

                // If the keywords has no more related entities then delete the category as well
                foreach (var keyword in policy.PolicyKeywords.Select(fk => fk.Keyword))
                {
                    if (await KeywordHelpers.HasNoRelatedEntities(keyword, _context, ignore: policy.PolicyKeywords))
                    {
                        _context.Remove(keyword);
                    }
                }

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
                var policies = await _context.Policies
                    .Include(f => f.Category)
                    .Include(f => f.PolicyKeywords)
                        .ThenInclude(fk => fk.Keyword)
                    .ToListAsync();

                var policyViewModels = policies?
                    .Select(f => new PolicyViewModel(f))
                    .ToList() ?? new List<PolicyViewModel>();

                return Ok(policyViewModels);
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
                var policy = await _context.Policies
                    .Include(f => f.Category)
                    .Include(f => f.PolicyKeywords)
                        .ThenInclude(fk => fk.Keyword)
                    .SingleOrDefaultAsync(c => c.Id == id);

                if (policy.IsNull())
                {
                    return NotFound();
                }

                return Ok(new PolicyViewModel(policy));
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
        public async Task<IActionResult> PostAsync([FromBody] PolicyViewModel policyVM)
        {
            try
            {
                var category = await _context.Categories.SingleOrDefaultAsync(c => c.Title.Equals(policyVM.Category.Title, StringComparison.OrdinalIgnoreCase));

                if (category.IsNotNull())
                {
                    policyVM.Category = category;
                }
                else
                {
                    policyVM.Category = new Category
                    {
                        Title = policyVM.Category.Title,
                        Url = UrlHelper.URLFriendly(policyVM.Category.Title),
                    };
                }

                var policy = new Policy
                {
                    Category = policyVM.Category,
                    Description = policyVM.Description,
                    FileUrl = policyVM.FileUrl,
                    Id = policyVM.Id,
                    PolicyKeywords = policyVM.PolicyKeywords,
                    Title = policyVM.Title,
                    Url = UrlHelper.URLFriendly(policyVM.Title)
                };

                var keywords = KeywordHelper.GetKeywordsFromString(policyVM.Keywords);
                var allKeywordEntities = GetAllKeywordEntitiesInternal(policyVM, keywords);
                KeywordHelper.SetKeywords<Policy, PolicyKeyword>(keywords, policy, allKeywordEntities);

                await _context.AddAsync(policy);

                await _context.SaveChangesAsync();

                return Ok(new PolicyViewModel(policy));
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
        public async Task<IActionResult> PutAsync(int id, [FromBody] PolicyViewModel policyVM)
        {
            try
            {
                var entity = await _context.Policies
                    .Include(f => f.Category)
                    .SingleOrDefaultAsync(c => c.Id == id);

                if (entity.IsNull())
                {
                    return NotFound();
                }

                if (!entity.Category.Title.Equals(policyVM.Category.Title, StringComparison.OrdinalIgnoreCase))
                {
                    var category = await _context.Categories.SingleOrDefaultAsync(c => c.Title.Equals(policyVM.Category.Title, StringComparison.OrdinalIgnoreCase));

                    if (category.IsNotNull())
                    {
                        entity.Category = category;
                    }
                    else
                    {
                        entity.Category = new Category
                        {
                            Title = policyVM.Category.Title,
                            Url = UrlHelper.URLFriendly(policyVM.Category.Title),
                        };
                    }
                }

                entity.Title = policyVM.Title;
                entity.Description = policyVM.Description;
                entity.FileUrl = policyVM.FileUrl;

                var keywords = KeywordHelper.GetKeywordsFromString(policyVM.Keywords);
                var allKeywordEntities = GetAllKeywordEntitiesInternal(policyVM, keywords);
                KeywordHelper.SetKeywords<Policy, PolicyKeyword>(keywords, entity, allKeywordEntities);

                await _context.SaveChangesAsync();

                return Ok(new PolicyViewModel(entity));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Private Helpers
        private List<Keyword> GetAllKeywordEntitiesInternal(PolicyViewModel policy, IEnumerable<string> keywords)
        {
            return _context.Keywords?
            .Include(k => k.PolicyKeywords)
                .ThenInclude(fk => fk.Policy)?
            .Where(k => keywords.Contains(k.Name, StringComparer.OrdinalIgnoreCase) || k.PolicyKeywords.Any(nk => nk.PolicyId.Equals(policy.Id)))
            .ToList();
        }
        #endregion
    }
}
