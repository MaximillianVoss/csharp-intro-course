# Урок 14. LINQ и обработка последовательностей: решения

Сначала выполните свою попытку по TASKS.md. Здесь приведены ответы.
Каждое решение запускается по ID; Run содержит исполняемые проверки.

<a id="14-01"></a>
## 14-01. Фильтр и проекция

Один снимок источника, проверка всех элементов, Where→Select→ToArray.

**Ход решения**

Проверка до фильтра не даёт пропустить неверное нечётное значение. Where оставляет подходящие исходные элементы, Select вычисляет квадрат. Ограничение гарантирует отсутствие переполнения. ToArray завершает вычисление внутри метода, поэтому исключения не откладываются до будущего обхода. Время/память O(n).

**Частая ошибка**

Select(x=>x%2==0) создаёт последовательность bool, а не отбирает числа.

Полный исходник: [solutions/Task01.cs](solutions/Task01.cs).

```csharp
using Course;

namespace Course.Lesson14;

public static class Task01
{
    public static int[] EvenSquares(IEnumerable<int> source)
    {
        ArgumentNullException.ThrowIfNull(source); var items = source.ToArray();
        if (items.Any(x => x < -1000 || x > 1000)) throw new ArgumentOutOfRangeException(nameof(source));
        return items.Where(x => x % 2 == 0).Select(x => x * x).ToArray();
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { 4, 0, 16, 4 }, EvenSquares(new[] { 3, -2, 0, 4, -2 }));
        Check.Equal(0, EvenSquares(Array.Empty<int>()).Length); Check.SequenceEqual(new[] { 1000000 }, EvenSquares(new[] { -1000 }));
        Check.Throws<ArgumentOutOfRangeException>(() => EvenSquares(new[] { 1001 })); Check.Throws<ArgumentNullException>(() => EvenSquares(null!));
    }
}
```

**Исполняемые проверки**

- Отрицательные, ноль и повторы
- Пустой источник
- Неверное нечётное число отклонено
- null источник отвергнут

<a id="14-02"></a>
## 14-02. Рейтинг с устойчивыми равенствами

После единой проверки использовать OrderByDescending(...).ThenBy(...).ToArray().

**Ход решения**

Второй OrderBy создал бы новый главный порядок и потерял приоритет Score. ThenBy уточняет уже заданный порядок. LINQ to Objects использует стабильную сортировку, поэтому равные ключи сохраняют порядок ссылок. Проверка ReferenceEquals на двух равных объектах обнаруживает потерю стабильности. Время O(n log n), память O(n).

**Частая ошибка**

Сравнение всего результата только через record-равенство не отличает перестановку двух равных объектов.

Полный исходник: [solutions/Task02.cs](solutions/Task02.cs).

```csharp
using Course;

namespace Course.Lesson14;

public static class Task02
{
    public sealed record Student(string Name, int Score);
    public static Student[] Rank(IEnumerable<Student> students)
    {
        ArgumentNullException.ThrowIfNull(students); var items = students.ToArray();
        if (items.Any(x => x is null || string.IsNullOrWhiteSpace(x.Name) || x.Score < 0 || x.Score > 100)) throw new ArgumentException("Student.");
        return items.OrderByDescending(x => x.Score).ThenBy(x => x.Name, StringComparer.Ordinal).ToArray();
    }
    public static void Run()
    {
        var first = new Student("A", 90); var second = new Student("A", 90); var input = new[] { new Student("B", 90), first, second, new Student("Z", 100) };
        var rank = Rank(input); Check.SequenceEqual(new[] { "Z", "A", "A", "B" }, rank.Select(x => x.Name));
        Check.True(ReferenceEquals(first, rank[1])); Check.True(ReferenceEquals(second, rank[2])); Check.Equal("B", input[0].Name);
        Check.Equal(0, Rank(Array.Empty<Student>()).Length); Check.Throws<ArgumentException>(() => Rank(new[] { new Student("x", 101) }));
        Check.Throws<ArgumentException>(() => Rank(new Student[] { null! })); Check.Throws<ArgumentNullException>(() => Rank(null!));
    }
}
```

