using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task14
{
    public static async Task<int> RunWithLeaseAsync(IAsyncDisposable lease, Func<CancellationToken, Task<int>> work, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(lease); ArgumentNullException.ThrowIfNull(work);
        await using (lease) { token.ThrowIfCancellationRequested(); return await work(token); }
    }
    private sealed class Lease : IAsyncDisposable
    {
        public int Count { get; private set; } public Task Disposal { get; set; } = Task.CompletedTask;
        public ValueTask DisposeAsync() { Count++; return new ValueTask(Disposal); }
    }
    public static async Task Run()
    {
        var success = new Lease(); Check.Equal(7, await RunWithLeaseAsync(success, _ => Task.FromResult(7))); Check.Equal(1, success.Count);
        var fault = new Lease(); await Check.ThrowsAsync<IOException>(() => RunWithLeaseAsync(fault, _ => Task.FromException<int>(new IOException()))); Check.Equal(1, fault.Count);
        int starts = 0; var cancelled = new Lease(); await Check.ThrowsAsync<OperationCanceledException>(() => RunWithLeaseAsync(cancelled, _ => { starts++; return Task.FromResult(0); }, new CancellationToken(true))); Check.Equal(0, starts); Check.Equal(1, cancelled.Count);
        var gate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously); var delayed = new Lease { Disposal = gate.Task }; var pending = RunWithLeaseAsync(delayed, _ => Task.FromResult(9)); Check.True(!pending.IsCompleted); Check.Equal(1, delayed.Count); gate.SetResult(); Check.Equal(9, await pending);
        var invalid = new Lease(); await Check.ThrowsAsync<ArgumentNullException>(() => RunWithLeaseAsync(invalid, null!)); Check.Equal(0, invalid.Count);
    }
}
