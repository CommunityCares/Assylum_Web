using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CommunityCares_Web.Data;
using CommunityCares_Web.Models;

namespace CommunityCares_Web.Controllers
{
    public class CollectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CollectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Collect
        public async Task<IActionResult> Index(int id)
        {
            var applicationDbContext = _context.Collects.Include(c => c.IdCampaignNavigation).Where(x =>x.IdCampaign ==id);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Collect/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Collects == null)
            {
                return NotFound();
            }

            var collect = await _context.Collects
                .Include(c => c.IdCampaignNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collect == null)
            {
                return NotFound();
            }

            return View(collect);
        }

        // GET: Collect/Create
        public IActionResult Create()
        {
            ViewData["IdCampaign"] = new SelectList(_context.Campaigns, "Id", "Id");
            return View();
        }

        // POST: Collect/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id,[Bind("Date,IdCampaign")] Collect collect)
        {
            if (ModelState.IsValid)
            {
                collect.IdCampaign = id;
                _context.Add(collect);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCampaign"] = new SelectList(_context.Campaigns, "Id", "Id", collect.IdCampaign);
            return View(collect);
        }

        // GET: Collect/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Collects == null)
            {
                return NotFound();
            }

            var collect = await _context.Collects.FindAsync(id);
            if (collect == null)
            {
                return NotFound();
            }
            ViewData["IdCampaign"] = new SelectList(_context.Campaigns, "Id", "Id", collect.IdCampaign);
            return View(collect);
        }

        // POST: Collect/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,IdCampaign")] Collect collect)
        {
            if (id != collect.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(collect);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollectExists(collect.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCampaign"] = new SelectList(_context.Campaigns, "Id", "Id", collect.IdCampaign);
            return View(collect);
        }

        // GET: Collect/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Collects == null)
            {
                return NotFound();
            }

            var collect = await _context.Collects
                .Include(c => c.IdCampaignNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collect == null)
            {
                return NotFound();
            }

            return View(collect);
        }

        // POST: Collect/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Collects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Collects'  is null.");
            }
            var collect = await _context.Collects.FindAsync(id);
            if (collect != null)
            {
                _context.Collects.Remove(collect);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CollectExists(int id)
        {
          return (_context.Collects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
