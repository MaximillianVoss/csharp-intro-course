using Course;

namespace Course.Lesson14;

public static class Task12
{
    public static T[] Page<T>(IEnumerable<T> source, int page, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(source); if (page < 1) throw new ArgumentOutOfRangeException(nameof(page)); if (pageSize < 1 || pageSize > 100) throw new ArgumentOutOfRangeException(nameof(pageSize));
        int skip = checked((page - 1) * pageSize); return source.Skip(skip).Take(pageSize).ToArray();
    }
    public static void Run()
    {
        var values = new[] { 10, 20, 30, 40, 50 }; Check.SequenceEqual(new[] { 10, 20 }, Page(values, 1, 2)); Check.SequenceEqual(new[] { 30, 40 }, Page(values, 2, 2));
        Check.SequenceEqual(new[] { 50 }, Page(values, 3, 2)); Check.Equal(0, Page(values, 4, 2).Length);
        Check.Throws<ArgumentOutOfRangeException>(() => Page(values, 0, 2)); Check.Throws<ArgumentOutOfRangeException>(() => Page(values, 1, 101)); Check.Throws<OverflowException>(() => Page(values, int.MaxValue, 100));
    }
}
