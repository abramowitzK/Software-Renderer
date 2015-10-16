using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GraphicsHW.Primitives;

namespace GraphicsHW.Util
{
    public class PixelBuffer
    {
        private bool[,] m_pixelArray;
        private int m_width;
        private int m_height;

        public PixelBuffer(int xmin, int xmax, int ymin, int ymax)
        {
            m_height = ymax - ymin + 1;
            m_width = xmax - xmin + 1;
            m_pixelArray = new bool[m_width, m_height];
            
        }
        public string WriteToXPM()
        {
            
            string xpm = @"/* XPM */ static char* sco100[] = { /* width height num_colors chars_per_pixel */""" + m_width + " " + m_height + @" 2 1"", /*colors*/ ""- c #ffffff"", ""@ c #000000"" /*pixels*/""";
            StringBuilder sb = new StringBuilder(xpm);
            for (int i = 0; i < m_pixelArray.GetLength(0); i++)
            {
                sb.Append(@"""");
                for (int j = 0; j < m_pixelArray.GetLength(1); j++)
                {
                    if (m_pixelArray[j, i])
                        sb.Append("@");
                    else
                        sb.Append("-");
                }
                sb.Append(@""",");
            }
            return sb.ToString();
        }
        public void WritePixel(int i, int j, bool isBlack)
        {
            m_pixelArray[i, m_height - j -1] = isBlack;
        }
        public void ScanConvertLines(List<Line> lines)
        {
            float deltaX;
            float deltaY;
            //Need to check for vertical line...

            foreach (var line in lines)
            {
                int steps = 0;
                float currentX = 0f;
                float currentY = 0f;
                float slope = CalcSlope(line);
                bool startedAtEnd = false;
                if (float.IsInfinity(slope))
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
                else if (System.Math.Abs(slope) < 1f)
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
                    if ((currentX > m_width - 1) || (currentY > m_height - 1))
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
        private float CalcSlope(Line line)
        {
            float rise = line.End[1] - line.Start[1];
            float run = line.End[0] - line.Start[0];
            return (rise / run);
        }
    }
}
