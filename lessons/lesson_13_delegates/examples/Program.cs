using Course;

int threshold = 3;
Predicate<string> longEnough = text => text.Length >= threshold;
var values = new[] { "a", "abcd", "xyz" }; var selected = new List<string>();
foreach (var value in values) if (longEnough(value)) selected.Add(value);
Check.SequenceEqual(new[] { "abcd", "xyz" }, selected);
threshold = 4; Check.True(!longEnough("xyz"));
Console.WriteLine(string.Join(", ", selected));
var signal = new Signal(); int observed = 0; Action<int> listener = value => observed = value;
signal.Changed += listener; signal.Publish(7); Check.Equal(7, observed);
signal.Changed -= listener; signal.Publish(9); Check.Equal(7, observed);
Console.WriteLine("Захват переменной и отписка проверены.");

public sealed class Signal
{
    public event Action<int>? Changed;
    public void Publish(int value) => Changed?.Invoke(value);
}
