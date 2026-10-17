using System;
using Course;

namespace Course.Lesson02;

public static class Task05
{
    public static string Direction(int code)
    {
        switch (code)
        {
            case 0: return "north";
            case 1: return "east";
            case 2: return "south";
            case 3: return "west";
            default: return "invalid";
        }
    }

    public static void Run()
    {
        Check.Equal("north", Direction(0));
        Check.Equal("east", Direction(1));
        Check.Equal("south", Direction(2));
        Check.Equal("west", Direction(3));
        Check.Equal("invalid", Direction(-1));
        Check.Equal("invalid", Direction(4));
        Check.Equal("invalid", Direction(int.MaxValue));
    }
}
