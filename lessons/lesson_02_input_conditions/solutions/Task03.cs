using System;
using Course;

namespace Course.Lesson02;

public static class Task03
{
    public static bool InWindow(int minute, int opening, int closing)
    {
        return minute >= opening && minute < closing;
    }

    public static void Run()
    {
        Check.Equal(false, InWindow(539, 540, 1020));
        Check.True(InWindow(540, 540, 1020));
        Check.True(InWindow(1019, 540, 1020));
        Check.Equal(false, InWindow(1020, 540, 1020));
        Check.Equal(false, InWindow(600, 600, 600));
    }
}
