using System;
using System.Linq;  // ez bővíti be a LINQ metódusokat

namespace RML_CSharp
{
    //Probléme: hogyan lehetne egy osztályt úgy bővíteni utólag új metódussal, hogy nem mi felügyeljük a forrását?
    //Azaz inkább: hogyan tudna a Microsoft előre írni ügyes metódusokat a mi osztályainkhoz, amikor még nem is deklaráltuk őket?
    //Megoldás: trükközzünk a fordítóval!
    //Tegyük lehetővé olyan metódus írását, amiről felismeri a fordító, hogy ez ilyen trükkös metódus és adott osztályok részének
    //tekinti, elfogad és lefordít olyan forrást, ami úgy hívja mint saját metódusát.

    //Példa: 
    //Az A fejlesztő valahol, valamikor, a saját forrásában előírta hogy az alapbéreseknek kell legyen HaviAlapber tulajdonságuk
    interface IAlapberes
    {
        decimal HaviAlapber { get; set; }
    }

    //Majd rögtön megírta a bővítő osztályt, ami HaviGyerektartas metódussal bővít minden olyan osztályt utólag is, ami 
    //megvalósítja az IAlapberes interfészt
    static class AlapberesExtensions
    {
        // Minden bővítő metódus egy static osztály static metódusa kell legyen, 
        // első paramétere maga a bővített osztály (vagy interfész) a this kulcsszóval megjelölve
        public static decimal HaviGyerektartas(this IAlapberes alapberes, int eletkor)
        {
            //Ebben a metódustörzsben nem tudhatjuk, hogy a majdan bebővített osztálynak
            //milyen tagjai lesznek, de amit az interfész előír, azokra hivatkozhatunk.
            if (eletkor < 18) return alapberes.HaviAlapber * 0.15M;
            else return alapberes.HaviAlapber * 0.25M;
        }
    }

    //Ekkor jön B fejlesztő évekkel és ezer kilométerekkel távolabb, a saját forrásában
    //deklarál egy akármilyen osztályt, ÉS megvalósítja az IAlapberes interfészt. Ettől fog bebővülni az osztálya.
    class Dolgozo : IAlapberes
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

    class Program
    {
        static void Main(string[] args)
        {
            Dolgozo dolgozo = new Dolgozo { HaviAlapber = 210000M };
            Console.WriteLine($"Alapbér      : {dolgozo.HaviAlapber}");
            Console.WriteLine($"Járulék      : {dolgozo.HaviJarulek}");
            Console.WriteLine($"SZJA         : {dolgozo.HaviSZJA()}");
            Console.WriteLine($"Gyerektartás : {dolgozo.HaviGyerektartas(15)}"); // Valójában nincs is HaviGyerektartas metódusa a Dolgozo osztálynak!

            //Ritkán fogsz saját bővítő metódust írni, de a rengeteget fogod használni a System.Linq bővítő metódusait
            //Példa
            //A tömb megvalósítja az IEnumerable<> interfészt, ezt sok metódus bővíti (de ha a using System.Linq sort eltávolítjuk, a kód nem fordul le!)
            Dolgozo[] dolgozok = new Dolgozo[] {
                new Dolgozo { Nev = "Cecil", HaviAlapber = 180000M},
                new Dolgozo { Nev = "András", HaviAlapber = 210000M},
                new Dolgozo { Nev = "Ferenc", HaviAlapber = 210000M},
                new Dolgozo { Nev = "Dénes", HaviAlapber = 410500M},
                new Dolgozo { Nev = "Béla", HaviAlapber = 320000M},
                new Dolgozo { Nev = "Elek", HaviAlapber = 650000M}
            };
            // A legtöbb LINQ bővítő metódus egy lambda kifejezést vár tőlünk, ezt kiértékelve remél hozzájutni a feldolgozandó adathoz
            Console.WriteLine("\nLINQ extensions");
            Console.WriteLine($"Átlag alapbér : {dolgozok.Average(d => d.HaviAlapber)}"); // Nyomj F12 gombot az Average metóduson és meglátod a System.Linq bővítéseit!
            Console.WriteLine($"Az első 300+  : {dolgozok.First(d => d.HaviAlapber > 300000M).Nev}"); // első igaznál megáll és nevet ad vissza
            Console.WriteLine($"Max járulék   : {dolgozok.Max(d => d.HaviJarulek)}");
            Console.WriteLine($"SZJA átlag a legkisebb és a legnagyobb alapbér nélkül : {dolgozok.OrderBy(d => d.HaviAlapber).Skip(1).SkipLast(1).Average(d => d.HaviSZJA())}");
            Console.WriteLine("Az három legmagasabb alapbér:\n\tNév\tAlapbér");
            foreach (var item in dolgozok.OrderByDescending(d => d.HaviAlapber).Take(3)) Console.WriteLine($"\t{item.Nev}\t{item.HaviAlapber}");
        }
    }
}
