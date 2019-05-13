using System;
using System.Linq;  // ez bővíti be a LINQ metódusokat

namespace RML_CSharp
{
    class Dolgozo
    {
        public string Nev { get; set; }

        public decimal HaviAlapber { get; set; }

        public decimal HaviJarulek
        {
            get { return HaviAlapber * 0.07M; }
        }

        public decimal HaviSZJA()
        {
            return HaviAlapber * 0.23M;
        }
    }

    static class DolgozoExtensions
    {
        // Minden bővítő metódus egy static osztály static metódusa kell legyen, 
        // első paramétere maga a bővített osztály a this kulcsszóval megjelölve
        public static decimal HaviGyerektartas(this Dolgozo dolgozo, int eletkor)
        {
            if (eletkor < 18) return dolgozo.HaviAlapber * 0.15M;
            else return dolgozo.HaviAlapber * 0.25M;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Dolgozo dolgozo = new Dolgozo { HaviAlapber = 210000M };
            Console.WriteLine($"Alapbér      : {dolgozo.HaviAlapber}");
            Console.WriteLine($"Járulék      : {dolgozo.HaviJarulek}");
            Console.WriteLine($"SZJA         : {dolgozo.HaviSZJA()}");
            Console.WriteLine($"Gyerektartás : {dolgozo.HaviGyerektartas(15)}"); // Nincs is HaviGyerektartas metódusa a Dolgozo osztálynak!

            // Gyakorlati alkalmazás: System.Linq névtér metódusokkal bővíti a mi adatszerkezeteinket
            // Ha a using System.Linq sort eltávolítjuk, a kód nem fordul le!
            // A legtöbb LINQ metódus egy lambda kifejezést vár tőlünk, ezt kiértékelve remél hozzájutni a feldolgozandó adathoz
            Dolgozo[] dolgozok = new Dolgozo[] {
                new Dolgozo { Nev = "Cecil", HaviAlapber = 180000M},
                new Dolgozo { Nev = "András", HaviAlapber = 210000M},
                new Dolgozo { Nev = "Ferenc", HaviAlapber = 210000M},
                new Dolgozo { Nev = "Dénes", HaviAlapber = 410500M},
                new Dolgozo { Nev = "Béla", HaviAlapber = 320000M},
                new Dolgozo { Nev = "Elek", HaviAlapber = 650000M}
            };
            Console.WriteLine("\nLINQ extensions");
            Console.WriteLine($"Átlag alapbér : {dolgozok.Average(d => d.HaviAlapber)}");
            Console.WriteLine($"Az első 300+  : {dolgozok.First(d => d.HaviAlapber > 300000M).Nev}");
            Console.WriteLine($"Max járulék   : {dolgozok.Max(d => d.HaviJarulek)}");
            Console.WriteLine($"SZJA átlag a legkisebb és a legnagyobb alapbér nélkül : {dolgozok.OrderBy(d => d.HaviAlapber).Skip(1).SkipLast(1).Average(d => d.HaviSZJA())}");
            Console.WriteLine("Az három legmagasabb alapbér:\n\tNév\tAlapbér");
            foreach (var item in dolgozok.OrderByDescending(d => d.HaviAlapber).Take(3)) Console.WriteLine($"\t{item.Nev}\t{item.HaviAlapber}");
        }
    }
}
