using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sklep.Data;
using Sklep.Models;

namespace Sklep.Controllers
{
    public class ProduktyController : Controller
    {
        private readonly AppDbContext _context;

        public ProduktyController(AppDbContext context)
        {
            _context = context;
        }

        private async Task<IEnumerable<string>> PobierzKategorie()
        {
            // Pobiera unikalne kategorie z tabeli Produkty
            return await _context.Produkty
                .Select(p => p.Kategoria)
                .Distinct()
                .ToListAsync();
        }


        // GET: Produkts
        public async Task<IActionResult> WedlugKategorii(string kategoria)
        {
            ViewBag.Kategorie = await PobierzKategorie();


            if (string.IsNullOrEmpty(kategoria))
            {
                // Jeśli nie podano kategorii, zwróć wszystkie produkty
                var wszystkieProdukty = await _context.Produkty.ToListAsync();
                return View(wszystkieProdukty);
            }

            // Filtruj produkty według podanej kategorii
            var produkty = await _context.Produkty
                .Where(p => p.Kategoria == kategoria)
                .ToListAsync();
            ViewBag.WybranaKategoria = kategoria;

            return View(produkty);
        }


        // GET: Produkts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkty
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produkt == null)
            {
                return NotFound();
            }

            return View(produkt);
        }

        // GET: Produkts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produkts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,Opis,Zdjecie,Kategoria,Cena")] Produkt produkt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produkt);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home"); //przekieruj do index w homecontrolller
            }
            return View(produkt);
        }

        // GET: Produkts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkty.FindAsync(id);
            if (produkt == null)
            {
                return NotFound();
            }
            return View(produkt);
        }

        // POST: Produkts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,Opis,Zdjecie,Kategoria,Cena")] Produkt produkt)
        {
            if (id != produkt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produkt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduktExists(produkt.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produkt);
        }

        // GET: Produkts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkty
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produkt == null)
            {
                return NotFound();
            }

            return View(produkt);
        }

        // POST: Produkts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produkt = await _context.Produkty.FindAsync(id);
            if (produkt != null)
            {
                _context.Produkty.Remove(produkt);
                await _context.SaveChangesAsync();
            }

          //  await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }



        private bool ProduktExists(int id)
        {
            return _context.Produkty.Any(e => e.Id == id);
        }
    }
}
