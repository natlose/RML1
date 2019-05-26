using System;

// Elmélet:
// A külön névterekbe (namespace) szervezés teszi lehetővé, hogy azonos nevű osztályaink ne ütközzenek egymással
// Egy osztálynak csak egy őse lehet.
// Probléma: hogyan biztosítható, hogy az egyetlen öröklési láncba nem szervezthető osztályoknak biztosan legyen egy bizonyos metódusuk?
// Megoldás: interface
// Az interface egy ígéret a fordító számára, hogy az az osztály, ami megvalósítja ezt az interfészt, rendelkezik minden olyan taggal, amit az interfész felsorol
// Le se fordul az az osztály, ami ennek nem tesz eleget
// Hagyomány, hogy az interfészek neve nagy I betűvel kezdődik, így az osztálydeklarációkban majd jobban látszik hogy ki ős, és ki interfész.

// Általában nem fogsz egyetlen forrásfájlban több névteret létrehozni.
// Ellenkezőleg, sok forrásod együtt fog alkotni egy nagy névteret, benne a sok - külön forrásokban deklarált - osztályaiddal.
// Csak az átláthatóság miatt raktam itt egyetlen forrásba össze több névteret.
namespace Iktato
{
    interface IIktathato
    {
        // Deklaráljuk, hogy minden osztálynak, aki megvalósítja ezt az interfészt, rendelkeznie kell Iktat() metódussal.
        // Ide nem is írhatunk {} metódustörzset, itt nem is szabad megvalósítani a metódust sehogy!
        void Iktat(); 
    }
}

namespace Vam
{
    interface IVamolhato
    {
        void Vamol();
    }
}

namespace BelfoldiRaktar
{
    class Bizonylat
    {
        public void Rogzit()
        {
            Console.WriteLine("BelfoldiRaktar.Bizonylat vagyok és engem rögzítenek!");
        }
    }

    class Kitarolas : Bizonylat, Iktato.IIktathato
    {
        // Meg kell valósítani az Iktato.IIktathato interfész miatt az Iktat() metódust.
        public void Iktat()
        {
            Console.WriteLine("BelfoldiRaktar.Kitarolas vagyok és engem iktatnak!");    
        }

        public void Kitarol()
        {
            Console.WriteLine("BelfoldiRaktar.Kitarolas vagyok és engem kitárolnak!");
        }
    }
}

namespace VamRaktar
{
    class Bizonylat
    {
        public void Rogzit()
        {
            Console.WriteLine("VamRaktar.Bizonylat vagyok és engem rögzítenek!");
        }
    }

    //Többszörös öröklés nincs, de több interfész megvalósítható párhuzamosan
    class Kitarolas : Bizonylat, Iktato.IIktathato, Vam.IVamolhato
    {
        public void Iktat()
        {
            Console.WriteLine("VamRaktar.Kitarolas vagyok és engem iktatnak!");
        }

        public void Vamol()
        {
            Console.WriteLine("VamRaktar.Kitarolas vagyok és engem vámolnak!");
        }

        public void Kitarol()
        {
            Console.WriteLine("VamRaktar.Bizonylat vagyok és engem kitárolnak!");
        }
    }
}

namespace RML_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Szervezhetünk struktúrát különböző osztályú objektumokból
            //Ha kijelentjük, hogy mind teljesít egy bizonyos interfészt, akkor tudhatjuk hogy legalább milyen metódusokkal rendelkezik mindegyik. Ezeket meghívhatjuk.
            //Például ezekről annyit tudunk, hogy biztos van Iktat() metódusuk, mert kimondtuk.
            Iktato.IIktathato[] iktathatok = new Iktato.IIktathato[]
            {
                //A tömb deklarációja miatt az alábbiakban csak olyan osztályokat példányosíthatok, amik teljesítik az IIktathato interfészt. Más le se fordulna. 
                new BelfoldiRaktar.Kitarolas(),  //Nem elég csak new Kitarolas(), mert több névtérben is van iyen (illetve az RML_CSharp-ban pedig egyáltalán nincs is)
                new VamRaktar.Kitarolas(),
                new BelfoldiRaktar.Kitarolas()
            };
                        // A fenti forma egy fordítói szívesség, egyenértékű a következő kóddal:
                        //Iktato.IIktathato[] iktathatok = new Iktato.IIktathato[3];
                        //iktathatok[0] = new BelfoldiRaktar.Kitarolas();
                        //iktathatok[1] = new VamRaktar.Kitarolas();
                        //iktathatok[2] = new BelfoldiRaktar.Kitarolas();
                        //(A fordító megszámolta hogy hány elemmel akarom inicializálni, erre a méretre deklarálta a tömböt, és befordítja helyettem a sok értékadó utasítást.)

            //Most, hogy van IIktathato objektumokból szervezett tömbünk, meghívhatjuk mindegyik Iktat() metódusát (kell neki lennie, hiszen megvalósítják az interfészt)
            //Azaz nem azért van biztosan ilyen metódusuk, mert kényszerűen örökölték, hanem mert vállalták "ígéretben".
            //Ez van olyan jó mint a többszörös öröklés, annak feloldhatatlan konflikusai nélkül.
            foreach (var iktathato in iktathatok) iktathato.Iktat();


            //Azonban csak az abban az interfészben deklarált metódusokat lehet hívni amelyre alapoztuk a tömb szervezését!
            //Nem lehet hívni a Vamol() metódust, hiába van az egyik tömbelemben olyan objektum, ami teljesíti az IVamolhato-t.
            //Ez nem fordul le:
            //iktathatok[1].Vamol();
            //Ha mégis szeretném azt hívmi (akkor ügyetlenül szerveztem a programomat) lehet kényszeríteni típust, akkor elérhetők a metódusai
            ((Vam.IVamolhato)iktathatok[1]).Vamol();


            //A metódusok esetleges névazonossága még nem jelent interfész megvalósítást!
            //A példa objektumai mind örököltek egy Rogzit() metódust az ősüktől
            //Ez:
            //foreach (var iktathato in iktathatok) iktathato.Rogzit();
            //nem fordul le két okból sem
            // - a tömbdeklarációból nem következik, hogy minden elemnek lesz Rogzit() metódusa, bármikor rakhatok olyan elemet a tömbbe amelyiknek nincs, 
            //   ugyanis elég ha van Iktat() metódusa 
            // - nem is alkotnak egy öröklési láncot a Bizonylat osztályok - ezek két külön névtér történetesen egyformán elnevezett osztályai, egyformán elenevezett metódusokkal.
        }
    }
}
