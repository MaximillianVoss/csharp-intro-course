namespace Course.Lesson07;

public static class Task14
{
    public static int UniqueCodeCount(string[] codes) => new HashSet<string>(codes, StringComparer.OrdinalIgnoreCase).Count;
    public static void Run()
    {
        Check.Equal(0, UniqueCodeCount(Array.Empty<string>()));
        Check.Equal(1, UniqueCodeCount(new[] { "id", "id" }));
        string[] sample = { "id", "ID", "id-1" };
        Check.Equal(2, UniqueCodeCount(sample)); Check.Equal(2, UniqueCodeCount(new[] { "id", "id " }));
        int wrong = new HashSet<string>(sample).Count;
        Check.Equal(3, wrong); Check.True(wrong != UniqueCodeCount(sample));
    }
}
