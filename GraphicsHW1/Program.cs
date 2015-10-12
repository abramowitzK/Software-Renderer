using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GraphicsHW.Primitives;
using GraphicsHW.Util;
using GraphicsHW.Math;

namespace GraphicsHW
{
    public class Program
    {
        static void Main(string[] args)
        {
            Arguments a = new Arguments(args);

            
            PostscriptReader rdr = new PostscriptReader(a.InputFile);
            List<Primitive> lines = rdr.ReadFile();
            foreach (var i in lines)
            {
                Console.WriteLine(i);
            }
        }
    }
}
