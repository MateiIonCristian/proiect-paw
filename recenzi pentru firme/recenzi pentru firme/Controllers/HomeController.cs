/*
 * DESCRIERE:
 * Acest controller gestionează paginile principale ale aplicației (Home, Despre, Privacy etc.)
 * și încarcă date statistice globale (precum numărul total de firme și recenzii active) pentru pagina de pornire.
 */

using Microsoft.AspNetCore.Mvc;
using recenzi_pentru_firme.Services;

namespace recenzi_pentru_firme.Controllers
{
    public class HomeController : Controller
    {
        private readonly FirmaService _firmaService;
        private readonly RecenzieService _recenzieService;

        public HomeController(FirmaService firmaService, RecenzieService recenzieService)
        {
            _firmaService = firmaService;
            _recenzieService = recenzieService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalFirme = (await _firmaService.GetToateFirmele()).Count();
            ViewBag.TotalRecenzii = (await _recenzieService.GetToateRecenziile()).Count();
            return View();
        }

        public IActionResult Profil() => View();

        public IActionResult Detalii() => View();

        public IActionResult Despre() => View();

        public IActionResult Privacy() => View();
    }
}
