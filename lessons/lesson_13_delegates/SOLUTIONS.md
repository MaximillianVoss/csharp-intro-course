# Урок 13. Делегаты, лямбды и события: решения

Сначала выполните свою попытку по TASKS.md. Здесь приведены ответы.
Каждое решение запускается по ID; Run содержит исполняемые проверки.

<a id="13-01"></a>
## 13-01. Один вызов преобразования

Проверить ссылку на делегат и вернуть результат одного вызова.

**Ход решения**

Func&lt;int,int&gt; описывает функцию от int к int. Передача функции не выполняет её. Только operation(value) запускает тело. Локальный счётчик в тесте доказывает, что метод не вычисляет результат повторно для проверки или логирования. Стоимость метода O(1) плюс стоимость переданной операции.

**Частая ошибка**

operation.ToString() показывает описание делегата, а не результат вычисления.

Полный исходник: [solutions/Task01.cs](solutions/Task01.cs).

```csharp
using Course;

namespace Course.Lesson13;

public static class Task01
{
    public static int Apply(int value, Func<int, int> operation) { ArgumentNullException.ThrowIfNull(operation); return operation(value); }
    public static void Run()
    {
        Check.Equal(7, Apply(4, x => x + 3)); Check.Equal(16, Apply(4, x => x * x));
        int calls = 0; Check.Equal(5, Apply(4, x => { calls++; return x + 1; })); Check.Equal(1, calls);
        Check.Throws<FormatException>(() => Apply(0, _ => throw new FormatException("bad")));
        Check.Throws<ArgumentNullException>(() => Apply(0, null!));
        var failure = new FormatException("same instance"); int faultCalls = 0;
        var observed = Check.Throws<FormatException>(() => Apply(1, _ => { faultCalls++; throw failure; })); Check.True(ReferenceEquals(failure, observed)); Check.Equal(1, faultCalls);
    }
}
```

**Исполняемые проверки**

- Две операции
- Ровно один вызов
- Ошибка операции распространяется
- null отвергнут
- Ошибка зависимости сохраняет тот же объект исключения, вызов ровно один

<a id="13-02"></a>
## 13-02. Композиция в заданном порядке

Вернуть лямбду, которая вызывает second на результате first.

**Ход решения**

Порядок важен: (3+1)*2 отличается от 3*2+1. Замыкание хранит ссылки на оба делегата. Проверки аргументов находятся снаружи возвращаемой лямбды, поэтому неверная конфигурация выявляется немедленно. Исключение first прерывает вычисление аргумента second. Создание O(1), вызов — сумма стоимостей операций.

**Частая ошибка**

Если вычислить first заранее при Compose, метод перестанет быть функцией от будущего x.

Полный исходник: [solutions/Task02.cs](solutions/Task02.cs).

```csharp
using Course;

namespace Course.Lesson13;

public static class Task02
{
    public static Func<T, T> Compose<T>(Func<T, T> first, Func<T, T> second)
    {
        ArgumentNullException.ThrowIfNull(first); ArgumentNullException.ThrowIfNull(second); return x => second(first(x));
    }
    public static void Run()
    {
        int calls = 0; var composed = Compose<int>(x => { calls++; return x + 1; }, x => x * 2);
        Check.Equal(0, calls); Check.Equal(8, composed(3)); Check.Equal(12, composed(5)); Check.Equal(2, calls);
        bool second = false; var bad = Compose<int>(_ => throw new FormatException(), x => { second = true; return x; });
        Check.Throws<FormatException>(() => bad(0)); Check.True(!second);
        Check.Throws<ArgumentNullException>(() => Compose<int>(null!, x => x)); Check.Throws<ArgumentNullException>(() => Compose<int>(x => x, null!));
    }
}
```

**Исполняемые проверки**

- Необратимый порядок операций
- Создание без вызовов
- Повторные вызовы
- Ошибка first подавляет second; null проверен сразу

<a id="13-03"></a>
## 13-03. Когда читается захваченная переменная

Первый результат 6, второй 15: переменная читается во время вызова.

**Ход решения**

