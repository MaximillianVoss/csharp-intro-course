# Урок 12. Обобщённые типы и алгоритмы: решения

Сначала выполните свою попытку по TASKS.md. Здесь приведены ответы.
Каждое решение запускается по ID; Run содержит исполняемые проверки.

<a id="12-01"></a>
## 12-01. Обмен двух значений

Хранить первое значение в локальной T, затем выполнить два присваивания.

**Ход решения**

Тип T фиксируется при вызове, поэтому временная переменная имеет ровно нужный тип. Для int копируется число; для ссылочного типа копируется ссылка на прежний объект. При алиасах первая и вторая переменные совпадают, но сохранённый temp возвращает то же значение. Время и дополнительная память O(1).

**Частая ошибка**

Передача без ref меняет только локальные копии параметров.

Полный исходник: [solutions/Task01.cs](solutions/Task01.cs).

```csharp
using Course;

namespace Course.Lesson12;

public static class Task01
{
    public static void Swap<T>(ref T first, ref T second) { T temp = first; first = second; second = temp; }
    public static void Run()
    {
        int a = 2, b = 7; Swap(ref a, ref b); Check.Equal(7, a); Check.Equal(2, b);
        string? x = null, y = "text"; Swap<string?>(ref x, ref y); Check.Equal("text", x); Check.Equal<string?>(null, y);
        Swap(ref a, ref a); Check.Equal(7, a);
    }
}
```

**Исполняемые проверки**

- Числа и строки
- null-ссылка
- Одна переменная передана дважды

<a id="12-02"></a>
## 12-02. Первый элемент или запасной

Использовать Count и индекс; не сравнивать элемент с default.

**Ход решения**

Число 0, false и null могут быть настоящими данными. Только Count отвечает на вопрос, имеется ли первый элемент. Благодаря IReadOnlyList известен доступ по индексу; время O(1), если сам список предоставляет обычный константный индексатор. Для произвольной реализации интерфейса стоимость её индексатора может отличаться.

**Частая ошибка**

Оператор ?? над первым элементом подменяет настоящее null запасным значением.

Полный исходник: [solutions/Task02.cs](solutions/Task02.cs).

```csharp
using Course;

namespace Course.Lesson12;

public static class Task02
{
    public static T FirstOr<T>(IReadOnlyList<T> items, T fallback)
    {
        ArgumentNullException.ThrowIfNull(items); return items.Count == 0 ? fallback : items[0];
    }
    public static void Run()
    {
        var values = new[] { 0, 5 }; Check.Equal(0, FirstOr(values, 9)); Check.SequenceEqual(new[] { 0, 5 }, values);
        Check.Equal(9, FirstOr(Array.Empty<int>(), 9));
        Check.Equal<string?>(null, FirstOr(new string?[] { null }, "x"));
        Check.Throws<ArgumentNullException>(() => FirstOr<int>(null!, 0));
    }
}
```

**Исполняемые проверки**

- Пустой список
- Первый default(int)
- Первый null и сохранение списка
- null-коллекция отвергнута

<a id="12-03"></a>
## 12-03. Статическое поле для каждого T

У каждого закрытого типа собственное статическое поле; результат 2,1.

**Ход решения**

Обобщённое определение Box&lt;T&gt; задаёт семейство типов. Box&lt;int&gt; и Box&lt;string&gt; — разные закрытые типы, поэтому Count одного не изменяет Count другого. Сначала обе величины 0, после трёх инкрементов получаем 2 и 1. Сброс делает проверку повторяемой и показывает границу времени жизни static.

**Частая ошибка**

Статическое поле принадлежит типу, а не одному экземпляру; создание новой Box&lt;int&gt; не сбросит Count.

Полный исходник: [solutions/Task03.cs](solutions/Task03.cs).

