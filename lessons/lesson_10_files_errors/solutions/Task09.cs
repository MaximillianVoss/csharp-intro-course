namespace Course.Lesson10;

public static class Task09
{
    public static string[] AppendAndRead(string[] messages)
    {
        string directory = Path.Combine(Path.GetTempPath(), "csharp-course-" + Guid.NewGuid().ToString("N")); Directory.CreateDirectory(directory);
        try
        {
            string path = Path.Combine(directory, "journal.txt"); System.Text.UTF8Encoding encoding = new(false, true);
            File.WriteAllText(path, "", encoding);
            foreach (string message in messages)
            {
                using StreamWriter writer = new(path, append: true, encoding: encoding); writer.WriteLine(message);
            }
            return File.ReadAllLines(path, encoding);
        }
        finally { Directory.Delete(directory, true); }
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { "первый", "", "третий" }, AppendAndRead(new[] { "первый", "", "третий" }));
        Check.SequenceEqual(Array.Empty<string>(), AppendAndRead(Array.Empty<string>()));
        Check.SequenceEqual(new[] { "один" }, AppendAndRead(new[] { "один" }));
    }
}
