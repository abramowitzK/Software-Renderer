using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GraphicsHW.Primitives;
using GraphicsHW.Math;

namespace GraphicsHW.Util
{
    //Stores the framebuffer and contains the drawing routines as well as the xpm writing routines
    public class PixelBuffer
    {
        //This declares a multidimensional array. Size specified at object instantiation
        private bool[,] m_pixelArray;
        private int m_width;
        private int m_height;
        private int m_ymax;
        private int m_ymin;
        private int m_xmin;
        private int m_xmax;

        public static Matrix3<double> GetVPMatrix(double xmin, double xmax, double ymin, double ymax, double vpxmin, double vpxmax, double vpymin, double vpymax)
        {

            Matrix3<double> T_uv = Trans2D.GetTranslationMatrix(vpxmin, vpymin);
            Matrix3<double> S = Trans2D.GetScalingMatrix((vpxmax - vpxmin) / (xmax - xmin), (vpymax - vpymin) / (ymax - ymin));
            Matrix3<double> T_xy = Trans2D.GetTranslationMatrix(-xmin, -ymin);
            return T_uv *S * T_xy;
        }
        public PixelBuffer(int xmin, int xmax, int ymin, int ymax, double vpxmin, double vpxmax, double vpymin, double vpymax)
        {

            //m_ymin = ymin;
            m_ymax = ymax;
            //m_xmin = xmin;
            m_xmax = xmax;
            m_height = 501;
            m_width = 501;
            m_pixelArray = new bool[502, 502];
            
        }
        public string WriteToXPM()
        {
            // Use string builder since strings are immutable in C# and are really slow to concatenate with + operator in a tight loop
            string xpm = @"/* XPM */ static char* sco100[] = { /* width height num_colors chars_per_pixel */""" + m_width + " " + m_height + @" 2 1"", /*colors*/ ""- c #ffffff"", ""@ c #000000"" /*pixels*/""";
            StringBuilder sb = new StringBuilder(xpm);
            for (int i = 0; i < m_pixelArray.GetLength(1); i++)
            {
                sb.Append(@"""");
                for (int j = 0; j < m_pixelArray.GetLength(0); j++)
                {
                    if (m_pixelArray[j, i])
                        sb.Append("@");
                    else
                        sb.Append("-");
                }
                sb.Append(@""",");
            }
            //Remove that extra comma, even though it doesn't seem to be a problem
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");

            return sb.ToString();
        }
        //Write pixel at specified location. Have to transform the y coordinate so origin is in bottom left
        public void WritePixel(int i, int j, bool isBlack)
        {
            m_pixelArray[i,m_height - j] = isBlack;
        }
        public void ScanConvertLines(List<Line2D> lines)
        {
            double deltaX;
            double deltaY;
            //Need to check for vertical line...
            //var is like auto in c++11
            foreach (var line in lines)
            {
                int steps = 0;
                double currentX = 0f;
                double currentY = 0f;
                double slope = CalcSlope(line);
                bool startedAtEnd = false;
                if (double.IsInfinity(slope))
                {
                    deltaX = 0;
                    if (line.Start[1] > line.End[1])
                    {
                        startedAtEnd = true;
                        currentX = line.End[0];
                        currentY = line.End[1];
                    }
                    else
                    {
                        currentX = line.Start[0];
                        currentY = line.Start[1];
                    }
                    deltaY = 1f;
                    steps = (int)System.Math.Abs((line.Start[1] - line.End[1]));
                }
                else if (System.Math.Abs(slope) < 1f) //Forgetting to do absolute value is what caused the skips in the line...
                {
                    deltaX = 1f;
                    deltaY = slope;
                    steps = (int)System.Math.Abs((line.Start[0] - line.End[0]));
                    if (line.Start[0] > line.End[0])
                    {
                        startedAtEnd = true;
                        currentX = line.End[0];
                        currentY = line.End[1];
                    }
                    else
                    {
                        currentX = line.Start[0];
                        currentY = line.Start[1];
                    }
                }
                else
                {
                    deltaY = 1f;
                    deltaX = 1/slope;
                    steps = (int)System.Math.Abs((line.Start[1] - line.End[1]));
                    if (line.Start[1] > line.End[1])
                    {
                        startedAtEnd = true;
                        currentX = line.End[0];
                        currentY = line.End[1];
                    }
                    else
                    {
                        currentX = line.Start[0];
                        currentY = line.Start[1];
                    }
                }
                // Starting pixel
                WritePixel((int)System.Math.Round(currentX), (int)System.Math.Round(currentY), true);
                for (int i = 0; i < steps; i++)
                {
                    currentX += deltaX;
                    currentY += deltaY;
                    if ((currentX > m_xmax) || (currentY > m_ymax))
                        break;
                    if ((currentX < 0) || (currentY < 0))
                        break;
                    WritePixel((int)System.Math.Round(currentX, MidpointRounding.AwayFromZero), (int)System.Math.Round(currentY, MidpointRounding.AwayFromZero), true);
                }
                if(startedAtEnd)
                    WritePixel((int)System.Math.Round(line.End[0], MidpointRounding.AwayFromZero), (int)System.Math.Round(line.End[1], MidpointRounding.AwayFromZero), true);
                else
                    WritePixel((int)System.Math.Round(line.End[0], MidpointRounding.AwayFromZero), (int)System.Math.Round(line.End[1], MidpointRounding.AwayFromZero), true);
            }
        }
        public void DrawPolygons(List<Polygon2D> polygons)
        {
            foreach (Polygon2D p in polygons)
            {
                List<Line2D> lines = p.GetLines();
                ScanConvertLines(lines);
               FillPolygon(p);
            }
        }
        public void FillPolygon(Polygon2D p)
        {
            //Tried using fill lines and storing them in the polygon. Had issues with concave polygons where it would draw outside of them because I didn't
            //have mechanism like the parity bit flip. This is attempt two....
            int Y = (int)(p.OrderByDescending(i => i.Y).First().Y);
            int lowestY = (int)(p.OrderByDescending(i => i.Y).Last().Y);
            int lowestX = (int)(p.OrderByDescending(i => i.X).Last().X);
            int highestX = (int)(p.OrderByDescending(i => i.X).First().X);
            while (Y >= lowestY)
            {
                List<Vector3<double>> Intersections = new List<Vector3<double>>();
                
                List<Line2D> lines = p.GetLines();
                for(int i = 0; i < lines.Count; i++)
                {
                    Line2D scanLine = new Line2D(new Vector3<double>(lowestX, Y, 1), new Vector3<double>(highestX, Y, 1));
                    Vector3<double> intPoint = Intersect(scanLine, lines[i]);
                    if (null != intPoint) // they interesected
                    {
                        if ((int)System.Math.Round(intPoint.Y) != lines[i].MaxY)
                        {

                            Intersections.Add(intPoint);
                        }
                        else
                        {
                            //we need to do something about the corners causing issues with rounding.

                        }
                    }
                }
                //sort items by x value so we can easily create lines

                var sorted = Intersections.OrderBy(i => i.X).ToList();

                while (sorted.Count > 1)
                {
                    //Start at least x. Not using a parity bit. but this functions exactly the same way.
                    //Y stays constant while drawing. X is incremented
                    int x = (int)System.Math.Round(sorted[0].X, MidpointRounding.AwayFromZero);
                    //Start drawing and remove first point.
                    sorted.RemoveAt(0);
                    x++;
                    while (x < sorted[0].X)
                    {
                        WritePixel(x, Y, true);
                        x++;
                    }
                    sorted.RemoveAt(0);
                }
                Y--;
            }
        }
        //Polygon intersection
        //Intersection between two lines
        private Vector3<double> Intersect(Line2D scanLine, Line2D intersectionLine)
        {
            var p0 = scanLine.End;
            var p1 = scanLine.Start;
            var p2 = intersectionLine.End;
            var p3 = intersectionLine.Start;
            var d0 = p1 - p0;
            var d2 = p3 - p2;
            //check if values are the same. This is to avoid floating point inaccuracies that were messsing with the calculations. Yay math.
            if ((int)(System.Math.Round(p1.X, MidpointRounding.AwayFromZero)) == (int)(System.Math.Round(p3.X, MidpointRounding.AwayFromZero)) && (int)(System.Math.Round(p1.Y, MidpointRounding.AwayFromZero)) == (int)(System.Math.Round(p3.Y, MidpointRounding.AwayFromZero)))
                return p1;
            if ((int)(System.Math.Round(p1.X, MidpointRounding.AwayFromZero)) == (int)(System.Math.Round(p2.X, MidpointRounding.AwayFromZero)) && (int)(System.Math.Round(p1.Y, MidpointRounding.AwayFromZero)) == (int)(System.Math.Round(p2.Y, MidpointRounding.AwayFromZero)))
                return p1;
            if ((int)(System.Math.Round(p0.X, MidpointRounding.AwayFromZero)) == (int)(System.Math.Round(p2.X, MidpointRounding.AwayFromZero)) && (int)(System.Math.Round(p0.Y, MidpointRounding.AwayFromZero)) == (int)(System.Math.Round(p2.Y, MidpointRounding.AwayFromZero)))
                return p0;
            if ((int)(System.Math.Round(p0.X, MidpointRounding.AwayFromZero)) == (int)(System.Math.Round(p3.X, MidpointRounding.AwayFromZero)) && (int)(System.Math.Round(p0.Y, MidpointRounding.AwayFromZero)) == (int)(System.Math.Round(p3.Y, MidpointRounding.AwayFromZero)))
                return p0;
            try
            {
                var t0 = ((p0.X - p2.X) * d2.Y + (p2.Y - p0.Y) * d2.X) / ((d0.Y * d2.X) - (d0.X * d2.Y));
                var t2 = ((p2.X - p0.X) * d0.Y + (p0.Y - p2.Y) * d0.X) / ((d2.Y * d0.X) - (d2.X - d0.Y));
                if (double.IsInfinity(t0) || double.IsInfinity(t2) || double.IsNegativeInfinity(t0) || double.IsNegativeInfinity(t2))
                {
                    //lines are parallel
                    return null;
                }
                if (t2 <= 1 && t2 >= 0)
                {
                    return (p2 + (t2 * d2));
                }
                return null;
            }
            catch (DivideByZeroException)
            {
                //Lines are parallel
                return null;
            }
        }
        private double CalcSlope(Line2D line)
        {
            double rise = line.End[1] - line.Start[1];
            double run = line.End[0] - line.Start[0];
            return (rise / run);
        }

    }
}
