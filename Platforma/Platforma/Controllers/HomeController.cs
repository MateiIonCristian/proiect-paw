using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platforma.Data;
using Platforma.Models;
using System.Diagnostics;

namespace Platforma.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalFirme = await _context.Firme.CountAsync();
            var ultimeleRecenzii = await _context.Recenzii
                .Include(r => r.Firma)
                .OrderByDescending(r => r.DataPublicarii)
                .Take(3)
                .ToListAsync();

            return View(ultimeleRecenzii);
        }

        public async Task<IActionResult> TopClasament()
        {
            var firme = await _context.Firme
                .Include(f => f.Recenzii)
                .Include(f => f.Categorie)
                .ToListAsync();
                
            var topFirme = firme
                .Select(f => new {
                    Firma = f,
                    Rating = f.Recenzii.Any() ? f.Recenzii.Average(r => r.Nota) : 0
                })
                .OrderByDescending(x => x.Rating)
                .ToList();
            
            return View(topFirme.Select(x => x.Firma).ToList());
        }

        public async Task<IActionResult> Categorii()
        {
            // Extragem obiectele Categorie, nu doar numele sub formă de string
            var categorii = await _context.Categorii.ToListAsync();
            return View(categorii);
        }

        public IActionResult Despre()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Profil()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}