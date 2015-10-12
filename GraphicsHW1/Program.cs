using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GraphicsHW.Primitives;
namespace GraphicsHW
    {
    class Program
        {
        static void Main(string[] args)
            {
            PostscriptReader rdr = new PostscriptReader("C:\\Users\\Kyle\\Desktop\\hw1-1.ps");
            rdr.ReadFile();
            }
        }
    }
