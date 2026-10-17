using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task01
{
    public static async Task<int> TransformAsync(int value, Func<int, Task<int>> transform)
    {
        ArgumentNullException.ThrowIfNull(transform); return await transform(value);
    }
    public static async Task Run()
    {
        Check.Equal(6, await TransformAsync(3, x => Task.FromResult(x * 2)));
        var gate = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously); int calls = 0;
        var pending = TransformAsync(3, x => { calls++; Check.Equal(3, x); return gate.Task; }); Check.True(!pending.IsCompleted); Check.Equal(1, calls);
        gate.SetResult(8); Check.Equal(8, await pending);
        await Check.ThrowsAsync<FormatException>(() => TransformAsync(1, _ => Task.FromException<int>(new FormatException())));
        await Check.ThrowsAsync<ArgumentNullException>(() => TransformAsync(0, null!));
    }
}
