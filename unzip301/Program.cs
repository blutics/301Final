using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unzip301
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Need an file!");
                return;
            }
            try
            {
                FileStream input = new FileStream(args[0], FileMode.Open, FileAccess.Read);
                //BufferedStream file = new BufferedStream(input, 4096 * 100);//, FileMode.Open, FileAccess.Read);
                Decompression process = new Decompression(input);

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(args + " was not accessible");
            }
        }
    }
}
