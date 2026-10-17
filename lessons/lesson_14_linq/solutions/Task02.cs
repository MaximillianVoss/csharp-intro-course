using Course;

namespace Course.Lesson14;

public static class Task02
{
    public sealed record Student(string Name, int Score);
    public static Student[] Rank(IEnumerable<Student> students)
    {
        ArgumentNullException.ThrowIfNull(students); var items = students.ToArray();
        if (items.Any(x => x is null || string.IsNullOrWhiteSpace(x.Name) || x.Score < 0 || x.Score > 100)) throw new ArgumentException("Student.");
        return items.OrderByDescending(x => x.Score).ThenBy(x => x.Name, StringComparer.Ordinal).ToArray();
    }
    public static void Run()
    {
        var first = new Student("A", 90); var second = new Student("A", 90); var input = new[] { new Student("B", 90), first, second, new Student("Z", 100) };
        var rank = Rank(input); Check.SequenceEqual(new[] { "Z", "A", "A", "B" }, rank.Select(x => x.Name));
        Check.True(ReferenceEquals(first, rank[1])); Check.True(ReferenceEquals(second, rank[2])); Check.Equal("B", input[0].Name);
        Check.Equal(0, Rank(Array.Empty<Student>()).Length); Check.Throws<ArgumentException>(() => Rank(new[] { new Student("x", 101) }));
        Check.Throws<ArgumentException>(() => Rank(new Student[] { null! })); Check.Throws<ArgumentNullException>(() => Rank(null!));
    }
}
