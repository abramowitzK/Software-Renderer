using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GraphicsHW.Math;

namespace GraphicsHW.Util
{
    //Homogenous Transformations
    public static class Trans2D
    {
        public static Matrix3<double> GetScalingMatrix(double xScale, double yScale)
        {
            return new Matrix3<double>
                (
                xScale, 0, 0,
                0, yScale, 0,
                0, 0, 1
                );
        }
        public static Matrix3<double> GetTranslationMatrix(double x, double y)
        {
            return new Matrix3<double>
                (1, 0, x,
                 0, 1, y,
                 0, 0, 1
                );
        }
        public static Matrix3<double> GetRotationMatrix(double theta)
        {
            //Convert to radians
            theta = theta * (System.Math.PI / 180.0);
            return new Matrix3<double>
                (System.Math.Cos(theta), -System.Math.Sin(theta), 0,
                 System.Math.Sin(theta), System.Math.Cos(theta), 0,
                 0,0,1
                );
        }
    }
    public static class Trans3D
    {
    }
}
