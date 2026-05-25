using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platforma.Data;
using Platforma.Models;

namespace Platforma.Controllers
{
    public class CategoriiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categorii
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categorii.ToListAsync());
        }

        // GET: Categorii/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorii/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nume")] Categorie categorie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categorie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categorie);
        }

        // GET: Categorii/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var categorie = await _context.Categorii.FindAsync(id);
            if (categorie == null) return NotFound();
            
            return View(categorie);
        }

        // POST: Categorii/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nume")] Categorie categorie)
        {
            if (id != categorie.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categorie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategorieExists(categorie.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categorie);
        }

        // GET: Categorii/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var categorie = await _context.Categorii
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categorie == null) return NotFound();

            return View(categorie);
        }

        // POST: Categorii/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categorie = await _context.Categorii.FindAsync(id);
            if (categorie != null)
            {
                _context.Categorii.Remove(categorie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategorieExists(int id)
        {
            return _context.Categorii.Any(e => e.Id == id);
        }
    }
}
