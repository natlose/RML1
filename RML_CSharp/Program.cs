using System;
using System.Collections.Generic;

namespace RML_CSharp
{
    class RaktariCikk { }
    class Szolgaltatas { }

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
            SpeciLista<RaktariCikk> speciCikkek = new SpeciLista<RaktariCikk>();
            speciCikkek.Betesz(new RaktariCikk());
            speciCikkek.Betesz(new RaktariCikk());
            Console.WriteLine(speciCikkek.TagokSzamanakNegyzete());

            SpeciLista<Szolgaltatas> speciSzolgaltatasok = new SpeciLista<Szolgaltatas>();
            speciSzolgaltatasok.Betesz(new Szolgaltatas());
            speciSzolgaltatasok.Betesz(new Szolgaltatas());
            speciSzolgaltatasok.Betesz(new Szolgaltatas());
            Console.WriteLine(speciSzolgaltatasok.TagokSzamanakNegyzete());

            //Saját típusok helyett valószínűbb hogy a System.Collections.Generic névtér osztályait fogod használni: 
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
