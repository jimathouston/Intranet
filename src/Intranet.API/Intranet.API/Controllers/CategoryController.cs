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
    public class CategoryController : Controller, IRestControllerAsync<Category>
    {
        private readonly IntranetApiContext _context;

        public CategoryController(IntranetApiContext context)
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
                var category = await _context.Categories
                    .Include(c => c.Faqs)
                    .SingleOrDefaultAsync(f => f.Id == id);

                if (category.IsNull())
                {
                    return NotFound();
                }

                if (category.Faqs.Any())
                {
                    return BadRequest(new { Error = "Must delete all related FAQ's first!" });
                }

                _context.Remove(category);
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
                var categories = await _context.Categories
                    .Include(c => c.Faqs)
                    .ToListAsync();

                return Ok(categories);
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
                var category = await _context.Categories
                    .Include(f => f.Faqs)
                    .SingleOrDefaultAsync(c => c.Id == id);

                if (category.IsNull())
                {
                    return NotFound();
                }

                return Ok(category);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region POST
        [AllowAnonymous]
        //[Authorize("IsAdmin")]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Category category)
        {
            try
            {
                var existingCategory = await _context
                    .Categories
                    .SingleOrDefaultAsync(c => c.Title.Equals(category.Title, StringComparison.OrdinalIgnoreCase));

                if (existingCategory.IsNotNull())
                {
                    return BadRequest(new { Error = $"A category with the title \"{category.Title}\" already exists." });
                }

                category.Url = UrlHelper.URLFriendly(category.Title);

                await _context.AddAsync(category);

                await _context.SaveChangesAsync();

                return Ok(category);
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
        public async Task<IActionResult> PutAsync(int id, [FromBody] Category category)
        {
            try
            {
                var entity = await _context.Categories.FindAsync(id);

                if (entity.IsNull())
                {
                    return NotFound();
                }

                entity.Title = category.Title;

                await _context.SaveChangesAsync();

                return Ok(entity);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion
    }
}
