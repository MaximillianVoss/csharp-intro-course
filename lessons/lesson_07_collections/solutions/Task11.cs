namespace Course.Lesson07;

public static class Task11
{
    public static Dictionary<string, List<int>> PositionsByTag(string[] tags)
    {
        Dictionary<string, List<int>> result = new(StringComparer.Ordinal);
        for (int i = 0; i < tags.Length; i++)
        {
            if (!result.TryGetValue(tags[i], out List<int>? positions))
            {
                positions = new List<int>(); result.Add(tags[i], positions);
            }
            positions.Add(i);
        }
        return result;
    }
    public static void Run()
    {
        Dictionary<string, List<int>> result = PositionsByTag(new[] { "red", "blue", "red", "Red" });
        Check.SequenceEqual(new[] { 0, 2 }, result["red"]); Check.SequenceEqual(new[] { 3 }, result["Red"]);
        result["red"].Add(99); Check.SequenceEqual(new[] { 1 }, result["blue"]);
        Check.Equal(0, PositionsByTag(Array.Empty<string>()).Count);
    }
}
