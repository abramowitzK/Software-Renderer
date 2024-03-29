﻿namespace GraphicsHW.Primitives
{
    public abstract class Primitive
    {
        public abstract PrimitiveType Type
        {
            get;
        }
    }
    public enum PrimitiveType
    {
        Line2D,
        Line3D,
        Polygon,
        Polygon3D
    }
}