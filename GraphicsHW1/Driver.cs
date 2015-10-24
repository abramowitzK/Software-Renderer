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
            List<Primitive> prims = rdr.ReadFile();
            List<Line2D> lines = prims.OfType<Line2D>().ToList();
            List<Polygon2D> polygons = prims.OfType<Polygon2D>().ToList();
            //Create clipper object
            Clipper c = new Clipper(a.XLower, a.XUpper, a.YLower, a.YUpper);
            
            //Transform all our endpoints
            foreach (Line2D i in lines)
            {
                //This method combines all the homogenoeous matrices into one and multiplies the endpoints by them
                i.Transform(a.Scale, a.Scale, a.Rotation, a.XTranslation, a.YTranslation);
            }
            foreach (Polygon2D i in polygons)
            {
                i.Transform(a.Scale, a.Scale, a.Rotation, a.XTranslation, a.YTranslation);
            }
            //Clip lines
            lines = c.ClipLines(lines);
            //Draw lines
            PixelBuffer pb = new PixelBuffer(a.XLower, a.XUpper, a.YLower, a.YUpper);
            pb.ScanConvertLines(lines);
            pb.DrawPolygons(polygons);
            //Write to console
            Console.Write(pb.WriteToXPM());
        }
    }
}
