using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unzip301
{
    class Decompression
    {
        private int unit;
        public int zipLength;
        public char[] mapTree;
        public List<string[]> mapList;
        public string filename;
        public Decompression(FileStream file)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string[] tmp = file.Name.Split('\\');
            filename = tmp[tmp.Length - 1].Split('.')[0];
            unit = 4096 * 1000;
            this.zipLength = (int)file.Length;
            BufferedStream bs = new BufferedStream(file);
            //Console.WriteLine(zipLength/unit);
            Console.WriteLine(String.Format("Text Length : {0}", zipLength));
            byte[] data = new byte[zipLength + unit - (zipLength % unit)];
            int[] count = new int[128];
            for (int i = 0; i < (int)(zipLength / unit) + 1; i++)
            {
                bs.Read(data, unit * i, unit);
                //Console.WriteLine(String.Format("{0} : Success!", i));
            }
            Console.WriteLine("Finished Reading!");

            int readIndex = 0;
            string stringBuffer = "";
            int maxDepth = 0;
            mapList = new List<string[]>();
            string[] tmpBuffer;
            while (true)
            {
                stringBuffer += (char)data[readIndex];
                if (data[readIndex]=='\n')
                {
                    if (data[readIndex + 1] == '\n')
                    {
                        stringBuffer += (char)data[readIndex];
                        readIndex++;
                    }
                    if (stringBuffer == "*****\n")
                    {
                        readIndex++;
                        stringBuffer = "";
                        break;
                    }

                    tmpBuffer = stringBuffer.Split(' ');
                    mapList.Add(tmpBuffer);
                    maxDepth = Math.Max(maxDepth, tmpBuffer[0].Length);                 
                    stringBuffer = "";
                }
                readIndex++;
            }
            while (true)
            {
                stringBuffer += (char)data[readIndex];
                if (data[readIndex] == '\n')
                {
                    readIndex++;
                    break;
                }
                readIndex++;
            }
            mapTree = new char[(int)Math.Pow(2, maxDepth+2)];
            for(int i = 0; i < mapTree.Length; i++)
            {
                mapTree[i] = 'æ';
            }

            int currentState = 1;
            foreach (string[] k in mapList)
            {
                foreach(char i in k[0])
                {
                    if (i == '0')
                    {
                        currentState *= 2;
                    }
                    else
                    {
                        currentState = 2 * currentState + 1;
                    }
                }

                if (k[1] == "space\n")
                {
                    mapTree[currentState] = ' ';
                }
                else if (k[1] == "newline\n")
                {
                    mapTree[currentState] = (char)0x0a;
                }
                else if (k[1] == "return\n")
                {
                    mapTree[currentState] = '\r';
                }
                else if (k[1] == "tab\n")
                {
                    mapTree[currentState] = '\t';
                }
                else
                {
                    mapTree[currentState] = k[1][0];
                }

                //Console.WriteLine(k[0] + " " + mapTree[currentState]+k[1]);

                currentState = 1;
            }
 
            Console.WriteLine("Head Information Collected!");

            StringBuilder binaryString = new StringBuilder();
            for(int i = readIndex; i < zipLength; i++)
            {
                binaryString.Append(Convert.ToString(data[i], 2).PadLeft(8, '0'));
            }

            Console.WriteLine("Binary String Prepared");

            StringBuilder resultString = new StringBuilder();
            currentState = 1;
            Console.WriteLine(binaryString.Length);
            string stringData = binaryString.ToString();
            for (int i = 0; i<stringData.Length; i++)
            {
                //Console.WriteLine(i);
                if (stringData[i] == '0')
                {
                    currentState *= 2;
                }
                else
                {
                    currentState = 2 * currentState + 1;
                }
                if(mapTree[currentState]!= 'æ')
                {
                    resultString.Append(mapTree[currentState]);
                    currentState = 1;
                }
            }
            
            byte[] result = Encoding.ASCII.GetBytes(resultString.ToString());

            Stream output = File.OpenWrite(filename + "2.txt");
            BufferedStream open = new BufferedStream(output);
            open.Write(result, 0, result.Length);
            open.Close();

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds.ToString() + "ms");
        }
    }
}
