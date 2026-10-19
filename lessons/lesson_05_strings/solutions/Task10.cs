using System;
using Course;
using System.Text;

namespace Course.Lesson05;

public static class Task10
{
    public static bool TryUnquote(string? token, out string value)
    {
        value = "";
        if (token is null || token.Length < 2 || token[0] != '"' || token[token.Length - 1] != '"')
            return false;
        var builder = new StringBuilder();
        for (int index = 1; index < token.Length - 1; index++)
        {
            char ch = token[index];
            if (ch == '"')
            {
                if (index + 1 >= token.Length - 1 || token[index + 1] != '"') return false;
                index++;
            }
            builder.Append(ch);
        }
        value = builder.ToString();
        return true;
    }

    public static void Run()
    {
        Check.True(TryUnquote("\"abc\"", out string value));
        Check.Equal("abc", value);
        Check.True(TryUnquote("\"\"", out value));
        Check.Equal("", value);
        Check.True(TryUnquote("\"a\"\"b\"", out value));
        Check.Equal("a\"b", value);
        Check.Equal(false, TryUnquote("\"a\"b\"", out value));
        Check.Equal("", value);
        Check.Equal(false, TryUnquote("\"\"\"", out value));
        Check.True(TryUnquote("\"\"\"\"", out value));
        Check.Equal("\"", value);
        Check.Equal(false, TryUnquote(null, out value));
        Check.Equal(false, TryUnquote("abc", out value));
        Check.Equal("", value);

        // Review 2026-10-19: whole quoted field boundaries
        Check.True(TryUnquote("\"a\nb\"", out value));
        Check.Equal("a\nb", value);
        Check.Equal(false, TryUnquote(" \"a\"", out value));
        Check.Equal("", value);
        Check.Equal(false, TryUnquote("\"", out value));
        Check.Equal("", value);
    }
}
