using System;
using Course;

namespace Course.Lesson01;

public static class Task14
{
    public static byte Channel(int value)
    {
        return checked((byte)value);
    }

    public static void Run()
    {
        Check.Equal((byte)0, Channel(0));
        Check.Equal((byte)128, Channel(128));
        Check.Equal((byte)255, Channel(255));
        Check.Throws<OverflowException>(() => Channel(-1));
        Check.Throws<OverflowException>(() => Channel(256));
        Check.Throws<OverflowException>(() => Channel(int.MaxValue));
    }
}
