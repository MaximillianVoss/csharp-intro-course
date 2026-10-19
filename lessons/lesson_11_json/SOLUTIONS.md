# Урок 11. JSON и контракты данных: решения

Сначала выполните свою попытку по TASKS.md. Здесь приведены ответы.
Каждое решение запускается по ID; Run содержит исполняемые проверки.

<a id="11-01"></a>
## 11-01. Карточка в JSON

Использовать сериализатор после валидации объекта.

**Ход решения**

Сначала защищаем границу метода: null не является карточкой. Проверяем имя и количество, затем передаём объект сериализатору. Проверка результата через JsonDocument сравнивает значения, поэтому не зависит от порядка свойств и Unicode-экранирования. Работа занимает O(L), где L — длина имени и результата. Строка C# может содержать непарный суррогат; до Serialize такой Name отклоняется, иначе writer способен заменить его U+FFFD и нарушить сохранение имени.

**Частая ошибка**

Склейка кавычек вручную ломает имена с переносом строки или кавычкой.

Полный исходник: [solutions/Task01.cs](solutions/Task01.cs).

```csharp
using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task01
{
    public sealed class Card { public string Name { get; set; } = ""; public int Count { get; set; } }
    public static string Serialize(Card card)
    {
        ArgumentNullException.ThrowIfNull(card);
        if (string.IsNullOrWhiteSpace(card.Name) || card.Count < 0 || !WellFormed(card.Name)) throw new ArgumentException("Invalid card.");
        return JsonSerializer.Serialize(card);
    }
    private static bool WellFormed(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsHighSurrogate(text[i])) { if (i + 1 == text.Length || !char.IsLowSurrogate(text[i + 1])) return false; i++; }
            else if (char.IsLowSurrogate(text[i])) return false;
        }
        return true;
    }
    public static void Run()
    {
        using var doc = JsonDocument.Parse(Serialize(new Card { Name = "чай", Count = 2 }));
        Check.Equal("чай", doc.RootElement.GetProperty("Name").GetString());
        Check.Equal(2, doc.RootElement.GetProperty("Count").GetInt32());
        var text = "a\"b\n";
        Check.Equal(text, JsonSerializer.Deserialize<Card>(Serialize(new Card { Name = text }))!.Name);
        Check.Throws<ArgumentException>(() => Serialize(new Card { Name = " " }));
        Check.Throws<ArgumentException>(() => Serialize(new Card { Name = "a", Count = -1 }));
        Check.Throws<ArgumentNullException>(() => Serialize(null!));
        var unicode = " до\uD83D\uDE00после ";
        Check.Equal(unicode, JsonSerializer.Deserialize<Card>(Serialize(new Card { Name = unicode }))!.Name);
        Check.Throws<ArgumentException>(() => Serialize(new Card { Name = "\uD800" }));
        Check.Throws<ArgumentException>(() => Serialize(new Card { Name = "\uDC00" }));
    }
}
```

**Исполняемые проверки**

- Русское имя и количество
- Кавычка и перенос строки восстанавливаются
- Пустое имя, отрицательное количество и null отклонены
- Имя с emoji и внешними пробелами сохраняется точно; одиночные high/low суррогаты отклонены

<a id="11-02"></a>
## 11-02. Чтение показания

Получить nullable DTO и явно проверить успешность чтения.

**Ход решения**

Deserialize может вернуть null даже для ссылочной модели. После проверки корня проверяем диапазон и единицу: синтаксически верный объект ещё не является корректным показанием. Не перехватываем JsonException с подменой на нулевое показание: это скрыло бы порчу данных. Время O(L).

**Частая ошибка**

Оператор ! подавляет предупреждение, но не проверяет JSON-корень null.

Полный исходник: [solutions/Task02.cs](solutions/Task02.cs).

```csharp
using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task02
{
    public sealed class Reading { public decimal Value { get; set; } public string? Unit { get; set; } }
    public static Reading Read(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        var value = JsonSerializer.Deserialize<Reading>(json);
        if (value is null || value.Value < -1000 || value.Value > 1000 || (value.Unit != "C" && value.Unit != "F"))
            throw new JsonException("Invalid reading.");
        return value;
    }
    public static void Run()
    {
        Check.Equal(12.5m, Read("{\"Value\":12.5,\"Unit\":\"C\"}").Value);
        Check.Equal(-1000m, Read("{\"Value\":-1000,\"Unit\":\"F\"}").Value);
        Check.Equal(1000m, Read("{\"Value\":1000,\"Unit\":\"C\"}").Value);
        Check.Equal(0m, Read("{\"Unit\":\"C\",\"Extra\":true}").Value);
        foreach (var json in new[] { "null", "{", "{\"Unit\":\"K\"}", "{\"Value\":1001,\"Unit\":\"C\"}", "{\"Value\":\"1\",\"Unit\":\"C\"}" })
            Check.Throws<JsonException>(() => Read(json));
        Check.Throws<ArgumentNullException>(() => Read(null!));
        Check.Throws<JsonException>(() => Read("{\"Value\":-1000.01,\"Unit\":\"C\"}"));
        Check.Throws<JsonException>(() => Read("[]"));
    }
}
```

