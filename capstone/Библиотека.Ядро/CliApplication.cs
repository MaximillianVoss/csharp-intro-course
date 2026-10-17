using System.Globalization;

namespace Course.Library;

public sealed class CliApplication(Func<string, ILibraryStore> storeFactory)
{
    public const string Usage = """
        Каталог книг
        [--data путь.json] add "Название" "Автор" год
        [--data путь.json] list [--unread]
        [--data путь.json] read ID
        [--data путь.json] remove ID
        [--data путь.json] search "Фрагмент названия или автора"
        [--data путь.json] stats
        help
        """;

    public async Task<int> RunAsync(string[] args, TextWriter output, TextWriter error, CancellationToken token = default)
    {
        try
        {
            if (args.Length == 0 || args is ["help"] || args is ["--help"])
            {
                await output.WriteLineAsync(Usage);
                return 0;
            }
            string data = Path.Combine("data", "library.json");
            int offset = 0;
            if (args[0] == "--data")
            {
                if (args.Length < 3 || string.IsNullOrWhiteSpace(args[1])) return await BadUsage(error);
                data = args[1]; offset = 2;
            }
            string command = args[offset];
            string[] values = args[(offset + 1)..];
            int id = 0, year = 0;
            bool valid = command switch
            {
                "add" => values.Length == 3 && int.TryParse(values[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out year),
                "list" => values.Length == 0 || values is ["--unread"],
                "read" or "remove" => values.Length == 1 && int.TryParse(values[0], out id) && id > 0,
                "search" => values.Length == 1 && !string.IsNullOrWhiteSpace(values[0]),
                "stats" => values.Length == 0,
                _ => false
            };
            if (!valid) return await BadUsage(error);
            ILibraryStore store = storeFactory(data);
            LibraryCatalog catalog = await store.LoadAsync(token);
            switch (command)
            {
                case "add":
                    Book added = catalog.Add(values[0], values[1], year);
                    await store.SaveAsync(catalog, token);
                    await output.WriteLineAsync($"Добавлена книга #{added.Id}.");
                    break;
                case "list":
                    PrintBooks(output, values.Length == 0 ? catalog.Books : catalog.Books.Where(b => !b.IsRead));
                    break;
                case "read":
                case "remove":
                    bool found = command == "read" ? catalog.MarkRead(id) : catalog.Remove(id);
                    if (!found) { await error.WriteLineAsync($"Книга #{id} не найдена."); return 3; }
                    await store.SaveAsync(catalog, token);
                    await output.WriteLineAsync($"Книга #{id}: {(command == "read" ? "прочитана" : "удалена")}.");
                    break;
                case "search":
                    PrintBooks(output, catalog.Search(values[0]));
                    break;
                case "stats":
                    var stats = catalog.Statistics();
                    await output.WriteLineAsync($"Всего: {stats.Total}; прочитано: {stats.Read}; осталось: {stats.Unread}.");
                    break;
            }
            return 0;
        }
        catch (OperationCanceledException) { await error.WriteLineAsync("Операция отменена."); return 130; }
        catch (ArgumentException e) { await error.WriteLineAsync("Ошибка аргумента: " + e.Message); return 2; }
        catch (InvalidOperationException e) { await error.WriteLineAsync(e.Message); return 1; }
        catch (InvalidDataException e) { await error.WriteLineAsync("Ошибка данных: " + e.Message); return 1; }
        catch (IOException e) { await error.WriteLineAsync("Ошибка файла: " + e.Message); return 1; }
        catch (UnauthorizedAccessException e) { await error.WriteLineAsync("Нет доступа к файлу: " + e.Message); return 1; }
    }

    private static async Task<int> BadUsage(TextWriter error)
    {
        await error.WriteLineAsync(Usage);
        return 2;
    }

    private static void PrintBooks(TextWriter output, IEnumerable<Book> books)
    {
        bool any = false;
        foreach (Book book in books.OrderBy(b => b.Id))
        {
            any = true;
            output.WriteLine($"[{book.Id}] {book.Title} — {book.Author} ({book.Year}) — {(book.IsRead ? "прочитано" : "не прочитано")}");
        }
        if (!any) output.WriteLine("Книг нет.");
    }
}
