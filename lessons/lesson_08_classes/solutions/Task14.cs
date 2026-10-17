namespace Course.Lesson08;

public static class Task14
{
    public sealed class Fraction
    {
        public int Numerator { get; }
        public int Denominator { get; }
        private Fraction(int numerator, int denominator) { Numerator = numerator; Denominator = denominator; }
        public static Fraction? Create(int numerator, int denominator)
        {
            if (denominator == 0) return null;
            if (denominator < 0) { numerator = -numerator; denominator = -denominator; }
            int a = Math.Abs(numerator), b = denominator;
            while (b != 0) { int remainder = a % b; a = b; b = remainder; }
            return new Fraction(numerator / a, denominator / a);
        }
    }
    public static void Run()
    {
        Fraction a = Fraction.Create(6, -8)!; Check.Equal(-3, a.Numerator); Check.Equal(4, a.Denominator);
        Fraction zero = Fraction.Create(0, -5)!; Check.Equal(0, zero.Numerator); Check.Equal(1, zero.Denominator);
        Check.Equal(3, Fraction.Create(-6, -8)!.Numerator); Check.Equal(7, Fraction.Create(3, 7)!.Denominator);
        Check.True(Fraction.Create(1, 0) is null);
    }
}