В замыкании хранится доступ к одной и той же переменной factor. При первом вызове в ней 2; после присваивания 5 второй вызов использует 5. Это отличается от сохранения отдельной копии до создания делегата. Локальная область Observe создаёт новое независимое замыкание при каждом обращении.

**Частая ошибка**

Текст лямбды не является снимком всех значений в момент её объявления.

Полный исходник: [solutions/Task03.cs](solutions/Task03.cs).

```csharp
using Course;

namespace Course.Lesson13;

public static class Task03
{
    public static int[] Observe()
    {
        int factor = 2; Func<int, int> multiply = x => x * factor; int a = multiply(3); factor = 5; int b = multiply(3); return new[] { a, b };
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { 6, 15 }, Observe()); Check.SequenceEqual(new[] { 6, 15 }, Observe());
        int factor = 2; int snapshot = factor; Func<int, int> fixedMultiply = x => x * snapshot; factor = 5;
        Check.Equal(6, fixedMultiply(3)); Check.Equal(5, factor);
    }
}
```

**Исполняемые проверки**

- Точный исходный фрагмент
- Повторный Observe независим
- Отдельный захват копии даёт иной результат

<a id="13-04"></a>
## 13-04. Исправление захвата в for

В каждой итерации сохранить i в новую локальную переменную.

**Ход решения**

У for одна переменная i на весь цикл. Все исходные лямбды обращаются к ней, а после выхода значение равно count. Локальная copy внутри тела имеет отдельную область на каждой итерации и сохраняет нужный индекс. Тесты во время цикла не обнаружили бы ошибку, поэтому вызываем делегаты после возврата. Время/память O(count).

**Частая ошибка**

Простое переименование i ничего не меняет: нужна другая переменная с иной областью жизни.

Полный исходник: [solutions/Task04.cs](solutions/Task04.cs).

```csharp
using Course;

namespace Course.Lesson13;

public static class Task04
{
    public static Func<int>[] Build(int count)
    {
        if (count < 0 || count > 1000) throw new ArgumentOutOfRangeException(nameof(count));
        var result = new Func<int>[count]; for (int i = 0; i < count; i++) { int copy = i; result[i] = () => copy; } return result;
    }
    private static Func<int>[] Broken(int count)
    {
        var result = new Func<int>[count]; for (int i = 0; i < count; i++) result[i] = () => i; return result;
    }
    public static void Run()
    {
        var broken = Broken(3); Check.SequenceEqual(new[] { 3, 3, 3 }, new[] { broken[0](), broken[1](), broken[2]() });
        var fixedValues = Build(3); Check.SequenceEqual(new[] { 0, 1, 2 }, new[] { fixedValues[0](), fixedValues[1](), fixedValues[2]() });
        Check.Equal(0, Build(0).Length); Check.Equal(0, Build(1)[0]());
        Check.Throws<ArgumentOutOfRangeException>(() => Build(-1)); Check.Throws<ArgumentOutOfRangeException>(() => Build(1001));
    }
}
```

**Исполняемые проверки**

- Исходник реально даёт 3,3,3
- Исправление даёт 0,1,2 после цикла
- Пустой и одиночный набор
- Неверная длина отвергнута

<a id="13-05"></a>
## 13-05. Предикат диапазона

Сохранить параметры фабрики в замыкании и сравнивать напрямую.

**Ход решения**

Predicate&lt;int&gt; возвращает bool и подходит для правила проверки. Параметры одного вызова фабрики принадлежат его замыканию, поэтому второй диапазон не меняет первый. Прямые сравнения не переполняются, в отличие от арифметических разностей. Создание и проверка занимают O(1).

**Частая ошибка**

Исключение внутри лямбды откладывает ошибку конфигурации до первого применения.

Полный исходник: [solutions/Task05.cs](solutions/Task05.cs).

