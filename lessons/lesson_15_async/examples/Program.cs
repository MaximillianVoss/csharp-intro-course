using Course;

var gate = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
var waiting = DecorateAsync(gate.Task); Check.True(!waiting.IsCompleted);
Console.WriteLine("Операция начата, результат ещё не готов.");
gate.SetResult("готово"); Check.Equal("[готово]", await waiting);
Console.WriteLine(await waiting);
var tasks = new[] { Task.FromResult(3), Task.FromResult(1) };
Check.SequenceEqual(new[] { 3, 1 }, await Task.WhenAll(tasks));
using var cancelled = new CancellationTokenSource(); cancelled.Cancel();
await Check.ThrowsAsync<OperationCanceledException>(() => CheckedAsync(cancelled.Token));
Console.WriteLine("Порядок результатов и предварительная отмена проверены.");

static async Task<string> DecorateAsync(Task<string> input) => "[" + await input + "]";
static Task CheckedAsync(CancellationToken token)
{
    token.ThrowIfCancellationRequested(); return Task.CompletedTask;
}
