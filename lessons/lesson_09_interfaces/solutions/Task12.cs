namespace Course.Lesson09;

public static class Task12
{
    public interface IValueSource { bool TryGet(string key, out string value); }
    public sealed class DictionarySource : IValueSource
    {
        private readonly Dictionary<string, string> data;
        public DictionarySource(Dictionary<string, string> data) { this.data = data; }
        public bool TryGet(string key, out string value)
        {
            if (data.TryGetValue(key, out string? found)) { value = found; return true; }
            value = ""; return false;
        }
    }
    public sealed class FallbackSource : IValueSource
    {
        private readonly IValueSource primary, backup;
        public FallbackSource(IValueSource primary, IValueSource backup) { this.primary = primary; this.backup = backup; }
        public bool TryGet(string key, out string value)
        {
            if (primary.TryGet(key, out value)) return true;
            return backup.TryGet(key, out value);
        }
    }
    private sealed class CountedSource : IValueSource
    {
        private readonly IValueSource inner; public int Calls { get; private set; }
        public CountedSource(IValueSource inner) { this.inner = inner; }
        public bool TryGet(string key, out string value) { Calls++; return inner.TryGet(key, out value); }
    }
    public static void Run()
    {
        CountedSource primary = new(new DictionarySource(new(StringComparer.Ordinal) { ["A"] = "" }));
        CountedSource backup = new(new DictionarySource(new(StringComparer.Ordinal) { ["A"] = "запас", ["B"] = "второй" }));
        IValueSource source = new FallbackSource(primary, backup);
        Check.True(source.TryGet("A", out string a)); Check.Equal("", a); Check.Equal(0, backup.Calls);
        Check.True(source.TryGet("B", out string b)); Check.Equal("второй", b); Check.Equal(1, backup.Calls);
        Check.True(!source.TryGet("C", out string c)); Check.Equal("", c);
        Check.Equal(3, primary.Calls); Check.Equal(2, backup.Calls);
    }
}