**Исполняемые проверки**

- Главный и вторичный порядок
- Ссылочный порядок точных равенств
- Вход не меняется
- Неверные поля и null отвергнуты

<a id="14-03"></a>
## 14-03. Запрос или снимок

Запрос даёт 2,3,4, снимок — 2,3.

**Ход решения**

Where сохраняет описание обхода и ссылку на источник. До ToArray он не создаёт отдельного массива. Первый ToArray выполняется до Add и фиксирует 2,3. Будущий обход query заново читает список, в котором уже 4 элемента. Повторный обход может давать новые данные и повторять побочные эффекты предиката.

**Частая ошибка**

var не означает ни ленивость, ни материализацию: это определяется возвращаемым типом операции.

Полный исходник: [solutions/Task03.cs](solutions/Task03.cs).

```csharp
using Course;

namespace Course.Lesson14;

public static class Task03
{
    public static (int[] Query, int[] Snapshot) Observe()
    {
        var numbers = new List<int> { 1, 2, 3 }; var query = numbers.Where(x => x > 1); var snapshot = query.ToArray(); numbers.Add(4);
        return (query.ToArray(), snapshot);
    }
    public static void Run()
    {
        var pair = Observe(); Check.SequenceEqual(new[] { 2, 3, 4 }, pair.Query); Check.SequenceEqual(new[] { 2, 3 }, pair.Snapshot);
        var again = Observe(); Check.SequenceEqual(pair.Query, again.Query); Check.SequenceEqual(pair.Snapshot, again.Snapshot);
    }
}
```

**Исполняемые проверки**

- Исходный фрагмент выполнен
- Запрос и снимок различаются
- Повторный вызов независим

<a id="14-04"></a>
## 14-04. Все сдали, но группа пуста

Материализовать, валидировать и потребовать Length>0 вместе с All.

**Ход решения**

All проверяет отсутствие нарушителей; в пустом наборе нарушителей нет, поэтому результат true математически согласован. Бизнес-правило дополнительно требует непустоты. Снимок нужен, чтобы Any/All не обходили одноразовый источник дважды. Проверяем все оценки до проверки успешности, иначе ранний неуспех скрыл бы неверные данные позже.

**Частая ошибка**

Замена All на Any проверяет хотя бы одного сдавшего, а не всех.

Полный исходник: [solutions/Task04.cs](solutions/Task04.cs).

```csharp
using Course;

namespace Course.Lesson14;

public static class Task04
{
    public static bool AllPassed(IEnumerable<int> scores, int threshold)
    {
        ArgumentNullException.ThrowIfNull(scores); if (threshold < 0 || threshold > 100) throw new ArgumentOutOfRangeException(nameof(threshold));
        var items = scores.ToArray(); if (items.Any(x => x < 0 || x > 100)) throw new ArgumentOutOfRangeException(nameof(scores));
        return items.Length > 0 && items.All(x => x >= threshold);
    }
    public static void Run()
    {
        Check.True(Array.Empty<int>().All(x => x >= 50)); Check.True(!AllPassed(Array.Empty<int>(), 50));
        Check.True(AllPassed(new[] { 50, 70 }, 50)); Check.True(!AllPassed(new[] { 49, 100 }, 50)); Check.True(AllPassed(new[] { 0 }, 0));
        Check.Throws<ArgumentOutOfRangeException>(() => AllPassed(new[] { 0, 101 }, 50)); Check.Throws<ArgumentOutOfRangeException>(() => AllPassed(new[] { 1 }, -1)); Check.Throws<ArgumentNullException>(() => AllPassed(null!, 50));
    }
}
```

**Исполняемые проверки**

- Исходный All на пустоте равен true
- Исправленный пустой случай
- Все/не все на границе
- Диапазоны и null проверены

<a id="14-05"></a>
## 14-05. Ноль или отсутствие совпадения

Where, преобразование в nullable и FirstOrDefault.

**Ход решения**

