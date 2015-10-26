using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GraphicsHW.Primitives;
using GraphicsHW.Math;

namespace GraphicsHW.Util
{
    public class Clipper
    {
        private double m_xMin;
        private double m_xMax;
        private double m_yMin;
        private double m_yMax;
        private Polygon2D m_clipPoly;
        public Clipper(int xmin, int xmax, int ymin, int ymax)
        {
            m_xMin = xmin;
            m_xMax = xmax;
            m_yMin = ymin;
            m_yMax = ymax;
            m_clipPoly = new Polygon2D(m_xMin, m_yMin);
            //Need to add vertices in clockwise order
            m_clipPoly.AddVertex(new Vector3<double>(m_xMax, m_yMin, 1));
            m_clipPoly.AddVertex(new Vector3<double>(m_xMax, m_yMax, 1));
            m_clipPoly.AddVertex(new Vector3<double>(m_xMin, m_yMax, 1));

        }
        #region LineClipping
        // Returns a list of clipped lines to draw. Some input lines may be discarded if entirely out of drawing area.
        public List<Line2D> ClipLines(List<Line2D> lines)
        {
            List<Line2D> newList = new List<Line2D>();
            foreach (var line in lines)
            {
                Line2D temp;
                if (null != (temp = ClipLine(line)))
                {
                    newList.Add(temp);
                }
            }
            return newList;
        }
        // Using the cohen sutherland algorithm
        private Line2D ClipLine(Line2D line)
        {
            
            Line2D newLine = new Line2D();
            newLine.Start = new Vector3<double>(line.Start[0], line.Start[1], 1);
            newLine.End = new Vector3<double>(line.End[0], line.End[1], 1);
            BitCodes code1 = GetCode(newLine.Start);
            BitCodes code2 = GetCode(newLine.End);
            while (true)
            {
                if ((code1 | code2) == BitCodes.Middle)
                {
                    //Hooray we don't need to clip!
                    return newLine;
                }
                else if ((code1 & code2) != BitCodes.Middle)
                {
                    //Line2D totally outside area
                    return null;
                }
                else
                {
                    //More complicated...
                    //at least one endpoint is outside
                    //Need to check for flipped bits
                    //can use xor
                    byte temp = (byte)(code1 ^ code2);
                    byte bitNumber = 4;
                    //If the codes are not either of the trivial cases, at least one bit must be flipped.
                    //Therefore this loop cannot go infinite
                    while (temp != 1)
                    {
                        temp = (byte)(temp >> 1);
                        bitNumber--;
                    }

                    //Pick which point to work on
                    double x = 0;
                    double y = 0;
                    BitCodes workingCode = code1;
                    switch (bitNumber)
                    {
                        case 1:
                            if ((code2 & BitCodes.Top) != 0)
                                workingCode = code2;
                            // y0 > WT and y1 <= WT
                            // yc = WT
                            // xc = ((WT - y0)/(y1 - y0))*(x1 - x0) + x0
                            x = ((m_yMax - newLine.Start[1]) / (newLine.End[1] - newLine.Start[1])) * (newLine.End[0] - newLine.Start[0]) + newLine.Start[0];
                            y = m_yMax;
                            break;
                        case 2:
                            if ((code2 & BitCodes.Bottom) != 0)
                                workingCode = code2;
                            // y0 < WB and y1 >= WB
                            // yc = WB
                            // xc = ((WB - y0)/(y1 - y0))*(x1 - x0) + x0
                            x = ((m_yMin - newLine.Start[1]) / (newLine.End[1] - newLine.Start[1])) * (newLine.End[0] - newLine.Start[0]) + newLine.Start[0];
                            y = m_yMin;
                            break;
                        case 3:
                            if ((code2 & BitCodes.Right) != 0)
                                workingCode = code2;
                            // x0 > WR and x2 <= WR
                            // xc = WR
                            // yc = ((WR - x0)/(x1 - x0))*(y1 - y0) + y0
                            
                            y = ((m_xMax - newLine.Start[0]) / (newLine.End[0] - newLine.Start[0])) * (newLine.End[1] - newLine.Start[1]) + newLine.Start[1];
                            x = m_xMax;
                            break;
                        case 4:
                            if ((code2 & BitCodes.Left) != 0)
                                workingCode = code2;
                            // x0 < WL and x1 >= WL
                            // xc = WL
                            // yc = ((WL - x0)/(x1 - x0))*(y1 - y0) + y0
                            
                            y = ((m_xMin - newLine.Start[0]) / (newLine.End[0] - newLine.Start[0])) * (newLine.End[1] - newLine.Start[1]) + newLine.Start[1];
                            x = m_xMin;
                            break;
                    }
                    if (code1 == workingCode)
                    {
                        newLine.Start[0] = x;
                        newLine.Start[1] = y;
                        code1 = GetCode(newLine.Start);
                    }
                    else
                    {
                        newLine.End[0] = x;
                        newLine.End[1] = y;
                        code2 = GetCode(newLine.End);
                    }
                }
            }
        }

