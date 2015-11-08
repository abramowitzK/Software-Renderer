using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GraphicsHW.Math;
using GraphicsHW.Util;

namespace GraphicsHW.Primitives
{
    public class Polygon2D : Primitive, IEnumerable<Vector3<double>>
    {
        //List of vertices stored in homogenous coordinates in counterclockwise order
        private List<Vector3<double>> m_vertices;
        private List<Line2D> m_fillLines;

        public Polygon2D()
        {
            m_vertices = new List<Vector3<double>>();
        }

        public Polygon2D(double x, double y)
        {
            m_vertices = new List<Vector3<double>>();
            m_vertices.Add(new Vector3<double>(x, y, 1.0));
        }

        public Polygon2D(Vector3<double> start)
        {
            m_vertices = new List<Vector3<double>>();
            m_vertices.Add(start);
        }

        public Polygon2D(List<Vector3<double>> vertices)
        {
            m_vertices = vertices;
        }

        public void Transform(double xScale, double yScale, double theta, int xTranslation, int yTranslation)
        {
            Matrix3<double> mat = Trans2D.GetCombinedMatrix(xScale, yScale, theta, xTranslation, yTranslation);
            for (int i = 0; i < this.Count(); i++)
            {
                m_vertices[i] = mat * m_vertices[i];
            }
        }

        public void MapToViewPort(Matrix3<double> vpMatrix)
        {
            for (int i = 0; i < this.Count(); i++)
            {
                m_vertices[i] = vpMatrix * m_vertices[i];
            }
        }

        public List<Vector3<double>> GetVertices()
        {
            return m_vertices;
        }
        public void Convert(int xmin, int xmax, int ymin, int ymax)
        {
            foreach (var v in this)
            {
                v.X = v.X - xmin;
                v.Y = (ymax - ymin) - v.Y + ymin - 1;
            }
        }
        //Scan lines for filling
        public List<Line2D> FillLines
        {
            get
            {
                if (null == m_fillLines)
                    m_fillLines = new List<Line2D>();
                return m_fillLines;
            }
            set { m_fillLines = value; }
        }
        public void AddVertex(Vector3<double> vertex)
        {
            m_vertices.Add(vertex);
        }
        //returns lines in counterclockwise order
        public List<Line2D> GetLines()
        {
            List<Line2D> lines = new List<Line2D>();
            for (int i = 0; i < this.Count(); i++)
            {
                if ((i + 1) < this.Count())
                    lines.Add(new Line2D(m_vertices[i], m_vertices[i + 1]));
                else
                    lines.Add(new Line2D(m_vertices[i], m_vertices.First()));
            }
            return lines;
        }

        public static Vector3<double> ParseVertex(string input)
        {
            string[] split = input.Split(' ');
            split = split.Where(i => !String.IsNullOrEmpty(i) || !String.IsNullOrWhiteSpace(i)).ToArray();
            return new Vector3<double>(double.Parse(split[0]), double.Parse(split[1]), 1.0);
        }
        #region IEnumerable
        public override PrimitiveType Type
        {
            get
            {
                return PrimitiveType.Polygon;
            }
        }

        public IEnumerator<Vector3<double>> GetEnumerator()
        {
            return new PolygonEnumerator(m_vertices);
            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }



    public class PolygonEnumerator : IEnumerator<Vector3<double>>
    {
        public PolygonEnumerator(List<Vector3<double>> list)
        {
            m_vertices = list;
        }

        public List<Vector3<double>> m_vertices;
        int position = -1;

        public Vector3<double> Current
        {
            get
            {
                try
                {
                    return m_vertices[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            position++;
            return (position < m_vertices.Count);
        }

        public void Reset()
        {
            position = -1;
        }
    }

}
