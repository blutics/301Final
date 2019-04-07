using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zip301
{
    class MinPriorityQueue
    {
        public List<Node> nlist = new List<Node>();

        public MinPriorityQueue()
        {
            
        }
        
        public void InsertNode(Node node)
        {
            nlist.Add(node);
            nlist.Sort(delegate (Node a, Node b)
            {
                return b.getWeight().CompareTo(a.getWeight());
            });
        }
        public Node PopMin()
        {
            Node last = nlist[nlist.Count - 1];
            nlist.RemoveAt(nlist.Count - 1);
            return last;
        }
        
        public void PrintState()
        {
            for (int i = 1; i < nlist.Count; i++)
            {
                Console.WriteLine(String.Format("{0} ", nlist[i].freq));
            }
        }
    }
}
