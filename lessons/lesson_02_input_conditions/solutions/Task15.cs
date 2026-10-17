using System;
using Course;
using System.Globalization;

namespace Course.Lesson02;

public static class Task15
{
    public static string Calculate(string? leftText, string? operation, string? rightText)
    {
        if (leftText is null || operation is null || rightText is null) return "eof";
        const NumberStyles style = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;
        if (!decimal.TryParse(leftText, style, CultureInfo.InvariantCulture, out decimal left) ||
            !decimal.TryParse(rightText, style, CultureInfo.InvariantCulture, out decimal right))
            return "invalid";
        if (left < -1_000_000m || left > 1_000_000m || right < -1_000_000m || right > 1_000_000m)
            return "range";
        try
        {
            decimal result;
            switch (operation)
            {
                case "+": result = left + right; break;
                case "-": result = left - right; break;
                case "*": result = left * right; break;
                case "/":
                    if (right == 0m) return "zero";
                    result = left / right;
                    break;
                default: return "operation";
            }
            return result.ToString(CultureInfo.InvariantCulture);
        }
        catch (OverflowException)
        {
            return "overflow";
        }
    }

    public static void Run()
    {
        Check.Equal("3.5", Calculate("1.5", "+", "2"));
        Check.Equal("-3", Calculate("2", "-", "5"));
        Check.Equal("-12", Calculate("-3", "*", "4"));
        Check.Equal("2.5", Calculate("5", "/", "2"));
        Check.Equal("zero", Calculate("4", "/", "0"));
        Check.Equal("eof", Calculate(null, "?", "bad"));
        Check.Equal("invalid", Calculate("1,5", "+", "2"));
        Check.Equal("invalid", Calculate(" 1", "+", "2"));
        Check.Equal("range", Calculate("1000001", "+", "2"));
        Check.Equal("operation", Calculate("1", "?", "2"));
        Check.Equal("invalid", Calculate("bad", "+", "1000001"));
        Check.Equal("10000000000000000000000000000", Calculate("1", "/", "0.0000000000000000000000000001"));
        Check.Equal("overflow", Calculate("1000000", "/", "0.0000000000000000000000000001"));
    }
}
