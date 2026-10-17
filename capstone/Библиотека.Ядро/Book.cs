namespace Course.Library;

public sealed record Book
{
    public int Id { get; }
    public string Title { get; }
    public string Author { get; }
    public int Year { get; }
    public bool IsRead { get; }

    public Book(int id, string title, string author, int year, bool isRead = false)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
        Title = ValidateText(title, 120, nameof(title));
        Author = ValidateText(author, 80, nameof(author));
        if (year is < 1450 or > 2100) throw new ArgumentOutOfRangeException(nameof(year));
        Id = id; Year = year; IsRead = isRead;
    }

    private static string ValidateText(string value, int limit, string parameter)
    {
        ArgumentNullException.ThrowIfNull(value, parameter);
        string trimmed = value.Trim();
        if (trimmed.Length == 0 || trimmed.Length > limit || trimmed.Any(char.IsControl))
            throw new ArgumentException($"Текст должен содержать 1–{limit} символов без управляющих знаков.", parameter);
        for (int i = 0; i < trimmed.Length; i++)
        {
            if (char.IsHighSurrogate(trimmed[i]))
            {
                if (i + 1 >= trimmed.Length || !char.IsLowSurrogate(trimmed[i + 1]))
                    throw new ArgumentException("Текст содержит непарный суррогат UTF-16.", parameter);
                i++;
            }
            else if (char.IsLowSurrogate(trimmed[i]))
                throw new ArgumentException("Текст содержит непарный суррогат UTF-16.", parameter);
        }
        return trimmed;
    }

    public Book AsRead() => IsRead ? this : new Book(Id, Title, Author, Year, true);
}
