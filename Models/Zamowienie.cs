using System.Collections.Generic;
namespace Sklep.Models
{
    public class Zamowienie
    {
        public int Id { get; set; }
        public DateTime DataZamowienia { get; set; }
        public int UzytkownikId { get; set; }
        public Uzytkownik Uzytkownik { get; set; }
        public List<ZamowienieProdukt> ZamowienieProdukty { get; set; }
    }

}
