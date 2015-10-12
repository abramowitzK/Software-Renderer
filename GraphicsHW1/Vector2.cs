using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsHW.Math
{
    public class Vector2<T>
    {
        private T[] m_data;
        public Vector2(T x, T y)
        {
            m_data = new T[2];
            m_data[0] = x;
            m_data[1] = y;
        }
        public Vector2()
        {
            m_data = new T[2];
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
    }
}
