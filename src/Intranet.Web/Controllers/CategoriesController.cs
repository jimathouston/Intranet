using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Intranet.Web.Domain.Data;
using Intranet.Web.Domain.Models.Entities;
using Intranet.Web.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Intranet.Web.Common.Helpers;

namespace Intranet.Web.Controllers
{
    [Authorize("IsAdmin")]
    public class CategoriesController : Controller
    {
        private readonly IntranetApiContext _context;

        public CategoriesController(IntranetApiContext context)
        {
            _context = context;
        }

        #region GET
        // GET: Categories
        public async Task<IActionResult> Index([FromQuery] string from = null)
        {
            var categories = await _context.Categories
                .Include(m => m.Faqs)
                .ToListAsync();

            ViewData["from"] = from;

            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id, [FromQuery] string from = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                    .Include(f => f.Faqs)
                    .SingleOrDefaultAsync(m => m.Id == id);

            if (category.IsNull())
            {
                return NotFound();
            }

            ViewData["from"] = from;

            return View(category);
        }
        #endregion

        #region CREATE
        // GET: Categories/Create
        public IActionResult Create([FromQuery] string from = null)
        {
            ViewData["from"] = from;

            return View(new Category());
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] Category category, [FromQuery] string from = null)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = await _context
                    .Categories
                    .SingleOrDefaultAsync(c => c.Title.Equals(category.Title, StringComparison.OrdinalIgnoreCase));

                if (existingCategory.IsNotNull())
                {
                    ModelState.AddModelError("Error", $"A category with the title \"{category.Title}\" already exists.");
                    return View(existingCategory);
                }

                category.Url = CustomUrlHelper.URLFriendly(category.Title);

                _context.Categories.Add(category);

                await _context.SaveChangesAsync();

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { Id = category.Id, from });
            }
            ViewData["from"] = from;
            return View(category);
        }
        #endregion

        #region EDIT
        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromQuery] string from = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(m => m.Faqs)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (category.IsNull())
            {
                return NotFound();
            }

            ViewData["from"] = from;

            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] Category category, [FromQuery] string from = null)
        {
            if (id != category?.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var entity = await _context.Categories.FindAsync(id);

                if (entity.IsNull())
                {
                    return NotFound();
                }

                entity.Title = category.Title;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.Message.Contains("Cannot insert duplicate key row in object 'dbo.Category' with unique index 'IX_Category_Title'."))
                    {
                        ModelState.AddModelError("Error", $"A category with the title \"{category.Title}\" already exists.");
                        return View(category);
                    }
                    throw;
                }
                
                return RedirectToAction("Details", new { id, from });
            }
            ViewData["from"] = from;
            return View(category);
        }
        #endregion

        #region DELETE
        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id, [FromQuery] string from = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(m => m.Faqs)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (category.IsNull())
            {
                return NotFound();
            }

            if (category.Faqs.Any())
            {
                ModelState.AddModelError("Error", "Must remove all FAQ's first!");
            }

            ViewData["from"] = from;

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, [FromQuery] string from = null)
        {
            var category = await _context.Categories
                .Include(m => m.Faqs)
                .SingleOrDefaultAsync(f => f.Id == id);

            if (category.IsNull())
            {
                return NotFound();
            }

            if (category.Faqs.Any())
            {
                ModelState.AddModelError("Error", "Must remove all FAQ's first!");
                return View(category);
            }

            _context.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { from });
        }
        #endregion

        #region Private Helpers
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
        #endregion
    }
}
