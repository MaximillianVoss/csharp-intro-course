namespace Course.Lesson09;

public static class Task02
{
    public class Printer
    {
        public string Describe() => "base description";
        public virtual string Render() => "base render";
    }
    public sealed class FancyPrinter : Printer
    {
        public new string Describe() => "fancy description";
        public override string Render() => "fancy render";
    }
    public static string[] Replay()
    {
        FancyPrinter derived = new(); Printer parent = derived;
        Check.True(ReferenceEquals(parent, derived));
        return new[] { parent.Describe(), parent.Render(), derived.Describe() };
    }
    public static void Run() => Check.SequenceEqual(new[] { "base description", "fancy render", "fancy description" }, Replay());
}
