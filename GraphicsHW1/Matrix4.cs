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
    public class Matrix4<T>
    {
        private Vector4<T>[] m_data;
        public Matrix4()
        {
            m_data = new Vector4<T>[4];
            m_data[0] = new Vector4<T>();
            m_data[1] = new Vector4<T>();
            m_data[2] = new Vector4<T>();
            m_data[3] = new Vector4<T>();
        }
        public Matrix4
            (T x1, T y1, T z1, T w1,
             T x2, T y2, T z2, T w2,
             T x3, T y3, T z3, T w3,
             T x4, T y4, T z4, T w4
            )
        {
            m_data = new Vector4<T>[4];
            m_data[0] = new Vector4<T>(x1, y1, z1, w1);
            m_data[1] = new Vector4<T>(x2, y2, z2, w2);
            m_data[2] = new Vector4<T>(x3, y3, z3, w3);
            m_data[3] = new Vector4<T>(x4, y4, z4, w4);
        }
        public Vector4<T> this[int i]
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
        public static Matrix4<T> operator *(Matrix4<T> mat1, Matrix4<T> mat2)
        {
            Matrix4<T> ret = new Matrix4<T>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    //We're going to start in top left and work our way right.
                    // Then go to next row of matrix. Left matrix rows by right matrix columns
                    ret[i][j] = (dynamic)mat1[i][0] * mat2[0][j] + (dynamic)mat1[i][1] * mat2[1][j] + (dynamic)mat1[i][2] * mat2[2][j] + (dynamic)mat1[i][3] * mat2[3][j];
                }
            }
            return ret;
        }
        public static Matrix4<double> GetParallelProjectionMatrix(double umin, double umax, double vmin, double vmax, double prp_x, double prp_y, double prp_z, double front, double back)
        {
            Matrix4<double> ret = new Matrix4<double>();
            ret[0][0] = 2 / (umax - umin);
            ret[0][1] = 0.0;
            ret[0][2] = ((umax + umin) - (2 * prp_x)) / ((umax - umin) * prp_z);
            ret[0][3] = -(umax + umin) / 2;
            ret[1][0] = 0.0;
            ret[1][1] = 2 / (vmax - vmin);
            ret[1][2] = ((vmax + vmin) - (2 * prp_y)) / ((vmax - vmin) * prp_z);
            ret[1][3] = -(vmax + vmin) / 2;
            ret[2][0] = 0.0;
            ret[2][1] = 0.0;
            ret[2][2] = 1 / (front - back);
            ret[2][3] = -front / (front - back);
            ret[3][0] = 0.0;
            ret[3][1] = 0.0;
            ret[3][2] = 0.0;
            ret[3][3] = 1.0;
            return ret;
        }
        public static Matrix4<double> GetPerspectiveProjectionMatrix(double umin, double umax, double vmin, double vmax, double prp_x, double prp_y, double prp_z, double back)
        {
            Matrix4<double> ret = new Matrix4<double>();
            ret[0][0] = (2 * prp_z) / ((umax - umin) * (prp_z - back));
            ret[0][1] = 0.0;
            ret[0][2] = ((umax + umin) - (2 * prp_x)) / ((umax - umin) * (prp_z - back));
            ret[0][3] = -((umax + umin) * prp_z) / ((umax - umin) * (prp_z - back));
            ret[1][0] = 0.0;
            ret[1][1] = (2 * prp_z) / ((vmax - vmin) * (prp_z - back));
            ret[1][2] = ((vmax + vmin) - (2 * prp_y)) / ((vmax - vmin) * (prp_z - back));
            ret[1][3] = -((vmax + vmin) * prp_z) / ((vmax - vmin) * (prp_z - back));
            ret[2][0] = 0.0;
            ret[2][1] = 0.0;
            ret[2][2] = 1 / (prp_z - back);
            ret[2][3] = -prp_z / (prp_z - back);
            ret[3][0] = 0.0;
            ret[3][1] = 0.0;
            ret[3][2] = 0.0;
            ret[3][3] = 1.0;
            return ret;
        }
        public static Vector4<double> operator *(Matrix4<T> mat, Vector4<double> vec)
        {
            double x = (dynamic)mat[0][0] * vec.X + (dynamic)mat[0][1] * vec.Y + (dynamic)mat[0][2] * vec.Z + (dynamic)mat[0][3] * vec.W;
            double y = (dynamic)mat[1][0] * vec.X + (dynamic)mat[1][1] * vec.Y + (dynamic)mat[1][2] * vec.Z + (dynamic)mat[1][3] * vec.W;
            double z = (dynamic)mat[2][0] * vec.X + (dynamic)mat[2][1] * vec.Y + (dynamic)mat[2][2] * vec.Z + (dynamic)mat[2][3] * vec.W;
            double w = (dynamic)mat[3][0] * vec.X + (dynamic)mat[3][1] * vec.Y + (dynamic)mat[3][2] * vec.Z + (dynamic)mat[3][3] * vec.W;
            return new Vector4<double>(x, y, z, w);
        }
        public override string ToString()
        {
            return this[0].ToString() + "\n" + this[1].ToString() + "\n" + this[2].ToString() + "\n" + this[3].ToString();
        }
    }
}

