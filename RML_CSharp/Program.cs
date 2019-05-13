using System;

namespace Iktato
{
    interface IIktathato
    {
        void Iktat(); // Minden osztálynak, aki megvalósítja ezt az interfészt, rendelkeznie kell Iktat() metódussal.
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
    class Bizonylat : Iktato.IIktathato
    {
        // Meg kell valósítani az Iktato.IIktathato interfész miatt az Iktat() metódust.
        public virtual void Iktat()
        {
            Console.WriteLine("BelfoldiRaktar.Bizonylat vagyok és engem iktatnak!");    
        }
    }

    class Kitarolas : Bizonylat
    {
        public void Kitarol()
        {
            Console.WriteLine("BelfoldiRaktar.Bizonylat vagyok és engem kitárolnak!");
        }
    }
}

namespace VamRaktar
{
    //Többszörös öröklés nincs, de több interfész megvalósítható párhuzamosan
    class Bizonylat : Iktato.IIktathato, Vam.IVamolhato
    {
        public virtual void Iktat()
        {
            Console.WriteLine("VamRaktar.Bizonylat vagyok és engem iktatnak!");
        }

        public void Vamol()
        {
            Console.WriteLine("VamRaktar.Bizonylat vagyok és engem vámolnak!");
        }
    }

    class Kitarolas : Bizonylat
    {
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
            //Ezekről annyit tudunk, hogy van Iktat() metódusuk
            //Nem elég csak new Kitarolas(), mert több névtérben is van iyen
            Iktato.IIktathato[] iktathatok = new Iktato.IIktathato[]
            {
                new BelfoldiRaktar.Kitarolas(),
                new VamRaktar.Kitarolas(),
                new BelfoldiRaktar.Kitarolas()
            };
            foreach (var iktathato in iktathatok) iktathato.Iktat();
            //Nem lehet hívni a Vamol() metódust, mert a deklarációban IIktathato van. 
            //Ez nem fordul le:
            //iktathatok[1].Vamol();
            //Lehet kényszeríteni típust, akkor elérhetők a metódusai
            ((Vam.IVamolhato)iktathatok[1]).Vamol();
        }
    }
}
