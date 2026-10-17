using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task08
{
    public static async Task<(bool Success, int Value)> TryReadAsync(Func<Task<int>> read)
    {
        ArgumentNullException.ThrowIfNull(read); try { return (true, await read()); } catch (FormatException) { return (false, 0); }
    }
    public static async Task Run()
    {
        Check.Equal((true, 0), await TryReadAsync(() => Task.FromResult(0))); Check.Equal((false, 0), await TryReadAsync(() => throw new FormatException()));
        var gate = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously); var pending = TryReadAsync(() => gate.Task); Check.True(!pending.IsCompleted); gate.SetException(new FormatException()); Check.Equal((false, 0), await pending);
        await Check.ThrowsAsync<IOException>(() => TryReadAsync(() => Task.FromException<int>(new IOException())));
        await Check.ThrowsAsync<OperationCanceledException>(() => TryReadAsync(() => Task.FromCanceled<int>(new CancellationToken(true)))); await Check.ThrowsAsync<ArgumentNullException>(() => TryReadAsync(null!));
    }
}
