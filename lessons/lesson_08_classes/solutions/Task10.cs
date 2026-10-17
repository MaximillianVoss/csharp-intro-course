namespace Course.Lesson08;

public static class Task10
{
    public sealed class Catalog
    {
        private readonly List<string> titles = new();
        public int Count => titles.Count;
        public bool TryAdd(string? title)
        {
            if (string.IsNullOrWhiteSpace(title) || titles.Count == 1000) return false;
            titles.Add(title); return true;
        }
        public string[] GetTitles() => titles.ToArray();
    }
    public static void Run()
    {
        Catalog catalog = new(); Check.True(catalog.TryAdd("A"));
        string[] snapshot = catalog.GetTitles(); snapshot[0] = "X"; Check.Equal("A", catalog.GetTitles()[0]);
        Check.True(catalog.TryAdd("A")); Check.Equal(1, snapshot.Length); Check.SequenceEqual(new[] { "A", "A" }, catalog.GetTitles());
        Check.True(!catalog.TryAdd(null)); Check.True(!catalog.TryAdd("  ")); Check.Equal(2, catalog.Count);
    }
}
