using GraphicsHW.Math;
using GraphicsHW.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GraphicsHW
{
    class SMFReader
    {
        StreamReader m_filestream;
        private int LineNumber
        {
            get;
            set;
        }
        public SMFReader(string filePath)
        {
            m_filestream = new StreamReader(filePath);
        }
        public List<Polygon3D> ReadFile()
        {
            List<Polygon3D> returnList = new List<Polygon3D>();
            List<Vector4<double>> vertexList = new List<Vector4<double>>();
            vertexList.Add(null); // Whoever made the indices in smf start at one is sadistic
            string line = m_filestream.ReadLine();
            while(null != line)
            {
                string[] splitLine = line.Split(' ');
                if (line.StartsWith("v"))
                {
                    vertexList.Add(Vector4<double>.ParseLine(splitLine));
                }
                else if (line.StartsWith("f"))
                {
                    List<Vector4<double>> polygonVerticies = new List<Vector4<double>>();
                    polygonVerticies.Add(vertexList[int.Parse(splitLine[1])]);
                    polygonVerticies.Add(vertexList[int.Parse(splitLine[2])]);
                    polygonVerticies.Add(vertexList[int.Parse(splitLine[3])]);
                    returnList.Add(new Polygon3D(polygonVerticies));
                }
                    line = m_filestream.ReadLine();
            }

            return returnList;
        }
    }
}
