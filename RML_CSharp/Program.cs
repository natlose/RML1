using System;
using System.Linq;
using System.Linq.Expressions;

namespace RML_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Probléma: hogyan lehetne az előfordított (és nem értelmezett) nyelvi környezetben a fordítás időpontjában 
            //még nem ismert matematikai, logikai kifejezés futásidejű kiértékelésére felkészülni?
            //Elsősorban azt kell megoldani, hogy a kiértékelő logikát és a kifejezést két független, térben-időben távoli fejlesztők alkotják meg.
            //Azaz: hogyan írhatna helyetted valami hatékony, okos logikát úgy a Microsoft, hogy csak évekkel később fogsz te jönni és megmondani, hogy mit kell csinálnia?
            //A dotnet megoldása:
            //- alkossunk olyan osztályokat, amelyek képesek unáris, bináris, stb. műveletek operandusainak és operátorának tárolására, végrehajtására
            //- alkossuk meg futási időben ezekből az objektumokból azt az objektumgráfot, ami leírja a kifejezést
            //- a kiértékelő logika legyen képes bejárni a gráfot és az így kapott eredményen végezze el a maga feladatát
            //- készítsük fel a fordítót arra, hogy a forrásban egyszerű szabályok szerint megfogalmazott kifejezést látva
            //  az objektumgráfot felépítő utasításokat tegye a kódba


            //Az objektumgráfot eredményező kifejezést nevezzük lambda kifejezésnek
            //Általános formátuma: ( feldolgozandó paraméterek listája ) => { feldolgozó utasítások }
            //Példa, ahol a gráf gyökérpontját eltároljuk, majd kérjük a bejárását (kiértékelését) a 8 paraméterrel:
            Expression<Func<int, int>> treeFromLambda = num => (66 - 53) * num;
            Console.WriteLine("'num => (66 - 53) * num' lambda kiértékelése: {0}", treeFromLambda.Compile().DynamicInvoke(8));

            //Ugyanezt a gráfot kézzel is felépíthettük volna a következő utasításokkal (ezektől mentett meg a fordító):
            ParameterExpression param1 = Expression.Parameter(typeof(int), "num");
            BinaryExpression e66Minus53 = BinaryExpression.MakeBinary(ExpressionType.Subtract, Expression.Constant(66), Expression.Constant(53));
            BinaryExpression eMultiply = BinaryExpression.MakeBinary(ExpressionType.Multiply, e66Minus53, param1);
            LambdaExpression lambda = Expression.Lambda<Func<int, int>>(eMultiply, param1);
            int result1 = (int) lambda.Compile().DynamicInvoke(8);
            Console.WriteLine($"'num => (66 - 53) * num' gráf kiértékelése: {result1}");

            //Az objektumgráf felépítésére LINQ operátorokkal is kérhetjük a fordítót
            //Az alábbi LINQ kifejezést elolvasva a fordító olyan utasításokat állít össze, 
            //melyek futási időben felépítik a System.Linq.Expressions osztályaiból az objektumgráfot.
            byte[] bytes = new byte[] { 132, 255, 32, 48, 250, 65, 166, 0, 128, 23, 64};
            var query = from b in bytes
                        where b > 64
                        orderby b
                        select b;
            // A ToArray iterálja végig a query enumerátorát és teszi el a kifejezés eredményeit tömb szerkezetbe
            byte[] result2 = query.ToArray(); // tekintsd meg a Debug-Locals ablakban a result2 értékét!
        }
    }
}
