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
        public void Transform(double xScale, double yScale, double theta, int xTranslation, int yTranslation)
        {
            Matrix3<double> mat = Trans2D.GetCombinedMatrix(xScale, yScale, theta, xTranslation, yTranslation);
            for (int i = 0; i < this.Count(); i++)
            {
                m_vertices[i] = mat * m_vertices[i];
            }
        }
        public void AddVertex(Vector3<double> vertex)
        {
            m_vertices.Add(vertex);
        }
        public static Vector3<double> ParseVertex(string input)
        {
            string[] split = input.Split(' ');
            return new Vector3<double>(double.Parse(split[0]), double.Parse(split[1]), 1.0);
        }
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
