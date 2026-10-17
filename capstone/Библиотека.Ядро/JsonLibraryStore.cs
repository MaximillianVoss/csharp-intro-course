using System.Text.Json;

namespace Course.Library;

public interface ILibraryStore
{
    Task<LibraryCatalog> LoadAsync(CancellationToken token = default);
    Task SaveAsync(LibraryCatalog catalog, CancellationToken token = default);
}

public sealed class JsonLibraryStore(string path) : ILibraryStore
{
    public const int MaxBytes = 1_048_576;
    private readonly string path = Path.GetFullPath(path);
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public async Task<LibraryCatalog> LoadAsync(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        byte[] bytes;
        try
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
            if (stream.Length > MaxBytes) throw new InvalidDataException("JSON превышает 1 МиБ.");
            using var buffer = new MemoryStream();
            // A bounded read also protects against file growth after checking Length.
            var chunk = new byte[8192];
            int count;
            while ((count = await stream.ReadAsync(chunk, token)) != 0)
            {
                if (buffer.Length + count > MaxBytes) throw new InvalidDataException("JSON превышает 1 МиБ.");
                buffer.Write(chunk, 0, count);
            }
            bytes = buffer.ToArray();
        }
        catch (FileNotFoundException) { return new LibraryCatalog(); }
        catch (DirectoryNotFoundException) { return new LibraryCatalog(); }

        try
        {
            Snapshot dto = JsonSerializer.Deserialize<Snapshot>(bytes, Options)
                ?? throw new InvalidDataException("Вместо каталога получен null.");
            if (dto.SchemaVersion != 1 || dto.Books is null)
                throw new InvalidDataException("Ожидается schemaVersion=1 и массив books.");
            var books = dto.Books.Select(b => b is null
                ? throw new InvalidDataException("Элемент книги равен null.")
                : new Book(b.Id, b.Title!, b.Author!, b.Year, b.IsRead)).ToArray();
            return new LibraryCatalog(books, dto.NextId);
        }
        catch (JsonException e) { throw new InvalidDataException("Некорректный JSON.", e); }
        catch (ArgumentException e) { throw new InvalidDataException("Данные нарушают правила каталога.", e); }
    }

    public async Task SaveAsync(LibraryCatalog catalog, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(catalog);
        token.ThrowIfCancellationRequested();
        var dto = new Snapshot
        {
            SchemaVersion = 1,
            NextId = catalog.NextId,
            Books = catalog.Books.OrderBy(b => b.Id).Select(b => (BookDto?)new BookDto
            {
                Id = b.Id, Title = b.Title, Author = b.Author, Year = b.Year, IsRead = b.IsRead
            }).ToArray()
        };
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(dto, Options);
        if (bytes.Length > MaxBytes) throw new InvalidDataException("Каталог превышает 1 МиБ.");
        string directory = Path.GetDirectoryName(path)!;
        Directory.CreateDirectory(directory);
        string temporary = Path.Combine(directory, "." + Path.GetFileName(path) + "." + Guid.NewGuid().ToString("N") + ".tmp");
        try
        {
            await File.WriteAllBytesAsync(temporary, bytes, token);
            token.ThrowIfCancellationRequested();
            File.Move(temporary, path, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporary)) File.Delete(temporary);
        }
    }

    private sealed class Snapshot
    {
        public int SchemaVersion { get; set; }
        public int NextId { get; set; }
        public BookDto?[]? Books { get; set; }
    }

    private sealed class BookDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int Year { get; set; }
        public bool IsRead { get; set; }
    }
}