FirstOrDefault<int> вернул бы 0 и при отсутствии, смешав его с настоящим нулём. Проекция x=>(int?)x меняет default результата на null. Все операции до FirstOrDefault ленивые, поэтому хвост после первого чётного не нужен. Время O(k) до совпадения, память O(1).

**Частая ошибка**

Преобразование результата в int? после FirstOrDefault уже не восстановит потерянную информацию.

Полный исходник: [solutions/Task05.cs](solutions/Task05.cs).

```csharp
using Course;

namespace Course.Lesson14;

public static class Task05
{
    public static int? FirstEven(IEnumerable<int> numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers); return numbers.Where(x => x % 2 == 0).Select(x => (int?)x).FirstOrDefault();
    }
    private static IEnumerable<int> DangerousTail() { yield return 0; throw new InvalidOperationException("Must not enumerate tail."); }
    public static void Run()
    {
        Check.Equal<int?>(0, FirstEven(new[] { 1, 0, 2 })); Check.Equal<int?>(null, FirstEven(new[] { 1, 3 })); Check.Equal<int?>(null, FirstEven(Array.Empty<int>()));
        Check.Equal<int?>(-2, FirstEven(new[] { -3, -2, 4 })); Check.Equal<int?>(0, FirstEven(DangerousTail())); Check.Throws<ArgumentNullException>(() => FirstEven(null!));
    }
}
```

**Исполняемые проверки**

- Настоящий ноль
- Отсутствие и пустота
- Отрицательное чётное
- Хвост с исключением не перечисляется

<a id="14-06"></a>
## 14-06. Частоты нормализованных слов

Сначала нормализовать один раз, затем GroupBy и проекция размера группы.

**Ход решения**

Равенство должно задаваться до группировки: Cat с пробелом и cat — одна группа. Сохраняем нормализованные значения и валидируем null до запросов. Сортировка делает порядок отчёта явным, независимо от порядка первого появления групп. Время O(n+k log k), память O(n).

**Частая ошибка**

ToLower без Invariant делает нормализацию зависимой от текущей культуры.

Полный исходник: [solutions/Task06.cs](solutions/Task06.cs).

```csharp
using Course;

namespace Course.Lesson14;

public static class Task06
{
    public sealed record WordCount(string Word, int Count);
    public static WordCount[] Frequencies(IEnumerable<string> words)
    {
        ArgumentNullException.ThrowIfNull(words); var items = words.ToArray(); if (items.Any(x => x is null)) throw new ArgumentException("Null word.");
        return items.Select(x => x.Trim().ToLowerInvariant()).Where(x => x.Length > 0).GroupBy(x => x, StringComparer.Ordinal)
            .Select(g => new WordCount(g.Key, g.Count())).OrderBy(x => x.Word, StringComparer.Ordinal).ToArray();
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { new WordCount("cat", 2), new WordCount("dog", 1) }, Frequencies(new[] { " Cat ", "DOG", "cat", " " }));
        Check.Equal(0, Frequencies(new[] { "", " " }).Length); Check.Equal(0, Frequencies(Array.Empty<string>()).Length);
        Check.Throws<ArgumentException>(() => Frequencies(new string[] { "x", null! })); Check.Throws<ArgumentNullException>(() => Frequencies(null!));
    }
}
```

**Исполняемые проверки**

- Регистр и внешние пробелы
- Пропуск пустых
- Порядок групп
- Пустой ввод и null-элемент

<a id="14-07"></a>
## 14-07. Внутреннее соединение заказов

Проверить оба набора, затем Join по Id/CustomerId.

**Ход решения**

Inner join возвращает только совпадения. Уникальность Customer.Id нужна, чтобы один заказ не размножился по нескольким клиентам; уникальность Order.Id сохраняет идентификатор результата. Сначала материализуем и валидируем всё, потом соединяем. Средняя сложность O(c+o+r log r), память O(c+o+r).

**Частая ошибка**

Попытка получить клиента через First без обработки отсутствия нарушает правило пропуска неизвестных ссылок.

Полный исходник: [solutions/Task07.cs](solutions/Task07.cs).