```csharp
using Course;

namespace Course.Lesson12;

public static class Task03
{
    public sealed class Box<T> { public static int Count; }
    public static (int IntCount, int StringCount) Observe()
    {
        Box<int>.Count = 0; Box<string>.Count = 0;
        Box<int>.Count++; Box<string>.Count++; Box<int>.Count++;
        return (Box<int>.Count, Box<string>.Count);
    }
    public static void Run()
    {
        Check.Equal((2, 1), Observe()); Check.Equal((2, 1), Observe());
        _ = new Box<int>(); Check.Equal(2, Box<int>.Count); Check.Equal(1, Box<string>.Count);
    }
}
```

**Исполняемые проверки**

- Точный порядок исходного фрагмента
- Повторный вызов независим
- Создание экземпляра не обнуляет static

<a id="12-04"></a>
## 12-04. Исправление обобщённого Head

Возвратить items[0] после проверки массива и длины.

**Ход решения**

Результат ToString имеет тип string?, который нельзя неявно преобразовать в произвольный T. Даже object-обёртка с обратным приведением не восстановит исходное значение и его тип. Исправленный вариант сохраняет значение и идентичность ссылочного объекта. Явная проверка пустоты даёт предусмотренную диагностическую ошибку.

**Частая ошибка**

ToString служит отображению, а не универсальному копированию значения.

Полный исходник: [solutions/Task04.cs](solutions/Task04.cs).

```csharp
using Course;

namespace Course.Lesson12;

public static class Task04
{
    public static T Head<T>(T[] items)
    {
        ArgumentNullException.ThrowIfNull(items); if (items.Length == 0) throw new InvalidOperationException("Empty."); return items[0];
    }
    public static void Run()
    {
        Check.Equal(42, Head(new[] { 42, 1 })); Check.Equal<string?>(null, Head(new string?[] { null }));
        var item = new object(); Check.True(ReferenceEquals(item, Head(new[] { item })));
        Check.Throws<InvalidOperationException>(() => Head(Array.Empty<int>()));
        Check.Throws<ArgumentNullException>(() => Head<int>(null!));
    }
}
```

**Исполняемые проверки**

- int и nullable string
- Ссылка на объект сохраняется
- Пустой и null массив отклонены

<a id="12-05"></a>
## 12-05. Поиск сущности с ограничением

Сначала проверить целостность списка, затем искать по свойству интерфейса.

**Ход решения**

Без where компилятор не знает, что у T есть Key. Ограничение class позволяет использовать null как отсутствие результата. Предварительная проверка всех элементов делает контракт одинаковым даже при совпадении в начале и null в конце. Возвращаем исходную ссылку, не создавая подменную сущность. Время O(n), память O(1).

**Частая ошибка**

Условие items[i].Key до проверки null даёт NullReferenceException вместо согласованной ошибки.

Полный исходник: [solutions/Task05.cs](solutions/Task05.cs).

```csharp
using Course;

namespace Course.Lesson12;

public static class Task05
{
    public interface IHasKey { int Key { get; } }
    public sealed record User(int Key, string Name) : IHasKey;
    public sealed record Ticket(int Key) : IHasKey;
    public static T? Find<T>(IReadOnlyList<T> items, int key) where T : class, IHasKey
    {
        ArgumentNullException.ThrowIfNull(items);
        foreach (var item in items) if (item is null) throw new ArgumentException("Null item.");
        foreach (var item in items) if (item.Key == key) return item;
        return null;
    }
    public static void Run()
    {
        var first = new User(2, "first"); var values = new[] { first, new User(1, "x"), new User(2, "last") };
        Check.True(ReferenceEquals(first, Find(values, 2))); Check.Equal<User?>(null, Find(values, 9));
        Check.Equal(new Ticket(-1), Find(new[] { new Ticket(-1) }, -1));
        Check.Throws<ArgumentException>(() => Find(new User[] { first, null! }, 2));
        Check.Throws<ArgumentNullException>(() => Find<User>(null!, 0));
    }
}
```

**Исполняемые проверки**

- Первое из двух совпадений
- Два разных класса с IHasKey
- Отсутствующий ключ
- null даже после раннего совпадения отклонён

<a id="12-06"></a>
## 12-06. Стек без потери default

Хранить значения в списке; успех определяется Count, а не самим значением.

**Ход решения**