**Исполняемые проверки**

- Дробное и граничные показания
- Отсутствующее Value равно 0
- Ошибочные единица, диапазон, синтаксис, тип и null-корень
- Значение чуть ниже -1000 и массив вместо Reading отклонены

<a id="11-03"></a>
## 11-03. Что действительно прочитано

Стандартное чтение сохраняет Limit=9 и Counter=4.

**Ход решения**

Имя limit не совпадает с Limit при стандартных опциях. Counter — поле, а поля по умолчанию исключены. Поэтому оба инициализатора остаются: вывод 9,4. Контрольный объект с Limit в верном регистре меняет только свойство. Так проверка отличает две независимые причины, а не просто запоминает строку ответа.

**Частая ошибка**

Нельзя переносить web-настройки ASP.NET на вызов JsonSerializer без опций.

Полный исходник: [solutions/Task03.cs](solutions/Task03.cs).

```csharp
using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task03
{
    public sealed class Settings { public int Limit { get; set; } = 9; public int Counter = 4; }
    public static (int Limit, int Counter) Observe(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        var s = JsonSerializer.Deserialize<Settings>(json) ?? throw new JsonException("Null root.");
        return (s.Limit, s.Counter);
    }
    public static void Run()
    {
        var s = JsonSerializer.Deserialize<Settings>("{\"limit\":2,\"Counter\":7}")!;
        Check.Equal((9, 4), (s.Limit, s.Counter));
        Check.Equal((9, 4), Observe("{\"limit\":2,\"Counter\":7}"));
        Check.Equal((2, 4), Observe("{\"Limit\":2,\"Counter\":7}"));
        Check.Equal((9, 4), Observe("{}"));
        Check.Throws<JsonException>(() => Observe("null"));
        Check.Throws<ArgumentNullException>(() => Observe(null!));
    }
}
```

**Исполняемые проверки**

- Точный исходный фрагмент даёт 9,4
- Верный регистр меняет Limit
- Пустой объект сохраняет значения; null-корень отклонён

<a id="11-04"></a>
## 11-04. Исправление времени жизни JsonElement

Вернуть document.RootElement.Clone() до Dispose документа.

**Ход решения**

RootElement не владеет самостоятельным снимком дерева: его чтение обращается к памяти JsonDocument. Using освобождает документ до того, как вызывающий читает свойство, поэтому исходный вариант даёт ObjectDisposedException. Clone создаёт самостоятельное представление, которое остаётся пригодным после Dispose исходного документа. Тесты читают и объект, и массив уже за границей метода. Время и дополнительная память O(L) для клонирования дерева. Clone отделяет память, но не исправляет некорректные Unicode escape. Поэтому дерево предварительно проверяется чтением всех строк и имён; ошибка декодирования переводится в JsonException.

**Частая ошибка**

Удаление using убирает симптом ценой неосвобождённого документа; копирование структуры JsonElement присваиванием не копирует его данные.

Полный исходник: [solutions/Task04.cs](solutions/Task04.cs).

```csharp
using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task04
{
    public static JsonElement ReadDetached(string json)
    {
        ArgumentNullException.ThrowIfNull(json); using var document = JsonDocument.Parse(json);
        try { ValidateStrings(document.RootElement); }
        catch (InvalidOperationException error) { throw new JsonException("Invalid Unicode in JSON.", error); }
        return document.RootElement.Clone();
    }
    private static JsonElement Broken(string json) { using var document = JsonDocument.Parse(json); return document.RootElement; }
    private static void ValidateStrings(JsonElement value)
    {
        if (value.ValueKind == JsonValueKind.String) { _ = value.GetString(); }
        else if (value.ValueKind == JsonValueKind.Array)
            foreach (var item in value.EnumerateArray()) ValidateStrings(item);
        else if (value.ValueKind == JsonValueKind.Object)
            foreach (var property in value.EnumerateObject()) { _ = property.Name; ValidateStrings(property.Value); }
    }
    public static void Run()
    {
        var broken = Broken("{\"Name\":\"Ada\"}"); Check.Throws<ObjectDisposedException>(() => broken.GetProperty("Name").GetString());
        var element = ReadDetached("{\"Name\":\"Ada\",\"Nested\":{\"Count\":2}}"); Check.Equal("Ada", element.GetProperty("Name").GetString()); Check.Equal(2, element.GetProperty("Nested").GetProperty("Count").GetInt32());
        var array = ReadDetached("[1,2]"); Check.Equal(2, array.GetArrayLength()); Check.Equal(2, array[1].GetInt32()); Check.Equal(JsonValueKind.Null, ReadDetached("null").ValueKind);
        Check.Throws<JsonException>(() => ReadDetached("{")); Check.Throws<ArgumentNullException>(() => ReadDetached(null!));
        Check.Throws<JsonException>(() => ReadDetached("{\"nested\":[\"\\uD800\"]}"));
        Check.Throws<JsonException>(() => ReadDetached("{\"\\uDC00\":1}"));
        Check.Equal("\uD83D\uDE00", ReadDetached("\"\\uD83D\\uDE00\"").GetString());
    }
}
```

**Исполняемые проверки**