```csharp
using Course;

namespace Course.Lesson13;

public static class Task05
{
    public static Predicate<int> InRange(int minimum, int maximum)
    {
        if (minimum > maximum) throw new ArgumentException("Invalid range."); return x => x >= minimum && x <= maximum;
    }
    public static void Run()
    {
        var rule = InRange(2, 4); Check.True(!rule(1)); Check.True(rule(2)); Check.True(rule(4)); Check.True(!rule(5));
        var point = InRange(3, 3); Check.True(point(3)); Check.True(!point(2)); Check.True(rule(2));
        var all = InRange(int.MinValue, int.MaxValue); Check.True(all(int.MinValue)); Check.True(all(int.MaxValue));
        Check.Throws<ArgumentException>(() => InRange(5, 4));
    }
}
```

**Исполняемые проверки**

- Обе включённые границы
- Одноточечный диапазон
- Крайние int
- Независимость и неверный порядок границ

<a id="13-06"></a>
## 13-06. Отбор без LINQ

Перебирать source и добавлять только при true.

**Ход решения**

Обход не должен знать условие отбора. Предикат может иметь наблюдаемый счётчик, поэтому повторный вызов для одного элемента нарушает контракт. Выходной список сохраняет порядок за счёт последовательного Add. При исключении управление покидает foreach и enumerator освобождается. Время O(n) плюс стоимость предиката, память O(k).

**Частая ошибка**

Проверка accept дважды в if и логировании меняет поведение функций с состоянием.

Полный исходник: [solutions/Task06.cs](solutions/Task06.cs).

```csharp
using Course;

namespace Course.Lesson13;

public static class Task06
{
    public static T[] Filter<T>(IEnumerable<T> source, Predicate<T> accept)
    {
        ArgumentNullException.ThrowIfNull(source); ArgumentNullException.ThrowIfNull(accept); var result = new List<T>();
        foreach (var item in source) if (accept(item)) result.Add(item); return result.ToArray();
    }
    public static void Run()
    {
        int calls = 0; Check.SequenceEqual(new[] { 2, 2 }, Filter(new[] { 3, 2, 2, 1 }, x => { calls++; return x % 2 == 0; })); Check.Equal(4, calls);
        Check.Equal(0, Filter(Array.Empty<int>(), _ => true).Length);
        int visits = 0; Check.Throws<FormatException>(() => Filter(new[] { 1, 2, 3 }, x => { visits++; if (x == 2) throw new FormatException(); return true; })); Check.Equal(2, visits);
        Check.Throws<ArgumentNullException>(() => Filter<int>(null!, _ => true)); Check.Throws<ArgumentNullException>(() => Filter(new[] { 1 }, null!));
        bool entered = false;
        IEnumerable<int> Observed() { entered = true; yield return 1; }
        Check.Throws<ArgumentNullException>(() => Filter(Observed(), null!)); Check.True(!entered);
        int enumerations = 0, moves = 0, disposed = 0;
        IEnumerable<int> Owned()
        {
            if (++enumerations > 1) throw new InvalidOperationException("Second enumeration.");
            try { foreach (int value in new[] { 1, 2, 3 }) { moves++; yield return value; } }
            finally { disposed++; }
        }
        Check.Throws<FormatException>(() => Filter(Owned(), x => x == 2 ? throw new FormatException() : true));
        Check.Equal(1, enumerations); Check.Equal(2, moves); Check.Equal(1, disposed);
    }
}
```

**Исполняемые проверки**

- Порядок и повторы
- Точное число вызовов
- Пустой источник
- Ошибка останавливает обход; null отвергнут
- Неверный предикат не начинает обход source
- Один enumerator, остановка продвижения и его освобождение при ошибке предиката

<a id="13-07"></a>
## 13-07. Составное сравнение имён

Передать сравнение двух ключей в Array.Sort над копией.

**Ход решения**

Сравнение длины определяет основной порядок. Только при нуле сравниваем сами строки Ordinal, что даёт одинаковый результат при разных языковых настройках. Копия защищает исходный массив. Для точных дубликатов порядок неразличим, поэтому стабильность Array.Sort не требуется. Сортировка O(n log n), копия O(n).

**Частая ошибка**

Вычитание длин возможно, но CompareTo яснее; текущая культура не соответствует контракту Ordinal.