Последний индекс равен Count-1. Сохраняем элемент до RemoveAt, затем возвращаем true. Пустой случай назначает out-параметр значением default и возвращает false. Для ссылок RemoveAt освобождает внутреннюю ссылку, поэтому снятый объект не удерживается контейнером. Амортизированное добавление O(1), снятие с конца O(1).

**Частая ошибка**

Возврат одного T не отличает пустоту от настоящего нуля или null.

Полный исходник: [solutions/Task06.cs](solutions/Task06.cs).

```csharp
using Course;

namespace Course.Lesson12;

public static class Task06
{
    public sealed class ArrayStack<T>
    {
        private readonly List<T> values = new();
        public int Count => values.Count;
        public void Push(T value) => values.Add(value);
        public bool TryPop(out T value)
        {
            if (values.Count == 0) { value = default!; return false; }
            int i = values.Count - 1; value = values[i]; values.RemoveAt(i); return true;
        }
    }
    public static void Run()
    {
        var stack = new ArrayStack<int>(); stack.Push(0); stack.Push(7); Check.Equal(2, stack.Count);
        Check.True(stack.TryPop(out var a)); Check.Equal(7, a); Check.True(stack.TryPop(out var b)); Check.Equal(0, b);
        Check.True(!stack.TryPop(out var c)); Check.Equal(0, c); Check.Equal(0, stack.Count);
        var strings = new ArrayStack<string?>(); strings.Push(null); Check.True(strings.TryPop(out var s)); Check.Equal<string?>(null, s);
    }
}
```

**Исполняемые проверки**

- LIFO и Count
- Настоящий 0 и пустота различаются
- Настоящий null в строковом стеке

<a id="12-07"></a>
## 12-07. Ограниченное копирование через вариантные интерфейсы

Проверить конфигурацию, затем читать пока copied&lt;limit и HasNext; после Read один Write.

**Ход решения**

ISource выдаёт T, поэтому out разрешает читать Dog как Animal. ISink потребляет T, поэтому приёмник любого Animal подходит месту, где будут передаваться только Dog. Направления противоположны, но оба безопасны. Условие copied&lt;limit находится перед HasNext и Read; лимит ноль не потребляет данные. Счётчики доказывают отсутствие дополнительного чтения за лимитом или EOF. Время O(min(limit,оставшиеся)), дополнительная память O(1).

**Частая ошибка**

Сначала Read, затем проверка лимита уже потребляет лишний элемент; in и out нельзя переставить по похожести названия.

Полный исходник: [solutions/Task07.cs](solutions/Task07.cs).

```csharp
using Course;

namespace Course.Lesson12;

public static class Task07
{
    public interface ISource<out T> { bool HasNext { get; } T Read(); }
    public interface ISink<in T> { void Write(T value); }
    public class Animal { public Animal(string name) { Name = name; } public string Name { get; } }
    public sealed class Dog : Animal { public Dog(string name) : base(name) { } }
    private sealed class ArraySource<T> : ISource<T>
    {
        private readonly T[] values; private int position; public int ReadCount { get; private set; }
        public ArraySource(T[] values) { this.values = values; } public bool HasNext => position < values.Length;
        public T Read() { if (!HasNext) throw new InvalidOperationException("Read after EOF."); ReadCount++; return values[position++]; }
    }
    private sealed class ListSink<T> : ISink<T>
    {
        public List<T> Values { get; } = new(); public int WriteCount { get; private set; }
        public void Write(T value) { WriteCount++; Values.Add(value); }
    }
    public static int CopyAtMost<T>(ISource<T> source, ISink<T> sink, int limit)
    {
        ArgumentNullException.ThrowIfNull(source); ArgumentNullException.ThrowIfNull(sink); if (limit < 0 || limit > 1000) throw new ArgumentOutOfRangeException(nameof(limit));
        int copied = 0; while (copied < limit && source.HasNext) { T value = source.Read(); sink.Write(value); copied++; } return copied;
    }
    public static void Run()
    {
        var first = new Dog("A"); var second = new Dog("B"); var dogs = new ArraySource<Dog>(new[] { first, second }); ISource<Animal> animals = dogs;
        var animalSink = new ListSink<Animal>(); Check.Equal(0, CopyAtMost(animals, animalSink, 0)); Check.Equal(0, dogs.ReadCount); Check.Equal(0, animalSink.WriteCount);
        Check.Equal(1, CopyAtMost(animals, animalSink, 1)); Check.Equal(1, dogs.ReadCount); Check.True(ReferenceEquals(first, animalSink.Values[0]));
        ISink<Dog> acceptsDogs = animalSink; Check.Equal(1, CopyAtMost(dogs, acceptsDogs, 5)); Check.Equal(2, dogs.ReadCount); Check.Equal(2, animalSink.WriteCount); Check.True(ReferenceEquals(second, animalSink.Values[1]));
        Check.Equal(0, CopyAtMost(dogs, acceptsDogs, 1)); Check.Equal(2, dogs.ReadCount);
        Check.Throws<ArgumentOutOfRangeException>(() => CopyAtMost(dogs, acceptsDogs, -1)); Check.Throws<ArgumentOutOfRangeException>(() => CopyAtMost(dogs, acceptsDogs, 1001));
        Check.Throws<ArgumentNullException>(() => CopyAtMost<Dog>(null!, acceptsDogs, 1)); Check.Throws<ArgumentNullException>(() => CopyAtMost<Dog>(dogs, null!, 1));
    }
}
```

