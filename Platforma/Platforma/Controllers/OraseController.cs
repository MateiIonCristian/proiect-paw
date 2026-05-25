using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platforma.Data;
using Platforma.Models;

namespace Platforma.Controllers
{
    public class OraseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OraseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orase
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orase.ToListAsync());
        }

        // GET: Orase/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orase/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nume")] Oras oras)
        {
            if (ModelState.IsValid)
            {
                _context.Add(oras);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(oras);
        }

        // GET: Orase/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var oras = await _context.Orase.FindAsync(id);
            if (oras == null) return NotFound();

            return View(oras);
        }

        // POST: Orase/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nume")] Oras oras)
        {
            if (id != oras.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(oras);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrasExists(oras.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(oras);
        }

        // GET: Orase/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var oras = await _context.Orase
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oras == null) return NotFound();

            return View(oras);
        }

        // POST: Orase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oras = await _context.Orase.FindAsync(id);
            if (oras != null)
            {
                _context.Orase.Remove(oras);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrasExists(int id)
        {
            return _context.Orase.Any(e => e.Id == id);
        }
    }
}