Полный исходник: [solutions/Task07.cs](solutions/Task07.cs).

```csharp
using Course;

namespace Course.Lesson13;

public static class Task07
{
    public static string[] SortNames(string[] names)
    {
        ArgumentNullException.ThrowIfNull(names); foreach (var name in names) if (name is null) throw new ArgumentException("Null name.");
        var result = (string[])names.Clone();
        Comparison<string> compare = (a, b) => { int length = a.Length.CompareTo(b.Length); return length != 0 ? length : StringComparer.Ordinal.Compare(a, b); };
        Array.Sort(result, compare); return result;
    }
    public static void Run()
    {
        var input = new[] { "bb", "a", "AA", "b", "" }; Check.SequenceEqual(new[] { "", "a", "b", "AA", "bb" }, SortNames(input));
        Check.SequenceEqual(new[] { "bb", "a", "AA", "b", "" }, input); Check.Equal(0, SortNames(Array.Empty<string>()).Length);
        Check.Throws<ArgumentException>(() => SortNames(new string[] { "a", null! })); Check.Throws<ArgumentNullException>(() => SortNames(null!));
        var previousCulture = System.Globalization.CultureInfo.CurrentCulture;
        try
        {
            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("tr-TR");
            Check.SequenceEqual(new[] { "I", "i", "\u0130", "\u0131" }, SortNames(new[] { "\u0131", "\u0130", "i", "I" }));
        }
        finally { System.Globalization.CultureInfo.CurrentCulture = previousCulture; }
    }
}
```

**Исполняемые проверки**

- Основной и вторичный ключ
- Пустая строка
- Исходный массив сохраняется
- null массив/элемент отвергнуты
- Ordinal-порядок I/i/İ/ı не меняется при tr-TR

<a id="13-08"></a>
## 13-08. Наблюдатель накопленной суммы

Сначала добавить элемент, затем один раз передать новую сумму.

**Ход решения**

Событие report наблюдает завершённый шаг, поэтому вызов расположен после сложения. Long предотвращает обычное переполнение int при суммировании; checked защищает обобщённый случай. Исключение callback прерывает метод, а возвращаемого итога при ошибке нет. Время O(n), дополнительное состояние O(1) без учёта наблюдателя.

**Частая ошибка**

Вызов callback до сложения сообщает предыдущую сумму и смещает все наблюдения.

Полный исходник: [solutions/Task08.cs](solutions/Task08.cs).

```csharp
using Course;

namespace Course.Lesson13;

public static class Task08
{
    public static long Accumulate(IReadOnlyList<int> values, Action<long> report)
    {
        ArgumentNullException.ThrowIfNull(values); ArgumentNullException.ThrowIfNull(report); long sum = 0;
        foreach (int value in values) { sum = checked(sum + value); report(sum); } return sum;
    }
    public static void Run()
    {
        var seen = new List<long>(); Check.Equal(5L, Accumulate(new[] { 2, -1, 4 }, seen.Add)); Check.SequenceEqual(new long[] { 2, 1, 5 }, seen);
        Check.Equal(4294967294L, Accumulate(new[] { int.MaxValue, int.MaxValue }, _ => { }));
        int calls = 0; Check.Equal(0L, Accumulate(Array.Empty<int>(), _ => calls++)); Check.Equal(0, calls);
        Check.Throws<FormatException>(() => Accumulate(new[] { 1, 2 }, _ => { calls++; throw new FormatException(); })); Check.Equal(1, calls);
        Check.Throws<ArgumentNullException>(() => Accumulate(new[] { 1 }, null!));
    }
}
```

**Исполняемые проверки**

- Положительные и отрицательные
- Сумма вне int
- Пустой ввод без callback
- Ошибка наблюдателя останавливает работу

<a id="13-09"></a>
## 13-09. Конвейер с внедрёнными действиями

Явно внедрить функции и сохранить результат преобразования в переменной.

**Ход решения**

