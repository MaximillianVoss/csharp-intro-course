using System;
using Course;

namespace Course.Lesson03;

public static class Task06
{
    public static int Steps(int start)
    {
        int x = start;
        int steps = 0;
        do
        {
            x /= 2;
            steps++;
        } while (x > 0);
        return steps;
    }

    public static void Run()
    {
        Check.Equal(1, Steps(0));
        Check.Equal(1, Steps(1));
        Check.Equal(2, Steps(2));
        Check.Equal(4, Steps(8));
        Check.Equal(4, Steps(9));
    }
}
