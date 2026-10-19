namespace Course.Lesson08;

public static class Task06
{
    public sealed record Product(string Code, string Title, decimal Price);
    public static Product? Reprice(Product original, decimal newPrice)
    {
        if (newPrice < 0 || newPrice > 1_000_000) return null;
        return original with { Price = newPrice };
    }
    public static void Run()
    {
        Product original = new("P1", "Книга", 100);
        Product? changed = Reprice(original, 120);
        Check.True(changed is not null); Check.Equal(120m, changed!.Price); Check.Equal(100m, original.Price);
        Check.Equal(new Product("P1", "Книга", 120m), changed);
        Check.True(Reprice(original, -1) is null); Check.True(Reprice(original, 1_000_001) is null);
        Product? same = Reprice(original, 100); Check.Equal(original, same); Check.True(!ReferenceEquals(original, same));
        Check.Equal(0m, Reprice(original, 0)!.Price);
    }
}