**Исполняемые проверки**

- Ковариантный источник Dog→Animal
- Контравариантный приёмник Animal→Dog
- Лимит 0 и 1 без лишних чтений
- EOF и точные счётчики записей
- Неверные аргументы отвергнуты

<a id="12-08"></a>
## 12-08. Создание независимых экземпляров T

Выделить T[] и заполнить каждую ячейку отдельным new T().

**Ход решения**

new T[count] выделяет ячейки ссылок и не создаёт ни одного T. Ограничение new() разрешает самостоятельный вызов new T(), а class делает требование разных ссылок осмысленным. Array.Fill(result,new T()) создало бы один объект на все ячейки; счётчик и мутация это обнаруживают. Сначала валидируем count, чтобы неверный ввод не запускал конструкторы. Время O(count) плюс конструкторы, память O(count).

**Частая ошибка**

Наличие массива нужной длины не доказывает создание объектов; нужны и ненулевые ссылки, и независимость ячеек.

Полный исходник: [solutions/Task08.cs](solutions/Task08.cs).

```csharp
using Course;

namespace Course.Lesson12;

public static class Task08
{
    public sealed class Widget
    {
        public static int Created { get; set; } public int Value { get; set; }
        public Widget() { Created++; }
    }
    public sealed class Note { public string Text { get; set; } = "empty"; }
    public static T[] CreateMany<T>(int count) where T : class, new()
    {
        if (count < 0 || count > 1000) throw new ArgumentOutOfRangeException(nameof(count));
        var result = new T[count]; for (int i = 0; i < result.Length; i++) result[i] = new T(); return result;
    }
    public static void Run()
    {
        Widget.Created = 0; var widgets = CreateMany<Widget>(3); Check.Equal(3, Widget.Created); Check.Equal(3, widgets.Length);
        Check.True(widgets[0] is not null && widgets[1] is not null && widgets[2] is not null);
        Check.True(!ReferenceEquals(widgets[0], widgets[1]) && !ReferenceEquals(widgets[0], widgets[2]) && !ReferenceEquals(widgets[1], widgets[2]));
        widgets[0].Value = 9; Check.Equal(0, widgets[1].Value); Check.Equal(0, widgets[2].Value);
        var notes = CreateMany<Note>(2); notes[0].Text = "changed"; Check.Equal("empty", notes[1].Text);
        Check.Equal(0, CreateMany<Widget>(0).Length); Check.Equal(3, Widget.Created);
        Check.Throws<ArgumentOutOfRangeException>(() => CreateMany<Widget>(-1)); Check.Throws<ArgumentOutOfRangeException>(() => CreateMany<Widget>(1001)); Check.Equal(3, Widget.Created);
    }
}
```

