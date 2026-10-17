using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task12
{
    public static async Task<string> RetryAsync(Func<CancellationToken, Task<string>> load, int attempts, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(load); if (attempts < 1 || attempts > 5) throw new ArgumentOutOfRangeException(nameof(attempts));
        for (int attempt = 0; ; attempt++) { token.ThrowIfCancellationRequested(); try { return await load(token); } catch (IOException) when (attempt < attempts - 1) { } }
    }
    public static async Task Run()
    {
        int calls = 0; Check.Equal("ok", await RetryAsync(_ => ++calls < 3 ? Task.FromException<string>(new IOException()) : Task.FromResult("ok"), 3)); Check.Equal(3, calls);
        calls = 0; await Check.ThrowsAsync<IOException>(() => RetryAsync(_ => { calls++; throw new IOException(); }, 2)); Check.Equal(2, calls);
        calls = 0; await Check.ThrowsAsync<FormatException>(() => RetryAsync(_ => { calls++; throw new FormatException(); }, 5)); Check.Equal(1, calls);
        using var cts = new CancellationTokenSource(); calls = 0; await Check.ThrowsAsync<OperationCanceledException>(() => RetryAsync(_ => { calls++; cts.Cancel(); throw new IOException(); }, 3, cts.Token)); Check.Equal(1, calls);
        await Check.ThrowsAsync<ArgumentOutOfRangeException>(() => RetryAsync(_ => Task.FromResult("x"), 0));
    }
}
