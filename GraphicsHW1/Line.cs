using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GraphicsHW.Math;

namespace GraphicsHW.Primitives
{
    class Line : Primitive
    {
        public override PrimitiveType Type
        {
            get
            {
                return PrimitiveType.Line;
            }
        }
        public Vector2<int> Start
        {
            get;
            set;
        }
        public Vector2<int> End
        {
            get;
            set;
        }
        public Line()
        {
            this.Start = new Vector2<int>();
            this.End = new Vector2<int>();
        }
        public static Line ParseLine(string lineText)
        {
            string[] splitLine = lineText.Split(' ');
            Line line = new Line();
            try
            {
                line.Start = new Vector2<int>(int.Parse(splitLine[0]), int.Parse(splitLine[1]));
                line.End = new Vector2<int>(int.Parse(splitLine[2]), int.Parse(splitLine[3]));
            }
            catch (FormatException ex)
            {
                Console.WriteLine("ERROR: Line in incorrect format!");
                Console.WriteLine(ex.ToString());
            }
            return line;
        }
        public void Scale(float xScale, float yScale)
        {
            Matrix2<float> mat = Matrix2<float>.GetScalingMatrix(xScale, yScale);
            Start = mat.Multiply(Start);
            End = mat.Multiply(End);
        }
        public void Rotate(double theta)
        {
            Matrix2<double> mat = Matrix2<double>.GetRotationMatrix(theta);
            Start = mat.Multiply(Start);
            End = mat.Multiply(End);
        }
        public void Translate(int x, int y)
        {
            Matrix2<int> mat;
        }
        public override string ToString()
        {
            return Start[0] + " " + Start[1] + " " + End[0] + " " + End[1] + " Line";
        }

    }
}
