namespace Course.Lesson16;

public static class Task05
{
    public static void Commit(Action save,TextWriter output) { save(); output.WriteLine("Сохранено"); }
    public static void Run()
    {
        int calls=0; using var success=new StringWriter();
        Commit(()=>calls++,success); Check.Equal(1,calls); Check.Equal("Сохранено"+Environment.NewLine,success.ToString());
        using var failure=new StringWriter(); calls=0;
        Check.Throws<IOException>(()=>Commit(()=>{calls++;throw new IOException("disk");},failure));
        Check.Equal(1,calls); Check.Equal("",failure.ToString());
    }
}
