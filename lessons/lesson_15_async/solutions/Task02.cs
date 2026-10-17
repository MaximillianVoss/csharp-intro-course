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
        await File.WriteAllTextAsync(path, text, Encoding.UTF8, token); return await File.ReadAllTextAsync(path, Encoding.UTF8, token);
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
        }
        finally { Directory.Delete(dir, true); }
    }
}
