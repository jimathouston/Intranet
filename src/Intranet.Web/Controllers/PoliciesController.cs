using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Intranet.Web.Domain.Data;
using Intranet.Web.Domain.Models.Entities;
using Intranet.Web.Common.Helpers;
using Intranet.Web.ViewModels;
using Intranet.Web.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Intranet.Web.Domain.Helpers;
using Intranet.Services.FileStorageService;

namespace Intranet.Web.Controllers
{
    public class PoliciesController : Controller
    {
        private readonly IntranetApiContext _context;
        private readonly IFileStorageService _fileStorageService;

        public PoliciesController(IntranetApiContext context, IFileStorageService fileStorageService)
        {
            _context = context;    
            _fileStorageService = fileStorageService;
        }

        #region GET
        // GET: Policies
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                    .Include(c => c.Policies)
                        .ThenInclude(f => f.PolicyTags)
                            .ThenInclude(pt => pt.Tag)
                    .ToListAsync() ?? new List<Category>();

            return View(categories);
        }

        // GET: Policies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies
                .Include(f => f.Category)
                .Include(f => f.PolicyTags)
                    .ThenInclude(pt => pt.Tag)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (policy.IsNull())
            {
                return NotFound();
            }

            return View(new PolicyViewModel(policy));
        }
        #endregion

        #region CREATE
        // GET: Policies/Create
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

            var vm = new PolicyViewModel();
            vm.Category = new Category();
            return View(vm);
        }

        // POST: Policies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize("IsAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Category,Title,Description,Tags")] PolicyViewModel policyVM)
        {
            if (ModelState.IsValid)
            {
                var category = await _context
                    .Categories
                    .SingleOrDefaultAsync(c => c.Title.Equals(policyVM.Category.Title, StringComparison.OrdinalIgnoreCase));

                if (category.IsNotNull())
                {
                    policyVM.Category = category;
                }
                else
                {
                    policyVM.Category = new Category
                    {
                        Title = policyVM.Category.Title,
                        Url = CustomUrlHelper.URLFriendly(policyVM.Category.Title),
                    };
                }

                var policy = new Policy
                {
                    Category = policyVM.Category,
                    Description = policyVM.Description,
                    FileUrl = policyVM.FileUrl,
                    Title = policyVM.Title,
                    Url = CustomUrlHelper.URLFriendly(policyVM.Title),
                };

                var file = Request.Form.Files.SingleOrDefault();

                if (file.IsNotNull())
                {
                    var filename = await _fileStorageService.SetBlobAsync(file);
                    policy.FileUrl = filename;
                }

                var tags = TagHelpers.GetTagsFromString(policyVM.Tags);
                var allTagEntities = GetAllTagEntitiesInternal(policyVM, tags);
                TagHelpers.SetTags<Policy, PolicyTag>(tags, policy, allTagEntities);

                await _context.AddAsync(policy);

                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { Id = policy.Id });
            }
            ViewData["Categories"] = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Title));
            return View(policyVM);
        } 
        #endregion

        #region UPDATE
        // GET: Policies/Edit/5
        [Authorize("IsAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies
                .Include(f => f.Category)
                .Include(f => f.PolicyTags)
                    .ThenInclude(pt => pt.Tag)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (policy == null)
            {
                return NotFound();
            }

            ViewData["Categories"] = new SelectList(_context.Categories, nameof(Category.Title), nameof(Category.Title), policy.Category.Title);

            return View(new PolicyViewModel(policy));
        }

        // POST: Policies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize("IsAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Category,Title,Description,Tags")] PolicyViewModel policyVM)
        {
            if (id != policyVM?.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
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
                            Url = CustomUrlHelper.URLFriendly(policyVM.Category.Title),
                        };
                    }
                }

                entity.Title = policyVM.Title;
                entity.Description = policyVM.Description;

                var file = Request.Form.Files.SingleOrDefault();

                if (file.IsNotNull())
                {
                    var filename = await _fileStorageService.SetBlobAsync(file);
                    entity.FileUrl =  filename;
                }

                var tags = TagHelpers.GetTagsFromString(policyVM.Tags);
                var allTagEntities = GetAllTagEntitiesInternal(policyVM, tags);
                TagHelpers.SetTags<Policy, PolicyTag>(tags, entity, allTagEntities);

                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { Id = entity.Id });
            }
            return View(policyVM);
        } 
        #endregion

        #region DELETE
        // GET: Policies/Delete/5
        [Authorize("IsAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies
                .Include(f => f.Category)
                .Include(f => f.PolicyTags)
                    .ThenInclude(pt => pt.Tag)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (policy == null)
            {
                return NotFound();
            }

            return View(new PolicyViewModel(policy));
        }

        // POST: Policies/Delete/5
        [Authorize("IsAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var policy = await _context.Policies
                    .Include(p => p.PolicyTags)
                        .ThenInclude(pt => pt.Tag)
                    .Include(p => p.Category)
                        .ThenInclude(c => c.Policies)
                    .SingleOrDefaultAsync(p => p.Id == id);

            if (policy.IsNull())
            {
                return NotFound();
            }

            // TODO: Delete blob as well

            _context.Remove(policy);

            // If the category has no more related entities then delete the category as well
            if (await CategoryHelpers.HasNoRelatedEntities(policy.Category, _context, ignore: policy))
            {
                _context.Remove(policy.Category);
            }

            // If the tags has no more related entities then delete the tag as well
            foreach (var tag in policy.PolicyTags.Select(ft => ft.Tag))
            {
                if (await TagHelpers.HasNoRelatedEntities(tag, _context, ignore: policy.PolicyTags))
                {
                    _context.Remove(tag);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        } 
        #endregion

        #region Private Helpers
        private List<Tag> GetAllTagEntitiesInternal(PolicyViewModel policy, IEnumerable<string> tags)
        {
            if (tags.IsNull())
            {
                return new List<Tag>();
            }

            return _context.Tags?
            .Include(k => k.PolicyTags)
                .ThenInclude(pt => pt.Policy)?
            .Where(k => tags.Contains(k.Name, StringComparer.OrdinalIgnoreCase) || k.PolicyTags.Any(nt => nt.PolicyId.Equals(policy.Id)))
            .ToList();
        }

        private bool PolicyExists(int id)
        {
            return _context.Policies.Any(e => e.Id == id);
        }
        #endregion
    }
}
