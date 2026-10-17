namespace Course.Lesson08;

public static class Task08
{
    public sealed class Temperature
    {
        public decimal Kelvin { get; private set; }
        public decimal Celsius => Kelvin - 273.15m;
        public Temperature(decimal kelvin) { Kelvin = kelvin; }
        public bool TrySetCelsius(decimal celsius)
        {
            if (celsius < -273.15m || celsius > 9726.85m) return false;
            Kelvin = celsius + 273.15m; return true;
        }
    }
    public static void Run()
    {
        Temperature value = new(273.15m); Check.Equal(0m, value.Celsius);
        Check.True(value.TrySetCelsius(-273.15m)); Check.Equal(0m, value.Kelvin);
        Check.True(!value.TrySetCelsius(-273.16m)); Check.Equal(0m, value.Kelvin);
        Check.True(value.TrySetCelsius(9726.85m)); Check.Equal(10000m, value.Kelvin);
        Check.True(!value.TrySetCelsius(decimal.MaxValue)); Check.Equal(10000m, value.Kelvin);
    }
}