```csharp
using Course;

namespace Course.Lesson14;

public static class Task07
{
    public sealed record Customer(int Id, string Name); public sealed record Order(int Id, int CustomerId); public sealed record Row(int OrderId, string CustomerName);
    public static Row[] JoinOrders(IEnumerable<Customer> customers, IEnumerable<Order> orders)
    {
        ArgumentNullException.ThrowIfNull(customers); ArgumentNullException.ThrowIfNull(orders); var cs = customers.ToArray(); var os = orders.ToArray();
        if (cs.Any(c => c is null || c.Id <= 0 || string.IsNullOrWhiteSpace(c.Name)) || os.Any(o => o is null || o.Id <= 0 || o.CustomerId <= 0)) throw new ArgumentException("Data.");
        if (cs.Select(c => c.Id).Distinct().Count() != cs.Length || os.Select(o => o.Id).Distinct().Count() != os.Length) throw new ArgumentException("Duplicate.");
        return os.Join(cs, o => o.CustomerId, c => c.Id, (o, c) => new Row(o.Id, c.Name)).OrderBy(r => r.OrderId).ToArray();
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { new Row(2, "A"), new Row(7, "A") }, JoinOrders(new[] { new Customer(1, "A") }, new[] { new Order(7, 1), new Order(3, 99), new Order(2, 1) }));
        Check.Equal(0, JoinOrders(Array.Empty<Customer>(), new[] { new Order(1, 2) }).Length);
        Check.Throws<ArgumentException>(() => JoinOrders(new[] { new Customer(1, "A"), new Customer(1, "B") }, Array.Empty<Order>()));
        Check.Throws<ArgumentException>(() => JoinOrders(Array.Empty<Customer>(), new[] { new Order(1, 2), new Order(1, 3) }));
        Check.Throws<ArgumentNullException>(() => JoinOrders(null!, Array.Empty<Order>()));
    }
}
```

**Исполняемые проверки**

- Несколько заказов клиента
- Неизвестный клиент пропускается
- Порядок по OrderId
- Дубликаты обоих ключей отвергнуты

<a id="14-08"></a>
## 14-08. Разворачивание рядов с координатами

Использовать индексированный SelectMany и вложенный индексированный Select.

**Ход решения**

Select с массивом результата на каждый ряд оставил бы вложенную структуру. SelectMany объединяет вложенные последовательности в одну, а индексированные перегрузки сохраняют координаты. Пустой второй ряд не создаёт элементы, но индекс третьего остаётся 2. Число элементов равно сумме длин рядов. Время/память O(N).

**Частая ошибка**

Нумерация непустых рядов заново теряет исходные координаты.

Полный исходник: [solutions/Task08.cs](solutions/Task08.cs).

```csharp
using Course;

namespace Course.Lesson14;

public static class Task08
{
    public sealed record Seat(int Row, int Column, string Label);
    public static Seat[] Flatten(string[][] rows)
    {
        ArgumentNullException.ThrowIfNull(rows); if (rows.Any(row => row is null || row.Any(x => x is null))) throw new ArgumentException("Null row/label.");
        return rows.SelectMany((row, r) => row.Select((label, c) => new Seat(r, c, label))).ToArray();
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { new Seat(0, 0, "A"), new Seat(0, 1, "B"), new Seat(2, 0, "A") }, Flatten(new[] { new[] { "A", "B" }, Array.Empty<string>(), new[] { "A" } }));
        Check.Equal(new Seat(0, 0, ""), Flatten(new[] { new[] { "" } })[0]); Check.Equal(0, Flatten(Array.Empty<string[]>()).Length);
        Check.Throws<ArgumentException>(() => Flatten(new string[][] { null! })); Check.Throws<ArgumentException>(() => Flatten(new[] { new string[] { null! } }));
    }
}
```

**Исполняемые проверки**

- Разные длины и пустой ряд
- Повторные и пустые подписи
- Полностью пустой ввод
- null ряды и элементы отвергнуты

<a id="14-09"></a>
## 14-09. Агрегация корзины без повторного обхода

