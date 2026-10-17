using System;
using Course;

namespace Course.Lesson04;

public static class Task14
{
    public static bool TryAdvanceClock(ref int hours, ref int minutes, int deltaMinutes)
    {
        if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59 ||
            deltaMinutes < 0 || deltaMinutes > 1440) return false;
        int total = (hours * 60 + minutes + deltaMinutes) % 1440;
        hours = total / 60;
        minutes = total % 60;
        return true;
    }

    public static void Run()
    {
        int hours = 23;
        int minutes = 50;
        Check.True(TryAdvanceClock(ref hours, ref minutes, 20));
        Check.Equal(0, hours);
        Check.Equal(10, minutes);
        Check.True(TryAdvanceClock(ref hours, ref minutes, 1440));
        Check.Equal(0, hours);
        Check.Equal(10, minutes);
        Check.True(TryAdvanceClock(ref hours, ref minutes, 0));
        Check.Equal(0, hours);
        Check.Equal(10, minutes);
        hours = 0;
        minutes = 59;
        Check.True(TryAdvanceClock(ref hours, ref minutes, 1));
        Check.Equal(1, hours);
        Check.Equal(0, minutes);
        static void Refusal(int originalHours, int originalMinutes, int delta)
        {
            int h = originalHours;
            int m = originalMinutes;
            Check.Equal(false, TryAdvanceClock(ref h, ref m, delta));
            Check.Equal(originalHours, h);
            Check.Equal(originalMinutes, m);
        }
        Refusal(-1, 10, 1);
        Refusal(24, 10, 1);
        Refusal(12, -1, 1);
        Refusal(12, 60, 1);
        Refusal(12, 30, -1);
        Refusal(12, 30, 1441);
        Refusal(12, 30, int.MaxValue);
    }
}
