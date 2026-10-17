namespace Course.Lesson10;

public static class Task02
{
    private sealed class Probe : IDisposable
    {
        private readonly List<string> log; public Probe(List<string> log) { this.log = log; }
        public void Dispose() { log.Add("dispose"); }
    }
    public static string[] Trace(bool fail)
    {
        List<string> log = new();
        try
        {
            log.Add("enter");
            using (Probe probe = new(log))
            {
                log.Add("body");
                if (fail) throw new InvalidOperationException("demo");
            }
            log.Add("after");
        }
        catch (InvalidOperationException) { log.Add("catch"); }
        finally { log.Add("finally"); }
        return log.ToArray();
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { "enter", "body", "dispose", "after", "finally" }, Trace(false));
        Check.SequenceEqual(new[] { "enter", "body", "dispose", "catch", "finally" }, Trace(true));
    }
}
