using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task10
{
    public static async Task SaveThenNotifyAsync(Func<Task> save, Action notify)
    {
        ArgumentNullException.ThrowIfNull(save); ArgumentNullException.ThrowIfNull(notify); await save(); notify();
    }
    private static Task Broken(Func<Task> save, Action notify) { _ = save(); notify(); return Task.CompletedTask; }
    public static async Task Run()
    {
        var gate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously); int notices = 0; var pending = SaveThenNotifyAsync(() => gate.Task, () => notices++); Check.True(!pending.IsCompleted); Check.Equal(0, notices); gate.SetResult(); await pending; Check.Equal(1, notices);
        var badGate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously); int early = 0; var bad = Broken(() => badGate.Task, () => early++); Check.True(bad.IsCompleted); Check.Equal(1, early); badGate.SetResult(); await badGate.Task;
        await Check.ThrowsAsync<IOException>(() => SaveThenNotifyAsync(() => Task.FromException(new IOException()), () => notices++)); Check.Equal(1, notices);
        int saves = 0; await Check.ThrowsAsync<ArgumentNullException>(() => SaveThenNotifyAsync(() => { saves++; return Task.CompletedTask; }, null!)); Check.Equal(0, saves);
        var error = new FormatException("notify failure");
        var observed = await Check.ThrowsAsync<FormatException>(() => SaveThenNotifyAsync(() => { saves++; return Task.CompletedTask; }, () => throw error));
        Check.True(ReferenceEquals(error, observed)); Check.Equal(1, saves);
    }
}