- Исходный вариант реально даёт ObjectDisposedException
- Исправленный объект и вложенные значения читаются после возврата
- Массив и JSON null
- Неверный JSON и null аргумент
- Вложенный непарный escape и некорректное имя свойства отклонены; корректная emoji-пара доступна после Clone

<a id="11-05"></a>
## 11-05. Нет поля и поле null

Разобрать документ и явно различить отсутствие, null и строку.

**Ход решения**

TryGetProperty отвечает за наличие ключа. ValueKind отвечает за тип значения присутствующего ключа. Вызов GetString без этой проверки привёл бы к исключению другого уровня или смешению состояний. Возвращаем самостоятельную string, которая переживает Dispose документа. Время O(L). После проверки ValueKind также возможна ошибка декодирования непарного escape; она не считается отсутствием/null и сообщается как JsonException.

**Частая ошибка**

Значение null и отсутствие ключа часто имеют разный смысл при обновлении данных.

Полный исходник: [solutions/Task05.cs](solutions/Task05.cs).

```csharp
using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task05
{
    public static string Label(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        if (root.ValueKind != JsonValueKind.Object) throw new JsonException("Object required.");
        if (!root.TryGetProperty("label", out var value)) return "нет";
        if (value.ValueKind == JsonValueKind.Null) return "пусто";
        if (value.ValueKind != JsonValueKind.String) throw new JsonException("String required.");
        return Decode(value)!;
    }
    private static string? Decode(JsonElement value)
    {
        try { return value.GetString(); }
        catch (InvalidOperationException error) { throw new JsonException("Invalid Unicode string.", error); }
    }
    public static void Run()
    {
        Check.Equal("нет", Label("{}")); Check.Equal("пусто", Label("{\"label\":null}"));
        Check.Equal("", Label("{\"label\":\"\"}")); Check.Equal("x", Label("{\"label\":\"x\"}"));
        Check.Throws<JsonException>(() => Label("{\"label\":4}")); Check.Throws<JsonException>(() => Label("[]"));
        Check.Throws<ArgumentNullException>(() => Label(null!));
        Check.Throws<JsonException>(() => Label("{\"label\":\"\\uDC00\"}"));
        Check.Equal("\uD83D\uDE00", Label("{\"label\":\"\\uD83D\\uDE00\"}"));
    }
}
```

**Исполняемые проверки**

- Все три состояния и непустая строка
- Число вместо строки и массив вместо объекта отвергнуты
- null аргумент отклонён
- Невалидный escape label отклонён JsonException, корректная emoji-пара сохранена

<a id="11-06"></a>
## 11-06. Настройки в собственном файле

Отделить отсутствие файла от ошибки чтения существующего файла.

**Ход решения**

Значение по умолчанию допустимо только в явно указанной ситуации отсутствия. Существующий JSON проверяем так же, как ввод извне. Для сохранения сначала валидируем, затем пишем текст. Это простое сохранение не обещает атомарность при сбое диска. Тест создаёт собственный каталог, поэтому не затрагивает пользовательские настройки.

**Частая ошибка**

catch(Exception) с возвратом настроек по умолчанию скрывает отказ доступа и порчу JSON.

Полный исходник: [solutions/Task06.cs](solutions/Task06.cs).

```csharp
using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task06
{
    public sealed class Settings { public int Volume { get; set; } = 50; }
    private static void PathValid(string path) { if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path required."); }
    public static void Save(string path, Settings settings)
    {
        PathValid(path); ArgumentNullException.ThrowIfNull(settings);
        if (settings.Volume < 0 || settings.Volume > 100) throw new ArgumentException("Volume.");
        File.WriteAllText(path, JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
    }
    public static Settings Load(string path)
    {
        PathValid(path);
        string json;
        try { json = File.ReadAllText(path); } catch (FileNotFoundException) { return new Settings(); }
        var s = JsonSerializer.Deserialize<Settings>(json);
        if (s is null || s.Volume < 0 || s.Volume > 100) throw new JsonException("Volume.");
        return s;
    }
    public static void Run()
    {
        var dir = Path.Combine(Path.GetTempPath(), "csharp-json-" + Guid.NewGuid()); Directory.CreateDirectory(dir);
        try
        {
            var path = Path.Combine(dir, "settings.json"); Check.Equal(50, Load(path).Volume);
            Save(path, new Settings { Volume = 0 }); Check.Equal(0, Load(path).Volume);
            Check.True(File.ReadAllText(path).Contains('\n'));
            Save(path, new Settings { Volume = 100 }); Check.Equal(100, Load(path).Volume);
            File.WriteAllText(path, "{"); Check.Throws<JsonException>(() => Load(path));
            File.WriteAllText(path, "{\"Volume\":101}"); Check.Throws<JsonException>(() => Load(path));
            Check.Throws<ArgumentException>(() => Save(path, new Settings { Volume = -1 }));
            Check.Throws<ArgumentNullException>(() => Save(path, null!)); Check.Throws<ArgumentException>(() => Load(""));
            Check.Throws<DirectoryNotFoundException>(() => Load(Path.Combine(dir, "absent-directory", "file.json")));
        }
        finally { Directory.Delete(dir, true); }
        // Independently exercise the promised corrupt-file cases in a fresh directory.
        var more = Path.Combine(Path.GetTempPath(), "csharp-json-review-" + Guid.NewGuid()); Directory.CreateDirectory(more);
        try
        {
            var p = Path.Combine(more, "data.json");
            foreach (var json in new[] { "", "null", "{\"Volume\":-1}" }) { File.WriteAllText(p, json); Check.Throws<JsonException>(() => Load(p)); }
        }
        finally { Directory.Delete(more, true); }
    }
}
```