**Исполняемые проверки**

- Три конструктора для трёх ячеек
- Попарная независимость и мутация одного объекта
- Второй тип класса
- Ноль и неверные границы без конструирований

<a id="12-09"></a>
## 12-09. Типобезопасный результат

Сделать состояния неизменяемыми и создавать их только именованными фабриками.

**Ход решения**

Публичные set оставили бы состояния Success=true с непустой Error или ошибку без текста. Закрытый конструктор принимает согласованные значения от фабрик. Тип T сохраняет тип результата без приведения из object. Храним default при неуспехе, но доступ Value защищён флагом. Это явный инвариант, а не предположение о значении.

**Частая ошибка**

Не определяйте успех как Value != null: это запрещает корректные nullable-результаты.

Полный исходник: [solutions/Task09.cs](solutions/Task09.cs).

```csharp
using Course;

namespace Course.Lesson12;

public static class Task09
{
    public sealed class Result<T>
    {
        private readonly T value;
        private Result(bool success, T value, string? error) { IsSuccess = success; this.value = value; Error = error; }
        public bool IsSuccess { get; }
        public string? Error { get; }
        public T Value => IsSuccess ? value : throw new InvalidOperationException("No value.");
        public static Result<T> Ok(T value) => new(true, value, null);
        public static Result<T> Fail(string error)
        {
            if (string.IsNullOrWhiteSpace(error)) throw new ArgumentException("Error required."); return new(false, default!, error);
        }
    }
    public static void Run()
    {
        var zero = Result<int>.Ok(0); Check.True(zero.IsSuccess); Check.Equal(0, zero.Value); Check.Equal<string?>(null, zero.Error);
        var nullable = Result<string?>.Ok(null); Check.True(nullable.IsSuccess); Check.Equal<string?>(null, nullable.Value);
        var bad = Result<int>.Fail("bad"); Check.True(!bad.IsSuccess); Check.Equal("bad", bad.Error); Check.Throws<InvalidOperationException>(() => { _ = bad.Value; });
        Check.Throws<ArgumentException>(() => Result<int>.Fail(" "));
    }
}
```

**Исполняемые проверки**

- Успешный ноль
- Успешный null
- Ошибка с текстом и запрет чтения Value
- Пустая ошибка отвергнута

<a id="12-10"></a>
## 12-10. Что означает копия T[]

Скопировать ячейки Array.Copy и проверить два разных уровня идентичности.

**Ход решения**

Массив хранит значения T. Если T — ссылка, копируется именно ссылка, а не объект по ней. Сравнение содержимого сразу после копирования не обнаруживает alias. Проверка ReferenceEquals массивов или замена ячейки обнаруживает его. Изменение свойства объекта доказывает оговорённую поверхностность. Время/память O(n).

**Частая ошибка**

Универсальная глубокая копия требует правил предметной модели и не получается простым generic-присваиванием.

Полный исходник: [solutions/Task10.cs](solutions/Task10.cs).

```csharp
using Course;

namespace Course.Lesson12;

public static class Task10
{
    public sealed class Box { public int Value { get; set; } }
    public static T[] Copy<T>(T[] source)
    {
        ArgumentNullException.ThrowIfNull(source); var copy = new T[source.Length]; Array.Copy(source, copy, source.Length); return copy;
    }
    private static T[] Broken<T>(T[] source) => source;
    public static void Run()
    {
        var input = new[] { 1, 2 }; var copy = Copy(input); Check.SequenceEqual(input, copy); copy[0] = 8; Check.Equal(1, input[0]);
        Check.True(ReferenceEquals(input, Broken(input))); Check.True(!ReferenceEquals(input, Copy(input)));
        Check.Equal(0, Copy(Array.Empty<int>()).Length);
        var boxes = new[] { new Box { Value = 1 } }; var other = Copy(boxes); other[0].Value = 2; Check.Equal(2, boxes[0].Value);
        other[0] = new Box { Value = 9 }; Check.Equal(2, boxes[0].Value);
        Check.Throws<ArgumentNullException>(() => Copy<int>(null!));
    }
}
```

