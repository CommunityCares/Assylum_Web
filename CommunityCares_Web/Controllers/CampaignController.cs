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
    public class CampaignController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CampaignController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Campaign
        public async Task<IActionResult> Index(int id)
        {
            var Campaign = _context.Campaigns.Where(x => x.Status == 1).Where(x => x.IdAssylum == id);
            return View(Campaign.ToList());
        }

        public async Task<IActionResult> Index2()
        {
            var Campaign = _context.Campaigns.Include(x=>x.IdAssylumNavigation).Where(x => x.Status == 1);
            return View(Campaign.ToList());
        }
        public async Task<IActionResult> Index3()
        {
            var Campaign = _context.Campaigns.Include(x => x.IdAssylumNavigation).Where(x => x.Status == 2);
            return View(Campaign.ToList());
        }

        // GET: Campaign/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Campaigns == null)
            {
                return NotFound();
            }

            var campaign = await _context.Campaigns
                .Include(c => c.IdAssylumNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campaign == null)
            {
                return NotFound();
            }

            return View(campaign);
        }

        // GET: Campaign/Create
        public IActionResult Create()
        {
            ViewData["IdAssylum"] = new SelectList(_context.Assylums, "Id", "Id");
            return View();
        }

        // POST: Campaign/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(byte id,[Bind("Name,Requirement,InitialDate,CloseDate,Status,RegisterDate,IdAssylum")] Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                campaign.RegisterDate = DateTime.Now;
                campaign.IdAssylum = id;
                _context.Add(campaign);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAssylum"] = new SelectList(_context.Assylums, "Id", "Id", campaign.IdAssylum);
            return View(campaign);
        }

        // GET: Campaign/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Campaigns == null)
            {
                return NotFound();
            }

            var campaign = await _context.Campaigns.FindAsync(id);
            if (campaign == null)
            {
                return NotFound();
            }
            ViewData["IdAssylum"] = new SelectList(_context.Assylums, "Id", "Id", campaign.IdAssylum);
            return View(campaign);
        }

        // POST: Campaign/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Requirement,InitialDate,CloseDate,Status,RegisterDate,IdAssylum")] Campaign campaign)
        {
            if (id != campaign.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(campaign);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CampaignExists(campaign.Id))
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
            ViewData["IdAssylum"] = new SelectList(_context.Assylums, "Id", "Id", campaign.IdAssylum);
            return View(campaign);
        }



        // GET: Campaign/Edit/5
        public async Task<IActionResult> Edit2(int? id)
        {
            if (id == null || _context.Campaigns == null)
            {
                return NotFound();
            }

            var campaign = await _context.Campaigns.FindAsync(id);
            if (campaign == null)
            {
                return NotFound();
            }
            ViewData["IdAssylum"] = new SelectList(_context.Assylums, "Id", "Id", campaign.IdAssylum);
            return View(campaign);
        }

        // POST: Campaign/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit2(int id)
        {

            var camp = await _context.Campaigns.FirstAsync(x => x.Id == id);

            camp.Status = 2;
            _context.Update(camp);
            await _context.SaveChangesAsync();





           
            return RedirectToAction("Index" , "Assylum");
        }
        // GET: Campaign/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Campaigns == null)
            {
                return NotFound();
            }

            var campaign = await _context.Campaigns
                .Include(c => c.IdAssylumNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campaign == null)
            {
                return NotFound();
            }

            return View(campaign);
        }

        // POST: Campaign/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Campaigns == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Campaigns'  is null.");
            }
            var campaign = await _context.Campaigns.FindAsync(id);
            if (campaign != null)
            {
                _context.Campaigns.Remove(campaign);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampaignExists(int id)
        {
          return (_context.Campaigns?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
