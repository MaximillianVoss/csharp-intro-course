namespace Course.Lesson09;

public static class Task11
{
    public interface ITextTransform { string Transform(string text); }
    public sealed class Identity : ITextTransform { public string Transform(string text) => text; }
    public sealed class TrimDecorator : ITextTransform
    {
        private readonly ITextTransform inner; public TrimDecorator(ITextTransform inner) { this.inner = inner; }
        public string Transform(string text) => inner.Transform(text).Trim();
    }
    public sealed class PrefixDecorator : ITextTransform
    {
        private readonly ITextTransform inner; private readonly string prefix;
        public PrefixDecorator(ITextTransform inner, string prefix) { this.inner = inner; this.prefix = prefix; }
        public string Transform(string text) => prefix + inner.Transform(text);
    }
    public static void Run()
    {
        ITextTransform first = new PrefixDecorator(new TrimDecorator(new Identity()), " > ");
        ITextTransform second = new TrimDecorator(new PrefixDecorator(new Identity(), " > "));
        Check.Equal(" > a", first.Transform("  a  ")); Check.Equal(">   a", second.Transform("  a  "));
        Check.Equal(" x ", new Identity().Transform(" x ")); Check.Equal(" > ", first.Transform(""));
        Check.Equal("abx", new PrefixDecorator(new PrefixDecorator(new Identity(), "b"), "a").Transform("x"));
    }
}
