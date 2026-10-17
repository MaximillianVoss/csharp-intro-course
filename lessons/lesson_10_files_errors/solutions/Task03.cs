namespace Course.Lesson10;

public static class Task03
{
    private sealed class FailingReader : TextReader
    {
        private readonly Exception error; public FailingReader(Exception error) { this.error = error; }
        public override string? ReadLine() => throw error;
    }
    public static string ReadFirstStatus(TextReader reader)
    {
        try { string? line = reader.ReadLine(); return line is null ? "eof" : "ok:" + line; }
        catch (FileNotFoundException) { return "missing"; }
        catch (IOException) { return "io"; }
    }
    public static void Run()
    {
        using StringReader text = new("A"); using StringReader eof = new("");
        Check.Equal("ok:A", ReadFirstStatus(text)); Check.Equal("eof", ReadFirstStatus(eof));
        using FailingReader missing = new(new FileNotFoundException()); using FailingReader io = new(new IOException());
        Check.Equal("missing", ReadFirstStatus(missing)); Check.Equal("io", ReadFirstStatus(io));
        using FailingReader unexpected = new(new FormatException()); Check.Throws<FormatException>(() => ReadFirstStatus(unexpected));
    }
}
