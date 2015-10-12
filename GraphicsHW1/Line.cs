using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Point Start
        {
            get;
            set;
        }
        public Point End
        {
            get;
            set;
        }
        public Line()
        {
            this.Start = new Point();
            this.End = new Point();
        }
        public static Line ParseLine(string lineText)
        {
            string[] splitLine = lineText.Split(' ');
            Line line = new Line();
            try
            {
                line.Start = new Point(int.Parse(splitLine[0]), int.Parse(splitLine[1]));
                line.End = new Point(int.Parse(splitLine[2]), int.Parse(splitLine[3]));
            }
            catch(FormatException ex)
            {
                Console.WriteLine("ERROR: Line in incorrect format!");
            }
            return line;
        }
        public override string ToString()
        {
            return Start.X + " " + Start.Y + " " + End.X + " " + End.Y + " Line";
        }

    }
}
