using Microsoft.AspNetCore.Mvc;
using Sklep.Models;
using System.Diagnostics;
using Sklep.Data;
using Microsoft.EntityFrameworkCore;

namespace Sklep.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var produkty = await _context.Produkty.ToListAsync();
            return View(produkty);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public async Task<IActionResult> Zamow()
        {
            // Pobierz wszystkie produkty do wy�wietlenia w formularzu
            var produkty = await _context.Produkty.ToListAsync();
            ViewBag.Produkty = produkty;
            return View(new Uzytkownik());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Zamow(Uzytkownik uzytkownik, int[] wybraneProdukty)
        {
            if (ModelState.IsValid)
            {
                // Zapisz u�ytkownika
                _context.Uzytkownicy.Add(uzytkownik);
                await _context.SaveChangesAsync();

                // Stw�rz zam�wienie dla u�ytkownika
                var zamowienie = new Zamowienie
                {
                    UzytkownikId = uzytkownik.Id,
                    DataZamowienia = DateTime.Now,
                    Produkty = new List<Produkt>()
                };

                // Powi�� wybrane produkty z zam�wieniem
                var produkty = await _context.Produkty.Where(p => wybraneProdukty.Contains(p.Id)).ToListAsync();
                zamowienie.Produkty.AddRange(produkty);

                // Zapisz zam�wienie
                _context.Zamowienia.Add(zamowienie);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            // W przypadku b��du walidacji, ponownie za�aduj list� produkt�w
            ViewBag.Produkty = await _context.Produkty.ToListAsync();
            return View(uzytkownik);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
