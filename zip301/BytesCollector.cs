using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zip301
{
    class BytesCollector
    {
        public List<byte[]> blist;
        public int bcount;
        public BytesCollector()
        {
            blist = new List<byte[]>();
            bcount = 0;
        }
        public void Insert(byte[] b)
        {
            bcount += b.Length;
            blist.Add(b);
        }
        public void Insert(string b)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(b);
            bcount += bytes.Length;
            blist.Add(bytes);
        }
        public byte[] ByteChunk()
        {
            byte[] result = new byte[bcount];
            int index = 0;
            foreach(byte[] k in blist)
            {
                for(int i =0;i<k.Length;i++)
                {
                    result[index + i] = k[i];
                }
                index += k.Length;
            }
            return result;
        }
    }
}
