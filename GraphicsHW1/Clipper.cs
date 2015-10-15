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
        private float m_xMin;
        private float m_xMax;
        private float m_yMin;
        private float m_yMax;

        public Clipper(int xmin, int xmax, int ymin, int ymax)
        {
            m_xMin = xmin;
            m_xMax = xmax;
            m_yMin = ymin;
            m_yMax = ymax;
        }
        // Returns a list of clipped lines to draw. Some input lines may be discarded if entirely out of drawing area.
        public List<Line> ClipLines(List<Line> lines)
        {
            List<Line> newList = new List<Line>();
            foreach (var line in lines)
            {
                Line temp;
                if (null != (temp = ClipLine(line)))
                {
                    newList.Add(temp);
                }
            }
            return newList;
        }
        // Using the cohen sutherland algorithm
        private Line ClipLine(Line line)
        {
            
            Line newLine = new Line();
            newLine.Start = new Vector2<float>(line.Start[0], line.Start[1]);
            newLine.End = new Vector2<float>(line.End[0], line.End[1]);
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
                    //Line totally outside area
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
                    float x = 0;
                    float y = 0;
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
                            x = m_xMax;
                            y = ((m_xMax - newLine.Start[0]) / (newLine.End[0] - newLine.Start[0])) * (newLine.End[1] - newLine.Start[1]) + newLine.Start[1];
                            break;
                        case 4:
                            if ((code2 & BitCodes.Left) != 0)
                                workingCode = code2;
                            // x0 < WL and x1 >= WL
                            // xc = WL
                            // yc = ((WL - x0)/(x1 - x0))*(y1 - y0) + y0
                            x = m_xMin;
                            y = ((m_xMin - newLine.Start[0]) / (newLine.End[0] - newLine.Start[0])) * (newLine.End[1] - newLine.Start[1]) + newLine.Start[1];
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
        private BitCodes GetCode(Vector2<float> point)
        {
            int x = (int)point[0];
            int y = (int)point[1];
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
    }
}
