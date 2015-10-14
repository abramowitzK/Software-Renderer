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
            //Parse the argument array in arbitrary order and give an iterface to retrieve them
            Arguments a = new Arguments(args);
            //Read the input file specified in the arguments
            PostscriptReader rdr = new PostscriptReader(a.InputFile);
            List<Line> lines = rdr.ReadFile();
            Clipper c = new Clipper(a.XLower, a.XUpper, a.YLower, a.YUpper);
            lines = c.ClipLines(lines);
            PixelBuffer pb = new PixelBuffer(a.XLower, a.XUpper, a.YLower, a.YUpper);
            foreach (Line i in lines)
            {
                i.Scale(a.Scale, a.Scale);
                i.Rotate(a.Rotation);
                i.Translate(a.XTranslation, a.YTranslation);
                Console.WriteLine(i);
            }
        }
    }
}
