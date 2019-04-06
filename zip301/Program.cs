﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zip301
{
    class Program
    {
        static void Main(string[] args)
        {
           
            if (args.Length != 0)
            {
                Console.WriteLine("Need an object of compression!");
            }
            try
            {
                FileStrem input = new FileStream("biglog.txt", FileAccess.Read, FileMode.Open);
                //BufferedStream file = new BufferedStream(input, 4096 * 100);//, FileMode.Open, FileAccess.Read);
                Compression process = new Compression(input);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(args+" was not accessible");
            }
        }

    }
}
