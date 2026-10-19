using System;
using Course;
using System.Text;

namespace Course.Lesson05;

public static class Task13
{
    public static bool SameLabel(string first, string second)
    {
        string normalizedFirst = first.Normalize(NormalizationForm.FormC);
        string normalizedSecond = second.Normalize(NormalizationForm.FormC);
        return string.Equals(normalizedFirst, normalizedSecond, StringComparison.Ordinal);
    }

    public static void Run()
    {
        Check.True(SameLabel("é", "e\u0301"));
        Check.Equal(false, SameLabel("A", "a"));
        Check.Equal(false, SameLabel("a", "a "));
        Check.Equal(false, SameLabel("①", "1"));
        Check.True(SameLabel("", ""));

        // Review 2026-10-19: normalization is not deaccenting or confusable folding
        Check.Equal(false, SameLabel("e", "é"));
        Check.Equal(false, SameLabel("A", "А"));
    }
}
