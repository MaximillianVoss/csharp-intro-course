using System.Text.Json;

namespace Course;

public static class Runner
{
    public static int Run(string[] args, (string Id, Action Body)[] tasks, bool template = false) =>
        RunAsync(args, tasks.Select(t => (t.Id, (Func<Task>)(() => { t.Body(); return Task.CompletedTask; }))).ToArray(), template).GetAwaiter().GetResult();

    public static async Task<int> RunAsync(string[] args, (string Id, Func<Task> Body)[] tasks, bool template = false)
    {
        if (args is ["--list"] || (template && args.Length == 0))
        {
            if (template) Console.WriteLine("ЗАГОТОВКИ: компилируются, решения ещё не выполнены.");
            foreach (var task in tasks) Console.WriteLine(task.Id);
            return 0;
        }
        if (args.Length > 1 || (args.Length == 1 && args[0] != "--all" && !tasks.Any(t => t.Id == args[0])))
        {
            Console.Error.WriteLine("Использование: [--all | --list | NN-XX]");
            return 2;
        }
        var selected = args.Length == 0 || args[0] == "--all" ? tasks : tasks.Where(t => t.Id == args[0]).ToArray();
        int failures = 0;
        foreach (var task in selected)
        {
            try { await task.Body(); Console.WriteLine($"PASS {task.Id}"); }
            catch (Exception e) { failures++; Console.Error.WriteLine($"FAIL {task.Id}: {e.GetType().Name}: {e.Message}"); }
        }
        Console.WriteLine("RESULT " + JsonSerializer.Serialize(new { tasks = selected.Length, assertions = Check.Assertions, failed = failures, template }));
        return failures == 0 ? 0 : 1;
    }
}
