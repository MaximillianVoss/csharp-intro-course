using System;
using Course;

namespace Course.Lesson04;

public static class Task15
{
    public static bool IsFourDigits(int code) { return code >= 1000 && code <= 9999; }
    public static int ExpectedDigit(int prefix)
    {
        int a = prefix / 100;
        int b = prefix / 10 % 10;
        int c = prefix % 10;
        return (3 * a + 2 * b + c) % 10;
    }
    public static string InspectCode(int code)
    {
        if (!IsFourDigits(code)) return "range";
        return code % 10 == ExpectedDigit(code / 10) ? "valid" : "checksum";
    }

    public static void Run()
    {
        Check.Equal(false, IsFourDigits(999));
        Check.True(IsFourDigits(1000));
        Check.True(IsFourDigits(9999));
        Check.Equal(0, ExpectedDigit(123));
        Check.Equal(7, ExpectedDigit(900));
        Check.Equal("valid", InspectCode(1230));
        Check.Equal("checksum", InspectCode(1234));
        Check.Equal("valid", InspectCode(1003));
        Check.Equal("valid", InspectCode(9994));
        Check.Equal("range", InspectCode(-1230));
        Check.Equal("range", InspectCode(10000));
    }
}
