using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zip301
{
    class Node
    {
        public double weight;
        public double height;
        public char letter;
        public Node left;
        public Node right;
        public int freq = 0;
        public Node()
        {
            height = 0;
            freq = 0;
            left = null;
            right = null;
        }
        public Node(char data, int value)
        {
            this.letter = data;
            this.freq = value;
            left = null;
            right = null;
        }
        public void setLeft(Node node)
        {
            this.left = node;

        }
        public void setRight(Node node)
        {
            this.right = node;
        }
        public double getWeight()
        {
            return freq - (500-letter) / 10000 - (500-height) / 1000000000;
        }
        public void print()//This will print out employee's information with a strict format
        {
            Console.WriteLine("===================================");
            Console.WriteLine("===================================");
        }
    }
}
