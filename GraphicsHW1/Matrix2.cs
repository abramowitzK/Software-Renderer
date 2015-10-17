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
        public Matrix2(T x1, T y1, T x2, T y2)
        {
            m_data = new Vector2<T>[2];
            m_data[0] = new Vector2<T>();
            m_data[1] = new Vector2<T>();
            m_data[0][0] = x1;
            m_data[0][1] = y1;
            m_data[1][0] = x2;
            m_data[1][1] = y2;
        }
        public Vector2<T> Multiply(Vector2<T> vec) 
        {
            T x = (dynamic)this[0][0] * vec[0] + (dynamic)this[0][1] * vec[1];
            T y = (dynamic)this[1][0] * vec[0] + (dynamic)this[1][1] * vec[1];
            return new Vector2<T>(x, y);
        }
        //Can't perform arithmetic with Generic types conveniently. Dynamic allows types to be determined at runtime and perform the correct math
        public Vector2<float> Multiply(Vector2<float> vec)
        {
            float x = (float)((dynamic)this[0][0] * vec[0] + (dynamic)this[0][1] * vec[1]);
            float y = (float)((dynamic)this[1][0] * vec[0] + (dynamic)this[1][1] * vec[1]);
            return new Vector2<float>(x, y);
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
        public static Matrix2<float> GetScalingMatrix(float xScale, float yScale)
        {
            return new Matrix2<float>(xScale, 0, 0, yScale);
        }
        public static Matrix2<double> GetRotationMatrix(double theta)
        {
            theta = theta * (System.Math.PI / 180.0);
            return new Matrix2<double>(System.Math.Cos(theta), -System.Math.Sin(theta), System.Math.Sin(theta), System.Math.Cos(theta));
        }
    }
}
