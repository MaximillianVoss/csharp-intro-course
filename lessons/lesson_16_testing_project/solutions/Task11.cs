namespace Course.Lesson16;

public static class Task11
{
    public static void WriteReport(IReadOnlyDictionary<string,int> rows,TextWriter writer)
    {
        foreach(var row in rows.OrderBy(p=>p.Key,StringComparer.Ordinal))writer.WriteLine($"{row.Key}={row.Value}");
    }
    public static void Run()
    {
        using var writer=new StringWriter(){NewLine="\n"};
        WriteReport(new Dictionary<string,int>{{"b",2},{"a",1}},writer);
        Check.Equal("a=1\nb=2\n",writer.ToString()); writer.Write("tail");
        Check.True(writer.ToString().EndsWith("tail",StringComparison.Ordinal));
        using var empty=new StringWriter(); WriteReport(new Dictionary<string,int>(),empty); Check.Equal("",empty.ToString());
    }
}
