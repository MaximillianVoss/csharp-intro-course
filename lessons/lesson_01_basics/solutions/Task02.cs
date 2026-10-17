using System;
using Course;

namespace Course.Lesson01;

public static class Task02
{
    public static string Duration(int seconds)
    {
        int hours = seconds / 3600;
        int minutes = seconds / 60 % 60;
        int rest = seconds % 60;
        return $"{hours}:{minutes}:{rest}";
    }

    public static void Run()
    {
        Check.Equal("1:1:1", Duration(3661));
        Check.Equal("0:59:59", Duration(3599));
        Check.Equal("0:0:0", Duration(0));
        Check.Equal("1:0:0", Duration(3600));
        Check.Equal("277:46:40", Duration(1_000_000));
    }
}