На каждом шаге валидировать строку и обновить оба компонента аккумулятора.

**Ход решения**

Начальное состояние позволяет Aggregate работать и на пустоте. Два Sum по source потребовали бы два обхода, опасных для одноразового источника. Один аккумулятор хранит Units и Total. Тип long сохраняет число единиц за пределами int, decimal хранит цену. Время O(n), дополнительная память O(1), если не считать источник.

**Частая ошибка**

Перегрузка Aggregate без seed падает на пустом наборе и не соответствует контракту.

Полный исходник: [solutions/Task09.cs](solutions/Task09.cs).

```csharp
using Course;

namespace Course.Lesson14;

public static class Task09
{
    public sealed record Line(int Quantity, decimal UnitPrice);
    public static (long Units, decimal Total) Totals(IEnumerable<Line> lines)
    {
        ArgumentNullException.ThrowIfNull(lines);
        return lines.Aggregate((Units: 0L, Total: 0m), (sum, line) =>
        {
            if (line is null || line.Quantity < 1 || line.Quantity > 1000000 || line.UnitPrice < 0 || line.UnitPrice > 1000000) throw new ArgumentException("Line.");
            return (checked(sum.Units + line.Quantity), checked(sum.Total + line.Quantity * line.UnitPrice));
        });
    }
    public static void Run()
    {
        Check.Equal((5L, 8.50m), Totals(new[] { new Line(2, 1.25m), new Line(3, 2m) })); Check.Equal((0L, 0m), Totals(Array.Empty<Line>()));
        Check.Equal(3000000000L, Totals(Enumerable.Repeat(new Line(1000000, 0), 3000)).Units);
        Check.Throws<ArgumentException>(() => Totals(new[] { new Line(0, 1) })); Check.Throws<ArgumentException>(() => Totals(new Line[] { null! })); Check.Throws<ArgumentNullException>(() => Totals(null!));
    }
}
```

**Исполняемые проверки**

- Дробные цены
- Пустая корзина
- Количество единиц больше int
- Неверная строка и null отвергнуты

<a id="14-10"></a>
## 14-10. Уникальность имени: Single или First

SingleOrDefault отражает требование единственности.

**Ход решения**

Первый найденный элемент отвечает на вопрос о наличии, но ничего не доказывает об уникальности. SingleOrDefault продолжает поиск достаточно далеко, чтобы обнаружить второе совпадение. Тест на одном совпадении пропустил бы дефект. Проверяем отсутствие отдельно: оно допустимо и не должно приводить к исключению Single.

**Частая ошибка**

Single без OrDefault не поддерживает согласованный случай отсутствия пользователя.

Полный исходник: [solutions/Task10.cs](solutions/Task10.cs).

```csharp
using Course;

namespace Course.Lesson14;

public static class Task10
{
    public sealed record User(string Name);
    public static User? FindUnique(IEnumerable<User> users, string name)
    {
        ArgumentNullException.ThrowIfNull(users); ArgumentNullException.ThrowIfNull(name); var items = users.ToArray();
        if (items.Any(x => x is null || x.Name is null)) throw new ArgumentException("User.");
        return items.SingleOrDefault(x => StringComparer.OrdinalIgnoreCase.Equals(x.Name, name));
    }
    private static User? Broken(IEnumerable<User> users, string name) => users.FirstOrDefault(x => StringComparer.OrdinalIgnoreCase.Equals(x.Name, name));
    public static void Run()
    {
        var ada = new User("Ada"); Check.True(ReferenceEquals(ada, FindUnique(new[] { ada }, "ADA"))); Check.Equal<User?>(null, FindUnique(new[] { ada }, "Bob"));
        var duplicate = new[] { ada, new User("ADA") }; Check.Throws<InvalidOperationException>(() => FindUnique(duplicate, "ada")); Check.True(ReferenceEquals(ada, Broken(duplicate, "ada")));
        Check.Equal<User?>(null, FindUnique(Array.Empty<User>(), "")); Check.Throws<ArgumentException>(() => FindUnique(new User[] { null! }, "x"));
    }
}
```