Одна переменная результата исключает повторное normalize при логировании и возврате. Предварительная проверка log важна: неверный logger не должен обнаружиться лишь после уже выполненного normalize. Наблюдаемый порядок — normalize, затем log. Тест с недопустимым logger проверяет отсутствие ранних побочных эффектов.

**Частая ошибка**

Простой expression-bodied вызов log(normalize(text)) не позволяет ясно проверить null-результат и вернуть тот же объект.

Полный исходник: [solutions/Task09.cs](solutions/Task09.cs).

```csharp
using Course;

namespace Course.Lesson13;

public static class Task09
{
    public static string Process(string text, Func<string, string> normalize, Action<string> log)
    {
        ArgumentNullException.ThrowIfNull(text); ArgumentNullException.ThrowIfNull(normalize); ArgumentNullException.ThrowIfNull(log);
        var result = normalize(text) ?? throw new InvalidOperationException("Normalizer returned null."); log(result); return result;
    }
    public static void Run()
    {
        var trace = new List<string>(); var result = Process(" a ", s => { trace.Add("normalize"); return s.Trim().ToUpperInvariant(); }, s => trace.Add("log:" + s));
        Check.Equal("A", result); Check.SequenceEqual(new[] { "normalize", "log:A" }, trace);
        int calls = 0; Check.Throws<ArgumentNullException>(() => Process("x", s => { calls++; return s; }, null!)); Check.Equal(0, calls);
        Check.Throws<InvalidOperationException>(() => Process("x", _ => null!, _ => calls++)); Check.Equal(0, calls);
        Check.Throws<FormatException>(() => Process("x", _ => throw new FormatException(), _ => calls++)); Check.Equal(0, calls);
        Check.Throws<FormatException>(() => Process("x", s => s, _ => throw new FormatException()));
    }
}
```

**Исполняемые проверки**

- Порядок normalizer→logger
- По одному вызову
- Все аргументы проверяются заранее
- null-результат и ошибки зависимостей

<a id="13-10"></a>
## 13-10. Тест числа вызовов предиката

Считать положительные ответы, отдельно проверять взаимодействие с предикатом.

**Ход решения**

Чистый предикат даёт тот же bool при обоих обращениях, поэтому тест только результата пропускает дефект. На нечётном элементе && останавливается после первого false; на двух чётных вызывает предикат дважды. Получается 1+2+2=5 вместо 3. Пустой ввод гарантирует ноль вызовов.

**Частая ошибка**

Использование одного счётчика для правильного и ошибочного прогона смешивает наблюдения.

Полный исходник: [solutions/Task10.cs](solutions/Task10.cs).

```csharp
using Course;

namespace Course.Lesson13;

public static class Task10
{
    public static int CountPassing(int[] values, Predicate<int> accept)
    {
        ArgumentNullException.ThrowIfNull(values); ArgumentNullException.ThrowIfNull(accept); int count = 0;
        foreach (var x in values) if (accept(x)) count++; return count;
    }
    private static int Broken(int[] values, Predicate<int> accept) { int count = 0; foreach (var x in values) if (accept(x) && accept(x)) count++; return count; }
    public static void Run()
    {
        int goodCalls = 0; Check.Equal(2, CountPassing(new[] { 1, 2, 4 }, x => { goodCalls++; return x % 2 == 0; })); Check.Equal(3, goodCalls);
        int badCalls = 0; Check.Equal(2, Broken(new[] { 1, 2, 4 }, x => { badCalls++; return x % 2 == 0; })); Check.Equal(5, badCalls); Check.True(badCalls != goodCalls);
        int emptyCalls = 0; Check.Equal(0, CountPassing(Array.Empty<int>(), x => { emptyCalls++; return true; })); Check.Equal(0, emptyCalls);
        Check.Throws<ArgumentNullException>(() => CountPassing(new[] { 1 }, null!));
    }
}
```

**Исполняемые проверки**

- Правильный результат и 3 вызова
- Ошибочный вариант реально даёт 5 вызовов
- Пустой ввод без вызовов
- null предикат отвергнут

<a id="13-11"></a>
## 13-11. Многоадресный делегат и исключение

