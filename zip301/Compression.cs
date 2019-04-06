using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zip301
{
    class Compression
    {
        public Compression(FileStream file)
        {
            
            Console.WriteLine("hello world");
            byte[] data = file.read();
            int[] count = new int[128];
            file.Read(data, 0, (int)(file.Length - 1));
            for (int i = 0; i < file.Length; i++)
            {
               count[data[i]] += 1;
            }
            
            for(int i = 0; i < count.Length; i++)
            {
                Console.WriteLine(String.Format("{0,2} : {1,10}", Convert.ToChar(i), count[i]));
            }
        }
    }
}