**Исполняемые проверки**

- Отсутствующий файл
- Крайние громкости 0 и 100 проходят round trip
- Испорченный файл и диапазон отвергнуты; путь проверен
- Пустой файл, null-корень и Volume=-1 не превращаются в настройки по умолчанию

<a id="11-07"></a>
## 11-07. Сумма вложенного заказа

Проверить каждый уровень объекта до доступа и суммирования.

**Ход решения**

У коллекции и её элементов независимая возможность быть null. Вводим nullable модель честно и проверяем её в цикле. Пустой заказ имеет нулевую сумму. Decimal сохраняет денежные десятичные значения; checked защищает накопление очень большого массива. Время O(n), память O(n) на DTO.

**Частая ошибка**

Нельзя валидировать лишь корневой Order и считать все его вложенные объекты безопасными.

Полный исходник: [solutions/Task07.cs](solutions/Task07.cs).

```csharp
using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task07
{
    public sealed class Order { public Line?[]? Lines { get; set; } }
    public sealed class Line { public int Quantity { get; set; } public decimal Price { get; set; } }
    public static decimal Total(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        var order = JsonSerializer.Deserialize<Order>(json);
        if (order?.Lines is null) throw new JsonException("Lines required.");
        decimal total = 0;
        foreach (var line in order.Lines)
        {
            if (line is null || line.Quantity < 1 || line.Quantity > 1000 || line.Price < 0 || line.Price > 1000000) throw new JsonException("Invalid line.");
            total = checked(total + line.Quantity * line.Price);
        }
        return total;
    }
    public static void Run()
    {
        Check.Equal(2.50m, Total("{\"Lines\":[{\"Quantity\":2,\"Price\":1.25},{\"Quantity\":1,\"Price\":0}]}"));
        Check.Equal(0m, Total("{\"Lines\":[]}"));
        Check.Equal(1000000000m, Total("{\"Lines\":[{\"Quantity\":1000,\"Price\":1000000}]}"));
        foreach (var json in new[] { "{}", "{\"Lines\":null}", "{\"Lines\":[null]}", "{\"Lines\":[{\"Quantity\":0}]}", "{\"Lines\":[{\"Quantity\":1,\"Price\":-1}]}" }) Check.Throws<JsonException>(() => Total(json));
        Check.Throws<ArgumentNullException>(() => Total(null!));
    }
}
```

**Исполняемые проверки**

- Две строки и пустой массив
- Граничные Quantity/Price
- null-массив, null-строка, отрицательная цена и нулевое количество отвергнуты

<a id="11-08"></a>
## 11-08. Координаты без DTO

Скопировать числа из DOM в независимую пару.

**Ход решения**

Сначала проверяем Object, затем TryGetProperty, Number и TryGetDouble. Проверка IsFinite нужна для чисел с огромной экспонентой: тип double не охватывает все возможные JSON-числа. После копирования чисел документ можно освободить. На входе с верным синтаксисом неправильный тип всё равно считается ошибкой контракта.

**Частая ошибка**

GetDouble без проверки типа может дать InvalidOperationException вместо предусмотренной JsonException.

Полный исходник: [solutions/Task08.cs](solutions/Task08.cs).

```csharp
using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task08
{
    public static (double Lat, double Lon) ReadPoint(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        double Number(string name, double bound)
        {
            if (root.ValueKind != JsonValueKind.Object || !root.TryGetProperty(name, out var v) || v.ValueKind != JsonValueKind.Number || !v.TryGetDouble(out var n) || !double.IsFinite(n) || n < -bound || n > bound)
                throw new JsonException("Invalid coordinate.");
            return n;
        }
        return (Number("lat", 90), Number("lon", 180));
    }
    public static void Run()
    {
        var p = ReadPoint("{\"lat\":55.75,\"lon\":37.62}"); Check.Near(55.75, p.Lat); Check.Near(37.62, p.Lon);
        Check.Equal((-90d, 180d), ReadPoint("{\"lat\":-90,\"lon\":180}"));
        foreach (var json in new[] { "{}", "[]", "{\"lat\":\"0\",\"lon\":0}", "{\"lat\":91,\"lon\":0}", "{\"lat\":0,\"lon\":1e999}" }) Check.Throws<JsonException>(() => ReadPoint(json));
        Check.Equal((90d, -180d), ReadPoint("{\"lat\":90,\"lon\":-180}"));
        Check.Throws<ArgumentNullException>(() => ReadPoint(null!));
    }
}
```

**Исполняемые проверки**

- Обычная точка и обе границы
- Пропуск, строковое число, массив и огромная экспонента отвергнуты
- Lat=90/Lon=-180 допустимы; null строка отклонена

