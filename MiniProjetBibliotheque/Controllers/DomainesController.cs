using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiniProjetBibliotheque.Data;
using MiniProjetBibliotheque.Models;

namespace MiniProjetBibliotheque.Controllers
{
    public class DomainesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DomainesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Domaines
        //public async Task<IActionResult> Index()
        //{
        //      return _context.Domaines != null ? 
        //                  View(await _context.Domaines.ToListAsync()) :
        //                  Problem("Entity set 'DbContextBibliotheque.Domaines'  is null.");
        //}
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";


            ViewData["CurrentFilter"] = searchString;

            var domaines = from a in _context.Domaines
                          select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                domaines = domaines.Where(s => s.NomDomaine.Contains(searchString)
                                       || s.Description.Contains(searchString));
            }


            switch (sortOrder)
            {
                case "name_desc":
                    domaines = domaines.OrderBy(s => s.NomDomaine);

                    break;

                default:
                    domaines = domaines.OrderBy(s => s.Description);
                    break;
            }
            return View(await domaines.ToListAsync());
        }

        // GET: Domaines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Domaines == null)
            {
                return NotFound();
            }

            var domaine = await _context.Domaines
                .Include(s => s.LivresDomaine)
                .FirstOrDefaultAsync(m => m.DomaineId == id);
            if (domaine == null)
            {
                return NotFound();
            }

            return View(domaine);
        }

        // GET: Domaines/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Domaines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("DomaineId,NomDomaine,Description")] Domaine domaine)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(domaine);
                await _context.SaveChangesAsync();

                TempData["AlertMessage"] = "Domaine ajouté avec succès";
                return RedirectToAction(nameof(Index));
            }
            return View(domaine);
        }

        // GET: Domaines/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Domaines == null)
            {
                return NotFound();
            }

            var domaine = await _context.Domaines.FindAsync(id);
            if (domaine == null)
            {
                return NotFound();
            }
            return View(domaine);
        }

        // POST: Domaines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("DomaineId,NomDomaine,Description")] Domaine domaine)
        {
            if (id != domaine.DomaineId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(domaine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DomaineExists(domaine.DomaineId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["AlertMessage"] = "Domaine modifié avec succès";
                return RedirectToAction(nameof(Index));
            }
            return View(domaine);
        }

        // GET: Domaines/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Domaines == null)
            {
                return NotFound();
            }

            var domaine = await _context.Domaines
                .FirstOrDefaultAsync(m => m.DomaineId == id);
            if (domaine == null)
            {
                return NotFound();
            }

            return View(domaine);
        }

        // POST: Domaines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Domaines == null)
            {
                return Problem("Entity set 'DbContextBibliotheque.Domaines'  is null.");
            }
            var domaine = await _context.Domaines.FindAsync(id);
            if (domaine != null)
            {
                _context.Domaines.Remove(domaine);
            }
            
            await _context.SaveChangesAsync();
            TempData["AlertMessage"] = "Domaine supprimé avec succès";
            return RedirectToAction(nameof(Index));
        }

        private bool DomaineExists(int id)
        {
          return (_context.Domaines?.Any(e => e.DomaineId == id)).GetValueOrDefault();
        }
    }
}
