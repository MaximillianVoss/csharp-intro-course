using Course;

namespace Course.Lesson13;

public static class Task15
{
    public sealed class CommandRouter
    {
        private readonly Dictionary<string, Action<IReadOnlyList<string>>> handlers = new(StringComparer.OrdinalIgnoreCase);
        private static void Validate(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length > 20) throw new ArgumentException("Name.");
            foreach (char c in name) if (!(c >= 'a' && c <= 'z') && !(c >= 'A' && c <= 'Z')) throw new ArgumentException("Name.");
        }
        public void Register(string name, Action<IReadOnlyList<string>> handler)
        {
            Validate(name); ArgumentNullException.ThrowIfNull(handler); if (!handlers.TryAdd(name, handler)) throw new InvalidOperationException("Duplicate.");
        }
        public bool Execute(string name, IReadOnlyList<string> args)
        {
            Validate(name); ArgumentNullException.ThrowIfNull(args); if (!handlers.TryGetValue(name, out var handler)) return false; handler(args); return true;
        }
    }
    public static void Run()
    {
        var router = new CommandRouter(); var log = new List<string>(); router.Register("echo", args => log.Add(string.Join(" ", args))); router.Register("count", args => log.Add(args.Count.ToString()));
        Check.True(router.Execute("ECHO", new[] { "a", "b" })); Check.True(router.Execute("count", Array.Empty<string>())); Check.SequenceEqual(new[] { "a b", "0" }, log);
        Check.True(!router.Execute("help", Array.Empty<string>())); Check.Equal(2, log.Count);
        Check.Throws<InvalidOperationException>(() => router.Register("Echo", _ => { })); Check.True(router.Execute("echo", new[] { "kept" })); Check.Equal("kept", log[2]);
        Check.Throws<ArgumentException>(() => router.Execute("bad name", Array.Empty<string>())); Check.Throws<ArgumentNullException>(() => router.Execute("help", null!));
        Check.Throws<ArgumentNullException>(() => router.Register("null", null!)); router.Register("fail", _ => throw new FormatException()); Check.Throws<FormatException>(() => router.Execute("fail", Array.Empty<string>()));
        var boundaryName = new string('a', 20); router.Register(boundaryName, _ => { }); Check.True(router.Execute(boundaryName, Array.Empty<string>()));
        foreach (var name in new[] { "", "echo1", "эхо", new string('a', 21) }) Check.Throws<ArgumentException>(() => router.Register(name, _ => { }));
    }
}
