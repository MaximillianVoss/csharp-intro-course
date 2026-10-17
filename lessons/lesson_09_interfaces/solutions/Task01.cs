namespace Course.Lesson09;

public static class Task01
{
    public interface IShape { double Area { get; } }
    public sealed class Rectangle : IShape
    {
        private readonly double width, height;
        public Rectangle(double width, double height) { this.width = width; this.height = height; }
        public double Area => width * height;
    }
    public sealed class Triangle : IShape
    {
        private readonly double width, height;
        public Triangle(double width, double height) { this.width = width; this.height = height; }
        public double Area => width * height / 2;
    }
    public static double TotalArea(IShape[] shapes) { double sum = 0; foreach (IShape shape in shapes) sum += shape.Area; return sum; }
    public static void Run()
    {
        Check.Near(18, TotalArea(new IShape[] { new Rectangle(3, 4), new Triangle(3, 4) }));
        Check.Near(0, TotalArea(Array.Empty<IShape>())); Check.Near(0, new Rectangle(0, 5).Area);
        Check.Near(1.5, new Triangle(1, 3).Area);
    }
}
