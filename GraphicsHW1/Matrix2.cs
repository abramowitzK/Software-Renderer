using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsHW.Math
{
    /*
    Matrix layout
    vec1 [x1 y1]
    vec2 [x2 y2]
    */
    class Matrix2<T>
    {
        private Vector2<T>[] m_data;
        public Matrix2()
        {
            m_data = new Vector2<T>[2];
        }
        public static Matrix2<T> operator *(Matrix2<T> left, Matrix2<T> right)
        {

            return new Matrix2<T>();
        }
        public Vector2<T> Multiply(Vector2<T> vec) 
        {
            T x = (dynamic)this[0][0] * vec[0] + (dynamic)this[0][1] * vec[1];
            T y = (dynamic)this[1][0] * vec[0] + (dynamic)this[1][1] * vec[1];
            return new Vector2<T>(x, y);
        }
        public Vector2<T> this[int i]
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
