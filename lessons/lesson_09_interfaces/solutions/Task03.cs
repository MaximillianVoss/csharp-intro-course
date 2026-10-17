namespace Course.Lesson09;

public static class Task03
{
    public interface ITaxPolicy { decimal Tax(decimal net); }
    public sealed class NoTax : ITaxPolicy { public decimal Tax(decimal net) => 0; }
    public sealed class RateTax : ITaxPolicy
    {
        private readonly decimal rate;
        public RateTax(decimal rate) { this.rate = rate; }
        public decimal Tax(decimal net) => net * rate;
    }
    public sealed class Invoice
    {
        private readonly ITaxPolicy policy;
        public Invoice(ITaxPolicy policy) { this.policy = policy; }
        public decimal Total(decimal net) => net + policy.Tax(net);
    }
    public static void Run()
    {
        Check.Equal(120m, new Invoice(new RateTax(0.2m)).Total(100));
        Check.Equal(100m, new Invoice(new NoTax()).Total(100));
        Check.Equal(0m, new Invoice(new RateTax(1)).Total(0));
        Check.Equal(1.65m, new Invoice(new RateTax(0.1m)).Total(1.5m));
    }
}
