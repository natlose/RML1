using System;
using System.Linq;
using System.Linq.Expressions;

namespace RML_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Kompozíció
            // (66 - 53) * param1
            ParameterExpression param1 = Expression.Parameter(typeof(int), "ANameJustForDebugging");
            BinaryExpression e66Minus53 = BinaryExpression.MakeBinary(ExpressionType.Subtract, Expression.Constant(66), Expression.Constant(53));
            BinaryExpression eMultiply = BinaryExpression.MakeBinary(ExpressionType.Multiply, e66Minus53, param1);
            LambdaExpression lambda = Expression.Lambda<Func<int, int>>(eMultiply, param1);
            int result1 = (int) lambda.Compile().DynamicInvoke(8);
            Console.WriteLine($"(66 - 53) * 8 = {result1}");

            // Dekompozíció
            Expression<Func<int, bool>> exprTree = num => num < 5;
            ParameterExpression param = (ParameterExpression)exprTree.Parameters[0];
            BinaryExpression operation = (BinaryExpression)exprTree.Body;
            ParameterExpression left = (ParameterExpression)operation.Left;
            ConstantExpression right = (ConstantExpression)operation.Right;
            Console.WriteLine($"{param.Name} => {left.Name} {operation.NodeType} {right.Value}");

            // Gyakorlati alkalmazás 
            byte[] bytes = new byte[] { 132, 255, 32, 48, 250, 65, 166, 0, 128, 23, 64};
            // Az alábbi LINQ kifejezést elolvasva a fordító olyan utasításokat állít össze, 
            // melyek futási időben felépítik a System.Linq.Expressions osztályaiból az objektumgráfot.
            var query = from b in bytes
                        where b > 64
                        orderby b
                        select b;
            // A ToArray iterálja végig a query enumerátorát és teszi el a kifejezés eredményeit tömb szerkezetbe
            byte[] result2 = query.ToArray(); // tekintsd meg a Debug-Locals ablakban a result2 értékét!
        }
    }
}
