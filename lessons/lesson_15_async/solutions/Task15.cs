using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task15
{
    public sealed class Item { public int Id { get; set; } public string? Name { get; set; } }
    public static async Task<int> ImportAsync(Stream source, Func<Item, CancellationToken, Task> save, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(source); ArgumentNullException.ThrowIfNull(save); token.ThrowIfCancellationRequested();
        var items = await JsonSerializer.DeserializeAsync<Item?[]>(source, cancellationToken: token) ?? throw new JsonException("Array required."); var ids = new HashSet<int>();
        foreach (var item in items) if (item is null || item.Id <= 0 || string.IsNullOrWhiteSpace(item.Name) || !ids.Add(item.Id)) throw new JsonException("Invalid item.");
        foreach (var item in items) { token.ThrowIfCancellationRequested(); await save(item!, token); } token.ThrowIfCancellationRequested(); return items.Length;
    }
    private static MemoryStream Json(string text) => new(Encoding.UTF8.GetBytes(text));
    public static async Task Run()
    {
        using var source = Json("[{\"Id\":2,\"Name\":\"B\"},{\"Id\":1,\"Name\":\"A\"}]"); var seen = new List<int>();
        Check.Equal(2, await ImportAsync(source, (item, token) => { Check.Equal(CancellationToken.None, token); seen.Add(item.Id); return Task.CompletedTask; })); Check.SequenceEqual(new[] { 2, 1 }, seen); Check.True(source.CanRead);
        foreach (var text in new[] { "[{\"Id\":1,\"Name\":\"A\"},{\"Id\":1,\"Name\":\"B\"}]", "[{\"Id\":1,\"Name\":\"A\"},null]" })
        {
            using var invalid = Json(text); int calls = 0; await Check.ThrowsAsync<JsonException>(() => ImportAsync(invalid, (_, _) => { calls++; return Task.CompletedTask; })); Check.Equal(0, calls);
        }
        using var failure = Json("[{\"Id\":1,\"Name\":\"A\"},{\"Id\":2,\"Name\":\"B\"},{\"Id\":3,\"Name\":\"C\"}]"); var saved = new List<int>();
        await Check.ThrowsAsync<IOException>(() => ImportAsync(failure, (item, _) => { if (item.Id == 2) throw new IOException(); saved.Add(item.Id); return Task.CompletedTask; })); Check.SequenceEqual(new[] { 1 }, saved);
        using var empty = Json("[]"); Check.Equal(0, await ImportAsync(empty, (_, _) => Task.CompletedTask));
        using var cancel = Json("[]"); await Check.ThrowsAsync<OperationCanceledException>(() => ImportAsync(cancel, (_, _) => Task.CompletedTask, new CancellationToken(true))); Check.Equal(0L, cancel.Position);
        using var ordered = Json("[{\"Id\":1,\"Name\":\"A\"},{\"Id\":2,\"Name\":\"B\"}]");
        var firstStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var finishFirst = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously); int sequentialCalls = 0;
        var sequential = ImportAsync(ordered, (item, _) =>
        {
            Interlocked.Increment(ref sequentialCalls);
            if (item.Id == 1) { firstStarted.SetResult(); return finishFirst.Task; }
            return Task.CompletedTask;
        });
        await firstStarted.Task; Check.Equal(1, Volatile.Read(ref sequentialCalls)); Check.True(!sequential.IsCompleted);
        finishFirst.SetResult(); Check.Equal(2, await sequential); Check.Equal(2, Volatile.Read(ref sequentialCalls));
        using var afterLast = Json("[{\"Id\":1,\"Name\":\"A\"}]"); using var afterLastToken = new CancellationTokenSource();
        int finalSaves = 0;
        await Check.ThrowsAsync<OperationCanceledException>(() => ImportAsync(afterLast, (_, token) =>
        {
            Check.Equal(afterLastToken.Token, token); finalSaves++; afterLastToken.Cancel(); return Task.CompletedTask;
        }, afterLastToken.Token));
        Check.Equal(1, finalSaves); Check.True(afterLast.CanRead);
        using var malformedName = Json("[{\"Id\":1,\"Name\":\"A\"},{\"Id\":2,\"Name\":\"\\uD800\"}]"); int invalidSaves = 0;
        await Check.ThrowsAsync<JsonException>(() => ImportAsync(malformedName, (_, _) => { invalidSaves++; return Task.CompletedTask; })); Check.Equal(0, invalidSaves);
    }
}
