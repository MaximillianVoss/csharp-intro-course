using Course;

namespace Course.Lesson14;

public static class Task13
{
    public sealed record Customer(int Id); public sealed record Purchase(int CustomerId, decimal Amount); public sealed record Summary(int CustomerId, int Orders, decimal Total);
    public static Summary[] CustomerTotals(Customer[] customers, Purchase[] purchases)
    {
        ArgumentNullException.ThrowIfNull(customers); ArgumentNullException.ThrowIfNull(purchases);
        if (customers.Any(x => x is null || x.Id <= 0) || purchases.Any(x => x is null || x.CustomerId <= 0 || x.Amount < 0) || customers.Select(x => x.Id).Distinct().Count() != customers.Length) throw new ArgumentException("Data.");
        return customers.GroupJoin(purchases, c => c.Id, p => p.CustomerId, (c, group) => new Summary(c.Id, group.Count(), group.Sum(p => p.Amount))).OrderBy(x => x.CustomerId).ToArray();
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { new Summary(1, 2, 4m), new Summary(2, 0, 0m) }, CustomerTotals(new[] { new Customer(2), new Customer(1) }, new[] { new Purchase(1, 2.5m), new Purchase(1, 1.5m), new Purchase(9, 7m) }));
        Check.Equal(0, CustomerTotals(Array.Empty<Customer>(), Array.Empty<Purchase>()).Length);
        Check.Throws<ArgumentException>(() => CustomerTotals(new[] { new Customer(1), new Customer(1) }, Array.Empty<Purchase>()));
        Check.Throws<ArgumentException>(() => CustomerTotals(new[] { new Customer(1) }, new[] { new Purchase(1, -1) }));
        Check.Throws<OverflowException>(() => CustomerTotals(new[] { new Customer(1) }, new[] { new Purchase(1, decimal.MaxValue), new Purchase(1, decimal.MaxValue) }));
    }
}
