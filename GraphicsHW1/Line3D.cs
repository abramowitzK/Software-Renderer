using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GraphicsHW.Math;
using GraphicsHW.Util;
namespace GraphicsHW.Primitives
{
    public class Line3D : Primitive
    {
        public override PrimitiveType Type
        {
            get
            {
                return PrimitiveType.Line3D;
            }
        }
        public Vector4<double> Start
        {
            get;
            set;
        }
        public Vector4<double> End
        {
            get;
            set;
        }
        public Line3D()
        {
            this.Start = new Vector4<double>();
            this.End = new Vector4<double>();
        }
        public Line3D(Vector4<double> start, Vector4<double> end)
        {
            this.Start = start;
            this.End = end;
        }
        //public static Line3D ParseLine(string lineText)
        //{
        //    string[] splitLine = lineText.Split(' ');
        //    Line3D line = new Line3D();
        //    try
        //    {
        //        line.Start = new Vector4<double>(double.Parse(splitLine[0]), double.Parse(splitLine[1]), 1);
        //        line.End = new Vector4<double>(double.Parse(splitLine[2]), double.Parse(splitLine[3]), 1);
        //    }
        //    catch (FormatException ex)
        //    {
        //        Console.WriteLine("ERROR: Line in incorrect format!");
        //        Console.WriteLine(ex.ToString());
        //    }
        //    return line;
        //}
        //public void Scale(double xScale, double yScale)
        //{
        //    Matrix4<double> mat = Trans2D.GetScalingMatrix(xScale, yScale);
        //    Start = mat * Start;
        //    End = mat * End;
        //}
        //public void Rotate(double theta)
        //{
        //    Matrix4<double> mat = Trans2D.GetRotationMatrix(theta);
        //    Start = mat * Start;
        //    End = mat * End;
        //}
        //public void Translate(int x, int y)
        //{
        //    Matrix4<double> mat = Trans2D.GetTranslationMatrix(x, y);
        //    Start = mat * Start;
        //    End = mat * End;
        //}
        //public void Transform(double xScale, double yScale, double theta, int xTranslation, int yTranslation)
        //{
        //    Matrix4<double> combined = Trans2D.GetTranslationMatrix(xTranslation, yTranslation) * Trans2D.GetRotationMatrix(theta) * Trans2D.GetScalingMatrix(xScale, yScale);
        //    Start = combined * Start;
        //    End = combined * End;
        //}
        public void MapToViewPort(Matrix4<double> vpMatrix)
        {
            Start = vpMatrix * Start;
            End = vpMatrix * End;
        }
        public override string ToString()
        {
            return Start[0] + " " + Start[1] + " " + End[0] + " " + End[1] + " Line";
        }
        public double Slope
        {
            get
            {
                double rise = End[1] - Start[1];
                double run = End[0] - Start[0];
                return (rise / run);
            }
        }
        public int MaxY
        {
            get
            {
                if (Start.Y > End.Y)
                    return (int)System.Math.Round(Start.Y, MidpointRounding.AwayFromZero);
                return (int)System.Math.Round(End.Y, MidpointRounding.AwayFromZero);
            }
        }
        public int MaxX
        {
            get
            {
                if (Start.X > End.X)
                    return (int)System.Math.Round(Start.X, MidpointRounding.AwayFromZero);
                return (int)System.Math.Round(End.X, MidpointRounding.AwayFromZero);
            }
        }

    }
}
