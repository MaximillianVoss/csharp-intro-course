using System;
using Course;

namespace Course.Lesson01;

public static class Task05
{
    public static double KilometresPerHour(double metres, double seconds)
    {
        return metres / seconds * 3.6;
    }

    public static void Run()
    {
        Check.Near(18, KilometresPerHour(100, 20), 1e-8);
        Check.Near(45, KilometresPerHour(1500, 120), 1e-8);
        Check.Near(0, KilometresPerHour(0, 20));
        Check.Near(9, KilometresPerHour(1.25, 0.5), 1e-8);
    }
}
