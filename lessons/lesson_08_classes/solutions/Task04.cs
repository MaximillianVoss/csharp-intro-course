namespace Course.Lesson08;

public static class Task04
{
    public sealed class Wallet
    {
        public decimal Balance { get; private set; }
        public Wallet(decimal initialBalance) { Balance = initialBalance; }
        public bool TrySpend(decimal amount)
        {
            if (amount <= 0 || amount > Balance) return false;
            Balance -= amount; return true;
        }
    }
    public static void Run()
    {
        Wallet wallet = new(10.50m); Check.True(wallet.TrySpend(3.25m)); Check.Equal(7.25m, wallet.Balance);
        Check.True(!wallet.TrySpend(8)); Check.Equal(7.25m, wallet.Balance);
        Check.True(!wallet.TrySpend(0)); Check.True(!wallet.TrySpend(-2));
        Check.True(wallet.TrySpend(7.25m)); Check.Equal(0m, wallet.Balance); Check.True(!wallet.TrySpend(1));
    }
}
