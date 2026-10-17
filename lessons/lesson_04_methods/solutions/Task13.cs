using System;
using Course;
using System.IO;

namespace Course.Lesson04;

public static class Task13
{
    public static void PrintRange(TextWriter output, int from, int to)
    {
        for (int value = from; value <= to; value++) output.WriteLine(value);
    }

    public static void Run()
    {
        using var output = new StringWriter();
        output.NewLine = "\n";
        PrintRange(output, -1, 1);
        Check.Equal("-1\n0\n1\n", output.ToString());
        using var empty = new StringWriter();
        PrintRange(empty, 2, 1);
        Check.Equal("", empty.ToString());
        using var single = new StringWriter();
        single.NewLine = "\n";
        PrintRange(single, 5, 5);
        Check.Equal("5\n", single.ToString());
        single.Write("still-open");
        Check.Equal("5\nstill-open", single.ToString());
    }
}
