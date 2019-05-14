using System;
using System.Linq;

namespace RML_CSharp
{
    class AkarmilyenOsztaly
    {
        int EgyIntMezo = 1;
        string EgyStringMezo = "";
        int EgyTulajdonsag { get; set; }
        int EgyMetodus(int egyparameter)
        {
            return $"{EgyIntMezo}{EgyStringMezo}{EgyTulajdonsag}".Length;
        }
    }

    // Konvenció: az attribútum osztályok neve Attribute-ra végződik.
    class LenyegesAttribute : System.Attribute
    {

    }

    // Itt a fordító támaszkodik a névkonvencióra
    [Lenyeges]
    class MasmilyenOsztaly
    {
        int valamiAprosag = 166;

        [Lenyeges]
        int NaEzAFontosAdat = 0;

        int UzletiLogika() { return valamiAprosag + NaEzAFontosAdat; }
    }

    [Obsolete("Használd helyette a VadiUjOsztaly-t!")]
    class ElavultOsztaly
    {

    }

    class Program
    {
        static void Main(string[] args)
        {
            // A System.Reflection névtér osztályai felolvassák a típus meta-adatait, amit a fordító belecsomagolt a szerelvénybe
            // Objektumgráfot építenek, ami leírja a típus minden tagját
            Type akarmilyen = System.Type.GetType("RML_CSharp.AkarmilyenOsztaly");
            // Most nézd meg az akarmilyen változó tartalmát a Debug-Locals ablakban!
            // Nem tartozik ide, de látszanak a kódolási könnyítések eredményei: a default konstruktor, a get/set EgyTulajdonsag metódusok, a mögöttes mező. A DeclaredMembers mindent leleplez.
            // Vedd észre, hogy CustomAttributes van mindenben: szerelvényben (Assembly), osztályban, mezőben, tulajdonságban, metódusban!

            Type masmilyen = System.Type.GetType("RML_CSharp.MasmilyenOsztaly");
            // Ebben már fogod látni a feltöltött CustomAttributes mezőket az osztályon és a mezőn.

            // Ezt persze kódból is lehet olvasni, így reagálnak a dotnetes osztályok is az attribútumainkra.
            Object[] objektumok = new object[] { new AkarmilyenOsztaly(), new AkarmilyenOsztaly(), new MasmilyenOsztaly(), new AkarmilyenOsztaly(), new MasmilyenOsztaly() };
            foreach (Object o in objektumok)
            {
                Console.Write(o.GetType().Name);
                if (o.GetType().CustomAttributes.Where(a => a.AttributeType.FullName == "RML_CSharp.LenyegesAttribute").Any()) Console.Write("\tLenyeges");
                Console.WriteLine();
            };

            // Az Obsolete attribútum jön a dotnettel és a VS is figyeli
            // Lógasd az egeret a zöld aláhúzás felett, illetve fordítás után nézd meg a figyelmeztetés-listát!
            ElavultOsztaly elavult = new ElavultOsztaly();

        }
    }
}
