/*
 * DESCRIERE:
 * Acest controller expune operațiile CRUD pentru gestiunea orașelor.
 * Accesul la vizualizarea listei de orașe este permis utilizatorilor autentificați, în timp ce
 * adăugarea, modificarea și ștergerea orașelor sunt restricționate exclusiv utilizatorilor cu rolul de Administrator.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using recenzi_pentru_firme.Models;
using recenzi_pentru_firme.Services;

namespace recenzi_pentru_firme.Controllers
{
    [Authorize]
    [Route("Administrare/Orase")]
    public class OraseController : Controller
    {
        private readonly OrasService _service;
        public OraseController(OrasService service) => _service = service;

        [Route("")]
        [Route("Lista")]
        public async Task<IActionResult> Index() => View(await _service.GetToateOrasele());

        [Route("Nou")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create() => View();

        [HttpPost]
        [Route("Nou")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(Oras oras)
        {
            if (ModelState.IsValid) { await _service.AdaugaOras(oras); return RedirectToAction(nameof(Index)); }
            return View(oras);
        }

        [Route("Editeaza/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var oras = await _service.GetOrasById(id);
            return oras == null ? NotFound() : View(oras);
        }

        [HttpPost]
        [Route("Editeaza/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, Oras oras)
        {
            if (id != oras.Id) return NotFound();
            if (ModelState.IsValid) { await _service.UpdateOras(oras); return RedirectToAction(nameof(Index)); }
            return View(oras);
        }

        [Route("Sterge/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var oras = await _service.GetOrasById(id);
            return oras == null ? NotFound() : View(oras);
        }

        [HttpPost, ActionName("Delete")]
        [Route("Sterge/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.StergeOras(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
