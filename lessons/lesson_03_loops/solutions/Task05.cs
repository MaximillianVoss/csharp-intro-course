using System;
using Course;

namespace Course.Lesson03;

public static class Task05
{
    public static int AcceptedCount(int limit)
    {
        int accepted = 0;
        for (int i = 1; i <= limit; i++)
        {
            if (i % 2 == 0 || i % 5 == 0) continue;
            accepted++;
        }
        return accepted;
    }

    public static void Run()
    {
        Check.Equal(0, AcceptedCount(0));
        Check.Equal(1, AcceptedCount(1));
        Check.Equal(2, AcceptedCount(5));
        Check.Equal(4, AcceptedCount(10));
        Check.Equal(400_000, AcceptedCount(1_000_000));
    }
}
