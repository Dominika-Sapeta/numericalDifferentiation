using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taylor
{
    public static class Functions
    {
        private static readonly Dictionary<Function, Func<double, double>[]> functions;

        static Functions()
        {
            functions = new Dictionary<Function, Func<double, double>[]>();

            functions[Function.Sin] = new Func<double, double>[]
            {
                x => Math.Sin(x),
                x => Math.Cos(x),
                x => -Math.Sin(x),
                x => -Math.Cos(x)
            };

            functions[Function.Cos] = new Func<double, double>[]
            {
                x => Math.Cos(x),
                x => -Math.Sin(x),
                x => -Math.Cos(x),
                x => Math.Sin(x)
            };

            functions[Function.Arccos] = new Func<double, double>[]
            {
                x => Math.Acos(x),
                x => -1.0/Math.Sqrt(1 - x*x),
                x => -x/Math.Pow(1 - x*x, 1/5),
                x => (-2 * x*x - 1)/Math.Pow(1 - x*x, 2.5)
            };

            functions[Function.Arcsin] = new Func<double, double>[]
            {
                x => Math.Asin(x),
                x => 1.0/Math.Sqrt(1 - x*x),
                x => x/Math.Pow(1 - x*x, 1/5),
                x => (2 * x*x + 1)/Math.Pow(1 - x*x, 2.5)
            };

            functions[Function.Ln] = new Func<double, double>[]
            {
                x => Math.Log(x),
                x => 1.0/x,
                x => -1.0/(x*x),
                x => 2.0/(x*x*x)
            };

            functions[Function.Log10] = new Func<double, double>[]
            {
                x => Math.Log10(x),
                x => -1.0/(x*x * Math.Log(10)),
                x => 2.0/(x*x*x * Math.Log(10))
            };
        }

        public static Func<double, double> Base(Function func)
        {
            return functions[func][0];
        }

        public static Func<double, double> Derivative(Function func, int level)
        {
            return functions[func][level];
        }
    }
}
