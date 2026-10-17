namespace Course.Lesson08;

public static class Task03
{
    public sealed class Box { public int Number { get; set; } }
    public readonly record struct Point(int X, int Y);
    public static int[] Replay()
    {
        Box a = new() { Number = 1 }; Box b = a; b.Number = 8;
        Point p = new(2, 3); Point q = p; q = q with { X = 9 };
        Check.True(ReferenceEquals(a, b)); Check.Equal(new Point(2, 3), p);
        return new[] { a.Number, b.Number, p.X, q.X };
    }
    public static void Run() => Check.SequenceEqual(new[] { 8, 8, 2, 9 }, Replay());
}
