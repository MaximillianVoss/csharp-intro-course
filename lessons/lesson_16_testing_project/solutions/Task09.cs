namespace Course.Lesson16;

public static class Task09
{
    public static string NormalizeNewlines(string text)=>text.Replace("\r\n","\n").Replace("\r","\n");
    public static void Run()
    {
        Check.Equal("A\n\nB\n",NormalizeNewlines("A\r\n\r\nB\r\n"));
        Check.Equal(" A\nB ",NormalizeNewlines(" A\rB "));
        Check.Equal("",NormalizeNewlines("")); Check.Equal("A\n",NormalizeNewlines("A\n"));
        Check.True(NormalizeNewlines("A\n\nB")!=NormalizeNewlines("A\nB"));
    }
}
