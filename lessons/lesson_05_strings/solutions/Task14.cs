using System;
using Course;
using System.Globalization;

namespace Course.Lesson05;

public static class Task14
{
    public static bool TryAmount(string? text, string cultureName, out decimal amount)
    {
        amount = 0m;
        const NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite |
                                    NumberStyles.AllowTrailingWhite | NumberStyles.AllowDecimalPoint;
        if (!decimal.TryParse(text, styles, CultureInfo.GetCultureInfo(cultureName), out decimal parsed))
            return false;
        if (parsed < -1_000_000m || parsed > 1_000_000m) return false;
        amount = parsed;
        return true;
    }

    public static void Run()
    {
        Check.True(TryAmount("1,5", "ru-RU", out decimal value));
        Check.Equal(1.5m, value);
        Check.Equal(false, TryAmount("1,5", "en-US", out value));
        Check.Equal(0m, value);
        Check.True(TryAmount(" -1.5 ", "en-US", out value));
        Check.Equal(-1.5m, value);
        Check.Equal(false, TryAmount("1.5", "ru-RU", out value));
        Check.Equal(false, TryAmount("1,000", "en-US", out value));
        Check.Equal(false, TryAmount(null, "ru-RU", out value));
        Check.Equal(false, TryAmount("1000001", "en-US", out value));
        Check.Equal(0m, value);

        // Review 2026-10-19: exponent and grouping remain forbidden
        Check.Equal(false, TryAmount("1e2", "en-US", out value));
        Check.Equal(0m, value);
        Check.Equal(false, TryAmount("1 000", "ru-RU", out value));
        Check.Equal(0m, value);
    }
}
