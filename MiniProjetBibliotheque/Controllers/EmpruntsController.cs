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
    public class EmpruntsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpruntsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Emprunts
        //public async Task<IActionResult> Index()
        //{
        //    var dbContextBibliotheque = _context.Emprunts.Include(e => e.Lecteur).Include(e => e.Livre);
        //    return View(await dbContextBibliotheque.ToListAsync());
        //}
        [Authorize]
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var emprunts = from a in _context.Emprunts.Include(e => e.Lecteur).Include(e => e.Livre)
                           select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                emprunts = emprunts.Where(s => s.Livre.Titre.Contains(searchString));
            }

            return View(await emprunts.ToListAsync());
        }

        // GET: Emprunts/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Emprunts == null)
            {
                return NotFound();
            }

            var emprunt = await _context.Emprunts
                .Include(e => e.Lecteur)
                .Include(e => e.Livre)
                .FirstOrDefaultAsync(m => m.EmpruntId == id);
            if (emprunt == null)
            {
                return NotFound();
            }

            return View(emprunt);
        }

        // GET: Emprunts/Create
        [Authorize]
        public IActionResult Create()
        {
            //ViewData["Id"] = new SelectList(_context.Users, "Id", "NomLecteur");
            ViewData["LivreId"] = new SelectList(_context.Livres, "LivreId", "Titre");
            return View();
        }

        // POST: Emprunts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("EmpruntId,Id,LivreId,DateEmprunt,DateRetour")] Emprunt emprunt)
        {
            if (!ModelState.IsValid)
            { 
                if(emprunt.DateRetour > emprunt.DateEmprunt)
                {
                    _context.Add(emprunt);
                    await _context.SaveChangesAsync();
                    TempData["AlertMessage"] = "Emprunt ajoutée avec succès";
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    TempData["AlertErreur"] = "La date de retour doit etre supérieur de la date d'emprunt ";


                }
                
            }
            //ViewData["Id"] = new SelectList(_context.Lecteurs, "Id", "NomLecteur", emprunt.Id);
            ViewData["LivreId"] = new SelectList(_context.Livres, "LivreId", "Titre", emprunt.LivreId);
            return View(emprunt);
        }

        // GET: Emprunts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Emprunts == null)
            {
                return NotFound();
            }

            var emprunt = await _context.Emprunts.FindAsync(id);
            if (emprunt == null)
            {
                return NotFound();
            }
            //ViewData["Id"] = new SelectList(_context.Lecteurs, "Id", "NomLecteur", emprunt.Id);
            ViewData["LivreId"] = new SelectList(_context.Livres, "LivreId", "Titre", emprunt.LivreId);
            return View(emprunt);
        }

        // POST: Emprunts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("EmpruntId,Id,LivreId,DateEmprunt,DateRetour")] Emprunt emprunt)
        {
            if (id != emprunt.EmpruntId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(emprunt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpruntExists(emprunt.EmpruntId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["AlertMessage"] = "Emprunt modifiée avec succès";
                return RedirectToAction(nameof(Index));
            }
            //ViewData["Id"] = new SelectList(_context.Lecteurs, "Id", "NomLecteur", emprunt.Id);
            ViewData["LivreId"] = new SelectList(_context.Livres, "LivreId", "Titre", emprunt.LivreId);
            return View(emprunt);
        }

        // GET: Emprunts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Emprunts == null)
            {
                return NotFound();
            }

            var emprunt = await _context.Emprunts
                .Include(e => e.Lecteur)
                .Include(e => e.Livre)
                .FirstOrDefaultAsync(m => m.EmpruntId == id);
            if (emprunt == null)
            {
                return NotFound();
            }

            return View(emprunt);
        }

        // POST: Emprunts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Emprunts == null)
            {
                return Problem("Entity set 'DbContextBibliotheque.Emprunts'  is null.");
            }
            var emprunt = await _context.Emprunts.FindAsync(id);
            if (emprunt != null)
            {
                _context.Emprunts.Remove(emprunt);
            }
            
            await _context.SaveChangesAsync();
            TempData["AlertMessage"] = "Emprunt supprimée avec succès";
            return RedirectToAction(nameof(Index));
        }

        private bool EmpruntExists(int id)
        {
          return (_context.Emprunts?.Any(e => e.EmpruntId == id)).GetValueOrDefault();
        }
    }
}
