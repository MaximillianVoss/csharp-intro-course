namespace Course.Library;

public sealed class LibraryCatalog
{
    private readonly List<Book> books;
    public int NextId { get; private set; }
    public IReadOnlyList<Book> Books => books.ToArray();

    public LibraryCatalog() : this([], 1) { }

    public LibraryCatalog(IEnumerable<Book> initial, int nextId)
    {
        ArgumentNullException.ThrowIfNull(initial);
        books = initial.ToList();
        if (books.Any(b => b is null) || books.Select(b => b.Id).Distinct().Count() != books.Count)
            throw new ArgumentException("Книги должны иметь уникальные положительные ID.", nameof(initial));
        if (nextId <= 0 || books.Any(b => b.Id >= nextId))
            throw new ArgumentOutOfRangeException(nameof(nextId));
        NextId = nextId;
    }

    public Book Add(string title, string author, int year)
    {
        if (NextId == int.MaxValue) throw new InvalidOperationException("Диапазон ID исчерпан.");
        var book = new Book(NextId, title, author, year);
        books.Add(book);
        NextId++;
        return book;
    }

    public bool MarkRead(int id)
    {
        int index = books.FindIndex(b => b.Id == id);
        if (index < 0) return false;
        books[index] = books[index].AsRead();
        return true;
    }

    public bool Remove(int id)
    {
        int index = books.FindIndex(b => b.Id == id);
        if (index < 0) return false;
        books.RemoveAt(index);
        return true;
    }

    public IReadOnlyList<Book> Search(string query)
    {
        ArgumentNullException.ThrowIfNull(query);
        string term = query.Trim();
        if (term.Length == 0) throw new ArgumentException("Поисковая строка пуста.", nameof(query));
        return books.Where(b => b.Title.Contains(term, StringComparison.OrdinalIgnoreCase)
            || b.Author.Contains(term, StringComparison.OrdinalIgnoreCase)).OrderBy(b => b.Id).ToArray();
    }

    public (int Total, int Read, int Unread) Statistics()
    {
        int read = books.Count(b => b.IsRead);
        return (books.Count, read, books.Count - read);
    }
}