        private BitCodes GetCode(Vector3<double> point)
        {
            double x = point[0];
            double y = point[1];
            // Start out in middle. Change if necessary by or-ing
            BitCodes code = BitCodes.Middle;
            if (x < m_xMin)
                code = code | BitCodes.Left;
            if (x > m_xMax)
                code = code | BitCodes.Right;
            if (y < m_yMin)
                code = code | BitCodes.Bottom;
            if (y > m_yMax)
                code = code | BitCodes.Top;
            return code;
        }
        [Flags]
        private enum BitCodes
        {
            Middle = 0,
            Left = 1,
            Right = 2,
            Bottom = 4,
            Top = 8,
        }
        #endregion
        #region PolygonClipping
        //Public interface to be called by main program. This will clip a list of arbitrary polygons against the clipping
        //polygon
        public List<Polygon2D> ClipPolygons(List<Polygon2D> polygons)
        {
            List<Polygon2D> newList = new List<Polygon2D>();
            foreach (var polygon in polygons)
            {
                Polygon2D temp;
                if (null != (temp = ClipPolygon(polygon)))
                {
                    newList.Add(temp);
                }
            }
            return newList;
        }
        // Clips a single polygon and returns the clipped polygon as a new instance
        private Polygon2D ClipPolygon(Polygon2D polygon)
        {
            var vertices = polygon.GetVertices();

            foreach (var line in m_clipPoly.GetLines())
            {
                vertices = SutherlandHodgman(vertices, line);
                //We clipped all our vertices and got nothing back, meaning nothing will be displayed.
                //Just return nothing
                if (vertices.Count == 0)
                    return null;
            }
            return new Polygon2D(vertices);
        }
        //Functions that are commented "Based on textbook algorithm" were inspired by ntroduction to Computer Graphics(Foley, Van Dam, et al)
        //All these functions rely on the polygon being specified in counterclockwise vertex ordering.
        //Based on textbook algorithm
        // Calculates the intersection of the line formed by two vertices and a line. 
        private Vector3<double> Intersect(Vector3<double> begin, Vector3<double> end, Line2D clipEdge)
        {
            Vector3<double> ret = new Vector3<double>();
            if (clipEdge.Start.Y == clipEdge.End.Y)
            {
                ret.X = begin.X + (clipEdge.Start.Y - begin.Y) * (end.X - begin.X) / (end.Y - begin.Y);
                ret.Y = clipEdge.Start.Y;
            }
            else
            {
                ret.X = clipEdge.Start.X;
                ret.Y = begin.Y + (clipEdge.Start.X - begin.X) * (end.Y - begin.Y) / (end.X - begin.X);
            }
            return ret;
        }
        //Based on textbook algorithm
        //Determines if a point is inside a given line (inside defined as being to the left of the clipEdge while oriented in the direction of the clip edge)
        private bool Inside(Vector3<double> test, Line2D clipEdge)
        {
            if (clipEdge.End.X > clipEdge.Start.X)
                if (test.Y >= clipEdge.Start.Y)
                    return true;
            if (clipEdge.End.X < clipEdge.Start.X)
                if (test.Y <= clipEdge.Start.Y)
                    return true;
            if (clipEdge.End.Y > clipEdge.Start.Y)
                if (test.X <= clipEdge.End.X)
                    return true;
            if (clipEdge.End.Y < clipEdge.Start.Y)
                if (test.X >= clipEdge.End.X)
                    return true;
            return false;
        }
        //Based on textbook algorithm
        //Implements the sutherlandHodgman polygon clipping algorithm. This function will only
        //clip the polygon against one clip edge at a time. Therefore, to clip the entire polygon, we feed the output
        //of this command into the input of another call to this with a different clip edge and keep
        // doing this until we run out of clip edges 
        private List<Vector3<double>> SutherlandHodgman(List<Vector3<double>> inVertices, Line2D clipEdge)
        {
            List<Vector3<double>> output = new List<Vector3<double>>();
            Vector3<double> s = inVertices.Last();
            Vector3<double> p;
            for(int i = 0; i < inVertices.Count; i++)
            {
                p = inVertices[i];
                if (Inside(p, clipEdge))
                {
                    if (Inside(s, clipEdge))
                    {
                        output.Add(p);
                    }
                    else
                    {
                        output.Add(Intersect(s, p, clipEdge));
                        output.Add(p);
                    }
                }
                else if(Inside(s, clipEdge))
                {
                    output.Add(Intersect(s, p, clipEdge));
                }
                s = p;
            }
            return output;
        }

        #endregion
    }
}
