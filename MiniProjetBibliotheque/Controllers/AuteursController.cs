using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiniProjetBibliotheque.Data;
using MiniProjetBibliotheque.Models;

namespace MiniProjetBibliotheque.Controllers
{
    public class AuteursController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuteursController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Auteurs
        //public async Task<IActionResult> Index()
        //{
        //      return _context.Auteurs != null ? 
        //                  View(await _context.Auteurs.ToListAsync()) :
        //                  Problem("Entity set 'DbContextBibliotheque.Auteurs'  is null.");
        //}
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";


            ViewData["CurrentFilter"] = searchString;

            var auteurs = from a in _context.Auteurs
                          select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                auteurs = auteurs.Where(s => s.NomAuteur.Contains(searchString)
                                       || s.PrenomAuteur.Contains(searchString));
            }


            switch (sortOrder)
            {
                case "name_desc":
                    auteurs = auteurs.OrderBy(s => s.NomAuteur);
                    
                    break;

                default:
                    auteurs = auteurs.OrderBy(s => s.PrenomAuteur);
                    break;
            }
            return View(await auteurs.ToListAsync());
        }

         

        // GET: Auteurs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Auteurs == null)
            {
                return NotFound();
            }

            var auteur = await _context.Auteurs
                .Include(s => s.LivresAuteur)
                .FirstOrDefaultAsync(m => m.AuteurId == id);
            if (auteur == null)
            {
                return NotFound();
            }

            return View(auteur);
        }

        // GET: Auteurs/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Auteurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        /* Method for displaying the Message Box */

        public async Task<IActionResult> Create([Bind("AuteurId,NomAuteur,PrenomAuteur,EmailAuteur,TelephoneAuteur,Grade")] Auteur auteur)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(auteur);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "Auteur ajouté avec succès";

                return RedirectToAction(nameof(Index));
            }
            return View(auteur);
        }

        private void MsgBox(string v, object page, AuteursController auteursController)
        {
            throw new NotImplementedException();
        }

        // GET: Auteurs/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Auteurs == null)
            {
                return NotFound();
            }

            var auteur = await _context.Auteurs.FindAsync(id);
            if (auteur == null)
            {
                return NotFound();
            }
           
            return View(auteur);
        }

        // POST: Auteurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("AuteurId,NomAuteur,PrenomAuteur,EmailAuteur,TelephoneAuteur,Grade")] Auteur auteur)
        {
            if (id != auteur.AuteurId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(auteur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuteurExists(auteur.AuteurId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["AlertMessage"] = "Auteur modifié avec succès";
                return RedirectToAction(nameof(Index));
            }
            return View(auteur);
        }

        // GET: Auteurs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Auteurs == null)
            {
                return NotFound();
            }

            var auteur = await _context.Auteurs
                .FirstOrDefaultAsync(m => m.AuteurId == id);
            if (auteur == null)
            {
                return NotFound();
            }

            return View(auteur);
        }

        // POST: Auteurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Auteurs == null)
            {
                return Problem("Entity set 'DbContextBibliotheque.Auteurs'  is null.");
            }
            var auteur = await _context.Auteurs.FindAsync(id);
            if (auteur != null)
            {
                _context.Auteurs.Remove(auteur);
            }
            
            await _context.SaveChangesAsync();
            TempData["AlertMessage"] = "Auteur supprimé avec succès";
            return RedirectToAction(nameof(Index));
        }

        private bool AuteurExists(int id)
        {
          return (_context.Auteurs?.Any(e => e.AuteurId == id)).GetValueOrDefault();
        }
    }
}
