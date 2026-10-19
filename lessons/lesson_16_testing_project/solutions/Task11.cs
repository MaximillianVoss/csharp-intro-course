namespace Course.Lesson16;

public static class Task11
{
    public static void WriteReport(IReadOnlyDictionary<string,int> rows,TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentNullException.ThrowIfNull(writer);
        foreach(var row in rows.OrderBy(p=>p.Key,StringComparer.Ordinal))writer.WriteLine($"{row.Key}={row.Value}");
    }
    public static void Run()
    {
        using var writer=new StringWriter(){NewLine="\n"};
        WriteReport(new Dictionary<string,int>{{"b",2},{"a",1}},writer);
        Check.Equal("a=1\nb=2\n",writer.ToString()); writer.Write("tail");
        Check.True(writer.ToString().EndsWith("tail",StringComparison.Ordinal));
        using var empty=new StringWriter(); WriteReport(new Dictionary<string,int>(),empty); Check.Equal("",empty.ToString());
        Check.Throws<ArgumentNullException>(()=>WriteReport(null!,empty));
        Check.Throws<ArgumentNullException>(()=>WriteReport(new Dictionary<string,int>(),null!));
        var culture=System.Globalization.CultureInfo.CurrentCulture;
        try
        {
            System.Globalization.CultureInfo.CurrentCulture=new("tr-TR");
            using var ordinal=new StringWriter(){NewLine="\n"};
            WriteReport(new Dictionary<string,int>{{"ı",4},{"İ",3},{"i",2},{"I",1}},ordinal);
            Check.Equal("I=1\ni=2\nİ=3\nı=4\n",ordinal.ToString());
        }
        finally{System.Globalization.CultureInfo.CurrentCulture=culture;}
    }
}
