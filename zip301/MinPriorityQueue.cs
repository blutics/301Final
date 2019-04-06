using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zip301
{
    class MinPriorityQueue
    {
        public double total = 0;
        private string sfile;
        public List<Node> nlist = new List<Node>();
        private int count = 0;
        private int exchange = 0;
        private int heapsize;
        public MinPriorityQueue(string filename, StreamReader file)
        {
            sfile = filename;
            this.Dataset(file);
        }
        public void Dataset(StreamReader file)
        {
            int n = int.Parse(file.ReadLine());
            heapsize = n - 1;
            nlist.Add(new Node("FirstElement|0|0|0|0"));
            for (int i = 0; i < n; i++)//instanciating each Node, n times
            {
                nlist.Add(new Node(file.ReadLine()));//init!
            }
        }
        private int Left(int i)
        {
            return 2 * i;
        }
        private int Right(int i)
        {
            return 2 * i + 1;
        }
        public int Parent(int i)
        {
            return (int)(i / 2);
        }
        public void MaxHeapify(int i)
        {
            int l = Left(i);
            int r = Right(i);
            int largest;
            count += 5;
            if (l <= heapsize && nlist[l].id > nlist[i].id)
            {
                largest = l;
            }
            else
            {
                largest = i;
            }
            if (r <= heapsize && nlist[r].id > nlist[largest].id)
            {
                largest = r;
            }
            if (largest != i)
            {
                Node tmp = nlist[i];
                nlist[i] = nlist[largest];
                nlist[largest] = tmp;
                exchange += 1;
                MaxHeapify(largest);
            }
        }
        public void BuildMaxHeap()
        {
            heapsize = nlist.Count - 1;
            for (int i = (int)(nlist.Count / 2); i >= 1; i--)
            {
                MaxHeapify(i);
            }

        }
        public void HeapSort()
        {
            if (sfile == "small.txt")
            {
                PrintState();
            }
            BuildMaxHeap();
            for (int i = nlist.Count - 1; i >= 2; i--)
            {
                if (sfile == "small.txt")
                {
                    PrintState();
                }
                Node tmp = nlist[1];
                nlist[1] = nlist[i];
                nlist[i] = tmp;
                heapsize -= 1;
                exchange += 1;
                MaxHeapify(1);

            }
            if (sfile == "small.txt")
            {
                PrintState();
            }
            Console.WriteLine("===================================");
            Console.WriteLine(String.Format("|{0,-30:s}|", " ** Heap Sorting Result *** "));
            Console.WriteLine(String.Format("|{0,-10:s} : {1,20:d}|", "Comparison", count));
            Console.WriteLine(String.Format("|{0,-10:s} : {1,20:d}|", "Exchange", exchange));
            Console.WriteLine("===================================");

            StreamWriter file = new StreamWriter("output.txt");
            for (int i = 1; i < nlist.Count; i++)//the resulte stores at the output.txt file
            {
                file.WriteLine(nlist[i].RawPrint());
            }
            file.Close();
        }
        public void PrintState()
        {
            for (int i = 1; i < nlist.Count; i++)
            {
                Console.Write(String.Format("{0} ", nlist[i].id));
            }
            Console.Write("\n");
        }
    }
}