<a id="11-09"></a>
## 11-09. Убираем склейку JSON

Записать StartObject, WriteString и EndObject через Utf8JsonWriter, затем получить UTF-8 текст.

**Ход решения**

Сначала проверяем UTF-16 по кодовым единицам: старший суррогат потребляет непосредственно следующий младший, одиночный младший или старший без пары отклоняется. Это нужно для точного round trip: writer может заменить некорректный Unicode символом U+FFFD. Корректные пары, включая emoji, остаются данными. Затем Utf8JsonWriter экранирует строковое значение без ручных кавычек. После EndObject и Flush байты MemoryStream декодируются как UTF-8. Проверяем исходный текст после разбора JSON, включая управляющие символы и emoji. Исходная склейка с кавычкой должна реально провалить разбор. Время и дополнительная память O(L).

**Частая ошибка**

Строка C# способна содержать непарный UTF-16-суррогат; успешная запись JSON не доказывает точного сохранения такой строки. Порядок Replace тоже опасен: повторное экранирование меняет данные.

Полный исходник: [solutions/Task09.cs](solutions/Task09.cs).

```csharp
using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task09
{
    public static string BuildMessage(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsHighSurrogate(text[i]))
            {
                if (i + 1 >= text.Length || !char.IsLowSurrogate(text[i + 1])) throw new ArgumentException("Unpaired high surrogate.", nameof(text));
                i++;
            }
            else if (char.IsLowSurrogate(text[i])) throw new ArgumentException("Unpaired low surrogate.", nameof(text));
        }
        using var buffer = new MemoryStream();
        using (var writer = new Utf8JsonWriter(buffer)) { writer.WriteStartObject(); writer.WriteString("text", text); writer.WriteEndObject(); writer.Flush(); }
        return System.Text.Encoding.UTF8.GetString(buffer.ToArray());
    }
    public static void Run()
    {
        foreach (var value in new[] { "", "обычный", "a\"b", "\\\n\t\r\0", "}{\"admin\":true}", "до\uD83D\uDE00после" })
        {
            using var doc = JsonDocument.Parse(BuildMessage(value));
            Check.Equal(value, doc.RootElement.GetProperty("text").GetString());
            int count = 0; foreach (var property in doc.RootElement.EnumerateObject()) count++; Check.Equal(1, count);
        }
        var broken = "{\"text\":\"" + "a\"b" + "\"}";
        Check.Throws<JsonException>(() => { using var doc = JsonDocument.Parse(broken); });
        foreach (var invalid in new[] { "\uD800", "\uDC00", "a\uD800b", "\uDC00\uD800", "\uD800\uD800" })
            Check.Throws<ArgumentException>(() => BuildMessage(invalid));
        Check.Throws<ArgumentNullException>(() => BuildMessage(null!));
    }
}
```

**Исполняемые проверки**

- Пустая строка, кириллица и спецсимволы проходят round trip
- Корректная суррогатная пара emoji сохраняется
- Одиночные старший/младший суррогаты и неверные сочетания отклонены
- Сломанный исходник с кавычкой отличён тестом
- null отвергнут

<a id="11-10"></a>
## 11-10. Тесты сохранения порядка

Сравнить последовательности и отдельно проверить независимость массива.

**Ход решения**

Для массива уже по возрастанию сортировка незаметна. Поэтому выбран вход 3,-1,3,0: он одновременно обнаруживает сортировку, удаление повторов и потерю отрицательных. Новый массив проверяем изменением результата и наблюдением исходного. Копирование сериализацией занимает O(n) времени и дополнительной памяти.

**Частая ошибка**

Assert.Equal на ссылках либо всегда падает, либо доказывает лишь совпадение объекта, а не данные.

Полный исходник: [solutions/Task10.cs](solutions/Task10.cs).

```csharp
using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task10
{
    public static int[] RoundTrip(int[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        return JsonSerializer.Deserialize<int[]>(JsonSerializer.Serialize(values))!;
    }
    private static int[] Broken(int[] values) { var result = RoundTrip(values); Array.Sort(result); return result; }
    public static void Run()
    {
        Check.SequenceEqual(Array.Empty<int>(), RoundTrip(Array.Empty<int>()));
        var input = new[] { 3, -1, 3, 0 }; var output = RoundTrip(input);
        Check.SequenceEqual(input, output); var sorted = Broken(input); bool differs = false; for (int i = 0; i < input.Length; i++) if (input[i] != sorted[i]) differs = true; Check.True(differs);
        output[0] = 99; Check.Equal(3, input[0]);
        Check.Throws<ArgumentNullException>(() => RoundTrip(null!));
    }
}
```

**Исполняемые проверки**

- Пустой массив
- Повторы, отрицательное и порядок
- Исходный массив не изменён
- Ошибочная сортировка реально отличена

<a id="11-11"></a>
## 11-11. Миграция версии настроек

Сначала определить версию, затем проверить ровно её обязательную схему.

**Ход решения**

Внутренняя модель едина, хотя внешних форматов два. Значение Retries=3 является правилом миграции v1, а не универсальным замалчиванием ошибок. В v2 пропущенное retries ошибочно. Это позволяет изменять файловый формат, сохраняя понятный переход и обнаруживая будущую неподдержанную версию.

