using Course;
using System.Text;
string directory = Path.Combine(Path.GetTempPath(), "csharp-course-example-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(directory);
try
{
    string path = Path.Combine(directory, "numbers.txt");
    File.WriteAllText(path, "10\nwrong\n20\n", new UTF8Encoding(false, true));
    long sum = 0; int errors = 0;
    using (StreamReader reader = new(path, Encoding.UTF8))
    {
        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            if (int.TryParse(line, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int value)) sum += value;
            else errors++;
        }
    }
    Check.Equal(30L, sum); Check.Equal(1, errors);
    Console.WriteLine($"Сумма: {sum}; ошибок данных: {errors}; ресурс закрыт до очистки.");
}
finally { Directory.Delete(directory, true); }
Check.True(!Directory.Exists(directory));
