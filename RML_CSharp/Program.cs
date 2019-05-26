using System;
using System.Collections.Generic;

namespace RML_CSharp
{
    //Probléma: Ha tök egyforma logikát kell megvalósítani sok különböző adatszerkezeten,
    //akkor a szigorúan típusos nyelvi paradigma miatt minden típusra meg kell írnunk ugyanazt a logikát.
    //Ezek csak abban fognak különbözni, hogy verem és halom műveletekben más méretekkel kell a címszámító aritmetikát megalkotni

    //Legyen például két típusunk:
    class RaktariCikk
    {
        public string Cikkszam { get; set; }
        public decimal ListaAr { get; set; }
    }
    class Szolgaltatas
    {
        public bool Atharitott { get; set; }
        public DateTime Teljesites { get; set; }
        public decimal Oradij { get; set; }
        public decimal FelhasznaltIdo { get; set; }
    }

    //Legyen a kívánt logika például az, hogy max. tízelemű regiszterbe gyűjthessünk ilyen objektumpéldányokat és bármikor lekérdezhessük a tagok számának négyzetét 
    //(tudom, hülyeség, de csak példa!)
    //Ekkor kétszer meg kéne írni ugyanazt:

    class SpeciListaRaktariCikk
    {
        private RaktariCikk[] array = new RaktariCikk[10];
        private int count;

        public void Betesz(RaktariCikk item)
        {
            if (count < array.Length)
            {
                array[count] = item;
                count++;
            }
        }

        public double TagokSzamanakNegyzete()
        {
            double c = count;
            return c * c;
        }
    }

    class SpeciListaSzolgaltatas
    {
        private Szolgaltatas[] array = new Szolgaltatas[10];
        private int count;

        public void Betesz(Szolgaltatas item)
        {
            if (count < array.Length)
            {
                array[count] = item;
                count++;
            }
        }

        public double TagokSzamanakNegyzete()
        {
            double c = count;
            return c * c;
        }
    }

    //Ennél persze a valós üzleti logikák bonyolultabbak.
    //A többszörözésük akkor okoz problémát, amikor később változtatni kell a logikán egy kicsit. A változást végig kell vezetni mindegyik változaton (lehetőleg egyformán!)
    //Miért ne kérjük meg inkább a fordítót a többszörözésre?
    //Adjunk neki egy mintát, fordítsa be az alapján az egyes változatokat! Igy később elég a mintán változtatnunk egy helyen. A fordító pedig megbízhatóan egyformára gyártja le a tényleges osztályokat.

    //Igy néz ki egy minta. Ez a <> jelekből látszik. A <> jelek között egy-több tetszőlegesen választott azonosítót lehet felsorolni.
    //Ezekre lehet hivatkozni mint típusokra a minta törzsében.
    //Amikor a fordító ezt meglátja, még nem fordít semmit, hiszen nem tudja melyik típus mekkora. Az majd az első használatnál derül ki.
    class SpeciLista<T> 
    {
        private T[] array = new T[10];
        private int count;
        
        public void Betesz(T item)
        {
            if (count<array.Length)
            {
                array[count] = item;
                count++;
            }
        }

        public double TagokSzamanakNegyzete()
        {
            double c = count;
            return c * c;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //És akkor itt most használjuk a mintát!
            //Ezzel megkérjük a fordítót, hogy csináljon egy olyan változatot, amelyben a RaktariCikk méretei szerint szervezi a címszámító aritmetikákat.
            //A lenti sorok következtében valami SpeciLista'1 nevű osztály lesz ténylegesen a kódban
            SpeciLista<RaktariCikk> speciCikkek = new SpeciLista<RaktariCikk>();
            speciCikkek.Betesz(new RaktariCikk());
            speciCikkek.Betesz(new RaktariCikk());
            Console.WriteLine(speciCikkek.TagokSzamanakNegyzete());

            //Újra használjuk a mintát.
            //A fordító megvizsgálja, hogy a SpeciLista'1 amit már kialakított, az jó-e ide?
            //Nem jó, mert az másik típusra való, ezért csinál egy SpeciLista'2 osztályt a kódba.
            SpeciLista<Szolgaltatas> speciSzolgaltatasok = new SpeciLista<Szolgaltatas>();
            speciSzolgaltatasok.Betesz(new Szolgaltatas());
            speciSzolgaltatasok.Betesz(new Szolgaltatas());
            speciSzolgaltatasok.Betesz(new Szolgaltatas());
            Console.WriteLine(speciSzolgaltatasok.TagokSzamanakNegyzete());

            //Saját generikus típusok (minták) írása helyett valószínűbb hogy a System.Collections.Generic névtér osztályait fogod használni: 
            // https://docs.microsoft.com/en-gb/dotnet/api/system.collections.generic?view=netframework-4.7.2
            List<RaktariCikk> cikkek = new List<RaktariCikk>();
            List<Szolgaltatas> szolgaltatasok = new List<Szolgaltatas>();
            cikkek.Add(new RaktariCikk());
            cikkek.Add(new RaktariCikk());
            szolgaltatasok.Add(new Szolgaltatas());
            szolgaltatasok.Add(new Szolgaltatas());
            foreach (var cikk in cikkek) Console.WriteLine(cikk.ToString());
            foreach (var szolgaltatas in szolgaltatasok) Console.WriteLine(szolgaltatas.ToString());
        }
    }
}

// List<T> reference: https://docs.microsoft.com/en-gb/dotnet/api/system.collections.generic.list-1?view=netframework-4.7.2#methods