**Исполняемые проверки**

- int и пустой массив
- Ошибочный alias реально обнаружен
- Замена ячейки независима
- Внутренний объект остаётся общим

<a id="12-11"></a>
## 12-11. Мультисловарь

Создавать список при первом ключе и копировать при чтении.

**Ход решения**

notnull сообщает ограничение компилятору, но проверка null на публичной границе всё равно полезна при подавленных предупреждениях. Словарь отвечает за группы, список — за порядок внутри группы. Выдача ToArray отделяет структуру результата от внутреннего List; сами ссылочные значения остаются теми же объектами. Среднее добавление O(1), чтение O(k).

**Частая ошибка**

Возврат внутреннего List под интерфейсом не всегда защищает от обратного приведения и изменения.

Полный исходник: [solutions/Task11.cs](solutions/Task11.cs).

```csharp
using Course;

namespace Course.Lesson12;

public static class Task11
{
    public sealed class MultiMap<TKey, TValue> where TKey : notnull
    {
        private readonly Dictionary<TKey, List<TValue>> groups;
        public MultiMap(IEqualityComparer<TKey>? comparer = null) { groups = new(comparer); }
        public void Add(TKey key, TValue value)
        {
            ArgumentNullException.ThrowIfNull(key);
            if (!groups.TryGetValue(key, out var list)) { list = new(); groups.Add(key, list); } list.Add(value);
        }
        public TValue[] Get(TKey key)
        {
            ArgumentNullException.ThrowIfNull(key); return groups.TryGetValue(key, out var list) ? list.ToArray() : new TValue[0];
        }
    }
    public static void Run()
    {
        var map = new MultiMap<string, int>(StringComparer.OrdinalIgnoreCase); map.Add("A", 1); map.Add("a", 2); map.Add("A", 2);
        Check.SequenceEqual(new[] { 1, 2, 2 }, map.Get("a")); Check.Equal(0, map.Get("B").Length);
        var snapshot = map.Get("A"); snapshot[0] = 99; Check.Equal(1, map.Get("A")[0]);
        var nullable = new MultiMap<int, string?>(); nullable.Add(1, null); Check.Equal<string?>(null, nullable.Get(1)[0]);
        Check.Throws<ArgumentNullException>(() => map.Add(null!, 1)); Check.Throws<ArgumentNullException>(() => map.Get(null!));
    }
}
```

**Исполняемые проверки**

- Объединение по comparer
- Порядок и повторы
- Отсутствующий ключ
- Снимок независим; null ключ отвергнут

<a id="12-12"></a>
## 12-12. Первый индекс двоичным поиском

Найти первую позицию, значение которой не меньше target.

**Ход решения**

Держим полуинтервал [left,right). Если середина меньше target, отбрасываем её и всё левее; иначе переносим right на середину. В конце left — место вставки перед равными. Проверка границы и CompareTo==0 отличает существующий элемент от позиции вставки. Формула середины left+(right-left)/2 избегает переполнения суммы.

**Частая ошибка**

Возврат на первом найденном равенстве не обеспечивает первый индекс среди повторов.

Полный исходник: [solutions/Task12.cs](solutions/Task12.cs).

```csharp
using Course;

namespace Course.Lesson12;

public static class Task12
{
    public static int FirstIndex<T>(IReadOnlyList<T> sorted, T target) where T : IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(sorted); ArgumentNullException.ThrowIfNull(target);
        int left = 0, right = sorted.Count;
        while (left < right) { int mid = left + (right - left) / 2; if (sorted[mid].CompareTo(target) < 0) left = mid + 1; else right = mid; }
        return left < sorted.Count && sorted[left].CompareTo(target) == 0 ? left : -1;
    }
    public static void Run()
    {
        var data = new[] { 1, 2, 2, 2, 5 }; Check.Equal(1, FirstIndex(data, 2)); Check.Equal(0, FirstIndex(data, 1));
        foreach (int missing in new[] { 0, 3, 9 }) Check.Equal(-1, FirstIndex(data, missing));
        Check.Equal(-1, FirstIndex(Array.Empty<int>(), 1)); Check.Equal(1, FirstIndex(new[] { "a", "b", "b" }, "b"));
        Check.Throws<ArgumentNullException>(() => FirstIndex<int>(null!, 1)); Check.Throws<ArgumentNullException>(() => FirstIndex(new[] { "a" }, null!));
    }
}
```

