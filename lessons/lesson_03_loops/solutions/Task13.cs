using System;
using Course;
using System.IO;

namespace Course.Lesson03;

public static class Task13
{
    public static string Maximum(TextReader reader)
    {
        bool found = false;
        int maximum = 0;
        while (true)
        {
            string? line = reader.ReadLine();
            if (line is null) break;
            if (!int.TryParse(line, out int value)) continue;
            if (!found || value > maximum) maximum = value;
            found = true;
        }
        return found ? maximum.ToString() : "empty";
    }

    public static void Run()
    {
        using var negatives = new StringReader("bad\n-8\n-3\n");
        Check.Equal("-3", Maximum(negatives));
        using var empty = new StringReader("");
        Check.Equal("empty", Maximum(empty));
        using var invalid = new StringReader("bad\n\n");
        Check.Equal("empty", Maximum(invalid));
        using var minimum = new StringReader("-2147483648");
        Check.Equal("-2147483648", Maximum(minimum));
        using var mixed = new StringReader("1\n10\nx\n0\n");
        Check.Equal("10", Maximum(mixed));

        // Review 2026-10-19: maximum preserves reader ownership
        Check.Equal<string?>(null, minimum.ReadLine());

        // Review 2026-10-19: zero maximum is present data
        using var zeroMaximum = new StringReader("bad\n0\n-2\n");
        Check.Equal("0", Maximum(zeroMaximum));
    }
}
