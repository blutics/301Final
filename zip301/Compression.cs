using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zip301
{
    class Compression
    {
        private int unit;
        public string filename;
        public int textLength;
        public MinPriorityQueue queue;
        public string[] map = new string[128];
        public Node root;
        public BitArray[] bitMap;
        public Compression(FileStream file)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            bitMap = new BitArray[128];
            string[] tmp = file.Name.Split('\\');
            filename = tmp[tmp.Length - 1].Split('.')[0];
            queue = new MinPriorityQueue();
            unit = 4096*1000;
            this.textLength = (int)file.Length;
            BufferedStream bs = new BufferedStream(file);
            //Console.WriteLine(textLength/unit);
            Console.WriteLine(String.Format("Text Length : {0}",textLength));
            byte[] data = new byte[textLength+unit-(textLength%unit)];
            int[] count = new int[128];
            for(int i = 0; i < (int)(textLength / unit)+1; i++)
            {
                bs.Read(data, unit*i, unit);
                //Console.WriteLine(String.Format("{0} : Success!", i));
            }
            Console.WriteLine("Finished Reading!");
            for (int i = 0; i < textLength; i++)
            {
               count[data[i]] += 1;
            }
            Console.WriteLine("Counting Completed!");
            
            for(int i = 0; i < count.Length; i++)
            {
                //Console.WriteLine(String.Format("{0,2} : {1,10}", Convert.ToChar(i), count[i]));
                if (count[i] != 0)
                {
                    queue.InsertNode(new Node((char)i, count[i]));
                }
            }

            
            Console.WriteLine("Queueing Completed!");
            root = Huffman(queue);
            Console.WriteLine("Huffman Completed!");
            CodeFilling(map, root, "");

            BytesCollector a = new BytesCollector();
            byte[] bytes;
       
            for (int k = 0; k < 128; k++)
            {
                if (count[k] != 0)
                {
                    a.Insert(map[k] + " " + (char)k);
                    //Console.WriteLine(map[k] + " " + (char)k);
                }
            }

            StringBuilder stringResult = new StringBuilder();
            for(int k = 0; k < textLength; k++)
            {
                stringResult.Append(map[data[k]]);
            }
            int p = stringResult.Length % 8;
            if (p != 0)
            {
                for(int k = 0; k < (8 - p); k++)
                {
                    stringResult.Append("0");
                }
            }
            byte[] byteBuffer = new byte[(int)(stringResult.Length / 8)];
            string binaryString = stringResult.ToString();
            for(int k = 0; k < (int)(stringResult.Length / 8); k++)
            {
                byteBuffer[k]=Convert.ToByte(binaryString.Substring(0*8,8),2);
            }
            Console.WriteLine(stringResult.Length);
            
            Console.WriteLine("Compression Completed!");

            //Console.WriteLine(bitCount);
            a.Insert("*****");
            byte[] buffer = a.ByteChunk();
            Console.WriteLine(filename);
            Stream output = File.OpenWrite(filename + ".zip301");
            BufferedStream open = new BufferedStream(output);
            open.Write(buffer, 0, buffer.Length);
            open.Write(byteBuffer, 0, byteBuffer.Length);
            open.Close();
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds.ToString() + "ms");


        }
        public Node Huffman(MinPriorityQueue queue)
        {
            int n = queue.nlist.Count;
            for(int i = 0; i < n-1 ; i++)
            {
                Node z = new Node();
                Node x = queue.PopMin();
                Node y = queue.PopMin();
                z.setLeft(x);   
                z.setRight(y);
                z.freq = x.freq + y.freq;
                z.height = Math.Max(x.height, y.height) + 1;
                queue.InsertNode(z);
            }
            return queue.PopMin();
        }
        public void CodeFilling(string[] table, Node current, string a)
        {
            if (current.left != null)
            {
                CodeFilling(table, current.left, a+"0");
            }
            if (current.right != null)
            {
                CodeFilling(table, current.right, a+"1");
            }
            if (current.right==null && current.left==null)
            {
                table[current.letter]= a;
            }
        }

    }
}
