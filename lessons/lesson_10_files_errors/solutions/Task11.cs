namespace Course.Lesson10;

public static class Task11
{
    public static void WriteThenCommit(string path, string text, bool failBeforeCommit)
    {
        string staging = Path.Combine(Path.GetDirectoryName(path)!, ".stage-" + Guid.NewGuid().ToString("N"));
        try
        {
            File.WriteAllText(staging, text, new System.Text.UTF8Encoding(false, true));
            if (failBeforeCommit) throw new IOException("Учебный отказ до фиксации.");
            File.Move(staging, path, overwrite: true);
        }
        finally { if (File.Exists(staging)) File.Delete(staging); }
    }
    public static void Run()
    {
        string directory = Path.Combine(Path.GetTempPath(), "csharp-course-" + Guid.NewGuid().ToString("N")); Directory.CreateDirectory(directory);
        try
        {
            string path = Path.Combine(directory, "data.txt"); File.WriteAllText(path, "old");
            Check.Throws<IOException>(() => WriteThenCommit(path, "new", true)); Check.Equal("old", File.ReadAllText(path));
            Check.Equal(1, Directory.GetFiles(directory).Length);
            string absent = Path.Combine(directory, "absent.txt");
            Check.Throws<IOException>(() => WriteThenCommit(absent, "not-committed", true));
            Check.True(!File.Exists(absent)); Check.Equal(1, Directory.GetFiles(directory).Length);
            WriteThenCommit(path, "new", false); Check.Equal("new", File.ReadAllText(path)); Check.Equal(1, Directory.GetFiles(directory).Length);
            string second = Path.Combine(directory, "second.txt"); WriteThenCommit(second, "создан", false); Check.Equal("создан", File.ReadAllText(second));
            Check.SequenceEqual(System.Text.Encoding.UTF8.GetBytes("создан"), File.ReadAllBytes(second));
        }
        finally { Directory.Delete(directory, true); }
    }
}
