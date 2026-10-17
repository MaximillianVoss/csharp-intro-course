using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task04
{
    public static async Task<int> DoubleAsync(Task<int> input)
    {
        ArgumentNullException.ThrowIfNull(input); int value = await input; return checked(value * 2);
    }
    public static async Task Run()
    {
        Check.Equal(8, await DoubleAsync(Task.FromResult(4))); Check.Equal(-6, await DoubleAsync(Task.FromResult(-3)));
        var gate = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously); var pending = DoubleAsync(gate.Task); Check.True(!pending.IsCompleted); gate.SetResult(5); Check.Equal(10, await pending);
        await Check.ThrowsAsync<FormatException>(() => DoubleAsync(Task.FromException<int>(new FormatException())));
        await Check.ThrowsAsync<OverflowException>(() => DoubleAsync(Task.FromResult(int.MaxValue))); await Check.ThrowsAsync<ArgumentNullException>(() => DoubleAsync(null!));
    }
}