**Частая ошибка**

Десериализация сразу в новую DTO тихо заполняет отсутствующие старые поля значениями по умолчанию.

Полный исходник: [solutions/Task11.cs](solutions/Task11.cs).

```csharp
using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task11
{
    public sealed record Config(string Name, int Retries);
    public static Config Read(string json)
    {
        ArgumentNullException.ThrowIfNull(json); using var doc = JsonDocument.Parse(json); var root = doc.RootElement;
        if (root.ValueKind != JsonValueKind.Object || !root.TryGetProperty("version", out var v) || !TryGetInt32Safe(v, out var version) || version < 1 || version > 2) throw new JsonException("Version.");
        var key = version == 1 ? "label" : "name";
        if (!root.TryGetProperty(key, out var name) || name.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(Decode(name))) throw new JsonException("Name.");
        int retries = 3;
        if (version == 2 && (!root.TryGetProperty("retries", out var r) || r.ValueKind != JsonValueKind.Number || !r.TryGetInt32(out retries) || retries < 0 || retries > 5)) throw new JsonException("Retries.");
        return new Config(Decode(name)!, retries);
    }
    private static bool TryGetInt32Safe(JsonElement value, out int number)
    {
        number = 0; return value.ValueKind == JsonValueKind.Number && value.TryGetInt32(out number);
    }
    private static string? Decode(JsonElement value)
    {
        try { return value.GetString(); }
        catch (InvalidOperationException error) { throw new JsonException("Invalid Unicode string.", error); }
    }
    public static void Run()
    {
        Check.Equal(new Config("demo", 3), Read("{\"version\":1,\"label\":\"demo\"}"));
        Check.Equal(new Config("x", 0), Read("{\"version\":2,\"name\":\"x\",\"retries\":0}"));
        foreach (var json in new[] { "{\"version\":3}", "{\"version\":2,\"name\":\"x\"}", "{\"version\":\"1\"}", "null", "{\"version\":1,\"label\":\" \"}" }) Check.Throws<JsonException>(() => Read(json));
        Check.Equal(new Config("v2", 5), Read("{\"version\":2,\"name\":\"v2\",\"retries\":5}"));
        Check.Throws<JsonException>(() => Read("{\"version\":2,\"name\":\"x\",\"retries\":6}"));
        Check.Throws<JsonException>(() => Read("{\"version\":1,\"label\":\"\\uD800\"}"));
        Check.Throws<ArgumentNullException>(() => Read(null!));
    }
}
```

**Исполняемые проверки**

- Обе версии и retries=0
- Неизвестная версия, отсутствующее retries и неверные типы отвергнуты
- retries=5 допустим, 6 отклонён; непарный escape имени и null аргумент проверены

<a id="11-12"></a>
## 11-12. Построчный импорт NDJSON

Изолировать обработку каждого документа в одной строке.

**Ход решения**

Номер увеличиваем сразу после каждого успешного ReadLine, включая пустую строку. Ошибка одной записи не должна отменять уже прочитанные независимые записи. Ловим только JsonException: отказ самого TextReader не является ошибкой формата строки. Память O(k+e), время пропорционально общему размеру текста.

**Частая ошибка**

Разбор всего NDJSON как массива JSON неверен: у формата другая граница документа.

Полный исходник: [solutions/Task12.cs](solutions/Task12.cs).

```csharp
using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task12
{
    public sealed class Item { public int Id { get; set; } }
    public sealed class Report { public List<int> Ids { get; } = new(); public List<int> ErrorLines { get; } = new(); }
    public static Report Import(TextReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader); var report = new Report(); int lineNo = 0; string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            lineNo++; if (string.IsNullOrWhiteSpace(line)) continue;
            try
            {
                var item = JsonSerializer.Deserialize<Item>(line);
                if (item is null || item.Id <= 0) throw new JsonException("Id.");
                report.Ids.Add(item.Id);
            }
            catch (JsonException) { report.ErrorLines.Add(lineNo); }
        }
        return report;
    }
    private sealed class FailedReader : TextReader
    {
        public override string? ReadLine() => throw new IOException("Read failure.");
    }
    public static void Run()
    {
        using var reader = new StringReader("{\"Id\":2}\n\n{bad}\n{\"Id\":3}\n{\"Id\":2}\nnull\n{}\n{\"Id\":-1}");
        var report = Import(reader); Check.SequenceEqual(new[] { 2, 3, 2 }, report.Ids); Check.SequenceEqual(new[] { 3, 6, 7, 8 }, report.ErrorLines);
        Check.Equal("", reader.ReadToEnd());
        using var empty = new StringReader(""); Check.Equal(0, Import(empty).Ids.Count);
        Check.Throws<ArgumentNullException>(() => Import(null!));
        using var failed = new FailedReader(); Check.Throws<IOException>(() => Import(failed));
    }
}
```

**Исполняемые проверки**

- Корректные строки до/после ошибки
- Пустая строка сохраняет нумерацию
- Повтор разрешён; отрицательный, отсутствующий Id и null отмечены
- EOF и владение reader
- IOException ReadLine распространяется, не превращаясь в ErrorLines

