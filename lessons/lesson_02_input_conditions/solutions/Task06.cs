using System;
using Course;

namespace Course.Lesson02;

public static class Task06
{
    public static string Inspect(bool ready, bool permitted)
    {
        string result = "unchanged";
        if (ready)
        {
            if (permitted)
            {
                result = "go";
            }
            else
            {
                result = "denied";
            }
        }
        return result;
    }

    public static void Run()
    {
        Check.Equal("go", Inspect(true, true));
        Check.Equal("denied", Inspect(true, false));
        Check.Equal("unchanged", Inspect(false, true));
        Check.Equal("unchanged", Inspect(false, false));
    }
}
