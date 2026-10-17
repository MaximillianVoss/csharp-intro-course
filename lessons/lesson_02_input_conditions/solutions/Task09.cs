using System;
using Course;

namespace Course.Lesson02;

public static class Task09
{
    public static bool IsLeap(int year)
    {
        return year % 400 == 0 || (year % 4 == 0 && year % 100 != 0);
    }

    public static void Run()
    {
        Check.Equal(false, IsLeap(2023));
        Check.True(IsLeap(2024));
        Check.Equal(false, IsLeap(1900));
        Check.True(IsLeap(2000));
        Check.Equal(false, IsLeap(1));
        Check.Equal(false, IsLeap(9999));
    }
}