**Исполняемые проверки**

- Ноль, одно и два совпадения
- Первый объект сохраняет идентичность
- Ошибочный First реально возвращает при дубликате
- null пользователь отвергнут

<a id="14-11"></a>
## 14-11. Словарь последних версий

Добавить исходный индекс и выбрать первый после сортировки Version↓,Index↓ в каждой группе.

**Ход решения**

ToDictionary не задаёт политику разрешения дубликатов, а бросает исключение. GroupBy собирает кандидатов. Явный исходный индекс делает правило последнего среди максимальных проверяемым и не зависящим от случайной перестановки. Для k групп суммарная сортировка не хуже O(n log n), память O(n). Ordinal сохраняет различие a и A.

**Частая ошибка**

Последняя запись вообще может иметь меньшую Version и не должна вытеснять более новую версию.

Полный исходник: [solutions/Task11.cs](solutions/Task11.cs).

```csharp
using Course;

namespace Course.Lesson14;

public static class Task11
{
    public sealed record Revision(string Key, int Version, string Value);
    public static Dictionary<string, Revision> Latest(IEnumerable<Revision> source)
    {
        ArgumentNullException.ThrowIfNull(source); var items = source.ToArray();
        if (items.Any(x => x is null || string.IsNullOrWhiteSpace(x.Key) || x.Version < 0 || x.Value is null)) throw new ArgumentException("Revision.");
        return items.Select((value, index) => new { Value = value, Index = index }).GroupBy(x => x.Value.Key, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.Value.Version).ThenByDescending(x => x.Index).First().Value, StringComparer.Ordinal);
    }
    public static void Run()
    {
        var last = new Revision("a", 2, "new"); var map = Latest(new[] { new Revision("a", 2, "old"), new Revision("a", 1, "x"), last, new Revision("a", 0, "later"), new Revision("A", 0, "upper") });
        Check.True(ReferenceEquals(last, map["a"])); Check.Equal(2, map.Count); Check.Equal("upper", map["A"].Value);
        Check.Equal(0, Latest(Array.Empty<Revision>()).Count); Check.Throws<ArgumentException>(() => Latest(new[] { new Revision("x", -1, "x") })); Check.Throws<ArgumentNullException>(() => Latest(null!));
    }
}
```

**Исполняемые проверки**

- Младшая версия в конце не выигрывает
- Равные максимальные версии: последняя
- Ordinal различает a/A
- Пустой и неверный ввод

<a id="14-12"></a>
## 14-12. Страница последовательности

Рассчитать безопасное смещение и применить Skip/Take/ToArray.

**Ход решения**

Нумерация с единицы требует page-1. Проверка до умножения не позволяет отрицательному размеру замаскировать ошибку. Skip просматривает пропущенный префикс у обычного IEnumerable, поэтому запрос далёкой страницы не обязательно дешёвый: O(offset+pageSize) до конца источника. Память O(pageSize).

**Частая ошибка**

Skip(page*pageSize) пропускает первую страницу уже при page=1.

Полный исходник: [solutions/Task12.cs](solutions/Task12.cs).

```csharp
using Course;

namespace Course.Lesson14;

public static class Task12
{
    public static T[] Page<T>(IEnumerable<T> source, int page, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(source); if (page < 1) throw new ArgumentOutOfRangeException(nameof(page)); if (pageSize < 1 || pageSize > 100) throw new ArgumentOutOfRangeException(nameof(pageSize));
        int skip = checked((page - 1) * pageSize); return source.Skip(skip).Take(pageSize).ToArray();
    }
    public static void Run()
    {
        var values = new[] { 10, 20, 30, 40, 50 }; Check.SequenceEqual(new[] { 10, 20 }, Page(values, 1, 2)); Check.SequenceEqual(new[] { 30, 40 }, Page(values, 2, 2));
        Check.SequenceEqual(new[] { 50 }, Page(values, 3, 2)); Check.Equal(0, Page(values, 4, 2).Length);
        Check.Throws<ArgumentOutOfRangeException>(() => Page(values, 0, 2)); Check.Throws<ArgumentOutOfRangeException>(() => Page(values, 1, 101)); Check.Throws<OverflowException>(() => Page(values, int.MaxValue, 100));
    }
}
```

