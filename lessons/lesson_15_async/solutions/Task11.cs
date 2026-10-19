using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task11
{
    public static async Task<int[]> MapBoundedAsync(int[] values, int maximum, Func<int, Task<int>> work, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(values); ArgumentNullException.ThrowIfNull(work); if (maximum < 1 || maximum > 100) throw new ArgumentOutOfRangeException(nameof(maximum)); if (values.Length > 1000) throw new ArgumentOutOfRangeException(nameof(values)); token.ThrowIfCancellationRequested();
        using var semaphore = new SemaphoreSlim(maximum);
        async Task<int> One(int value) { await semaphore.WaitAsync(token); try { token.ThrowIfCancellationRequested(); return await work(value); } finally { semaphore.Release(); } }
        return await Task.WhenAll(values.Select(One));
    }
    public static async Task Run()
    {
        var gates = new[] { new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously), new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously) };
        var startedThird = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously); int starts = 0;
        var pending = MapBoundedAsync(new[] { 1, 2, 3 }, 2, value => { Interlocked.Increment(ref starts); if (value == 3) { startedThird.SetResult(); return Task.FromResult(30); } return gates[value - 1].Task; });
        Check.Equal(2, Volatile.Read(ref starts)); gates[1].SetResult(20); await startedThird.Task; Check.Equal(3, Volatile.Read(ref starts)); gates[0].SetResult(10); Check.SequenceEqual(new[] { 10, 20, 30 }, await pending);
        int afterFault = 0; await Check.ThrowsAsync<IOException>(() => MapBoundedAsync(new[] { 1, 2 }, 1, value => { afterFault++; return value == 1 ? Task.FromException<int>(new IOException()) : Task.FromResult(2); })); Check.Equal(2, afterFault);
        int cancelledCalls = 0; await Check.ThrowsAsync<OperationCanceledException>(() => MapBoundedAsync(new[] { 1 }, 1, x => { cancelledCalls++; return Task.FromResult(x); }, new CancellationToken(true))); Check.Equal(0, cancelledCalls);
        await Check.ThrowsAsync<ArgumentOutOfRangeException>(() => MapBoundedAsync(Array.Empty<int>(), 0, Task.FromResult));
        using var during = new CancellationTokenSource();
        var active = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously); int duringStarts = 0;
        var cancelling = MapBoundedAsync(new[] { 1, 2 }, 1, _ => { Interlocked.Increment(ref duringStarts); return active.Task; }, during.Token);
        during.Cancel(); Check.True(!cancelling.IsCompleted); Check.Equal(1, Volatile.Read(ref duringStarts));
        active.SetResult(1); await Check.ThrowsAsync<OperationCanceledException>(() => cancelling); Check.Equal(1, Volatile.Read(ref duringStarts));
        var survivor = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        var failedButWaiting = MapBoundedAsync(new[] { 1, 2 }, 2, value => value == 1 ? Task.FromException<int>(new IOException()) : survivor.Task);
        Check.True(!failedButWaiting.IsCompleted); survivor.SetResult(2); await Check.ThrowsAsync<IOException>(() => failedButWaiting);
    }
}
