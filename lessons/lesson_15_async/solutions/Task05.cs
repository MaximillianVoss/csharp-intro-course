using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task05
{
    public static async Task<int> LengthAsync(Func<CancellationToken, Task<string>> load, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(load); token.ThrowIfCancellationRequested(); var text = await load(token); token.ThrowIfCancellationRequested();
        return text?.Length ?? throw new InvalidOperationException("Null text.");
    }
    public static async Task Run()
    {
        using var cts = new CancellationTokenSource(); Check.Equal(3, await LengthAsync(t => { Check.Equal(cts.Token, t); return Task.FromResult("abc"); }, cts.Token));
        cts.Cancel(); int calls = 0; await Check.ThrowsAsync<OperationCanceledException>(() => LengthAsync(_ => { calls++; return Task.FromResult("x"); }, cts.Token)); Check.Equal(0, calls);
        using var during = new CancellationTokenSource(); await Check.ThrowsAsync<OperationCanceledException>(() => LengthAsync(_ => { during.Cancel(); return Task.FromResult("abc"); }, during.Token));
        await Check.ThrowsAsync<InvalidOperationException>(() => LengthAsync(_ => Task.FromResult<string>(null!))); await Check.ThrowsAsync<ArgumentNullException>(() => LengthAsync(null!));
    }
}
