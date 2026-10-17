namespace Course.Lesson08;

public static class Task02
{
    public sealed class Rectangle
    {
        public decimal Width { get; private set; }
        public decimal Height { get; private set; }
        public decimal Area => Width * Height;
        public Rectangle(decimal width, decimal height) { Width = width; Height = height; }
        public bool TryResize(decimal width, decimal height)
        {
            if (width < 0 || height < 0 || width > 1_000_000 || height > 1_000_000) return false;
            Width = width; Height = height; return true;
        }
    }
    public static void Run()
    {
        Rectangle rectangle = new(2, 3); Check.Equal(6m, rectangle.Area);
        Check.True(!rectangle.TryResize(4, -1)); Check.Equal(2m, rectangle.Width); Check.Equal(3m, rectangle.Height);
        Check.True(rectangle.TryResize(4, 5)); Check.Equal(20m, rectangle.Area);
        Check.True(rectangle.TryResize(0, 3)); Check.Equal(0m, rectangle.Area);
        Check.True(!rectangle.TryResize(1_000_001, 1));
    }
}
