using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GraphicsHW.Math;
using GraphicsHW.Util;
namespace GraphicsHW.Primitives
{
    public class Line2D : Primitive
    {
        public override PrimitiveType Type
        {
            get
            {
                return PrimitiveType.Line2D;
            }
        }
        public Vector3<double> Start
        {
            get;
            set;
        }
        public Vector3<double> End
        {
            get;
            set;
        }
        public Line2D()
        {
            this.Start = new Vector3<double>();
            this.End = new Vector3<double>();
        }
        public Line2D(Vector3<double> start, Vector3<double> end)
        {
            this.Start = start;
            this.End = end;
        }
        public static Line2D ParseLine(string lineText)
        {
            string[] splitLine = lineText.Split(' ');
            Line2D line = new Line2D();
            try
            {
                line.Start = new Vector3<double>(double.Parse(splitLine[0]), double.Parse(splitLine[1]), 1);
                line.End = new Vector3<double>(double.Parse(splitLine[2]), double.Parse(splitLine[3]), 1);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("ERROR: Line in incorrect format!");
                Console.WriteLine(ex.ToString());
            }
            return line;
        }
        public void Scale(double xScale, double yScale)
        {
            Matrix3<double> mat = Trans2D.GetScalingMatrix(xScale, yScale);
            Start = mat * Start;
            End = mat * End;
        }
        public void Rotate(double theta)
        {
            Matrix3<double> mat = Trans2D.GetRotationMatrix(theta);
            Start = mat * Start;
            End = mat * End;
        }
        public void Translate(int x, int y)
        {
            Matrix3<double> mat = Trans2D.GetTranslationMatrix(x, y);
            Start = mat * Start;
            End = mat * End;
        }
        public void Transform(double xScale, double yScale, double theta, int xTranslation, int yTranslation)
        {
            Matrix3<double> combined = Trans2D.GetTranslationMatrix(xTranslation, yTranslation) * Trans2D.GetRotationMatrix(theta) * Trans2D.GetScalingMatrix(xScale, yScale);
            Start = combined * Start;
            End = combined * End;
        }
        public override string ToString()
        {
            return Start[0] + " " + Start[1] + " " + End[0] + " " + End[1] + " Line";
        }

    }
}