<a id="11-13"></a>
## 11-13. Статус как строка

Прочитать JSON-строку через DOM, сравнить ровно с двумя именами и отдельно проверить enum при записи.

**Ход решения**

Отключение целых значений у JsonStringEnumConverter не делает набор принимаемых строк точным: конвертер может принять пробелы или комбинации имён. Поэтому Read сначала проверяет ValueKind.String, извлекает декодированное значение и сравнивает его ровно с Pending/Done через OrdinalIgnoreCase. Никаких Trim и Enum.Parse на входе нет. Внешний пробельный синтаксис JSON не относится к содержимому строки и не мешает. Write проверяет Enum.IsDefined, затем сериализует через строковый конвертер в каноническое имя. Проверки покрывают оба состояния, разные регистры, составные имена, внутренние пробелы, числовые токены/строки, null и неопределённое enum-значение. Время чтения O(L).

**Частая ошибка**

allowIntegerValues:false запрещает числовое представление, но сам по себе не запрещает все нежелательные строковые варианты. Trim молча расширяет оговорённый язык входа.

Полный исходник: [solutions/Task13.cs](solutions/Task13.cs).

```csharp
using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task13
{
    public enum Status { Pending, Done }
    private static JsonSerializerOptions Options()
    {
        var options = new JsonSerializerOptions(); options.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false)); return options;
    }
    public static Status Read(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        using var document = JsonDocument.Parse(json);
        if (document.RootElement.ValueKind != JsonValueKind.String) throw new JsonException("Status must be a string.");
        string? name;
        try { name = document.RootElement.GetString(); }
        catch (InvalidOperationException error) { throw new JsonException("Invalid Unicode in status name.", error); }
        if (string.Equals(name, nameof(Status.Pending), StringComparison.OrdinalIgnoreCase)) return Status.Pending;
        if (string.Equals(name, nameof(Status.Done), StringComparison.OrdinalIgnoreCase)) return Status.Done;
        throw new JsonException("Unknown status name.");
    }
    public static string Write(Status status)
    {
        if (!Enum.IsDefined(status)) throw new ArgumentOutOfRangeException(nameof(status));
        return JsonSerializer.Serialize(status, Options());
    }
    public static void Run()
    {
        Check.Equal(Status.Done, Read("\"done\"")); Check.Equal(Status.Pending, Read("\"pEnDiNg\""));
        Check.Equal(Status.Done, Read(" \r\n\"Done\" \t"));
        Check.Equal("\"Pending\"", Write(Status.Pending)); Check.Equal("\"Done\"", Write(Status.Done));
        Check.Equal(Status.Pending, Read(Write(Status.Pending))); Check.Equal(Status.Done, Read(Write(Status.Done)));
        foreach (var json in new[] { "1", "\"1\"", "\"Lost\"", "null", "{}", "[]", "true", "\"Pending, Done\"", "\"Done, Done\"", "\" done \"", "\"Pending \"", "\"\\tDone\"", "\"\"", "\"\\uD800\"", "{" })
            Check.Throws<JsonException>(() => Read(json));
        Check.Throws<ArgumentOutOfRangeException>(() => Write((Status)99));
        Check.Throws<ArgumentNullException>(() => Read(null!));
    }
}
```

**Исполняемые проверки**

- Оба состояния, регистр и внешние пробелы JSON
- Оба составных имени и внутренние пробелы отклонены
- Число, числовая/неизвестная строка, null и неверные JSON-типы отклонены
- Каноническая запись обоих состояний
- Неопределённое enum-значение и null аргумент отклонены

<a id="11-14"></a>
## 11-14. Частичное обновление профиля

Итерировать свойства, отслеживать дубликаты и собирать новую модель.

**Ход решения**

DTO с nullable Bio не позволяет понять, было ли bio вообще передано. DOM сохраняет наличие ключа. Все изменения держим в локальных переменных; если позднее поле ошибочно, исходная модель остаётся прежней. HashSet имён также обнаруживает повтор: по условию последняя запись не должна молча выигрывать. Некорректное декодирование ключа и строки — тоже ошибка patch; она переводится в JsonException до создания нового record.

**Частая ошибка**

GetProperty обычно оставляет незаметной проблему повторных имён; явный обход позволяет запретить неоднозначный patch.

Полный исходник: [solutions/Task14.cs](solutions/Task14.cs).

