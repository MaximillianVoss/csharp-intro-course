namespace Course.Lesson10;

public static class Task07
{
    public static string ResolveChild(string root, string? fileName)
    {
        if (string.IsNullOrEmpty(fileName) || fileName.Length > 100 || fileName == "." || fileName == "..") throw new ArgumentException("Неверное имя.", nameof(fileName));
        foreach (char c in fileName)
            if (!((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '.' || c == '-' || c == '_'))
                throw new ArgumentException("Допускается один компонент ASCII.", nameof(fileName));
        return Path.Combine(root, fileName);
    }
    public static void Run()
    {
        string root = Path.Combine(Path.GetTempPath(), "csharp-course-" + Guid.NewGuid().ToString("N")); Directory.CreateDirectory(root);
        try
        {
            Check.Equal(Path.Combine(root, "report-1.txt"), ResolveChild(root, "report-1.txt"));
            foreach (string? invalid in new string?[] { "../report.txt", "a\\b", "C:other", "..", ".", null, " ", new string('a', 101) })
                Check.Throws<ArgumentException>(() => ResolveChild(root, invalid));
        }
        finally { Directory.Delete(root, true); }
    }
}