**Исполняемые проверки**

- Дубликаты дают первый индекс
- До начала, между и после элементов
- Пустой и строковый список
- null аргументы отвергнуты

<a id="12-13"></a>
## 12-13. Чтение коллекции базового типа

Строка A допустима благодаря out T; строка B запрещена, потому что List допускает запись.

**Ход решения**

Каждая Cat является Animal, поэтому чтение Cat как Animal безопасно. Но List&lt;Animal&gt; разрешил бы добавить другого наследника Animal в исходный List&lt;Cat&gt;. Инвариантность List запрещает такое присваивание. Контрольный вызов Names с List&lt;Cat&gt; компилируется через ковариантный интерфейс; метод только читает. Время/выходная память O(n).

**Частая ошибка**

readonly у переменной списка не делает сам List ковариантным или неизменяемым.

Полный исходник: [solutions/Task13.cs](solutions/Task13.cs).

```csharp
using Course;

namespace Course.Lesson12;

public static class Task13
{
    public class Animal { public Animal(string name) { Name = name; } public string Name { get; } }
    public sealed class Cat : Animal { public Cat(string name) : base(name) { } }
    public static string[] Names(IReadOnlyList<Animal> animals)
    {
        ArgumentNullException.ThrowIfNull(animals); var result = new string[animals.Count];
        for (int i = 0; i < animals.Count; i++) { if (animals[i] is null) throw new ArgumentException("Null animal."); result[i] = animals[i].Name; } return result;
    }
    public static void Run()
    {
        List<Cat> cats = new() { new Cat("Мур"), new Cat("Барс") }; IReadOnlyList<Animal> view = cats;
        Check.SequenceEqual(new[] { "Мур", "Барс" }, Names(cats)); Check.Equal(2, view.Count);
        Check.Equal(0, Names(new List<Cat>()).Length); Check.Throws<ArgumentException>(() => Names(new Animal[] { null! }));
        Check.Throws<ArgumentNullException>(() => Names(null!));
    }
}
```

**Исполняемые проверки**

- Прямая передача List&lt;Cat&gt;
- Пустой список
- null список и null элемент отвергнуты

<a id="12-14"></a>
## 12-14. Кольцевой буфер последних значений

Перезаписывать ячейку next; начало снимка равно next лишь после полного заполнения.

**Ход решения**

Пока Count меньше ёмкости, старейшая ячейка имеет индекс 0. После заполнения next указывает на старейшее значение, которое заменится следующим. Перебор (start+i)%capacity восстанавливает порядок. Независимый массив снимка не даёт изменять буфер извне. Неиспользованные ячейки не попадают в результат.

**Частая ошибка**

Вывод физического массива после оборота кольца показывает неверный хронологический порядок.

Полный исходник: [solutions/Task14.cs](solutions/Task14.cs).

```csharp
using Course;

namespace Course.Lesson12;

public static class Task14
{
    public sealed class RingBuffer<T>
    {
        private readonly T[] data; private int next;
        public RingBuffer(int capacity) { if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity)); data = new T[capacity]; }
        public int Count { get; private set; }
        public void Add(T value) { data[next] = value; next = (next + 1) % data.Length; if (Count < data.Length) Count++; }
        public T[] Snapshot()
        {
            var result = new T[Count]; int start = Count == data.Length ? next : 0;
            for (int i = 0; i < Count; i++) result[i] = data[(start + i) % data.Length]; return result;
        }
    }
    public static void Run()
    {
        var ring = new RingBuffer<int>(3); Check.Equal(0, ring.Count); ring.Add(1); ring.Add(2); Check.SequenceEqual(new[] { 1, 2 }, ring.Snapshot());
        ring.Add(3); ring.Add(4); Check.SequenceEqual(new[] { 2, 3, 4 }, ring.Snapshot());
        ring.Add(5); ring.Add(6); Check.SequenceEqual(new[] { 4, 5, 6 }, ring.Snapshot()); Check.Equal(3, ring.Count);
        var snap = ring.Snapshot(); snap[0] = 0; Check.Equal(4, ring.Snapshot()[0]);
        var single = new RingBuffer<string?>(1); single.Add("x"); single.Add(null); Check.Equal<string?>(null, single.Snapshot()[0]);
        Check.Throws<ArgumentOutOfRangeException>(() => new RingBuffer<int>(0));
    }
}
```

