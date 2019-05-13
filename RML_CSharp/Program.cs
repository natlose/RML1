using System;
using System.Linq;

namespace RML_CSharp
{
    class Szamla
    {
        public string Sorszam { get; set; }
        public DateTime Kelt { get; set; }
        public DateTime Teljesites { get; set; }
        public decimal Netto { get; set; }
        public decimal Afa { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Ezen a ponton nézd meg a Debug - Locals ablakban, hogy már fordítás közben rögzültek a névtelen típusok, mintha így deklaráltuk volna:
            // f_AnonymousType0<string, int>[] kesesLista;
            // f_AnonymousType1<string, decimal>[] ertekLista;
            // f_AnonymousType2<System.DateTime, decimal, int>[] statLista;

            Szamla[] szamlak = new Szamla[]
            {
                new Szamla { Sorszam = "001", Kelt = new DateTime(2019, 05, 01), Teljesites = new DateTime(2019, 04, 25), Netto = 100000M, Afa = 27000M},
                new Szamla { Sorszam = "002", Kelt = new DateTime(2019, 05, 01), Teljesites = new DateTime(2019, 04, 12), Netto = 50000M, Afa = 13500M},
                new Szamla { Sorszam = "003", Kelt = new DateTime(2019, 05, 02), Teljesites = new DateTime(2019, 04, 20), Netto = 200000M, Afa = 54000M},
                new Szamla { Sorszam = "004", Kelt = new DateTime(2019, 05, 02), Teljesites = new DateTime(2019, 04, 16), Netto = 10000M, Afa = 2700M},
                new Szamla { Sorszam = "005", Kelt = new DateTime(2019, 05, 03), Teljesites = new DateTime(2019, 04, 12), Netto = 1000M, Afa = 270M},
                new Szamla { Sorszam = "006", Kelt = new DateTime(2019, 05, 03), Teljesites = new DateTime(2019, 04, 16), Netto = 1000M, Afa = 270M},
                new Szamla { Sorszam = "007", Kelt = new DateTime(2019, 05, 03), Teljesites = new DateTime(2019, 04, 12), Netto = 1000M, Afa = 270M},
                new Szamla { Sorszam = "008", Kelt = new DateTime(2019, 05, 03), Teljesites = new DateTime(2019, 05, 03), Netto = 1000M, Afa = 270M},
                new Szamla { Sorszam = "009", Kelt = new DateTime(2019, 05, 03), Teljesites = new DateTime(2019, 04, 13), Netto = 1000M, Afa = 270M}
            };

            var kesoSzamlak = from szamla in szamlak
                              where (szamla.Kelt - szamla.Teljesites).Days > 15
                              select szamla;
            var kesesLista = from szamla in kesoSzamlak
                             select new { ssz = szamla.Sorszam, kesedelem = (szamla.Kelt - szamla.Teljesites).Days - 5 }; // anonymous type
            var ertekLista = from szamla in kesoSzamlak
                             select new { ssz = szamla.Sorszam, brutto = szamla.Netto + szamla.Afa }; // anonymous type
            var statLista = from szamla in kesoSzamlak
                            group szamla by szamla.Teljesites into szamlaGroup
                            orderby szamlaGroup.Key
                            select new  // anonymous type
                            {
                                telj = szamlaGroup.Key,
                                osszeg = szamlaGroup.Sum(sz => sz.Netto + sz.Afa),
                                db = szamlaGroup.Count()
                            };

            Console.WriteLine("KÉSEDELEM");
            Console.WriteLine("\tSorszám\tKésedelem");
            foreach (var item in kesesLista.ToArray()) Console.WriteLine($"\t{item.ssz}\t{item.kesedelem}");

            Console.WriteLine("ÉRTÉK");
            Console.WriteLine("\tSorszám\tBruttó");
            foreach (var item in ertekLista.ToArray()) Console.WriteLine($"\t{item.ssz}\t{item.brutto}");

            Console.WriteLine("TELJESÍTÉS SZERINT");
            Console.WriteLine("\tTeljesítés\tÖsszesen\tDarab");
            foreach (var item in statLista.ToArray()) Console.WriteLine($"\t{item.telj.ToShortDateString()}\t{item.osszeg}\t\t{item.db}");
        }
    }
}
