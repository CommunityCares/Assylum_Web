using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CommunityCares_Web.Data;
using CommunityCares_Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace CommunityCares_Web.Controllers
{
    public class AssylumController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssylumController(ApplicationDbContext context)
        {
            _context = context;
        }
        //[Authorize(Roles ="Admin")]
        // GET: Assylum
        public async Task<IActionResult> Index()
        {
            var assylum = _context.Assylums.Where(x => x.Status == 1);
            return View(assylum.ToList());
        }

        // GET: Assylum/Details/5
        public async Task<IActionResult> Details(byte? id)
        {
            if (id == null || _context.Assylums == null)
            {
                return NotFound();
            }

            var assylum = await _context.Assylums
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assylum == null)
            {
                return NotFound();
            }

            return View(assylum);
        }

        // GET: Assylum/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Assylum/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Nit,RepresentativeName,BussinessEmail,CellphoneNumber,Address,Latitude,Longitude,Status,RegisterDate,Region")] Assylum assylum)
        {
            if (ModelState.IsValid)
            {
                assylum.RegisterDate = DateTime.Now;
                _context.Add(assylum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(assylum);
        }

        // GET: Assylum/Edit/5
        public async Task<IActionResult> Edit(byte? id)
        {
            if (id == null || _context.Assylums == null)
            {
                return NotFound();
            }

            var assylum = await _context.Assylums.FindAsync(id);
            if (assylum == null)
            {
                return NotFound();
            }
            return View(assylum);
        }

        // POST: Assylum/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(byte id, [Bind("Id,Name,Nit,RepresentativeName,BussinessEmail,CellphoneNumber,Address,Latitude,Longitude,Status,RegisterDate,Region")] Assylum assylum)
        {
            if (id != assylum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assylum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssylumExists(assylum.Id))
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
            return View(assylum);
        }

        // GET: Assylum/Delete/5
        public async Task<IActionResult> Delete(byte? id)
        {
            if (id == null || _context.Assylums == null)
            {
                return NotFound();
            }

            var assylum = await _context.Assylums
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assylum == null)
            {
                return NotFound();
            }

            return View(assylum);
        }

        // POST: Assylum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(byte id)
        {
            if (_context.Assylums == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Assylums'  is null.");
            }
            var assylum = await _context.Assylums.FindAsync(id);
            if (assylum != null)
            {
                _context.Assylums.Remove(assylum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssylumExists(byte id)
        {
          return (_context.Assylums?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
