using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task02
{
    public static async Task<string> RoundTripAsync(string path, string text, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path."); ArgumentNullException.ThrowIfNull(text); token.ThrowIfCancellationRequested();
        var encoding = new UTF8Encoding(false, true);
        byte[] bytes;
        try { bytes = encoding.GetBytes(text); }
        catch (EncoderFallbackException error) { throw new ArgumentException("Unpaired surrogate in text.", nameof(text), error); }
        await File.WriteAllBytesAsync(path, bytes, token);
        return encoding.GetString(await File.ReadAllBytesAsync(path, token));
    }
    public static async Task Run()
    {
        var dir = Path.Combine(Path.GetTempPath(), "csharp-async-" + Guid.NewGuid()); Directory.CreateDirectory(dir);
        try
        {
            var path = Path.Combine(dir, "sample.txt"); Check.Equal("строка\n二", await RoundTripAsync(path, "строка\n二")); Check.Equal("", await RoundTripAsync(path, ""));
            var absent = Path.Combine(dir, "cancelled.txt"); using var cts = new CancellationTokenSource(); cts.Cancel();
            await Check.ThrowsAsync<OperationCanceledException>(() => RoundTripAsync(absent, "x", cts.Token)); Check.True(!File.Exists(absent));
            await Check.ThrowsAsync<ArgumentException>(() => RoundTripAsync(" ", "x")); await Check.ThrowsAsync<ArgumentNullException>(() => RoundTripAsync(path, null!));
            Check.Equal("\r\n\uD83D\uDE00\0", await RoundTripAsync(path, "\r\n\uD83D\uDE00\0"));
            foreach (var text in new[] { "\uFEFFabc", "\uFEFF\uFEFFabc", "\uFEFF", "\uFEFF\uFEFF\uD83D\uDE00" })
                Check.Equal(text, await RoundTripAsync(path, text));
            var before = await File.ReadAllBytesAsync(path);
            await Check.ThrowsAsync<ArgumentException>(() => RoundTripAsync(path, "\uD800"));
            Check.SequenceEqual(before, await File.ReadAllBytesAsync(path));
            var invalidPath = Path.Combine(dir, "invalid-unicode.txt");
            await Check.ThrowsAsync<ArgumentException>(() => RoundTripAsync(invalidPath, "\uDC00"));
            Check.True(!File.Exists(invalidPath));
        }
        finally { Directory.Delete(dir, true); }
    }
}
