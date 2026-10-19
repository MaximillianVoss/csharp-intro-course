namespace Course.Lesson16;

public static class Task13
{
    public sealed class FlagDto { public bool? Enabled{get;set;} }
    public static bool ReadFlag(string json)
    {
        var dto=System.Text.Json.JsonSerializer.Deserialize<FlagDto>(json,new System.Text.Json.JsonSerializerOptions{PropertyNameCaseInsensitive=true});
        return dto?.Enabled??throw new InvalidDataException("Обязателен enabled.");
    }
    public static void Run()
    {
        Check.True(!ReadFlag("{\"enabled\":false}"));Check.True(ReadFlag("{\"ENABLED\":true}"));
        foreach(string json in new[]{"{}","null","{\"enabled\":null}"})Check.Throws<InvalidDataException>(()=>ReadFlag(json));
        Check.Throws<System.Text.Json.JsonException>(()=>ReadFlag("{\"enabled\":\"yes\"}"));
        Check.Throws<ArgumentNullException>(()=>ReadFlag(null!));
        foreach(string json in new[]{"{broken","[]","true","{\"enabled\":1}"})
            Check.Throws<System.Text.Json.JsonException>(()=>ReadFlag(json));
        Check.True(ReadFlag("{\"extra\":{\"any\":[]},\"EnAbLeD\":true}"));
    }
}
