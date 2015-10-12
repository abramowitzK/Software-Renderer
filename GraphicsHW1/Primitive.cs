namespace GraphicsHW.Primitives
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
    Point,
    Line
    }
}