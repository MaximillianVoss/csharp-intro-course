using Course;

namespace Course.Lesson14;

public static class Task10
{
    public sealed record User(string Name);
    public static User? FindUnique(IEnumerable<User> users, string name)
    {
        ArgumentNullException.ThrowIfNull(users); ArgumentNullException.ThrowIfNull(name); var items = users.ToArray();
        if (items.Any(x => x is null || x.Name is null)) throw new ArgumentException("User.");
        return items.SingleOrDefault(x => StringComparer.OrdinalIgnoreCase.Equals(x.Name, name));
    }
    private static User? Broken(IEnumerable<User> users, string name) => users.FirstOrDefault(x => StringComparer.OrdinalIgnoreCase.Equals(x.Name, name));
    public static void Run()
    {
        var ada = new User("Ada"); Check.True(ReferenceEquals(ada, FindUnique(new[] { ada }, "ADA"))); Check.Equal<User?>(null, FindUnique(new[] { ada }, "Bob"));
        var duplicate = new[] { ada, new User("ADA") }; Check.Throws<InvalidOperationException>(() => FindUnique(duplicate, "ada")); Check.True(ReferenceEquals(ada, Broken(duplicate, "ada")));
        Check.Equal<User?>(null, FindUnique(Array.Empty<User>(), "")); Check.Throws<ArgumentException>(() => FindUnique(new User[] { null! }, "x"));
        Check.Throws<ArgumentException>(() => FindUnique(new[] { ada, new User(null!) }, "Ada"));
        Check.Throws<ArgumentNullException>(() => FindUnique(new[] { ada }, null!));
    }
}
