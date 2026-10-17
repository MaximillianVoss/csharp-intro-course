using Course;

namespace Course.Lesson14;

public static class Task07
{
    public sealed record Customer(int Id, string Name); public sealed record Order(int Id, int CustomerId); public sealed record Row(int OrderId, string CustomerName);
    public static Row[] JoinOrders(IEnumerable<Customer> customers, IEnumerable<Order> orders)
    {
        ArgumentNullException.ThrowIfNull(customers); ArgumentNullException.ThrowIfNull(orders); var cs = customers.ToArray(); var os = orders.ToArray();
        if (cs.Any(c => c is null || c.Id <= 0 || string.IsNullOrWhiteSpace(c.Name)) || os.Any(o => o is null || o.Id <= 0 || o.CustomerId <= 0)) throw new ArgumentException("Data.");
        if (cs.Select(c => c.Id).Distinct().Count() != cs.Length || os.Select(o => o.Id).Distinct().Count() != os.Length) throw new ArgumentException("Duplicate.");
        return os.Join(cs, o => o.CustomerId, c => c.Id, (o, c) => new Row(o.Id, c.Name)).OrderBy(r => r.OrderId).ToArray();
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { new Row(2, "A"), new Row(7, "A") }, JoinOrders(new[] { new Customer(1, "A") }, new[] { new Order(7, 1), new Order(3, 99), new Order(2, 1) }));
        Check.Equal(0, JoinOrders(Array.Empty<Customer>(), new[] { new Order(1, 2) }).Length);
        Check.Throws<ArgumentException>(() => JoinOrders(new[] { new Customer(1, "A"), new Customer(1, "B") }, Array.Empty<Order>()));
        Check.Throws<ArgumentException>(() => JoinOrders(Array.Empty<Customer>(), new[] { new Order(1, 2), new Order(1, 3) }));
        Check.Throws<ArgumentNullException>(() => JoinOrders(null!, Array.Empty<Order>()));
    }
}
