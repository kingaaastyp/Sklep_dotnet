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
            // Pobierz wszystkie produkty do wyœwietlenia w formularzu
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
                // Zapisz u¿ytkownika
                _context.Uzytkownicy.Add(uzytkownik);
                await _context.SaveChangesAsync();

                // Stwórz zamówienie
                var zamowienie = new Zamowienie
                {
                    UzytkownikId = uzytkownik.Id,
                    DataZamowienia = DateTime.Now
                };

                _context.Zamowienia.Add(zamowienie);
                await _context.SaveChangesAsync();

                // Zapisz produkty do zamówienia w tabeli ³¹cz¹cej
                foreach (var produktId in wybraneProdukty)
                {
                    _context.ZamowienieProdukty.Add(new ZamowienieProdukt
                    {
                        ZamowienieId = zamowienie.Id,
                        ProduktId = produktId
                    });
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

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
