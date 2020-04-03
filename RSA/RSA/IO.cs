using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    class IO
    {
        public static string EnterText(string message)
        {
            string result = null;

            do
                Console.WriteLine(message);
            while ((result = Console.ReadLine().Trim()) == string.Empty);

            return result;
        }

        public static int EnterNumber(string message)
        {
            int num = 0;

            do
                Console.Write(message);
            while (!int.TryParse(Console.ReadLine(), out num));

            return num;
        }
    }
}
