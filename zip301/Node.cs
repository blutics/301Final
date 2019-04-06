using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zip301
{
    class Node
    {
        public string name;
        public int id;
        int age;
        string job;
        int year;
        public string rawdata;
        public Node(string data)
        {
            string[] tmp = data.Split('|');
            name = tmp[0];
            id = int.Parse(tmp[1]);
            age = int.Parse(tmp[2]);
            job = tmp[3];
            year = int.Parse(tmp[4]);
            rawdata = data;
        }
        public void print()//This will print out employee's information with a strict format
        {
            Console.WriteLine("===================================");
            Console.WriteLine(String.Format("|{0,-10:s} : {1,-20:s}|", "Name", name));
            Console.WriteLine(String.Format("|{0,-10:s} : {1,-20:d}|", "ID", id));
            Console.WriteLine(String.Format("|{0,-10:s} : {1,-20:d}|", "Age", age));
            Console.WriteLine(String.Format("|{0,-10:s} : {1,-20:s}|", "Job", job));
            Console.WriteLine(String.Format("|{0,-10:s} : {1,-20:d}|", "year", year));
            Console.WriteLine("===================================");
        }
        public string RawPrint()
        {
            return rawdata;
        }
    }
}
