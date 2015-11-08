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
            //These are extension methods that make life super easy (why I chose C#).
            //List manipulation is a breeze and it's typesafe with runtime generics
            List<Line2D> lines = prims.OfType<Line2D>().ToList();
            List<Polygon2D> polygons = prims.OfType<Polygon2D>().ToList();
            //Create clipper object
            Clipper c = new Clipper((int)a.VP_XLower, (int)a.VP_XUpper, (int)a.VP_YLower, (int)a.VP_YUpper);
            
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

            foreach (Line2D i in lines)
            {
                i.MapToViewPort(PixelBuffer.GetVPMatrix(a.XLower, a.XUpper, a.YLower, a.YUpper, a.VP_XLower, a.VP_XUpper, a.VP_YLower, a.VP_YUpper));
            }
            foreach (Polygon2D i in polygons)
            {
                i.MapToViewPort(PixelBuffer.GetVPMatrix(a.XLower, a.XUpper, a.YLower, a.YUpper, a.VP_XLower, a.VP_XUpper, a.VP_YLower, a.VP_YUpper));
            }
            //Clip lines
            lines = c.ClipLines(lines);
            //Clip polygons
            polygons = c.ClipPolygons(polygons);
            //Draw lines
            PixelBuffer pb = new PixelBuffer(a.XLower, a.XUpper, a.YLower, a.YUpper, a.VP_XLower, a.VP_XUpper, a.VP_YLower, a.VP_YUpper);
            if(lines.Count >= 1)
                pb.ScanConvertLines(lines);
            //Draw polygons if there are any to draw
            foreach (var p in polygons)
            {
                foreach (var v in p)
                {
                    v.Round();
                }
            }
            if(polygons.Count >= 1)
                pb.DrawPolygons(polygons);
            //Write to console
            Console.Write(pb.WriteToXPM());
        }
    }
}
