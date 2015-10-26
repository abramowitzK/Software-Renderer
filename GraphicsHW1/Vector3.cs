using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsHW.Math
{
    public class Vector3<T>
    {
        private T[] m_data;
        public Vector3(T x, T y, T z)
        {
            m_data = new T[3];
            m_data[0] = x;
            m_data[1] = y;
            m_data[2] = z;
        }
        public Vector3()
        {
            m_data = new T[3];
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
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            sb.Append(X);
            sb.Append(", ");
            sb.Append(Y);
            sb.Append(", ");
            sb.Append(Z);
            sb.Append(")");
            return sb.ToString();
        }
        public override bool Equals(object obj)
        {
            Vector3<T> test = obj as Vector3<T>;
            if (test != null)
            {
                if ((dynamic)test.X == this.X && (dynamic)test.Y == this.Y && (dynamic)test.Z == this.Z)
                    return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
