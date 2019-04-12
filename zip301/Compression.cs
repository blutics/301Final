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
        public Compression(FileStream file)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string[] tmp = file.Name.Split('\\');
            filename = tmp[tmp.Length - 1].Split('.')[0];
            queue = new MinPriorityQueue();
            unit = 4096*1000;
            this.textLength = (int)file.Length;
            BufferedStream bs = new BufferedStream(file);
            //Console.WriteLine(textLength/unit);
            Console.WriteLine("========================================================");
            Console.WriteLine(String.Format("{0,-37} : {1,16}", "Input File", tmp[tmp.Length - 1]));
            Console.WriteLine(String.Format("{0,-37} : {1,10} bytes", "Original File Size", textLength));
            Console.WriteLine("========================================================");
            byte[] data = new byte[textLength+unit-(textLength%unit)];
            int[] count = new int[128];
            for(int i = 0; i < (int)(textLength / unit)+1; i++)
            {
                bs.Read(data, unit*i, unit);
                //Console.WriteLine(String.Format("{0} : Success!", i));
            }
            Console.WriteLine(String.Format("{0,-37} : {1,13} ms", "Finished Reading", sw.ElapsedMilliseconds.ToString()));
            for (int i = 0; i < textLength; i++)
            {
               count[data[i]] += 1;
            }
            Console.WriteLine(String.Format("{0,-37} : {1,13} ms", "Counting Complete", sw.ElapsedMilliseconds.ToString()));

            for (int i = 0; i < count.Length; i++)
            {
                //Console.WriteLine(String.Format("{0,2} : {1,10}", Convert.ToChar(i), count[i]));
                if (count[i] != 0)
                {
                    queue.InsertNode(new Node((char)i, count[i]));
                }
            }

            Console.WriteLine(String.Format("{0,-37} : {1,13} ms", "Queueing Completed", sw.ElapsedMilliseconds.ToString()));
            root = Huffman(queue);
            Console.WriteLine(String.Format("{0,-37} : {1,13} ms", "Huffman Tree Prepared", sw.ElapsedMilliseconds.ToString()));
            CodeFilling(map, root, "");

            BytesCollector a = new BytesCollector();
            
            for (int k = 0; k < 128; k++)
            {
                if (count[k] != 0)
                {
                    if (k == '\n')
                    {
                        a.Insert(map[k] + " " + "newline\n");
                    }else if (k==' ')
                    {
                        a.Insert(map[k] + " " + "space\n");
                    }else if (k == '\t')
                    {
                        a.Insert(map[k] + " " + "tab\n");
                    }else if (k == '\r')
                    {
                        a.Insert(map[k] + " " + "return\n");
                    }
                    else
                    {
                        a.Insert(map[k] + " " + (char)k+"\n");
                    }
                    //Console.WriteLine(map[k] + " " + (char)k);
                }
            }

            StringBuilder stringResult = new StringBuilder();
            for(int k = 0; k < textLength; k++)
            {
                stringResult.Append(map[data[k]]);
                //Console.WriteLine(map[data[k]]);
            }
            for(int k = 0; k<100; k++)
            {
                //Console.WriteLine(stringResult[k]);
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
            for (int k = 0; k < (int)(stringResult.Length / 8); k++)
            {
                byteBuffer[k]=Convert.ToByte(binaryString.Substring(k*8,8),2);
            }

            Console.WriteLine(String.Format("{0,-37} : {1,13} ms", "Compression Completed", sw.ElapsedMilliseconds.ToString()));

            //Console.WriteLine(bitCount);
            a.Insert("*****\n");
            a.Insert(String.Format("{0}\n",stringResult.Length-6));
            byte[] buffer = a.ByteChunk();
            Stream output = File.OpenWrite(filename + ".zip301");
            BufferedStream open = new BufferedStream(output);
            open.Write(buffer, 0, buffer.Length);
            open.Write(byteBuffer, 0, byteBuffer.Length);
            Console.WriteLine(String.Format("{0,-37} : {1,13} ms", "Writing Finished", sw.ElapsedMilliseconds.ToString()));
            Console.WriteLine("========================================================");

            open.Close();
            sw.Stop();
            Console.WriteLine(String.Format("{0,-37} : {1,11} bits\n", "The number of Compressed Data bits", stringResult.Length - 6));
            Console.WriteLine("========================================================");

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
