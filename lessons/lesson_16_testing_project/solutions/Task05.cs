namespace Course.Lesson16;

public static class Task05
{
    public static void Commit(Action save,TextWriter output)
    {
        ArgumentNullException.ThrowIfNull(save);
        ArgumentNullException.ThrowIfNull(output);
        save();
        output.WriteLine("Сохранено");
    }
    private sealed class FailingWriter : TextWriter
    {
        public override System.Text.Encoding Encoding => System.Text.Encoding.UTF8;
        public override void WriteLine(string? value) => throw new IOException("output");
    }
    public static void Run()
    {
        int calls=0; using var success=new StringWriter();
        Check.Throws<ArgumentNullException>(()=>Commit(()=>calls++,null!));
        Check.Equal(0,calls);
        Check.Throws<ArgumentNullException>(()=>Commit(null!,success));
        Check.Equal("",success.ToString());
        Commit(()=>calls++,success); Check.Equal(1,calls); Check.Equal("Сохранено"+Environment.NewLine,success.ToString());
        success.Write("tail"); Check.True(success.ToString().EndsWith("tail",StringComparison.Ordinal));
        using var failure=new StringWriter(); calls=0;
        Check.Throws<IOException>(()=>Commit(()=>{calls++;throw new IOException("disk");},failure));
        Check.Equal(1,calls); Check.Equal("",failure.ToString());
        calls=0;
        Check.Throws<FormatException>(()=>Commit(()=>{calls++;throw new FormatException("data");},failure));
        Check.Equal(1,calls); Check.Equal("",failure.ToString());
        using var outputFailure=new FailingWriter(); calls=0;
        Check.Throws<IOException>(()=>Commit(()=>calls++,outputFailure));
        Check.Equal(1,calls);
    }
}
