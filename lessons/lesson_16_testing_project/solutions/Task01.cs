namespace Course.Lesson16;

public static class Task01
{
    public static bool IsPassing(int score)
    {
        if (score is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(score));
        return score >= 60;
    }
    public static void Run()
    {
        Check.True(!IsPassing(0)); Check.True(!IsPassing(59));
        Check.True(IsPassing(60)); Check.True(IsPassing(100));
        Check.Throws<ArgumentOutOfRangeException>(() => IsPassing(-1));
        Check.Throws<ArgumentOutOfRangeException>(() => IsPassing(101));
        Func<int,bool> mutant = score => score > 60;
        Check.True(mutant(60) != IsPassing(60));
    }
}
