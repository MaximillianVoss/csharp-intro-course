using System;
using Course;

namespace Course.Lesson01;

public static class Task01
{
    public static string Label(string item, int count)
    {
        return $"{item}: {count} шт.";
    }

    public static void Run()
    {
        Check.Equal("Кабель: 12 шт.", Label("Кабель", 12));
        Check.Equal("Кабель: 0 шт.", Label("Кабель", 0));
        Check.Equal("Белая бумага: 1000000 шт.", Label("Белая бумага", 1_000_000));
    }
}