**Исполняемые проверки**

- Первая, средняя и неполная последняя
- Страница за концом
- Неверные параметры
- Переполнение смещения

<a id="14-13"></a>
## 14-13. Клиенты без заказов тоже в отчёте

GroupJoin связывает клиента с целой последовательностью его покупок.

**Ход решения**

Обычный Join потерял бы клиента 2. GroupJoin выдаёт для него пустую группу, у Count и decimal Sum определены нужные нулевые результаты. Внешним источником должны быть клиенты. Неизвестные покупки не имеют внешнего клиента и естественно пропускаются. Средняя сложность O(c+p+c log c), память O(c+p).

**Частая ошибка**

DefaultIfEmpty с null-покупкой требует дополнительных проверок; GroupJoin для агрегатов проще.

Полный исходник: [solutions/Task13.cs](solutions/Task13.cs).

```csharp
using Course;

namespace Course.Lesson14;

public static class Task13
{
    public sealed record Customer(int Id); public sealed record Purchase(int CustomerId, decimal Amount); public sealed record Summary(int CustomerId, int Orders, decimal Total);
    public static Summary[] CustomerTotals(Customer[] customers, Purchase[] purchases)
    {
        ArgumentNullException.ThrowIfNull(customers); ArgumentNullException.ThrowIfNull(purchases);
        if (customers.Any(x => x is null || x.Id <= 0) || purchases.Any(x => x is null || x.CustomerId <= 0 || x.Amount < 0) || customers.Select(x => x.Id).Distinct().Count() != customers.Length) throw new ArgumentException("Data.");
        return customers.GroupJoin(purchases, c => c.Id, p => p.CustomerId, (c, group) => new Summary(c.Id, group.Count(), group.Sum(p => p.Amount))).OrderBy(x => x.CustomerId).ToArray();
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { new Summary(1, 2, 4m), new Summary(2, 0, 0m) }, CustomerTotals(new[] { new Customer(2), new Customer(1) }, new[] { new Purchase(1, 2.5m), new Purchase(1, 1.5m), new Purchase(9, 7m) }));
        Check.Equal(0, CustomerTotals(Array.Empty<Customer>(), Array.Empty<Purchase>()).Length);
        Check.Throws<ArgumentException>(() => CustomerTotals(new[] { new Customer(1), new Customer(1) }, Array.Empty<Purchase>()));
        Check.Throws<ArgumentException>(() => CustomerTotals(new[] { new Customer(1) }, new[] { new Purchase(1, -1) }));
    }
}
```

**Исполняемые проверки**

- Клиент с двумя покупками
- Клиент без покупок
- Неизвестный клиент игнорируется
- Дубликат клиента и отрицательная сумма отвергнуты

<a id="14-14"></a>
## 14-14. Одноразовый источник

Материализовать один раз и считать Length/Sum по независимому массиву.

**Ход решения**

IEnumerable не гарантирует повторяемость, одинаковые данные и отсутствие побочных эффектов. Два терминальных оператора над source создают два обхода. Снимок расходует O(n) памяти, но фиксирует одно чтение; Sum вычисляем в long. Исполняемый одноразовый источник делает нарушение наблюдаемым без измерения времени.

**Частая ошибка**

Выражение source.ToArray().Length вместе с source.Sum всё ещё обходит исходник дважды.

Полный исходник: [solutions/Task14.cs](solutions/Task14.cs).

