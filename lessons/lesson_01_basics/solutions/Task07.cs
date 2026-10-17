using System;
using Course;

namespace Course.Lesson01;

public static class Task07
{
    public static long BrokenArea(int width, int height)
    {
        long area = unchecked(width * height);
        return area;
    }
    public static long Area(int width, int height)
    {
        return (long)width * height;
    }

    public static void Run()
    {
        Check.Equal(-1_794_967_296L, BrokenArea(50_000, 50_000));
        Check.Equal(2_500_000_000L, Area(50_000, 50_000));
        Check.Equal(4_611_686_014_132_420_609L, Area(int.MaxValue, int.MaxValue));
        Check.Equal(0L, Area(0, int.MaxValue));
    }
}
