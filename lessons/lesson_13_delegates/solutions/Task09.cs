using Course;

namespace Course.Lesson13;

public static class Task09
{
    public static string Process(string text, Func<string, string> normalize, Action<string> log)
    {
        ArgumentNullException.ThrowIfNull(text); ArgumentNullException.ThrowIfNull(normalize); ArgumentNullException.ThrowIfNull(log);
        var result = normalize(text) ?? throw new InvalidOperationException("Normalizer returned null."); log(result); return result;
    }
    public static void Run()
    {
        var trace = new List<string>(); var result = Process(" a ", s => { trace.Add("normalize"); return s.Trim().ToUpperInvariant(); }, s => trace.Add("log:" + s));
        Check.Equal("A", result); Check.SequenceEqual(new[] { "normalize", "log:A" }, trace);
        int calls = 0; Check.Throws<ArgumentNullException>(() => Process("x", s => { calls++; return s; }, null!)); Check.Equal(0, calls);
        Check.Throws<InvalidOperationException>(() => Process("x", _ => null!, _ => calls++)); Check.Equal(0, calls);
        Check.Throws<FormatException>(() => Process("x", _ => throw new FormatException(), _ => calls++)); Check.Equal(0, calls);
        Check.Throws<FormatException>(() => Process("x", s => s, _ => throw new FormatException()));
    }
}
