using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task07
{
    public static async Task<string> FirstAsync(Task<string> first, Task<string> second)
    {
        ArgumentNullException.ThrowIfNull(first); ArgumentNullException.ThrowIfNull(second);
        if (first.IsCompleted) return await first; if (second.IsCompleted) return await second; return await await Task.WhenAny(first, second);
    }
    public static async Task Run()
    {
        var loser = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously); Check.Equal("B", await FirstAsync(loser.Task, Task.FromResult("B"))); Check.True(!loser.Task.IsCompleted); loser.SetResult("later"); Check.Equal("later", await loser.Task);
        Check.Equal("A", await FirstAsync(Task.FromResult("A"), Task.FromResult("B")));
        var a = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously); var b = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously); var race = FirstAsync(a.Task, b.Task); a.SetResult("first"); Check.Equal("first", await race); b.SetResult("second"); await b.Task;
        await Check.ThrowsAsync<FormatException>(() => FirstAsync(Task.FromException<string>(new FormatException()), Task.FromResult("ok")));
        await Check.ThrowsAsync<OperationCanceledException>(() => FirstAsync(Task.FromCanceled<string>(new CancellationToken(true)), Task.FromResult("ok")));
    }
}
