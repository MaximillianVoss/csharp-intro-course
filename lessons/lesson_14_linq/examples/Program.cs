using Course;

var requests = new List<Request> { new("/books", 200), new("/books", 500), new("/help", 200) };
var failures = requests.Where(x => x.Status >= 500);
var snapshot = failures.ToArray(); requests.Add(new Request("/help", 503));
Check.Equal(1, snapshot.Length); Check.Equal(2, failures.Count());
var report = requests.GroupBy(x => x.Path, StringComparer.Ordinal)
    .Select(group => new { Path = group.Key, Count = group.Count(), Errors = group.Count(x => x.Status >= 500) })
    .OrderByDescending(x => x.Errors).ThenBy(x => x.Path, StringComparer.Ordinal).ToArray();
Check.Equal("/books", report[0].Path); Check.Equal(2, report[0].Count);
foreach (var row in report) Console.WriteLine($"{row.Path}: всего {row.Count}, ошибок {row.Errors}");
Console.WriteLine("Снимок и отложенный запрос проверены отдельно.");

public sealed record Request(string Path, int Status);
