using System;
using Course;

namespace Course.Lesson02;

public static class Task11
{
    public static bool IsTriangle(int a, int b, int c)
    {
        return a > 0 && b > 0 && c > 0 && a + b > c && a + c > b && b + c > a;
    }
    public static bool Wrong(int a, int b, int c)
    {
        return a > 0 && b > 0 && c > 0 && a + b >= c;
    }

    public static void Run()
    {
        Check.True(IsTriangle(3, 4, 5));
        Check.True(IsTriangle(2, 2, 2));
        Check.Equal(false, IsTriangle(1, 2, 3));
        Check.Equal(false, IsTriangle(10, 1, 2));
        Check.Equal(false, IsTriangle(1, 10, 2));
        Check.Equal(false, IsTriangle(1, 2, 10));
        Check.Equal(false, IsTriangle(0, 2, 2));
        Check.Equal(false, IsTriangle(-1, 2, 2));
        Check.True(Wrong(1, 2, 3));
        Check.True(Wrong(10, 1, 2));
    }
}
