using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taylor
{
    class Program
    {
        private static double h;
      
        static void Main()
        {
            h = Read("h = ");

            Function f = Function.Ln;

            double[] values = new double[0];

            do
            {
                Console.WriteLine("\nWybierz opcję:");
                Console.WriteLine("\t0 - Wybierz funkcję f(x) = sin(x)");
                Console.WriteLine("\t1 - Wybierz funkcję f(x) = cos(x)");
                Console.WriteLine("\t2 - Wybierz funkcję f(x) = arccos(x)");
                Console.WriteLine("\t3 - Wybierz funkcję f(x) = arcsin(x)");
                Console.WriteLine("\t4 - Wybierz funkcję f(x) = ln(x)");
                Console.WriteLine("\t5 - Wybierz funkcję f(x) = log10(x)");
                Console.WriteLine("\tR - Wczytaj wartości");
                Console.WriteLine("\tZ - Zapisz wyniki do pliku (tablica różnic zwykłych)");
                Console.WriteLine("\tW - Zapisz wyniki do pliku (tablica różnic wstecznych)");
                Console.WriteLine("\tq - Zakończ pracę z programem");
                Console.WriteLine();

                ConsoleKeyInfo key = Console.ReadKey(true);

                string fileName;
                switch (key.Key)
                {
                    case ConsoleKey.D0:
                        f = Function.Sin;
                        break;
                    case ConsoleKey.D1:
                        f = Function.Cos;
                        break;
                    case ConsoleKey.D2:
                        f = Function.Arccos;
                        break;
                    case ConsoleKey.D3:
                        f = Function.Arcsin;
                        break;
                    case ConsoleKey.D4:
                        f = Function.Ln;
                        break;
                    case ConsoleKey.D5:
                        f = Function.Log10;
                        break;
                    case ConsoleKey.R:
                        fileName = ReadFileName("Plik z wartościami: ", true);
                        values = ReadValues(fileName);
                        break;
                    case ConsoleKey.Z:
                        fileName = ReadFileName("Plik na wyniki (jeśli istnieje zostanie nadpisany): ", false);

                        WriteResultsFromNormal(fileName, values, f);

                        break;
                    case ConsoleKey.W:
                        fileName = ReadFileName("Plik na wyniki (jeśli istnieje zostanie nadpisany): ", false);

                        WriteResultsFromReverse(fileName, values, f);

                        break;
                    case ConsoleKey.Q:
                        return;
                };
            }
            while (true);
        }

        
        private static void WriteResultsFromNormal(string fileName, double[] values, Function f)
        {
            using (var sw = new StreamWriter(fileName, false))
            {
                sw.WriteLine("Funkcja {0}", f);

                sw.WriteLine("Tablica różnic zwykłych\n\n");

                foreach (var value in values)
                {
                    sw.WriteLine("x0 = {0:0.0000}", value);

                    var table = Table.Normal(Functions.Base(f), value, h, 8);

                    WriteTable(sw, table);

                    sw.WriteLine();
                    WriteResults(false, f, value, sw, table);
                    sw.WriteLine();
                }
            }
        }

        
        private static void WriteResultsFromReverse(string fileName, double[] values, Function f)
        {
            using (var sw = new StreamWriter(fileName, false))
            {
                sw.WriteLine("Funkcja {0}", f);

                sw.WriteLine("Tablica różnic wstecznych\n\n");

                foreach (var value in values)
                {
                    sw.WriteLine("x0 = {0:0.0000}", value);

                    var table = Table.Reverse(Functions.Base(f), value, h, 8);

                    WriteTable(sw, table);

                    sw.WriteLine();
                    WriteResults(true, f, value, sw, table);
                    sw.WriteLine();
                }
            }
        }

       
        private static void WriteResults(bool reverse, Function func, double x0, StreamWriter sw, double[,] table)
        {
            sw.WriteLine("Pierwsza pochodna");
            WriteValues(reverse, func, x0, 1, sw, table);

            sw.WriteLine("Druga pochodna");
            WriteValues(reverse, func, x0, 2, sw, table);

            sw.WriteLine("Trzecia pochodna");
            WriteValues(reverse, func, x0, 3, sw, table);
        }

        
        private static void WriteValues(bool reverse, Function func, double x0, int level, StreamWriter sw, double[,] table)
        {         
            double calculated;

            if (reverse)
            {
                calculated = Derivative.Reverse(table, h, level);
            }
            else
            {
                calculated = Derivative.Normal(table, h, level);
            }

            sw.Write("\tObliczona:");
            sw.Write("{0:0.0000}", calculated);

            var exact = Functions.Derivative(func, level)(x0);

            sw.Write("\tDokładna: {0:0.0000}", exact);
            sw.WriteLine("\tBłąd: {0:0.0000}", Math.Abs(exact - calculated));
        }

        
        private static double[] ReadValues(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            var values = new double[lines.Length];

            for (int i = 0; i < values.Length; i++)
            {
                var line = lines[i];

                if (line.StartsWith("pi*"))
                {
                    var factor = double.Parse(line.Substring(3));

                    values[i] = Math.PI * factor;
                }
                else if (line.StartsWith("pi/"))
                {
                    var factor = double.Parse(line.Substring(3));

                    values[i] = Math.PI / factor;
                }
                else if (line == "pi")
                {
                    values[i] = Math.PI;
                }
                else
                {
                    values[i] = double.Parse(line);
                }
            }

            return values;
        }

        
        private static string ReadFileName(string prompt, bool shouldExist)
        {
            string path;

            do
            {
                Console.Write(prompt);
                path = Console.ReadLine();

                if (!shouldExist)
                {
                    return path;
                }

                if (File.Exists(path))
                {
                    return path;
                }
            }
            while (true);
        }


        private static double Read(string text)
        {
            string s;
            double res;

            do
            {
                Console.Write(text);
                s = Console.ReadLine();
            }
            while (!double.TryParse(s, out res));

            return res;
        }

       
        private static void WriteTable(TextWriter output, double[,] tbl)
        {
            output.Write("x\t\tf(x)\t\t");
            for (int i = 2; i <= tbl.GetUpperBound(0); i++)
            {
                output.Write("{0}\t\t", i - 1);
            }
            output.WriteLine();

            for (int i = 0; i <= tbl.GetUpperBound(1); i++)
            {
                output.Write("{0:0.0000}\t\t", tbl[0, i]);
                output.Write("{0:0.0000}\t\t", tbl[1, i]);

                for (int j = 2; j <= tbl.GetUpperBound(0) - i; j++)
                {
                    output.Write("{0:0.0000}\t\t", tbl[j, i]);
                }
                output.WriteLine();
            }
        }
    }
}