Стандартный multicast прекращается на ошибке; для изоляции нужен явный цикл с try/catch.

**Ход решения**

Список вызовов делегата исполняется последовательно. Исключение покидает весь Invoke, поэтому B не запускается. InvokeAll намеренно задаёт другую политику: перехват вокруг каждого отдельного вызова. Предварительная проверка элементов предотвращает частичное исполнение при ошибочной конфигурации. Время O(n) плюс обработчики, память O(e).

**Частая ошибка**

Один try вокруг всего foreach снова остановит обработку на первой ошибке.

Полный исходник: [solutions/Task11.cs](solutions/Task11.cs).

```csharp
using Course;

namespace Course.Lesson13;

public static class Task11
{
    public static List<Exception> InvokeAll(Action[] handlers)
    {
        if (handlers is null) throw new ArgumentException("Null array."); foreach (var handler in handlers) if (handler is null) throw new ArgumentException("Null handler.");
        var errors = new List<Exception>(); foreach (var handler in handlers) { try { handler(); } catch (Exception error) { errors.Add(error); } } return errors;
    }
    public static void Run()
    {
        var trace = new List<string>(); Action handlers = () => trace.Add("A"); handlers += () => throw new FormatException("bad"); handlers += () => trace.Add("B");
        Check.Throws<FormatException>(() => handlers()); Check.SequenceEqual(new[] { "A" }, trace); trace.Clear();
        var errors = InvokeAll(new Action[] { () => trace.Add("A"), () => throw new FormatException(), () => trace.Add("B"), () => throw new IOException() });
        Check.SequenceEqual(new[] { "A", "B" }, trace); Check.Equal(2, errors.Count); Check.True(errors[0] is FormatException); Check.True(errors[1] is IOException);
        int calls = 0; Check.Throws<ArgumentException>(() => InvokeAll(new Action[] { () => calls++, null! })); Check.Equal(0, calls);
        Check.Equal(0, InvokeAll(Array.Empty<Action>()).Count); Check.Throws<ArgumentException>(() => InvokeAll(null!));
    }
}
```

**Исполняемые проверки**

- Исходный multicast останавливается
- Изолированный обход продолжает B
- Две ошибки сохраняют порядок
- Конфигурация проверена до вызовов

<a id="13-12"></a>
## 13-12. Событие пересечения порога

Запомнить old, вычислить и проверить next, зафиксировать состояние, затем вызвать событие.

**Ход решения**

Проверка только Value&gt;=threshold отправляла бы уведомление на каждом шаге выше порога. Нам нужен факт перехода между областями. Состояние меняется до callback, поэтому подписчик видит актуальное Value. Сохранённый делегат позволяет корректно отписаться. Ошибка подписчика не откатывает завершённое изменение счётчика.

**Частая ошибка**

Для отписки нужна та же ссылка на делегат; новая похожая лямбда не снимает прежнюю подписку.

Полный исходник: [solutions/Task12.cs](solutions/Task12.cs).

```csharp
using Course;

namespace Course.Lesson13;

public static class Task12
{
    public sealed class Counter
    {
        private readonly int threshold;
        public Counter(int threshold) { if (threshold <= 0) throw new ArgumentOutOfRangeException(nameof(threshold)); this.threshold = threshold; }
        public int Value { get; private set; }
        public event Action<int>? Reached;
        public void Add(int delta)
        {
            int old = Value, next = checked(old + delta); if (next < 0) throw new ArgumentOutOfRangeException(nameof(delta));
            Value = next; if (old < threshold && next >= threshold) Reached?.Invoke(next);
        }
    }
    public static void Run()
    {
        var counter = new Counter(5); var seen = new List<int>(); Action<int> listener = v => { Check.Equal(v, counter.Value); seen.Add(v); }; counter.Reached += listener;
        counter.Add(5); counter.Add(1); counter.Add(-4); counter.Add(3); Check.SequenceEqual(new[] { 5, 5 }, seen);
        counter.Reached -= listener; counter.Add(-3); counter.Add(3); Check.Equal(2, seen.Count);
        Check.Throws<ArgumentOutOfRangeException>(() => counter.Add(-6)); Check.Equal(5, counter.Value);
        Check.Throws<OverflowException>(() => counter.Add(int.MaxValue)); Check.Equal(5, counter.Value);
        var broken = new Counter(1); broken.Reached += _ => throw new FormatException(); Check.Throws<FormatException>(() => broken.Add(1)); Check.Equal(1, broken.Value);
        Check.Throws<ArgumentOutOfRangeException>(() => new Counter(0));
    }
}
```

