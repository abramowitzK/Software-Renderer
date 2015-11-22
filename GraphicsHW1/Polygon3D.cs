using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GraphicsHW.Math;
using GraphicsHW.Util;

namespace GraphicsHW.Primitives
{
    public class Polygon3D : Primitive, IEnumerable<Vector4<double>>
    {
        //List of vertices stored in homogenous coordinates in counterclockwise order
        private List<Vector4<double>> m_vertices;

        public Polygon3D()
        {
            m_vertices = new List<Vector4<double>>();
        }

        public Polygon3D(double x, double y, double z)
        {
            m_vertices = new List<Vector4<double>>();
            m_vertices.Add(new Vector4<double>(x, y, z, 1.0));
        }

        public Polygon3D(Vector4<double> start)
        {
            m_vertices = new List<Vector4<double>>();
            m_vertices.Add(start);
        }

        public Polygon3D(List<Vector4<double>> vertices)
        {
            m_vertices = vertices;
        }

        //public void Transform(double xScale, double yScale, double theta, int xTranslation, int yTranslation)
        //{
        //    Matrix4<double> mat = Trans2D.GetCombinedMatrix(xScale, yScale, theta, xTranslation, yTranslation);
        //    for (int i = 0; i < this.Count(); i++)
        //    {
        //        m_vertices[i] = mat * m_vertices[i];
        //    }
        //}
        public void ProjectAndView(Matrix4<double> combined)
        {
            for (int i = 0; i < this.Count(); i++)
            {
                this.m_vertices[i] = combined*this.m_vertices[i];
            }
        }
        public void MapToViewPort(Matrix3<double> vpMatrix)
        {
            for (int i = 0; i < this.Count(); i++)
            {
                m_vertices[i] = vpMatrix * m_vertices[i];
            }
        }

        public List<Vector4<double>> GetVertices()
        {
            return m_vertices;
        }
        //public void Convert(int xmin, int xmax, int ymin, int ymax)
        //{
        //    foreach (var v in this)
        //    {
        //        v.X = v.X - xmin;
        //        v.Y = (ymax - ymin) - v.Y + ymin - 1;
        //    }
        //}
        public void AddVertex(Vector4<double> vertex)
        {
            m_vertices.Add(vertex);
        }
        //returns lines in counterclockwise order
        public List<Line3D> GetLines()
        {
            List<Line3D> lines = new List<Line3D>();
            for (int i = 0; i < this.Count(); i++)
            {
                if ((i + 1) < this.Count())
                    lines.Add(new Line3D(m_vertices[i], m_vertices[i + 1]));
                else
                    lines.Add(new Line3D(m_vertices[i], m_vertices.First()));
            }
            return lines;
        }

        public static Vector4<double> ParseVertex(string input)
        {
            string[] split = input.Split(' ');
            split = split.Where(i => !String.IsNullOrEmpty(i) || !String.IsNullOrWhiteSpace(i)).ToArray();
            return new Vector4<double>(double.Parse(split[0]), double.Parse(split[1]), double.Parse(split[2]), 1.0);
        }
        #region IEnumerable
        public override PrimitiveType Type
        {
            get
            {
                return PrimitiveType.Polygon3D;
            }
        }

        public IEnumerator<Vector4<double>> GetEnumerator()
        {
            return new Polygon3DEnumerator(m_vertices);

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }



    public class Polygon3DEnumerator : IEnumerator<Vector4<double>>
    {
        public Polygon3DEnumerator(List<Vector4<double>> list)
        {
            m_vertices = list;
        }

        public List<Vector4<double>> m_vertices;
        int position = -1;

        public Vector4<double> Current
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
