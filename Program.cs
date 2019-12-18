using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHash
{
    class Program
    {

        static void Main(string[] args)
        {

            CustomHash<string> ch = new CustomHash<string>(16);

            Console.WriteLine("Words:");
            int hash = Int32.Parse(Console.ReadLine());
            while (hash != -1)
            {

                Console.WriteLine("Hex: {0:X}", hash);

                string word = Console.ReadLine();
                ch.Insert((uint)hash, word);

                // Update
                hash = Int32.Parse(Console.ReadLine());
            }

            // Now retrieve
            Console.WriteLine("Find: ");
            hash = Int32.Parse(Console.ReadLine());
            while (hash != -1)
            {
                bool cont = ch.Contains((uint)hash);

                // Print if contains, and value if does
                Console.WriteLine("Exists: " + cont + ((cont) ? (" Value: " + ch.Get((uint)hash)) : ("")));

                hash = Int32.Parse(Console.ReadLine());
            }

            Console.WriteLine(ch.ToString());

            Console.WriteLine("Press any key to close....");
            Console.CursorVisible = false;
            Console.ReadKey();
        }
    }
}
