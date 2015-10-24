using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GraphicsHW.Math;

namespace GraphicsHW.Util
{
    public static class Trans2D
    {
        public static Matrix3<float> GetScalingMatrix(float xScale, float yScale)
        {
            return new Matrix3<float>
                (
                xScale, 0, 0,
                0, yScale, 0,
                0, 0, 1
                );
        }
        public static Matrix3<float> GetTranslationMatrix(float x, float y)
        {
            return new Matrix3<float>
                (1, 0, x,
                 0, 1, y,
                 0, 0, 1
                );
        }
    }
}
