using Course;

namespace Course.Lesson13;

public static class Task07
{
    public static string[] SortNames(string[] names)
    {
        ArgumentNullException.ThrowIfNull(names); foreach (var name in names) if (name is null) throw new ArgumentException("Null name.");
        var result = (string[])names.Clone();
        Comparison<string> compare = (a, b) => { int length = a.Length.CompareTo(b.Length); return length != 0 ? length : StringComparer.Ordinal.Compare(a, b); };
        Array.Sort(result, compare); return result;
    }
    public static void Run()
    {
        var input = new[] { "bb", "a", "AA", "b", "" }; Check.SequenceEqual(new[] { "", "a", "b", "AA", "bb" }, SortNames(input));
        Check.SequenceEqual(new[] { "bb", "a", "AA", "b", "" }, input); Check.Equal(0, SortNames(Array.Empty<string>()).Length);
        Check.Throws<ArgumentException>(() => SortNames(new string[] { "a", null! })); Check.Throws<ArgumentNullException>(() => SortNames(null!));
    }
}
