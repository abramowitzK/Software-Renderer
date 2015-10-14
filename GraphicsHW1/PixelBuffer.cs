using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsHW.Util
{
    public class PixelBuffer
    {
        private bool[,] m_pixelArray;
        private int m_width;
        private int m_height;

        public PixelBuffer(int xmin, int xmax, int ymin, int ymax)
        {
            m_height = ymax - ymin;
            m_width = xmax - xmin;
            m_pixelArray = new bool[m_width, m_height];
            
        }
        public string WriteToXPM()
        {
            string xpm = "/* XPM */ static char* sco100[] = { /* width height num_colors chars_per_pixel */\"" + m_width + " " + m_height + " 2 1\", /*colors*/ \"- c #ffffff\", \"@ c #000000\" /*pixels*/";
            for (int i = 0; i < m_pixelArray.GetLength(0); i++)
            {
                xpm += "\"";
                for (int j = 0; j < m_pixelArray.GetLength(1); i++)
                {
                    if (m_pixelArray[i, j])
                        xpm += "@";
                    else
                        xpm += "-";
                }
                xpm += "\"";
            }
            return xpm;
        }
        public void WritePixel(int i, int j, bool isBlack)
        {
            m_pixelArray[i, j] = isBlack;
        }
    }
}
