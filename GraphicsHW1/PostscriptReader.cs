using System.Collections.Generic;
using System.Linq;
using System.IO;

using GraphicsHW.Primitives;
using System;

namespace GraphicsHW
{
    public class PostscriptReader
    {
        StreamReader m_filestream;
        private int LineNumber
        {
            get;
            set;
        }
        public PostscriptReader(string filePath)
        {
            m_filestream = new StreamReader(filePath);
        }
        public List<Primitive> ReadFile()
        {
            List<Primitive> returnList = new List<Primitive>();
            string line = null;
            LineNumber = -1;
            int polygonIndex = 0;
            while (!(line = m_filestream.ReadLine()).StartsWith("%%%END"))
            {
                if (line == "%%%BEGIN")
                {
                    LineNumber = 0;
                    continue;
                }
                // We started our data block. Everything else is basically ignored
                if (LineNumber >= 0)
                {
                    if (line.EndsWith("Line"))
                    {
                        returnList.Add(Line2D.ParseLine(line));
                    }
                    else if (line.EndsWith("moveto"))
                    {
                        //Begin polygon
                        polygonIndex = returnList.Count;
                        returnList.Add(new Polygon2D(Polygon2D.ParseVertex(line)));

                    }
                    else if (line.EndsWith("lineto"))
                    {
                        //Drawing polygon
                        if (!Polygon2D.ParseVertex(line).Equals((returnList[polygonIndex] as Polygon2D).First()))
                            (returnList[polygonIndex] as Polygon2D).AddVertex(Polygon2D.ParseVertex(line));     
                    }
                    else if (line.EndsWith("stroke"))
                    {
                        //End polygon
                        polygonIndex = -1;
                    }
                    else
                    {
                        //Console.WriteLine("WARNING! Line number " + LineNumber + " contains invalid data. Skipping...");
                        continue; //We don't know what this is. Skip to prevent critical failure and output warning to console;
                    }
                }
            }
            return returnList;
        }

    }
}
