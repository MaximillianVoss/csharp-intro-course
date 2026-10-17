using System;
using Course;
using System.Globalization;

namespace Course.Lesson04;

public static class Task05
{
    public static string Describe(int value) { return $"int:{value}"; }
    public static string Describe(double value)
    {
        return "double:" + value.ToString(CultureInfo.InvariantCulture);
    }

    public static void Run()
    {
        Check.Equal("int:7", Describe(7));
        Check.Equal("double:7", Describe(7.0));
        var x = 7;
        double y = 7;
        Check.Equal("int:7", Describe(x));
        Check.Equal("double:7", Describe(y));
        Check.Equal("double:1.25", Describe(1.25));
    }
}
