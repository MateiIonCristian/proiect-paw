using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Platforma.Data;
using Platforma.Models;

namespace Platforma.Controllers
{
    public class FirmeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FirmeController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> Index() {
            var firme = await _context.Firme.Include(f => f.Recenzii).Include(f => f.Categorie).ToListAsync();
            return View(firme);
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null) return NotFound();
            var firma = await _context.Firme.Include(f => f.Recenzii).Include(f => f.Categorie).FirstOrDefaultAsync(m => m.Id == id);
            if (firma == null) return NotFound();
            return View(firma);
        }

        public async Task<IActionResult> Create() {
            ViewBag.Categorii = new SelectList(await _context.Categorii.ToListAsync(), "Id", "Nume");
            ViewBag.Orase = new SelectList(await _context.Orase.ToListAsync(), "Id", "Nume");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nume,Descriere,Adresa,CategorieId,OrasId")] Firma firma) {
            if (ModelState.IsValid) {
                _context.Add(firma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categorii = new SelectList(await _context.Categorii.ToListAsync(), "Id", "Nume", firma.CategorieId);
            ViewBag.Orase = new SelectList(await _context.Orase.ToListAsync(), "Id", "Nume", firma.OrasId);
            return View(firma);
        }

        public async Task<IActionResult> Edit(int? id) {
            if (id == null) return NotFound();
            var firma = await _context.Firme.FindAsync(id);
            if (firma == null) return NotFound();
            ViewBag.Categorii = new SelectList(await _context.Categorii.ToListAsync(), "Id", "Nume", firma.CategorieId);
            ViewBag.Orase = new SelectList(await _context.Orase.ToListAsync(), "Id", "Nume", firma.OrasId);
            return View(firma);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nume,Descriere,Adresa,CategorieId,OrasId")] Firma firma) {
            if (id != firma.Id) return NotFound();
            if (ModelState.IsValid) {
                _context.Update(firma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(firma);
        }

        public async Task<IActionResult> Delete(int? id) {
            var firma = await _context.Firme.FirstOrDefaultAsync(m => m.Id == id);
            return View(firma);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var firma = await _context.Firme.FindAsync(id);
            if (firma != null) { _context.Firme.Remove(firma); await _context.SaveChangesAsync(); }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddRecenzie() {
            ViewBag.Firme = await _context.Firme.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRecenzie(int firmaId, int nota, string comentariu) {
            var recenzie = new Recenzie { FirmaId = firmaId, Nota = nota, Comentariu = comentariu, DataPublicarii = DateTime.Now };
            _context.Recenzii.Add(recenzie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = firmaId });
        }
    }
}