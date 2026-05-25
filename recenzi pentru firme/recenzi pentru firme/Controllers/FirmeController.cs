/*
  --------------------------------------------------------------------------------
  DESCRIERE: Controller-ul principal pentru gestionarea firmelor din aplicatie.
             - Extrage orasele si categoriile sub forma de SelectList pentru formulare.
             - Permite crearea, editarea, listarea si stergerea firmelor inregistrate.
             - Restrictionaza actiunile administrative (Creare/Editare/Stergere) pe baza de roluri.
  ---------------
Gestionează catalogul de firme, adăugarea/editarea/ștergerea lor,
detaliile fiecărei firme (inclusiv serviciile și datele de contact)
și funcționalitatea de căutare dinamică (AJAX).
-----------------------------------------------------------------
*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using recenzi_pentru_firme.Models;
using recenzi_pentru_firme.Services;

namespace recenzi_pentru_firme.Controllers
{
    [Authorize]
    public class FirmeController : Controller
    {
        private readonly FirmaService _firmaService;
        private readonly CategorieService _categorieService;
        private readonly OrasService _orasService;

        public FirmeController(FirmaService firmaService, CategorieService categorieService, OrasService orasService)
        {
            _firmaService = firmaService;
            _categorieService = categorieService;
            _orasService = orasService;
        }

        public async Task<IActionResult> Index()
        {
            var firme = await _firmaService.GetToateFirmele();
            return View(firme);
        }

        public async Task<IActionResult> Details(int id)
        {
            var firma = await _firmaService.GetFirmaById(id);
            if (firma == null) return NotFound();
            return View(firma);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create()
        {
            await PopulareDropDowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(Firma firma)
        {
            if (ModelState.IsValid)
            {
                await _firmaService.AdaugaFirma(firma);
                return RedirectToAction(nameof(Index));
            }
            await PopulareDropDowns(firma.CategorieId, firma.OrasId);
            return View(firma);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var firma = await _firmaService.GetFirmaSimpla(id);
            if (firma == null) return NotFound();
            
            await PopulareDropDowns(firma.CategorieId, firma.OrasId);
            return View(firma);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, Firma firma)
        {
            if (id != firma.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _firmaService.UpdateFirma(firma);
                return RedirectToAction(nameof(Index));
            }
            await PopulareDropDowns(firma.CategorieId, firma.OrasId);
            return View(firma);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var firma = await _firmaService.GetFirmaById(id);
            if (firma == null) return NotFound();
            return View(firma);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _firmaService.StergeFirma(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<JsonResult> Search(string term)
        {
            var results = await _firmaService.CautaFirme(term);
            var simplifiedResults = results.Select(f => new { f.Id, f.Nume, f.Adresa, f.Descriere });
            return Json(simplifiedResults);
        }

        private async Task PopulareDropDowns(object selectedCategorie = null, object selectedOras = null)
        {
            ViewBag.Categorii = new SelectList(await _categorieService.GetToateCategoriile(), "Id", "Nume", selectedCategorie);
            ViewBag.Orase = new SelectList(await _orasService.GetToateOrasele(), "Id", "Nume", selectedOras);
        }
    }
}
