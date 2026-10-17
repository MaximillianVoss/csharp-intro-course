namespace Course.Lesson10;

public static class Task14
{
    public static string ReadStrictUtf8(string path)
    {
        byte[] bytes = File.ReadAllBytes(path);
        int offset = bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF ? 3 : 0;
        return new System.Text.UTF8Encoding(false, true).GetString(bytes, offset, bytes.Length - offset);
    }
    public static void Run()
    {
        string directory = Path.Combine(Path.GetTempPath(), "csharp-course-" + Guid.NewGuid().ToString("N")); Directory.CreateDirectory(directory);
        try
        {
            string path = Path.Combine(directory, "bytes.txt"); File.WriteAllBytes(path, new byte[] { 0xD0, 0xAF }); Check.Equal("Я", ReadStrictUtf8(path));
            File.WriteAllBytes(path, new byte[] { 0xEF, 0xBB, 0xBF, 0x41 }); Check.Equal("A", ReadStrictUtf8(path));
            File.WriteAllBytes(path, Array.Empty<byte>()); Check.Equal("", ReadStrictUtf8(path));
            byte[] corrupt = { 0xC3, 0x28 }; File.WriteAllBytes(path, corrupt);
            Check.Throws<System.Text.DecoderFallbackException>(() => ReadStrictUtf8(path));
            string wrong = System.Text.Encoding.UTF8.GetString(corrupt); Check.True(wrong.Contains('\uFFFD'));
        }
        finally { Directory.Delete(directory, true); }
    }
}
