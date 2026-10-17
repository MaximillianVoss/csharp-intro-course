using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task03
{
    public static async Task<string[]> ObserveAsync()
    {
        var trace = new List<string>(); var gate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        async Task WorkAsync() { trace.Add("start"); await gate.Task; trace.Add("end"); }
        Task work = WorkAsync(); Check.True(!work.IsCompleted); trace.Add("caller"); gate.SetResult(); await work; return trace.ToArray();
    }
    public static async Task Run()
    {
        Check.SequenceEqual(new[] { "start", "caller", "end" }, await ObserveAsync());
        Check.SequenceEqual(new[] { "start", "caller", "end" }, await ObserveAsync());
    }
}