```csharp
using Course;

namespace Course.Lesson14;

public static class Task14
{
    public static (int Count, long Sum) Analyze(IEnumerable<int> source)
    {
        ArgumentNullException.ThrowIfNull(source); var snapshot = source.ToArray(); return (snapshot.Length, snapshot.Sum(x => (long)x));
    }
    private sealed class OnceSequence : IEnumerable<int>
    {
        private readonly int[] values; private bool used;
        public OnceSequence(int[] values) { this.values = values; }
        public IEnumerator<int> GetEnumerator()
        {
            if (used) throw new InvalidOperationException("Second enumeration."); used = true; return ((IEnumerable<int>)values).GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
    private static IEnumerable<int> Once(int[] values) => new OnceSequence(values);
    private static (int, long) Broken(IEnumerable<int> source) => (source.Count(), source.Sum(x => (long)x));
    public static void Run()
    {
        Check.Equal((2, 4294967294L), Analyze(Once(new[] { int.MaxValue, int.MaxValue })));
        Check.Throws<InvalidOperationException>(() => Broken(Once(new[] { 1, 2 })));
        Check.Equal((0, 0L), Analyze(Once(Array.Empty<int>()))); Check.Throws<ArgumentNullException>(() => Analyze(null!));
    }
}
```

**Исполняемые проверки**

- Одноразовый источник работает у исправления
- Исходный код реально падает на втором обходе
- Пустота
- Сумма за пределами int и null

<a id="14-15"></a>
## 14-15. Отчёт по журналу запросов

Проверить снимок полностью, сгруппировать, спроецировать агрегаты, упорядочить и ограничить.

**Ход решения**

Число Requests и средняя длительность считаются по всей группе, а не только ошибкам. Where перед GroupBy потерял бы успешные запросы. Нормализации Path нет: /A и /a различаются по контракту. Top=0 ограничивает результат, но не отменяет проверку входа. Время O(n+k log k), память O(n+k).

**Частая ошибка**

Среднее от средних разных подгрупп не равно среднему всех запросов при разных размерах групп.

Полный исходник: [solutions/Task15.cs](solutions/Task15.cs).

```csharp
using Course;

namespace Course.Lesson14;

public static class Task15
{
    public sealed record LogEntry(string Path, int Status, double Milliseconds); public sealed record Endpoint(string Path, int Requests, int Failures, double AverageMs);
    public static Endpoint[] Report(IEnumerable<LogEntry> entries, int top)
    {
        ArgumentNullException.ThrowIfNull(entries); if (top < 0 || top > 100) throw new ArgumentOutOfRangeException(nameof(top)); var items = entries.ToArray();
        if (items.Any(x => x is null || string.IsNullOrWhiteSpace(x.Path) || x.Status < 100 || x.Status > 599 || !double.IsFinite(x.Milliseconds) || x.Milliseconds < 0 || x.Milliseconds > 1000000)) throw new ArgumentException("Entry.");
        return items.GroupBy(x => x.Path, StringComparer.Ordinal).Select(g => new Endpoint(g.Key, g.Count(), g.Count(x => x.Status >= 500), g.Average(x => x.Milliseconds)))
            .OrderByDescending(x => x.Failures).ThenBy(x => x.Path, StringComparer.Ordinal).Take(top).ToArray();
    }
    public static void Run()
    {
        var data = new[] { new LogEntry("/b", 200, 10), new LogEntry("/a", 500, 20), new LogEntry("/a", 200, 0), new LogEntry("/c", 500, 4) };
        var report = Report(data, 2); Check.SequenceEqual(new[] { "/a", "/c" }, report.Select(x => x.Path)); Check.Equal(2, report[0].Requests); Check.Equal(1, report[0].Failures); Check.Near(10, report[0].AverageMs);
        Check.Equal(0, Report(data, 0).Length); Check.Equal(0, Report(Array.Empty<LogEntry>(), 5).Length);
        Check.Throws<ArgumentException>(() => Report(new[] { new LogEntry("/a", 200, double.NaN) }, 0)); Check.Throws<ArgumentException>(() => Report(new[] { new LogEntry("/a", 600, 1) }, 1)); Check.Throws<ArgumentOutOfRangeException>(() => Report(data, 101));
    }
}
```

**Исполняемые проверки**

- Смешанные успешные/ошибочные запросы
- Порядок равных Failures по Path
- Пустота и top=0
- NaN, неверный статус и неверные параметры отклонены