**Исполняемые проверки**

- Одно уведомление при росте выше порога
- Повторное пересечение после спуска
- Отписка
- Неверные границы, переполнение и ошибка подписчика

<a id="13-13"></a>
## 13-13. Мемоизация успешных вычислений

Сохранить Dictionary в замыкании, TryGetValue, вычислить, затем Add.

**Ход решения**

Неуспешный compute не доходит до Add, поэтому следующий вызов повторит попытку. TryGetValue отделяет наличие записи от значения, сохраняя кэшированный ноль. Каждый Memoize создаёт новый словарь. Средняя стоимость попадания O(1), память O(k) разных успешных аргументов; эффект безопасен лишь при согласованной политике чистоты функции.

**Частая ошибка**

Использование 0 как признака отсутствия повторяет ошибку с default в контейнерах.

Полный исходник: [solutions/Task13.cs](solutions/Task13.cs).

```csharp
using Course;

namespace Course.Lesson13;

public static class Task13
{
    public static Func<int, int> Memoize(Func<int, int> compute)
    {
        ArgumentNullException.ThrowIfNull(compute); var cache = new Dictionary<int, int>();
        return key => { if (cache.TryGetValue(key, out var value)) return value; value = compute(key); cache.Add(key, value); return value; };
    }
    public static void Run()
    {
        int calls = 0; var memo = Memoize(x => { calls++; return x * x; }); Check.Equal(9, memo(3)); Check.Equal(9, memo(3)); Check.Equal(1, calls);
        Check.Equal(0, memo(0)); Check.Equal(0, memo(0)); Check.Equal(2, calls);
        int attempts = 0; var bad = Memoize(_ => { attempts++; throw new FormatException(); }); Check.Throws<FormatException>(() => bad(1)); Check.Throws<FormatException>(() => bad(1)); Check.Equal(2, attempts);
        var other = Memoize(x => { calls++; return x; }); Check.Equal(3, other(3)); Check.Equal(3, calls);
        Check.Throws<ArgumentNullException>(() => Memoize(null!));
    }
}
```

**Исполняемые проверки**

- Повтор и новый ключ
- Кэшируется ноль
- Исключение повторяется без записи
- Независимость кэшей и null

<a id="13-14"></a>
## 13-14. Составное правило с коротким замыканием

Проверить и скопировать массив делегатов; выполнить последовательный AND.

**Ход решения**

Короткое замыкание может быть частью корректности: второе правило вправе предполагать успешность первого. Копия массива защищает состав конвейера от последующей замены делегата вызывающим кодом. Состояние самих замыканий при этом не копируется. Время вызова O(k) до первого отказа, память O(k) на конфигурацию.

**Частая ошибка**

Оператор & вместо && либо предварительное вычисление всех правил уничтожает короткое замыкание.

Полный исходник: [solutions/Task14.cs](solutions/Task14.cs).

```csharp
using Course;

namespace Course.Lesson13;

public static class Task14
{
    public static Predicate<T> All<T>(params Predicate<T>[] rules)
    {
        if (rules is null) throw new ArgumentException("Null rules."); foreach (var rule in rules) if (rule is null) throw new ArgumentException("Null rule.");
        var copy = (Predicate<T>[])rules.Clone(); return value => { foreach (var rule in copy) if (!rule(value)) return false; return true; };
    }
    public static void Run()
    {
        int later = 0; var both = All<int>(x => x > 0, x => { later++; return x % 2 == 0; }); Check.True(!both(-2)); Check.Equal(0, later); Check.True(both(2)); Check.Equal(1, later);
        Check.True(All<int>()(0)); Predicate<int>[] source = { x => x > 0 }; var saved = All(source); source[0] = _ => false; Check.True(saved(1));
        Check.Throws<ArgumentException>(() => All<int>(null!)); Check.Throws<ArgumentException>(() => All<int>(new Predicate<int>[] { null! }));
        var error = All<int>(_ => throw new FormatException(), _ => { later++; return true; }); Check.Throws<FormatException>(() => error(1)); Check.Equal(1, later);
    }
}
```

