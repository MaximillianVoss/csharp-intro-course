namespace Course.Lesson16;

public static class Task07
{
    public static Task<int> Work()=>Task.FromException<int>(new IOException("read"));
    public static async Task<string> Observe()
    {
        string trace="";
        try { Task<int> pending=Work(); trace+="A"; await pending; trace+="X"; }
        catch(IOException) { trace+="B"; }
        return trace;
    }
    public static void Run()
    {
        Task<int> pending=Work(); Check.True(pending.IsFaulted);
        Check.Throws<IOException>(()=>pending.GetAwaiter().GetResult());
        Check.Equal("AB",Observe().GetAwaiter().GetResult());
    }
}
