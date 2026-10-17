using Course;

namespace Course.Lesson12;

public static class Task04
{
    public static T Head<T>(T[] items)
    {
        ArgumentNullException.ThrowIfNull(items); if (items.Length == 0) throw new InvalidOperationException("Empty."); return items[0];
    }
    public static void Run()
    {
        Check.Equal(42, Head(new[] { 42, 1 })); Check.Equal<string?>(null, Head(new string?[] { null }));
        var item = new object(); Check.True(ReferenceEquals(item, Head(new[] { item })));
        Check.Throws<InvalidOperationException>(() => Head(Array.Empty<int>()));
        Check.Throws<ArgumentNullException>(() => Head<int>(null!));
    }
}
