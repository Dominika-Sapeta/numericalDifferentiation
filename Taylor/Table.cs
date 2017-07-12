using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taylor
{
    public static class Table
    {

        public static double[,] Normal(Func<double, double> f, double x0, double h, int steps)
        {
            var table = new double[steps + 1, steps];
            table[0, 0] = x0;
            table[1, 0] = f(x0);

            for (int i = 1; i < steps; i++)
            {
                table[0, i] = table[0, i - 1] + h;
                table[1, i] = f(table[0, i]);
            }

            int col = 1;
            for (int remaining = steps - 1; remaining >= 0; remaining--, col++)
            {     
                
                for (int i = 0; i < remaining; i++)
                {
                    table[col + 1, i] = (table[col, i + 1] - table[col, i]);
                }
            }

            return table;
        }

        public static double[,] Reverse(Func<double, double> f, double x0, double h, int steps)
        {
            var table = new double[steps + 1, steps];

            x0 = x0 - (steps - 1) * h;

            table[0, 0] = x0;
            table[1, 0] = f(x0);

            for (int i = 1; i < steps; i++)
            {
                table[0, i] = table[0, i - 1] + h;
                table[1, i] = f(table[0, i]);
            }

            int col = 1;
            for (int remaining = steps - 1; remaining >= 0; remaining--, col++)
            {
               
                for (int i = 0; i < remaining; i++)
                {
                    table[col + 1, i] = (table[col, i + 1] - table[col, i]);
                }
            }

            return table;
        }       
    }
}
