namespace Course.Lesson07;

public static class Task08
{
    public static bool TryReserve(Dictionary<string, int> stock, Dictionary<string, int> request)
    {
        foreach (var pair in request)
            if (pair.Value > 0 && (!stock.TryGetValue(pair.Key, out int count) || count < pair.Value)) return false;
        foreach (var pair in request) if (pair.Value > 0) stock[pair.Key] -= pair.Value;
        return true;
    }
    public static void Run()
    {
        Dictionary<string, int> stock = new(StringComparer.Ordinal) { ["A"] = 5, ["B"] = 2 };
        Check.True(!TryReserve(stock, new(StringComparer.Ordinal) { ["A"] = 3, ["B"] = 3 }));
        Check.Equal(5, stock["A"]); Check.Equal(2, stock["B"]);
        Check.True(TryReserve(stock, new(StringComparer.Ordinal) { ["A"] = 3 })); Check.Equal(2, stock["A"]);
        Check.True(!TryReserve(stock, new(StringComparer.Ordinal) { ["C"] = 1 }));
        Check.True(TryReserve(stock, new(StringComparer.Ordinal) { ["C"] = 0 })); Check.True(!stock.ContainsKey("C"));
        Check.True(TryReserve(stock, new(StringComparer.Ordinal)));
    }
}
