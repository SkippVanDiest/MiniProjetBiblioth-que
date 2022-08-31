using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiniProjetBibliotheque.Data;
using MiniProjetBibliotheque.Models;

namespace MiniProjetBibliotheque.Controllers
{
    public class LecteursController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LecteursController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lecteurs
        //public async Task<IActionResult> Index()
        //{
        //      return _context.Lecteurs != null ? 
        //                  View(await _context.Lecteurs.ToListAsync()) :
        //                  Problem("Entity set 'DbContextBibliotheque.Lecteurs'  is null.");
        //}
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";


            ViewData["CurrentFilter"] = searchString;

            var lecteurs = from a in _context.Lecteurs
                          select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                lecteurs = lecteurs.Where(s => s.NomLecteur.Contains(searchString)
                                       || s.PrenomLecteur.Contains(searchString));
            }


            switch (sortOrder)
            {
                case "name_desc":
                    lecteurs = lecteurs.OrderBy(s => s.NomLecteur);

                    break;

                default:
                    lecteurs = lecteurs.OrderBy(s => s.PrenomLecteur);
                    break;
            }
            return View(await lecteurs.ToListAsync());
        }


        // GET: Lecteurs/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Lecteurs == null)
            {
                return NotFound();
            }

            var lecteur = await _context.Lecteurs
                .Include(s => s.EmpruntsLecteur)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lecteur == null)
            {
                return NotFound();
            }

            return View(lecteur);
        }

        // GET: Lecteurs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lecteurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomLecteur,PrenomLecteur,Email,Telephone,Adresse,PasswordHash")] Lecteur lecteur)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(lecteur);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "Lecteur ajouté avec succès";
                return RedirectToAction(nameof(Index));
            }
            return View(lecteur);
        }

        // GET: Lecteurs/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Lecteurs == null)
            {
                return NotFound();
            }

            var lecteur = await _context.Lecteurs.FindAsync(id);
            if (lecteur == null)
            {
                return NotFound();
            }
            return View(lecteur);
        }

        // POST: Lecteurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,NomLecteur,PrenomLecteur,Email,PhoneNumber,Adresse,PasswordHash")] Lecteur lecteur)
        {
            if (id != lecteur.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(lecteur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LecteurExists(lecteur.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["AlertMessage"] = "Lecteur modifié avec succès";
                return RedirectToAction(nameof(Index));
            }
            return View(lecteur);
        }

        // GET: Lecteurs/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Lecteurs == null)
            {
                return NotFound();
            }

            var lecteur = await _context.Lecteurs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lecteur == null)
            {
                return NotFound();
            }

            return View(lecteur);
        }

        // POST: Lecteurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Lecteurs == null)
            {
                return Problem("Entity set 'DbContextBibliotheque.Lecteurs'  is null.");
            }
            var lecteur = await _context.Lecteurs.FindAsync(id);
            if (lecteur != null)
            {
                _context.Lecteurs.Remove(lecteur);
            }
            
            await _context.SaveChangesAsync();
            TempData["AlertMessage"] = "Lecteur supprimé avec succès";
            return RedirectToAction(nameof(Index));
        }

        private bool LecteurExists(string id)
        {
          return (_context.Lecteurs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
