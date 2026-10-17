using System;
using Course;
using System.IO;

namespace Course.Lesson03;

public static class Task08
{
    public static long Original(TextReader reader)
    {
        long sum = 0;
        string? line = reader.ReadLine();
        while (line is not null)
        {
            if (int.TryParse(line, out int value)) sum += value;
            line = reader.ReadLine();
        }
        return sum;
    }
    public static long SumLines(TextReader reader)
    {
        long sum = 0;
        while (true)
        {
            string? line = reader.ReadLine();
            if (line is null) break;
            if (int.TryParse(line, out int value)) sum += value;
        }
        return sum;
    }

    public static void Run()
    {
        using var original = new StringReader("3\nbad\n-2\n");
        using var revised = new StringReader("3\nbad\n-2\n");
        Check.Equal(1L, SumLines(revised));
        Check.Equal(1L, Original(original));
        using var empty = new StringReader("");
        Check.Equal(0L, SumLines(empty));
        using var invalid = new StringReader("bad\n\n");
        Check.Equal(0L, SumLines(invalid));
        using var large = new StringReader("2147483647\n2147483647\n");
        Check.Equal(4_294_967_294L, SumLines(large));
    }
}
