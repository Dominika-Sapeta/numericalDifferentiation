using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taylor
{
    public static class Derivative
    {

        public static double Normal(double[,] table, double h, int level)
        {
            switch (level)
            {
                case 1:
                    return (table[2, 0] - 0.5 * table[3, 0] + 1.0 / 3.0 * table[4, 0] - 0.25 * table[5, 0]) / h;
                case 2:
                    return (table[3, 0] - table[4, 0] + 11.0 / 12.0 * table[5, 0] - 10.0 / 12.0 * table[6, 0]) / h / h;
                case 3:
                    return (table[4, 0] - 3.0 / 2.0 * table[5, 0] + 7.0 / 4.0 * table[6, 0] - 45.0 / 24.0 * table[7, 0]) / h / h / h;
                default:
                    return 0;
            }
        }


        public static double Reverse(double[,] table, double h, int level)
        {
            int top = table.GetUpperBound(1) - 1;

            switch (level)
            {
                case 1:
                    return (table[2, top] + 0.5 * table[3, top - 1] + 1.0 / 3.0 * table[4, top - 2]) / h;
                case 2:
                    return (table[3, top - 1] + table[4, top - 2] + 11.0 / 12.0 * table[5, top - 3]) / h / h;
                case 3:
                    return (table[4, top - 2] + 3.0 / 2.0 * table[5, top - 3] + 7.0 / 4.0 * table[6, top - 4] + 45.0 / 24.0 * table[7, top - 5]) / h / h / h;
                default:
                    return 0;
            }
        }
    }
}
