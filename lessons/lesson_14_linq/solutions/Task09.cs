using Course;

namespace Course.Lesson14;

public static class Task09
{
    public sealed record Line(int Quantity, decimal UnitPrice);
    public static (long Units, decimal Total) Totals(IEnumerable<Line> lines)
    {
        ArgumentNullException.ThrowIfNull(lines);
        return lines.Aggregate((Units: 0L, Total: 0m), (sum, line) =>
        {
            if (line is null || line.Quantity < 1 || line.Quantity > 1000000 || line.UnitPrice < 0 || line.UnitPrice > 1000000) throw new ArgumentException("Line.");
            return (checked(sum.Units + line.Quantity), checked(sum.Total + line.Quantity * line.UnitPrice));
        });
    }
    public static void Run()
    {
        Check.Equal((5L, 8.50m), Totals(new[] { new Line(2, 1.25m), new Line(3, 2m) })); Check.Equal((0L, 0m), Totals(Array.Empty<Line>()));
        Check.Equal(3000000000L, Totals(Enumerable.Repeat(new Line(1000000, 0), 3000)).Units);
        Check.Throws<ArgumentException>(() => Totals(new[] { new Line(0, 1) })); Check.Throws<ArgumentException>(() => Totals(new Line[] { null! })); Check.Throws<ArgumentNullException>(() => Totals(null!));
    }
}
