using System;
using Course;

namespace Course.Lesson02;

public static class Task02
{
    public static string Sign(int value)
    {
        if (value < 0) return "negative";
        if (value > 0) return "positive";
        return "zero";
    }

    public static void Run()
    {
        Check.Equal("negative", Sign(-1));
        Check.Equal("zero", Sign(0));
        Check.Equal("positive", Sign(1));
        Check.Equal("negative", Sign(int.MinValue));
        Check.Equal("positive", Sign(int.MaxValue));
    }
}
