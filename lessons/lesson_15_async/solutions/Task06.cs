using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task06
{
    private static async Task<int> InvokeAsync(Func<Task<int>> factory)
    {
        var task = factory() ?? throw new InvalidOperationException("Null task."); return await task;
    }
    public static async Task<int[]> RunAllAsync(Func<Task<int>>[] factories)
    {
        if (factories is null || factories.Any(f => f is null)) throw new ArgumentException("Factories.");
        var tasks = new Task<int>[factories.Length]; for (int i = 0; i < tasks.Length; i++) tasks[i] = InvokeAsync(factories[i]); return await Task.WhenAll(tasks);
    }
    public static async Task Run()
    {
        var a = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously); var b = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously); int starts = 0;
        var pending = RunAllAsync(new Func<Task<int>>[] { () => { starts++; return a.Task; }, () => { starts++; return b.Task; } }); Check.Equal(2, starts); b.SetResult(20); Check.True(!pending.IsCompleted); a.SetResult(10); Check.SequenceEqual(new[] { 10, 20 }, await pending);
        int later = 0; await Check.ThrowsAsync<FormatException>(() => RunAllAsync(new Func<Task<int>>[] { () => throw new FormatException(), () => { later++; return Task.FromResult(2); } })); Check.Equal(1, later);
        await Check.ThrowsAsync<InvalidOperationException>(() => RunAllAsync(new Func<Task<int>>[] { () => null! })); Check.Equal(0, (await RunAllAsync(Array.Empty<Func<Task<int>>>())).Length);
        await Check.ThrowsAsync<ArgumentException>(() => RunAllAsync(new Func<Task<int>>[] { () => { later++; return Task.FromResult(0); }, null! })); Check.Equal(1, later);
    }
}
