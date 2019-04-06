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
        private int unit;
        public int textLength;
        public Compression(FileStream file)
        {
            unit = 4096*100;
            this.textLength = (int)file.Length;
            BufferedStream bs = new BufferedStream(file);
            Console.WriteLine(textLength/unit);
            Console.WriteLine(textLength);
            byte[] data = new byte[textLength+unit-(textLength%unit)];
            int[] count = new int[128];
            for(int i = 0; i < (int)(textLength / unit); i++)
            {
                bs.Read(data, unit*i, unit);
                Console.WriteLine(String.Format("{0} : Success!", i));
            }
            Console.WriteLine("Finished Reading!");
            for (int i = 0; i < data.Length; i++)
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
