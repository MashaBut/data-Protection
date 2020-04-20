using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Выберите способ генерации ключей:\n1-автоматическая генерация, 2 - ручной ввод");
            string ind = Console.ReadLine();
            Console.WriteLine("Введите сообщение");
            string M = Console.ReadLine();
            int q;
            if (ind == "2") { Console.WriteLine("Введите число q"); q = int.Parse(Console.ReadLine()); }
            else { Random rdm = new Random(); q = rdm.Next(10, 100); }

            int p = q + 1;
            while (true)
            {
                bool f = false;
                for (int i = 2; i < p - 1; i++)
                {
                    if (p % i == 0)
                    {
                        f = true;
                        break;
                    }
                }
                if (!f && (p - 1) % q == 0)
                {
                    break;
                }
                else
                {
                    p++;
                }
            }
            double g = 0;
            Random rand = new Random();
            while (g < 1)
            {
                g = Math.Pow(rand.Next(1, p - 1), (p - 1) / q);
            }
            int x = rand.Next(0, q);
            int y = fast(Convert.ToInt32(g), x, p);
            int s = 0;
            int r = 0;
            while (true)
            {
                int k = rand.Next(0, q);
                r = fast(Convert.ToInt32(g), k, p) % q;
                int k1 = 0;
                while ((k1 * k) % q != 1)
                {
                    k1++;
                }
                s = Convert.ToInt32(k1 * (hash(M, q) + x * r)) % q;
                if (r != 0 || s != 0)
                {
                    Console.WriteLine("Сообщение с подписью: [{0}, {1}, {2}]", M, r, s);
                    break;
                }
            }

            Console.WriteLine("Проверка подписи:\nВведите собщение для проверки:");
            M = Console.ReadLine();
            int s1 = 0;
            while ((s1 * s) % q != 1)
            {
                s1++;
            }
            int w = s1 % q;
            int u1 = (hash(M, q) * w) % q;
            int u2 = (r * w) % q;
            double mp1 = fast(Convert.ToInt32(g), u1, p);
            double mp2 = fast(y, u2, p);
            double res = mp1 * mp2;
            res %= p;
            res %= q;
            int v = Convert.ToInt32(res);
            Console.WriteLine("v = {0}", v);
            if (v == r)
            {
                Console.WriteLine("Значения совпадают, ЭЦП верна: {0} = {1}", v, r);
            }
            else
            {
                Console.WriteLine("Значения не совпадают, ЭЦП не верна: {0} != {1}", v, r);
            }

            Console.ReadLine();
        }
        public static int NOD(int a, int b)
        {
            if (a == b)
                return a;
            else
                if (a > b)
                return NOD(a - b, b);
            else
                return NOD(b - a, a);
        }
        public static int fast(int a, int r, int n)
        {
            int a1 = a;
            int z1 = r;
            int x = 1;
            while (z1 != 0)
            {
                while (z1 % 2 == 0)
                {
                    z1 /= 2;
                    a1 = (a1 * a1) % n;
                }
                z1 -= 1;
                x = (x * a1) % n;
            }
            return x;
        }
        public static int hash(string s, int n)
        {
            char[] b = new char[33];
            b[0] = 'а'; b[1] = 'б'; b[2] = 'в'; b[3] = 'г'; b[4] = 'д'; b[5] = 'е'; b[6] = 'ё'; b[7] = 'ж'; b[8] = 'з'; b[9] = 'и'; b[10] = 'й';
            b[11] = 'к'; b[12] = 'л'; b[13] = 'м'; b[14] = 'н'; b[15] = 'о'; b[16] = 'п'; b[17] = 'р'; b[18] = 'с'; b[19] = 'т'; b[20] = 'у'; b[21] = 'ф';
            b[22] = 'х'; b[23] = 'ц'; b[24] = 'ч'; b[25] = 'ш'; b[26] = 'щ'; b[27] = 'ъ'; b[28] = 'ы'; b[29] = 'ь'; b[30] = 'э'; b[31] = 'ю'; b[32] = 'я';
            int k = 150;
            int f = 0;
            s = s.ToLower();
            for (int i = 0; i < s.Length; i++)
            {
                for (int j = 0; j < 33; j++)
                {
                    if (b[j] == s[i])
                    {
                        f = j + 1;
                    }
                }
                k = Convert.ToInt32(Math.Pow(k + f, 2) % n);
            }
            return k;
        }
    }
}