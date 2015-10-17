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
    public class Driver
    {
        static void Main(string[] args)
        {
            //Parse the argument array in arbitrary order and give an iterface to retrieve them
            Arguments a = new Arguments(args);
            //Read the input file specified in the arguments
            PostscriptReader rdr = new PostscriptReader(a.InputFile);
            List<Line> lines = rdr.ReadFile();
            //Create clipper object
            Clipper c = new Clipper(a.XLower, a.XUpper, a.YLower, a.YUpper);
            
            //Transform all our endpoints
            foreach (Line i in lines)
            {
                i.Scale(a.Scale, a.Scale);
                i.Rotate(a.Rotation);
                i.Translate(a.XTranslation, a.YTranslation);
            }
            //Clip lines
            lines = c.ClipLines(lines);
            //Draw lines
            PixelBuffer pb = new PixelBuffer(a.XLower, a.XUpper, a.YLower, a.YUpper);
            pb.ScanConvertLines(lines);
            //Write to console
            Console.Write(pb.WriteToXPM());
        }
    }
}
