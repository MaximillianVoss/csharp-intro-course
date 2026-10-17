namespace Course.Lesson09;

public static class Task06
{
    public abstract class ShippingPolicy
    {
        protected abstract decimal BaseFee { get; }
        protected abstract decimal Minimum { get; }
        protected abstract decimal VariableCost(decimal kilograms);
        public decimal Quote(decimal kilograms) => Math.Max(Minimum, BaseFee + VariableCost(kilograms));
    }
    public sealed class Ground : ShippingPolicy
    {
        protected override decimal BaseFee => 20;
        protected override decimal Minimum => 50;
        protected override decimal VariableCost(decimal kilograms) => 5 * kilograms;
    }
    public sealed class Air : ShippingPolicy
    {
        protected override decimal BaseFee => 100;
        protected override decimal Minimum => 100;
        protected override decimal VariableCost(decimal kilograms) => 20 * kilograms;
    }
    public static void Run()
    {
        ShippingPolicy ground = new Ground(); ShippingPolicy air = new Air();
        Check.Equal(50m, ground.Quote(2)); Check.Equal(70m, ground.Quote(10));
        Check.Equal(140m, air.Quote(2)); Check.Equal(100m, air.Quote(0));
    }
}
