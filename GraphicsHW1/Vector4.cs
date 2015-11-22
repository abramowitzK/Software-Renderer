using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsHW.Math
{
    public class Vector4<T>
    {
        private T[] m_data;
        public Vector4(T x, T y, T z, T w)
        {
            m_data = new T[4];
            m_data[0] = x;
            m_data[1] = y;
            m_data[2] = z;
            m_data[3] = w;
        }
        public Vector4()
        {
            m_data = new T[4];
        }
        public T this[int i]
        {
            get
            {
                return m_data[i];
            }
            set
            {
                m_data[i] = value;
            }
        }
        public T X
        {
            get
            {
                return m_data[0];
            }
            set
            {
                m_data[0] = value;
            }
        }
        public T Y
        {
            get
            {
                return m_data[1];
            }
            set
            {
                m_data[1] = value;
            }
        }
        public T Z
        {
            get
            {
                return m_data[2];
            }
            set
            {
                m_data[2] = value;
            }
        }
        public T W
        {
            get
            {
                return m_data[3];
            }
            set
            {
                m_data[3] = value;
            }
        }
        public static Vector4<T> operator -(Vector4<T> vec1, Vector4<T> vec2)
        {
            return new Vector4<T>((dynamic)vec1.X - vec2.X, (dynamic)vec1.Y - vec2.Y, (dynamic)vec1.Z - vec2.Z,(dynamic)1.0);
        }
        public static Vector4<T> operator +(Vector4<T> vec1, Vector4<T> vec2)
        {
            return new Vector4<T>((dynamic)vec1.X + vec2.X, (dynamic)vec1.Y + vec2.Y, (dynamic)vec1.Z + vec1.Z,(dynamic)1.0);
        }
        public static Vector4<T> operator *(double t, Vector4<T> vec1)
        {
            vec1.X = (dynamic)vec1.X * t;
            vec1.Y = (dynamic)vec1.Y * t;
            vec1.Z = (dynamic)vec1.Z * t;
            vec1.W = (dynamic)vec1.Z * t;
            return vec1;
        }
        public static Vector4<double> ParseLine(string[] splitLine)
        {
            double x = double.Parse(splitLine[1]);
            double y = double.Parse(splitLine[2]);
            double z = double.Parse(splitLine[3]);
            return new Vector4<double>(x, y, z, 1.0);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            sb.Append(X);
            sb.Append(", ");
            sb.Append(Y);
            sb.Append(", ");
            sb.Append(Z);
            sb.Append(", ");
            sb.Append(W);
            sb.Append(")");
            return sb.ToString();
        }
        public override bool Equals(object obj)
        {
            Vector4<T> test = obj as Vector4<T>;
            if (test != null)
            {
                if ((dynamic)test.X == this.X && (dynamic)test.Y == this.Y && (dynamic)test.Z == this.Z && (dynamic)test.W == this.W)
                    return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public void Round()
        {
            X = System.Math.Round((dynamic)X, MidpointRounding.AwayFromZero);
            Y = System.Math.Round((dynamic)Y, MidpointRounding.AwayFromZero);
            Z = System.Math.Round((dynamic)Z, MidpointRounding.AwayFromZero);
        }
    }
}
