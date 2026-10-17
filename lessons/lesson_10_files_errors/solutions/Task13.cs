namespace Course.Lesson10;

public static class Task13
{
    public sealed class ConfigurationReadException : Exception
    {
        public ConfigurationReadException(string message, Exception inner) : base(message, inner) { }
    }
    private sealed class FailingReader : TextReader
    {
        private readonly Exception error; public FailingReader(Exception error) { this.error = error; }
        public override string ReadToEnd() => throw error;
    }
    public static string LoadRequired(TextReader reader)
    {
        try
        {
            string text = reader.ReadToEnd(); if (string.IsNullOrWhiteSpace(text)) throw new FormatException("Пустые настройки.");
            return text;
        }
        catch (IOException error) { throw new ConfigurationReadException("Не удалось прочитать настройки.", error); }
    }
    public static void Run()
    {
        using StringReader normal = new("mode=demo"); Check.Equal("mode=demo", LoadRequired(normal));
        using StringReader empty = new(" "); Check.Throws<FormatException>(() => LoadRequired(empty));
        IOException cause = new("broken channel"); using FailingReader failing = new(cause);
        ConfigurationReadException wrapped = Check.Throws<ConfigurationReadException>(() => LoadRequired(failing));
        Check.Equal("Не удалось прочитать настройки.", wrapped.Message); Check.True(ReferenceEquals(cause, wrapped.InnerException));
        using FailingReader unexpected = new(new InvalidOperationException()); Check.Throws<InvalidOperationException>(() => LoadRequired(unexpected));
    }
}
