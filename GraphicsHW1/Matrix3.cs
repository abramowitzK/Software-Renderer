using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsHW.Math
{
    class Matrix3<T>
    {
        private T[,] m_data;
        public Matrix3()
        {
            m_data = new T[3, 3];
        }
    }
}
