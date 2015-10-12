using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsHW.Primitives
{
    public class Point : Primitive
    {
        public override PrimitiveType Type
        {
            get
            {
                return PrimitiveType.Point;
            }
        }
        public int X
        {
            get;
            set;
        }
        public int Y
        {
            get;
            set;
        }
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        //Default constructor
        public Point()
        {
            this.X = 0;
            this.Y = 0;
        }
    }
}
