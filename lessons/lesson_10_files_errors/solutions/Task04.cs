namespace Course.Lesson10;

public static class Task04
{
    public static string RoundTrip(string text)
    {
        string directory = Path.Combine(Path.GetTempPath(), "csharp-course-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            string path = Path.Combine(directory, "text.txt");
            System.Text.UTF8Encoding encoding = new(false, true);
            File.WriteAllText(path, text, encoding);
            return File.ReadAllText(path, encoding);
        }
        finally { Directory.Delete(directory, true); }
    }
    public static void Run()
    {
        Check.Equal("Привет\r\nC#\n🙂", RoundTrip("Привет\r\nC#\n🙂")); Check.Equal("", RoundTrip(""));
        string directory = Path.Combine(Path.GetTempPath(), "csharp-course-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            string path = Path.Combine(directory, "bytes.txt"); File.WriteAllText(path, "Я", new System.Text.UTF8Encoding(false, true));
            Check.SequenceEqual(new byte[] { 0xD0, 0xAF }, File.ReadAllBytes(path));
        }
        finally { Directory.Delete(directory, true); }
        Check.True(!Directory.Exists(directory));
    }
}
