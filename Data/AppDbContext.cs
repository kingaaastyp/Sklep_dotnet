using Microsoft.EntityFrameworkCore;
using Sklep.Models;


namespace Sklep.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Produkt> Produkty { get; set; }
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public DbSet<Zamowienie> Zamowienia { get; set; }
    }
}
