using Course;

namespace Course.Lesson12;

public static class Task05
{
    public interface IHasKey { int Key { get; } }
    public sealed record User(int Key, string Name) : IHasKey;
    public sealed record Ticket(int Key) : IHasKey;
    public static T? Find<T>(IReadOnlyList<T> items, int key) where T : class, IHasKey
    {
        ArgumentNullException.ThrowIfNull(items);
        foreach (var item in items) if (item is null) throw new ArgumentException("Null item.");
        foreach (var item in items) if (item.Key == key) return item;
        return null;
    }
    public static void Run()
    {
        var first = new User(2, "first"); var values = new[] { first, new User(1, "x"), new User(2, "last") };
        Check.True(ReferenceEquals(first, Find(values, 2))); Check.Equal<User?>(null, Find(values, 9));
        Check.Equal(new Ticket(-1), Find(new[] { new Ticket(-1) }, -1));
        Check.Throws<ArgumentException>(() => Find(new User[] { first, null! }, 2));
        Check.Throws<ArgumentNullException>(() => Find<User>(null!, 0));
    }
}