```csharp
using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task14
{
    public sealed record Profile(string Name, string? Bio);
    public static Profile Patch(Profile original, string json)
    {
        ArgumentNullException.ThrowIfNull(original); ArgumentNullException.ThrowIfNull(json);
        if (string.IsNullOrWhiteSpace(original.Name)) throw new ArgumentException("Original name.");
        using var doc = JsonDocument.Parse(json); if (doc.RootElement.ValueKind != JsonValueKind.Object) throw new JsonException("Object.");
        string name = original.Name; string? bio = original.Bio; var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var property in doc.RootElement.EnumerateObject())
        {
            var key = PropertyName(property);
            if (!seen.Add(key)) throw new JsonException("Duplicate.");
            if (key == "name" && property.Value.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(Decode(property.Value))) name = Decode(property.Value)!;
            else if (key == "bio" && (property.Value.ValueKind == JsonValueKind.String || property.Value.ValueKind == JsonValueKind.Null)) bio = Decode(property.Value);
            else throw new JsonException("Invalid patch.");
        }
        return new Profile(name, bio);
    }
    private static string? Decode(JsonElement value)
    {
        try { return value.GetString(); }
        catch (InvalidOperationException error) { throw new JsonException("Invalid Unicode string.", error); }
    }
    private static string PropertyName(JsonProperty value)
    {
        try { return value.Name; }
        catch (InvalidOperationException error) { throw new JsonException("Invalid Unicode property name.", error); }
    }
    public static void Run()
    {
        var old = new Profile("Ada", "old"); Check.Equal(old, Patch(old, "{}"));
        Check.Equal(new Profile("Ada", null), Patch(old, "{\"bio\":null}"));
        Check.Equal(new Profile("New", "old"), Patch(old, "{\"name\":\"New\"}")); Check.Equal("Ada", old.Name);
        foreach (var json in new[] { "{\"extra\":1}", "{\"bio\":1}", "{\"name\":\" \"}", "{\"bio\":null,\"bio\":\"x\"}", "[]" }) Check.Throws<JsonException>(() => Patch(old, json));
        Check.Throws<ArgumentNullException>(() => Patch(null!, "{}"));
        Check.Throws<JsonException>(() => Patch(old, "{\"name\":\"New\",\"bio\":\"\\uD800\"}")); Check.Equal(new Profile("Ada", "old"), old);
        Check.Throws<JsonException>(() => Patch(old, "{\"\\uDC00\":null}"));
        Check.Throws<ArgumentException>(() => Patch(new Profile(" ", null), "{}"));
        Check.Throws<ArgumentNullException>(() => Patch(old, null!));
    }
}
```

**Исполняемые проверки**

- Пропуск и сброс Bio
- Замена имени и сохранение исходника
- Неизвестный ключ, повтор, неверный тип и пустое имя отклонены
- Невалидный Unicode ключ/значение и неверный original отклонены; original после поздней ошибки прежний

<a id="11-15"></a>
## 11-15. Каталог с безопасным импортом

Подготовить временный список, проверить все элементы и только затем заменить состояние.

**Ход решения**

Нельзя добавлять элементы сразу в рабочий каталог: ошибка в конце оставит частично обновлённые данные. Десериализуем в локальный массив, проверяем его HashSet идентификаторов и лишь затем присваиваем. Экспорт возвращает текст, поэтому вызывающий не может изменить внутренние объекты через ссылку. Время и память O(n).

**Частая ошибка**

Clear перед валидацией уничтожает рабочие данные даже при неуспешном импорте.

Полный исходник: [solutions/Task15.cs](solutions/Task15.cs).

```csharp
using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task15
{
    public sealed class Item { public int Id { get; set; } public string? Name { get; set; } }
    public sealed class Catalog
    {
        private Item?[] items = Array.Empty<Item?>();
        public int Count => items.Length;
        public void Import(string json)
        {
            ArgumentNullException.ThrowIfNull(json);
            var next = JsonSerializer.Deserialize<Item?[]>(json) ?? throw new JsonException("Array required.");
            var ids = new HashSet<int>();
            foreach (var item in next)
                if (item is null || item.Id <= 0 || string.IsNullOrWhiteSpace(item.Name) || !ids.Add(item.Id)) throw new JsonException("Invalid item.");
            items = next;
        }
        public string Export() => JsonSerializer.Serialize(items);
    }
    public static void Run()
    {
        var catalog = new Catalog(); catalog.Import("[{\"Id\":2,\"Name\":\"B\"},{\"Id\":1,\"Name\":\"A\"}]");
        var before = catalog.Export(); Check.Equal(2, catalog.Count);
        Check.Equal(2, JsonSerializer.Deserialize<Item[]>(before)![0].Id);
        Check.Throws<JsonException>(() => catalog.Import("[{\"Id\":3,\"Name\":\"C\"},{\"Id\":3,\"Name\":\"D\"}]")); Check.Equal(before, catalog.Export());
        foreach (var bad in new[] { "[null]", "null", "[{\"Id\":1,\"Name\":\" \"}]" }) { Check.Throws<JsonException>(() => catalog.Import(bad)); Check.Equal(before, catalog.Export()); }
        catalog.Import("[]"); Check.Equal(0, catalog.Count);
        Check.Throws<ArgumentNullException>(() => catalog.Import(null!));
        catalog.Import(before);
        Check.Throws<JsonException>(() => catalog.Import("[{\"Id\":1,\"Name\":\"A\"},{\"Id\":2,\"Name\":\"\\uD800\"}]")); Check.Equal(before, catalog.Export());
    }
}
```

**Исполняемые проверки**

- Первый импорт и сохранение порядка
- Дубликат в конце сохраняет прежний снимок
- null-элемент и неверное имя отклонены
- Пустой импорт очищает каталог
- Невалидный Unicode во второй записи сохраняет весь прежний экспорт
