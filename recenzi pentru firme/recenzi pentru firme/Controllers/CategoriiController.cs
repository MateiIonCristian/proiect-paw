/*
  --------------------------------------------------------------------------------
  DESCRIERE: Controller-ul destinat gestionarii categoriilor de firme.
             Permite adaugarea, modificarea, vizualizarea si stergerea categoriilor,
             folosind serviciul CategorieService si asigurand acces securizat.
Permite vizualizarea, crearea, editarea și ștergerea categoriilor 
în care sunt încadrate firmele.
  --------------------------------------------------------------------------------
*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using recenzi_pentru_firme.Models;
using recenzi_pentru_firme.Services;

namespace recenzi_pentru_firme.Controllers
{
    [Authorize]
    [Route("Administrare/Categorii")]
    public class CategoriiController : Controller
    {
        private readonly CategorieService _service;
        public CategoriiController(CategorieService service) => _service = service;

        [Route("")]
        [Route("Lista")]
        public async Task<IActionResult> Index() => View(await _service.GetToateCategoriile());

        [Route("Nou")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create() => View();

        [HttpPost]
        [Route("Nou")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(Categorie cat)
        {
            if (ModelState.IsValid) { await _service.AdaugaCategorie(cat); return RedirectToAction(nameof(Index)); }
            return View(cat);
        }

        [Route("Editeaza/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var cat = await _service.GetCategorieById(id);
            return cat == null ? NotFound() : View(cat);
        }

        [HttpPost]
        [Route("Editeaza/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, Categorie cat)
        {
            if (id != cat.Id) return NotFound();
            if (ModelState.IsValid) { await _service.UpdateCategorie(cat); return RedirectToAction(nameof(Index)); }
            return View(cat);
        }

        [Route("Sterge/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _service.GetCategorieById(id);
            return cat == null ? NotFound() : View(cat);
        }

        [HttpPost, ActionName("Delete")]
        [Route("Sterge/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.StergeCategorie(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
