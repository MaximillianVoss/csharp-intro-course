namespace Course.Lesson10;

public static class Task01
{
    public static int ReadPort(TextReader reader)
    {
        string line = reader.ReadLine() ?? throw new EndOfStreamException("Ожидалась строка порта.");
        int port = int.Parse(line, System.Globalization.CultureInfo.InvariantCulture);
        if (port < 1 || port > 65535) throw new ArgumentOutOfRangeException(nameof(port), port, "Порт вне 1..65535.");
        return port;
    }
    public static void Run()
    {
        using StringReader normal = new(" 8080\n"); Check.Equal(8080, ReadPort(normal));
        using StringReader eof = new(""); Check.Throws<EndOfStreamException>(() => ReadPort(eof));
        using StringReader bad = new("x"); Check.Throws<FormatException>(() => ReadPort(bad));
        using StringReader huge = new("999999999999"); Check.Throws<OverflowException>(() => ReadPort(huge));
        using StringReader zero = new("0"); Check.Throws<ArgumentOutOfRangeException>(() => ReadPort(zero));
        using StringReader tooLarge = new("65536"); Check.Throws<ArgumentOutOfRangeException>(() => ReadPort(tooLarge));
        using StringReader lower = new("1"); Check.Equal(1, ReadPort(lower));
        using StringReader upper = new("65535"); Check.Equal(65535, ReadPort(upper));
        using StringReader blank = new("\n"); Check.Throws<FormatException>(() => ReadPort(blank));
        using StringReader tail = new("8080\ntail"); Check.Equal(8080, ReadPort(tail)); Check.Equal("tail", tail.ReadLine());
        using StringReader badTail = new("bad\ntail"); Check.Throws<FormatException>(() => ReadPort(badTail)); Check.Equal("tail", badTail.ReadLine());
    }
}
