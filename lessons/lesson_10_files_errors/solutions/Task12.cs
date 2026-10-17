namespace Course.Lesson10;

public static class Task12
{
    public static string? ReadOptionalText(string path)
    {
        try { return File.ReadAllText(path, System.Text.Encoding.UTF8); }
        catch (FileNotFoundException) { return null; }
        catch (DirectoryNotFoundException) { return null; }
    }
    public static void Run()
    {
        string directory = Path.Combine(Path.GetTempPath(), "csharp-course-" + Guid.NewGuid().ToString("N")); Directory.CreateDirectory(directory);
        try
        {
            string path = Path.Combine(directory, "file.txt"); Check.True(ReadOptionalText(path) is null);
            Check.True(ReadOptionalText(Path.Combine(directory, "missing", "file.txt")) is null);
            File.WriteAllText(path, ""); Check.Equal("", ReadOptionalText(path));
            File.WriteAllText(path, "данные", System.Text.Encoding.UTF8); Check.Equal("данные", ReadOptionalText(path));
            Check.Throws<ArgumentException>(() => ReadOptionalText(path + '\0'));
        }
        finally { Directory.Delete(directory, true); }
    }
}
