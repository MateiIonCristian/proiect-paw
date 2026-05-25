/*
 * DESCRIERE:
 * Acest controller expune operațiile CRUD pentru recenziile postate de utilizatori.
 * Autentificarea este obligatorie pentru a adăuga recenzii la firme.
 * Operațiile de editare și ștergere ale recenziilor existente sunt restricționate utilizatorilor cu rolul de Administrator.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using recenzi_pentru_firme.Models;
using recenzi_pentru_firme.Services;

namespace recenzi_pentru_firme.Controllers
{
    [Authorize]
    public class RecenziiController : Controller
    {
        private readonly RecenzieService _service;
        private readonly FirmaService _firmaService;
        public RecenziiController(RecenzieService service, FirmaService firmaService)
        {
            _service = service;
            _firmaService = firmaService;
        }

        public async Task<IActionResult> Index() => View(await _service.GetToateRecenziile());

        public async Task<IActionResult> Create(int? firmaId)
        {
            await PopulareFirme(firmaId);
            var rec = new Recenzie { Autor = User.Identity?.Name ?? "", FirmaId = firmaId ?? 0 };
            return View(rec);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Recenzie rec)
        {
            rec.Autor = User.Identity?.Name ?? rec.Autor;
            ModelState.Remove("Autor");
            
            if (ModelState.IsValid) { await _service.AdaugaRecenzie(rec); return RedirectToAction(nameof(Index)); }
            await PopulareFirme(rec.FirmaId);
            return View(rec);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var rec = await _service.GetRecenzieById(id);
            if (rec == null) return NotFound();
            await PopulareFirme(rec.FirmaId);
            return View(rec);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, Recenzie rec)
        {
            if (id != rec.Id) return NotFound();
            if (ModelState.IsValid) { await _service.UpdateRecenzie(rec); return RedirectToAction(nameof(Index)); }
            await PopulareFirme(rec.FirmaId);
            return View(rec);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var rec = await _service.GetRecenzieById(id);
            if (rec == null) return NotFound();
            return View(rec);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.StergeRecenzie(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulareFirme(object selectedFirma = null)
        {
            ViewBag.Firme = new SelectList(await _firmaService.GetToateFirmele(), "Id", "Nume", selectedFirma);
        }
    }
}