**Исполняемые проверки**

- Частичное заполнение
- Несколько оборотов
- Ёмкость 1 и default/null
- Снимок независим; неверная ёмкость отвергнута

<a id="12-15"></a>
## 12-15. Типобезопасная прямоугольная сетка

Линейный индекс row*Columns+column после независимой проверки каждой координаты.

**Ход решения**

Проверять только конечный индекс недостаточно: координата (0,Columns) попала бы в первую ячейку второй строки. Проверяем row и column отдельно, затем преобразуем. Ограничение размеров гарантирует безопасное произведение. Заполнение циклом копирует значение T, а для ссылок — одну ссылку во все ячейки. Доступ O(1), копирование строки O(columns), память O(rows*columns).

**Частая ошибка**

Формула row*Rows+column работает случайно только на квадратной сетке.

Полный исходник: [solutions/Task15.cs](solutions/Task15.cs).

```csharp
using Course;

namespace Course.Lesson12;

public static class Task15
{
    public sealed class Grid<T>
    {
        private readonly T[] data;
        public Grid(int rows, int columns, T initial)
        {
            if (rows < 1 || rows > 1000) throw new ArgumentOutOfRangeException(nameof(rows));
            if (columns < 1 || columns > 1000) throw new ArgumentOutOfRangeException(nameof(columns));
            Rows = rows; Columns = columns; data = new T[rows * columns]; Array.Fill(data, initial);
        }
        public int Rows { get; } public int Columns { get; }
        private int Index(int row, int column)
        {
            if (row < 0 || row >= Rows) throw new ArgumentOutOfRangeException(nameof(row));
            if (column < 0 || column >= Columns) throw new ArgumentOutOfRangeException(nameof(column)); return row * Columns + column;
        }
        public T this[int row, int column] { get => data[Index(row, column)]; set => data[Index(row, column)] = value; }
        public T[] CopyRow(int row) { int start = Index(row, 0); var result = new T[Columns]; Array.Copy(data, start, result, 0, Columns); return result; }
    }
    public static void Run()
    {
        var grid = new Grid<int>(2, 3, 0); grid[1, 2] = 7; Check.SequenceEqual(new[] { 0, 0, 7 }, grid.CopyRow(1)); Check.Equal(0, grid[0, 2]);
        var row = grid.CopyRow(1); row[2] = 99; Check.Equal(7, grid[1, 2]); Check.Equal(2, grid.Rows); Check.Equal(3, grid.Columns);
        Check.Throws<ArgumentOutOfRangeException>(() => { _ = grid[0, 3]; }); Check.Throws<ArgumentOutOfRangeException>(() => { grid[-1, 0] = 1; });
        Check.Throws<ArgumentOutOfRangeException>(() => grid.CopyRow(2)); Check.Throws<ArgumentOutOfRangeException>(() => new Grid<int>(0, 2, 0)); Check.Throws<ArgumentOutOfRangeException>(() => new Grid<int>(2, 1001, 0));
        var item = new object(); var refs = new Grid<object>(1, 2, item); Check.True(ReferenceEquals(refs[0, 0], refs[0, 1]));
    }
}
```

**Исполняемые проверки**

- Прямоугольник 2x3 и адрес последней ячейки
- Независимость строк и копии
- Отрицательные/выходящие координаты
- Ссылочное заполнение и неверные размеры