**Исполняемые проверки**

- Первый false останавливает следующее правило
- Успешный набор и пустой набор
- Замена исходного массива не влияет
- null и исключение правила

<a id="13-15"></a>
## 13-15. Реестр команд

Разделить проверку имени, регистрацию и запуск через TryGetValue.

**Ход решения**

Comparer задаёт единое правило и для регистрации, и для поиска. TryAdd либо ContainsKey предотвращает тихую замену существующей команды. Выполнение не содержит switch по конкретным именам, поэтому новая команда добавляется регистрацией. Проверки аргументов выполняются даже для неизвестной команды. Средний поиск O(1), память O(k).

**Частая ошибка**

Присваивание handlers[name]=handler незаметно перезаписывает регистрацию, нарушая контракт.

Полный исходник: [solutions/Task15.cs](solutions/Task15.cs).

```csharp
using Course;

namespace Course.Lesson13;

public static class Task15
{
    public sealed class CommandRouter
    {
        private readonly Dictionary<string, Action<IReadOnlyList<string>>> handlers = new(StringComparer.OrdinalIgnoreCase);
        private static void Validate(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length > 20) throw new ArgumentException("Name.");
            foreach (char c in name) if (!(c >= 'a' && c <= 'z') && !(c >= 'A' && c <= 'Z')) throw new ArgumentException("Name.");
        }
        public void Register(string name, Action<IReadOnlyList<string>> handler)
        {
            Validate(name); ArgumentNullException.ThrowIfNull(handler); if (!handlers.TryAdd(name, handler)) throw new InvalidOperationException("Duplicate.");
        }
        public bool Execute(string name, IReadOnlyList<string> args)
        {
            Validate(name); ArgumentNullException.ThrowIfNull(args); if (!handlers.TryGetValue(name, out var handler)) return false; handler(args); return true;
        }
    }
    public static void Run()
    {
        var router = new CommandRouter(); var log = new List<string>(); router.Register("echo", args => log.Add(string.Join(" ", args))); router.Register("count", args => log.Add(args.Count.ToString()));
        Check.True(router.Execute("ECHO", new[] { "a", "b" })); Check.True(router.Execute("count", Array.Empty<string>())); Check.SequenceEqual(new[] { "a b", "0" }, log);
        Check.True(!router.Execute("help", Array.Empty<string>())); Check.Equal(2, log.Count);
        Check.Throws<InvalidOperationException>(() => router.Register("Echo", _ => { })); Check.True(router.Execute("echo", new[] { "kept" })); Check.Equal("kept", log[2]);
        Check.Throws<ArgumentException>(() => router.Execute("bad name", Array.Empty<string>())); Check.Throws<ArgumentNullException>(() => router.Execute("help", null!));
        Check.Throws<ArgumentNullException>(() => router.Register("null", null!)); router.Register("fail", _ => throw new FormatException()); Check.Throws<FormatException>(() => router.Execute("fail", Array.Empty<string>()));
        var boundaryName = new string('a', 20); router.Register(boundaryName, _ => { }); Check.True(router.Execute(boundaryName, Array.Empty<string>()));
        foreach (var name in new[] { "", "echo1", "эхо", new string('a', 21) }) Check.Throws<ArgumentException>(() => router.Register(name, _ => { }));
    }
}
```

**Исполняемые проверки**

- Независимые echo/count команды
- Регистр и неизвестная команда
- Дубликат не перезаписывает
- Неверное имя, null и исключение обработчика
- Имя длины20принимается;21,цифра,кириллица и пустота отвергаются
