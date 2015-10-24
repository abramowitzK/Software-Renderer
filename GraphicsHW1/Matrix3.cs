using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsHW.Math
{
    /*
    Matrix layout
    vec1 [x1 y1 z1]
    vec2 [x2 y2 z2]
    vec3 [x3 y3 z3]
    */
    public class Matrix3<T>
    {
        private Vector3<T>[] m_data;
        public Matrix3()
        {
            m_data = new Vector3<T>[3];
            m_data[0] = new Vector3<T>();
            m_data[1] = new Vector3<T>();
            m_data[2] = new Vector3<T>();
        }
        public Matrix3
            (T x1, T y1, T z1,
             T x2, T y2, T z2,
             T x3, T y3, T z3
            )
        {
            m_data = new Vector3<T>[3];
            m_data[0] = new Vector3<T>(x1, y1, z1);
            m_data[1] = new Vector3<T>(x2, y2, z2);
            m_data[2] = new Vector3<T>(x3, y3, z3);
        }
        public Vector3<T> this[int i]
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
        public static Matrix3<T> operator* (Matrix3<T> mat1, Matrix3<T> mat2)
        {
            Matrix3<T> ret = new Matrix3<T>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    //We're going to start in top left and work our way right.
                    // Then go to next row of matrix. Left matrix rows by right matrix columns
                    ret[i][j] = (dynamic)mat1[i][0] * mat2[0][j] + (dynamic)mat1[i][1] * mat2[1][j] + (dynamic)mat1[i][2] * mat2[2][j];
                }
            }
            return ret;
        }
        public override string ToString()
        {
            return this[0].ToString() + "\n" + this[1].ToString() + "\n" + this[2].ToString();
        }
    }
}
