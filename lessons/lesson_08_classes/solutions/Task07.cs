namespace Course.Lesson08;

public static class Task07
{
    public sealed record CartLine(string Code, decimal UnitPrice, int Quantity)
    {
        public decimal Total => UnitPrice * Quantity;
    }
    public sealed class Cart
    {
        private readonly List<CartLine> lines = new();
        public int Count => lines.Count;
        public decimal Total { get { decimal total = 0; foreach (CartLine line in lines) total += line.Total; return total; } }
        public bool TryAdd(string? code, decimal price, int quantity)
        {
            if (string.IsNullOrWhiteSpace(code) || price < 0 || price > 1_000_000 || quantity < 1 || quantity > 1000 || lines.Count == 1000) return false;
            lines.Add(new CartLine(code, price, quantity)); return true;
        }
    }
    public static void Run()
    {
        Cart cart = new(); Check.Equal(0m, cart.Total);
        Check.True(cart.TryAdd("A", 2.50m, 2)); Check.True(cart.TryAdd("A", 2.50m, 1));
        Check.Equal(2, cart.Count); Check.Equal(7.50m, cart.Total);
        Check.True(!cart.TryAdd(null, 1, 1)); Check.True(!cart.TryAdd(" ", 1, 1));
        Check.True(!cart.TryAdd("B", 1, 0)); Check.Equal(2, cart.Count);
        Check.True(cart.TryAdd("free", 0, 1)); Check.Equal(7.50m, cart.Total);
    }
}
