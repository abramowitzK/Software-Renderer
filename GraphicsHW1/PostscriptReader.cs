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
        public List<Line> ReadFile()
        {
            List<Line> returnList = new List<Line>();
            string line = null;
            LineNumber = -1;
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
                        returnList.Add(Line.ParseLine(line));
                    }
                    else
                    {
                        Console.WriteLine("WARNING! Line number " + LineNumber + " contains invalid data. Skipping...");
                        continue; //We don't know what this is. Skip to prevent critical failure and output warning to console;
                    }
                }
            }
            return returnList;
        }

    }
}
