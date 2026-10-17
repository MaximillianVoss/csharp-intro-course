# Урок 9. Интерфейсы, наследование и подстановка: решения

Сначала выполните свою попытку по TASKS.md. Здесь приведены ответы.
Каждое решение запускается по ID; Run содержит исполняемые проверки.

<a id="09-01"></a>
## 09-01. Общая операция разных фигур

Интерфейс гарантирует Area; цикл работает с любым его исполнителем.

**Ход решения**

Переменная IShape не сообщает геометрию, но сообщает доступную операцию. Конкретная формула выбирается реализацией свойства. Новую фигуру можно добавить без изменения TotalArea, если она выполняет тот же контракт. Размеры ограничены, NaN и Infinity не подаются. O(n), память O(1).

**Частая ошибка**

Проверять is Rectangle и перечислять все типы; делить целочисленно; считать интерфейс создаваемым объектом.

Полный исходник: [solutions/Task01.cs](solutions/Task01.cs).

```csharp
namespace Course.Lesson09;

public static class Task01
{
    public interface IShape { double Area { get; } }
    public sealed class Rectangle : IShape
    {
        private readonly double width, height;
        public Rectangle(double width, double height) { this.width = width; this.height = height; }
        public double Area => width * height;
    }
    public sealed class Triangle : IShape
    {
        private readonly double width, height;
        public Triangle(double width, double height) { this.width = width; this.height = height; }
        public double Area => width * height / 2;
    }
    public static double TotalArea(IShape[] shapes) { double sum = 0; foreach (IShape shape in shapes) sum += shape.Area; return sum; }
    public static void Run()
    {
        Check.Near(18, TotalArea(new IShape[] { new Rectangle(3, 4), new Triangle(3, 4) }));
        Check.Near(0, TotalArea(Array.Empty<IShape>())); Check.Near(0, new Rectangle(0, 5).Area);
        Check.Near(1.5, new Triangle(1, 3).Area);
    }
}
```

**Исполняемые проверки**

- Смешанные фигуры
- Пустой набор
- Нулевая площадь
- Треугольник с дробной площадью

<a id="09-02"></a>
## 09-02. Скрытие метода и переопределение

new скрывает член по статическому типу переменной; override меняет поведение виртуального вызова.

**Ход решения**

parent и derived ссылаются на один объект. Describe не виртуальный, поэтому parent.Describe выбирает реализацию Printer. Render виртуальный и диспетчеризуется к FancyPrinter. derived.Describe видит скрывающий член FancyPrinter. Точное выполнение фрагмента проверяет все три строки, а ReferenceEquals подтверждает общий объект.

**Частая ошибка**

Считать new эквивалентом override; объяснять результат разными объектами.

Полный исходник: [solutions/Task02.cs](solutions/Task02.cs).

```csharp
namespace Course.Lesson09;

public static class Task02
{
    public class Printer
    {
        public string Describe() => "base description";
        public virtual string Render() => "base render";
    }
    public sealed class FancyPrinter : Printer
    {
        public new string Describe() => "fancy description";
        public override string Render() => "fancy render";
    }
    public static string[] Replay()
    {
        FancyPrinter derived = new(); Printer parent = derived;
        Check.True(ReferenceEquals(parent, derived));
        return new[] { parent.Describe(), parent.Render(), derived.Describe() };
    }
    public static void Run() => Check.SequenceEqual(new[] { "base description", "fancy render", "fancy description" }, Replay());
}
```

**Исполняемые проверки**

- Все три строки точного фрагмента
- Обе переменные ссылаются на один объект

<a id="09-03"></a>
## 09-03. Налоговая политика как зависимость

Сохраните интерфейсную зависимость в readonly-поле и используйте её в Total.

**Ход решения**

Конструктор делает зависимость явной: объект без политики не создаётся по предусловию. Одна Invoice работает с любым подходящим расчётом. Политика возвращает только налог, поэтому Total добавляет net ровно один раз. decimal даёт точный результат десятичных примеров. O(1).

**Частая ошибка**

Скрыто создать RateTax внутри Invoice; политика возвращает полную сумму вместо налога, приводя к двойному net.

Полный исходник: [solutions/Task03.cs](solutions/Task03.cs).

```csharp
namespace Course.Lesson09;

public static class Task03
{
    public interface ITaxPolicy { decimal Tax(decimal net); }
    public sealed class NoTax : ITaxPolicy { public decimal Tax(decimal net) => 0; }
    public sealed class RateTax : ITaxPolicy
    {
        private readonly decimal rate;
        public RateTax(decimal rate) { this.rate = rate; }
        public decimal Tax(decimal net) => net * rate;
    }
    public sealed class Invoice
    {
        private readonly ITaxPolicy policy;
        public Invoice(ITaxPolicy policy) { this.policy = policy; }
        public decimal Total(decimal net) => net + policy.Tax(net);
    }
    public static void Run()
    {
        Check.Equal(120m, new Invoice(new RateTax(0.2m)).Total(100));
        Check.Equal(100m, new Invoice(new NoTax()).Total(100));
        Check.Equal(0m, new Invoice(new RateTax(1)).Total(0));
        Check.Equal(1.65m, new Invoice(new RateTax(0.1m)).Total(1.5m));
    }
}
```

**Исполняемые проверки**

- Ставка 20%
- Отсутствие налога
- Нулевой net
- Дробная сумма

<a id="09-04"></a>
## 09-04. Одинаковая команда разным устройствам

Исполняющая сторона знает только ICommand; параметры хранят сами объекты команд.

**Ход решения**

Команда объединяет будущую операцию и её параметры. ExecuteAll не должен разбирать тип: он вызывает единый контракт. Считающая тестовая команда фиксирует, что вызов был ровно один раз, а не при построении и ещё раз при выдаче. O(n), память O(n).

**Частая ошибка**

Печатать вместо возврата результата; вызывать Execute повторно для проверки; менять порядок.

Полный исходник: [solutions/Task04.cs](solutions/Task04.cs).

```csharp
namespace Course.Lesson09;

public static class Task04
{
    public interface ICommand { string Execute(); }
    public sealed class EchoCommand : ICommand
    {
        private readonly string text; public EchoCommand(string text) { this.text = text; }
        public string Execute() => text;
    }
    public sealed class SumCommand : ICommand
    {
        private readonly int left, right;
        public SumCommand(int left, int right) { this.left = left; this.right = right; }
        public string Execute() => (left + right).ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
    private sealed class CountingCommand : ICommand
    {
        public int Calls { get; private set; }
        public string Execute() { Calls++; return "counted"; }
    }
    public static string[] ExecuteAll(ICommand[] commands)
    {
        string[] result = new string[commands.Length];
        for (int i = 0; i < commands.Length; i++) result[i] = commands[i].Execute();
        return result;
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { "готово", "5" }, ExecuteAll(new ICommand[] { new EchoCommand("готово"), new SumCommand(2, 3) }));
        Check.SequenceEqual(new[] { "-3" }, ExecuteAll(new ICommand[] { new SumCommand(-5, 2) }));
        Check.Equal(0, ExecuteAll(Array.Empty<ICommand>()).Length);
        CountingCommand command = new(); ExecuteAll(new ICommand[] { command }); Check.Equal(1, command.Calls);
    }
}
```

**Исполняемые проверки**

- Порядок разных команд
- Отрицательная сумма
- Пустой массив
- Каждая команда выполнена один раз

<a id="09-05"></a>
## 09-05. Часы для повторяемого теста

Зависимость IClock даёт один и тот же результат в каждом запуске; границу проверяют разные фиксированные часы.

**Ход решения**

Если Greeting обращается к DateTime.Now, тест зависит от момента запуска. FixedClock заменяет внешнее время данными, контролируемыми тестом. 11:59 проверяет нижнюю сторону, 12:00 различает < и <=, 23:59 — другую часть суток. O(1). Ошибочный предикат вычисляется в тесте и даёт неверное "утро".

**Частая ошибка**

Проверять только утро при утреннем запуске; подменить FixedClock, но всё равно читать DateTime.Now внутри сервиса.

Полный исходник: [solutions/Task05.cs](solutions/Task05.cs).

```csharp
namespace Course.Lesson09;

public static class Task05
{
    public interface IClock { DateTime Now { get; } }
    public sealed class FixedClock : IClock
    {
        public DateTime Now { get; }
        public FixedClock(DateTime now) { Now = now; }
    }
    public sealed class Greeting
    {
        private readonly IClock clock;
        public Greeting(IClock clock) { this.clock = clock; }
        public string Get() => clock.Now.Hour < 12 ? "утро" : "день";
    }
    public static void Run()
    {
        Check.Equal("утро", new Greeting(new FixedClock(new DateTime(2030, 1, 1, 0, 0, 0))).Get());
        Check.Equal("утро", new Greeting(new FixedClock(new DateTime(2030, 1, 1, 11, 59, 0))).Get());
        FixedClock noon = new(new DateTime(2030, 1, 1, 12, 0, 0));
        Check.Equal("день", new Greeting(noon).Get());
        Check.Equal("день", new Greeting(new FixedClock(new DateTime(2030, 1, 1, 23, 59, 0))).Get());
        string wrong = noon.Now.Hour <= 12 ? "утро" : "день";
        Check.True(wrong != new Greeting(noon).Get());
    }
}
```

**Исполняемые проверки**

- Четыре фиксированных времени
- Граница 12:00 различает <=
- Ни одного обращения к реальному времени

<a id="09-06"></a>
## 09-06. Общая рамка расчёта доставки

База фиксирует общий порядок действий, наследники задают только отличающиеся части.

**Ход решения**

Интерфейс задаёт контракт, а абстрактный класс может ещё разделять реализацию. Не виртуальный Quote защищает единый учёт минимума: наследник не заменяет весь алгоритм. protected ограничивает детали расчёта и оставляет снаружи понятную Quote. Все вычисления O(1).

**Частая ошибка**

Дублировать полный Quote в обоих классах и забыть минимум в одном; пытаться создать ShippingPolicy напрямую.

Полный исходник: [solutions/Task06.cs](solutions/Task06.cs).

```csharp
namespace Course.Lesson09;

public static class Task06
{
    public abstract class ShippingPolicy
    {
        protected abstract decimal BaseFee { get; }
        protected abstract decimal Minimum { get; }
        protected abstract decimal VariableCost(decimal kilograms);
        public decimal Quote(decimal kilograms) => Math.Max(Minimum, BaseFee + VariableCost(kilograms));
    }
    public sealed class Ground : ShippingPolicy
    {
        protected override decimal BaseFee => 20;
        protected override decimal Minimum => 50;
        protected override decimal VariableCost(decimal kilograms) => 5 * kilograms;
    }
    public sealed class Air : ShippingPolicy
    {
        protected override decimal BaseFee => 100;
        protected override decimal Minimum => 100;
        protected override decimal VariableCost(decimal kilograms) => 20 * kilograms;
    }
    public static void Run()
    {
        ShippingPolicy ground = new Ground(); ShippingPolicy air = new Air();
        Check.Equal(50m, ground.Quote(2)); Check.Equal(70m, ground.Quote(10));
        Check.Equal(140m, air.Quote(2)); Check.Equal(100m, air.Quote(0));
    }
}
```

**Исполняемые проверки**

- Минимум для лёгкого груза
- Переход выше минимума
- Другая формула Air
- Нулевой вес

<a id="09-07"></a>
## 09-07. Перегрузка не заменила виртуальный метод

Добавьте public override string Speak() с той же сигнатурой, что у базового метода.

**Ход решения**

Speak(string) и Speak() имеют разные списки параметров. Исходный метод добавляет перегрузку, не переопределяя виртуальный контракт без аргументов. override заставляет компилятор проверить совпадение сигнатуры. Тесты вызывают исходный тип и исправленный через Animal, затем массив разных Animal для проверки диспетчеризации.

**Частая ошибка**

Добавить optional-параметр вместо override; проверить только вызов BrokenDog.Speak("!") и не заметить базовый путь.

Полный исходник: [solutions/Task07.cs](solutions/Task07.cs).

```csharp
namespace Course.Lesson09;

public static class Task07
{
    public class Animal { public virtual string Speak() => "unknown"; }
    public sealed class BrokenDog : Animal { public string Speak(string suffix) => "woof" + suffix; }
    public sealed class Dog : Animal { public override string Speak() => "woof"; }
    public static void Run()
    {
        Animal broken = new BrokenDog(); Check.Equal("unknown", broken.Speak());
        Check.Equal("woof!", new BrokenDog().Speak("!"));
        Animal fixedDog = new Dog(); Check.Equal("woof", fixedDog.Speak());
        Animal[] animals = { new Animal(), new Dog() }; Check.Equal("unknown", animals[0].Speak()); Check.Equal("woof", animals[1].Speak());
    }
}
```

**Исполняемые проверки**

- Исходный базовый вызов unknown
- Исходная перегрузка действительно woof!
- Исправленный виртуальный вызов woof
- Смешанный массив базовых ссылок

<a id="09-08"></a>
## 09-08. Узкий интерфейс для читателя

Разделите чтение и запись; используйте на границе минимально необходимый интерфейс.

**Ход решения**

Потребитель чтения не должен требовать бесполезную реализацию Write. Один MemoryName может реализовать оба контракта, а ReadOnlyName — только IReadName. Компилятор подтверждает, что BuildHeading не зависит от изменяемости. Вызов Read выполняется один раз. Сложность O(длина строки) на построение заголовка.

**Частая ошибка**

Сделать Write пустым в read-only типе, сохранив слишком широкий интерфейс; приводить source к MemoryName.

Полный исходник: [solutions/Task08.cs](solutions/Task08.cs).

```csharp
namespace Course.Lesson09;

public static class Task08
{
    public interface IReadName { string Read(); }
    public interface IWriteName { void Write(string value); }
    public sealed class MemoryName : IReadName, IWriteName
    {
        private string value; public MemoryName(string initial) { value = initial; }
        public string Read() => value; public void Write(string value) { this.value = value; }
    }
    public sealed class ReadOnlyName : IReadName
    {
        private readonly string value; public ReadOnlyName(string value) { this.value = value; }
        public string Read() => value;
    }
    public static string BuildHeading(IReadName source) => "Имя: " + source.Read();
    public static void Run()
    {
        MemoryName storage = new("A"); Check.Equal("Имя: A", BuildHeading(storage));
        IWriteName writer = storage; writer.Write("B"); Check.Equal("Имя: B", BuildHeading(storage));
        Check.Equal("Имя: C", BuildHeading(new ReadOnlyName("C"))); Check.Equal("Имя: ", BuildHeading(new ReadOnlyName("")));
    }
}
```

**Исполняемые проверки**

- Чтение из изменяемого хранилища
- После записи виден новый текст
- Тип только для чтения принимается
- Пустая строка остаётся допустимой

<a id="09-09"></a>
## 09-09. Интерфейс не отменяет владение данными

Клонируйте массив при Save и при каждом Load; сам интерфейс не создаёт копию автоматически.

**Ход решения**

Абстракция определяет не только сигнатуры, но и обещания о владении. Копия только на выходе не защищает от изменения переданного массива, копия только на входе не защищает от изменения загруженного. Для int[] Clone достаточно; массив ссылочных изменяемых объектов потребовал бы другого контракта. O(n) для Save/Load.

**Частая ошибка**

Возвращать внутренний массив через интерфейс, считая его скрытым; копировать только в одном направлении.

Полный исходник: [solutions/Task09.cs](solutions/Task09.cs).

```csharp
namespace Course.Lesson09;

public static class Task09
{
    public interface IIntRepository { void Save(int[] values); int[] Load(); }
    public sealed class MemoryRepository : IIntRepository
    {
        private int[] data = Array.Empty<int>();
        public void Save(int[] values) { data = (int[])values.Clone(); }
        public int[] Load() => (int[])data.Clone();
    }
    public static void Run()
    {
        IIntRepository repository = new MemoryRepository(); Check.Equal(0, repository.Load().Length);
        int[] input = { 1, 2 }; repository.Save(input); input[0] = 9;
        Check.SequenceEqual(new[] { 1, 2 }, repository.Load());
        int[] loaded = repository.Load(); loaded[1] = 8; Check.SequenceEqual(new[] { 1, 2 }, repository.Load());
        repository.Save(new[] { 7 }); Check.SequenceEqual(new[] { 7 }, repository.Load());
    }
}
```

**Исполняемые проверки**

- Пустое начальное состояние
- Копия при сохранении
- Копия при чтении
- Полная замена данных

<a id="09-10"></a>
## 09-10. Один контракт для листа и папки

Папка сама реализует INode и делегирует вычисление каждому ребёнку.

**Ход решения**

Общий контракт позволяет папке содержать и файлы, и другие папки. Рекурсия следует структуре данных и завершается на листьях. Дерево и допустимая глубина — явное предусловие, защиту от циклов здесь не реализуем. long вмещает суммарный размер. Вычисление Size O(число узлов), стек O(глубина).

**Частая ошибка**

Хранить List<FileNode> и потерять возможность вложенности; жёстко поддержать только два уровня; создавать цикл папки с собой.

Полный исходник: [solutions/Task10.cs](solutions/Task10.cs).

```csharp
namespace Course.Lesson09;

public static class Task10
{
    public interface INode { long Size { get; } }
    public sealed class FileNode : INode
    {
        public long Size { get; } public FileNode(long size) { Size = size; }
    }
    public sealed class FolderNode : INode
    {
        private readonly List<INode> children = new();
        public void Add(INode node) { children.Add(node); }
        public long Size { get { long total = 0; foreach (INode child in children) total += child.Size; return total; } }
    }
    public static void Run()
    {
        FolderNode root = new(); FolderNode nested = new(); nested.Add(new FileNode(5)); nested.Add(new FileNode(7));
        root.Add(new FileNode(3)); root.Add(nested); INode view = root; Check.Equal(15L, view.Size);
        Check.Equal(0L, new FolderNode().Size); nested.Add(new FileNode(1)); Check.Equal(16L, root.Size);
        FolderNode large = new(); for (int i = 0; i < 3; i++) large.Add(new FileNode(1_000_000_000));
        Check.Equal(3_000_000_000L, large.Size);
    }
}
```

**Исполняемые проверки**

- Вложенное дерево
- Пустая папка
- Добавление после первого расчёта видно
- Размер через интерфейс
- Сумма больше int.MaxValue

<a id="09-11"></a>
## 09-11. Добавление поведения обёрткой

Декоратор реализует тот же интерфейс, что оборачиваемый объект, и выполняет свой шаг после внутреннего.

**Ход решения**

В первой цепочке сначала обрезается a, затем добавляется префикс с пробелом слева. Во второй сначала появляется строка " >   a  ", затем внешнее Trim удаляет только внешние края: получается ">   a". Композиция позволяет собрать поведение без растущего числа подклассов. Для k обёрток и результирующей длины L затраты порядка O(kL).

**Частая ошибка**

Обрезать вход вместо результата inner; забыть вызов inner; считать композицию коммутативной.

Полный исходник: [solutions/Task11.cs](solutions/Task11.cs).

```csharp
namespace Course.Lesson09;

public static class Task11
{
    public interface ITextTransform { string Transform(string text); }
    public sealed class Identity : ITextTransform { public string Transform(string text) => text; }
    public sealed class TrimDecorator : ITextTransform
    {
        private readonly ITextTransform inner; public TrimDecorator(ITextTransform inner) { this.inner = inner; }
        public string Transform(string text) => inner.Transform(text).Trim();
    }
    public sealed class PrefixDecorator : ITextTransform
    {
        private readonly ITextTransform inner; private readonly string prefix;
        public PrefixDecorator(ITextTransform inner, string prefix) { this.inner = inner; this.prefix = prefix; }
        public string Transform(string text) => prefix + inner.Transform(text);
    }
    public static void Run()
    {
        ITextTransform first = new PrefixDecorator(new TrimDecorator(new Identity()), " > ");
        ITextTransform second = new TrimDecorator(new PrefixDecorator(new Identity(), " > "));
        Check.Equal(" > a", first.Transform("  a  ")); Check.Equal(">   a", second.Transform("  a  "));
        Check.Equal(" x ", new Identity().Transform(" x ")); Check.Equal(" > ", first.Transform(""));
        Check.Equal("abx", new PrefixDecorator(new PrefixDecorator(new Identity(), "b"), "a").Transform("x"));
    }
}
```

**Исполняемые проверки**

- Оба порядка с точными пробелами
- Identity сохраняет текст
- Пустая строка
- Вложенные префиксы

<a id="09-12"></a>
## 09-12. Резервный источник вызывается по необходимости

Признаком наличия служит bool, а не содержимое строки; используйте короткий путь по успешному первому результату.

**Ход решения**

Пустое значение и отсутствие — разные состояния контракта. Проверка IsNullOrEmpty неверно отправила бы найденный пустой A в резерв. Две зависимости объединяются через интерфейс без знания о словаре. Тестовый счётчик обнаруживает лишний запрос к backup даже если окончательный текст случайно верен. Ожидаемое O(1) для словарей.

**Частая ошибка**

Вызвать оба источника заранее; считать пустой результат отсутствием; оставить out неопределённым при false.

Полный исходник: [solutions/Task12.cs](solutions/Task12.cs).

```csharp
namespace Course.Lesson09;

public static class Task12
{
    public interface IValueSource { bool TryGet(string key, out string value); }
    public sealed class DictionarySource : IValueSource
    {
        private readonly Dictionary<string, string> data;
        public DictionarySource(Dictionary<string, string> data) { this.data = data; }
        public bool TryGet(string key, out string value)
        {
            if (data.TryGetValue(key, out string? found)) { value = found; return true; }
            value = ""; return false;
        }
    }
    public sealed class FallbackSource : IValueSource
    {
        private readonly IValueSource primary, backup;
        public FallbackSource(IValueSource primary, IValueSource backup) { this.primary = primary; this.backup = backup; }
        public bool TryGet(string key, out string value)
        {
            if (primary.TryGet(key, out value)) return true;
            return backup.TryGet(key, out value);
        }
    }
    private sealed class CountedSource : IValueSource
    {
        private readonly IValueSource inner; public int Calls { get; private set; }
        public CountedSource(IValueSource inner) { this.inner = inner; }
        public bool TryGet(string key, out string value) { Calls++; return inner.TryGet(key, out value); }
    }
    public static void Run()
    {
        CountedSource primary = new(new DictionarySource(new(StringComparer.Ordinal) { ["A"] = "" }));
        CountedSource backup = new(new DictionarySource(new(StringComparer.Ordinal) { ["A"] = "запас", ["B"] = "второй" }));
        IValueSource source = new FallbackSource(primary, backup);
        Check.True(source.TryGet("A", out string a)); Check.Equal("", a); Check.Equal(0, backup.Calls);
        Check.True(source.TryGet("B", out string b)); Check.Equal("второй", b); Check.Equal(1, backup.Calls);
        Check.True(!source.TryGet("C", out string c)); Check.Equal("", c);
        Check.Equal(3, primary.Calls); Check.Equal(2, backup.Calls);
    }
}
```

**Исполняемые проверки**

- Найденная пустая строка не идёт в backup
- Промах primary использует backup
- Отсутствие в обоих
- Точные числа вызовов

<a id="09-13"></a>
## 09-13. Два представления одного отчёта

Запишите string ICompactReport.Render() и string IDetailedReport.Render(), затем получите интерфейсные ссылки.

**Ход решения**

Один объект может предоставлять разные контракты с совпадающим именем члена. Явная реализация делает выбор представления частью статического типа ссылки и предотвращает неоднозначный прямой вызов. В обычном проекте часто проще дать говорящие разные имена; здесь именно изучается разрешение совпадения. Форматирование O(длина заголовка).

**Частая ошибка**

Одна публичная Render реализует оба интерфейса одинаково; ожидать прямого вызова Report.Render при явной реализации.

Полный исходник: [solutions/Task13.cs](solutions/Task13.cs).

```csharp
namespace Course.Lesson09;

public static class Task13
{
    public interface ICompactReport { string Render(); }
    public interface IDetailedReport { string Render(); }
    public sealed class Report : ICompactReport, IDetailedReport
    {
        private readonly string title; private readonly int count;
        public Report(string title, int count) { this.title = title; this.count = count; }
        string ICompactReport.Render() => title;
        string IDetailedReport.Render() => title + ": " + count.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
    public static string[] BothViews(Report report)
    {
        ICompactReport compact = report; IDetailedReport detailed = report;
        return new[] { compact.Render(), detailed.Render() };
    }
    public static void Run()
    {
        Report report = new("Задачи", 3); Check.SequenceEqual(new[] { "Задачи", "Задачи: 3" }, BothViews(report));
        Check.SequenceEqual(new[] { "", ": 0" }, BothViews(new Report("", 0)));
        ICompactReport compact = report; IDetailedReport detailed = report; Check.True(ReferenceEquals(compact, detailed));
    }
}
```

**Исполняемые проверки**

- Два разных представления
- Нулевой count
- Пустой заголовок
- Один и тот же объект доступен по двум контрактам

<a id="09-14"></a>
## 09-14. Совпадение сигнатур ещё не очередь

Общий проверяющий сценарий наблюдает порядок, а не внутреннюю структуру реализации.

**Ход решения**

Компилятор проверяет имена и типы операций, но не гарантирует FIFO. Вход с несколькими разными числами и перемежением добавлений обнаруживает замену очереди стеком; единственный элемент её не обнаружит. Возвращаем bool о соответствии, а внешние Check.True проверяют ожидаемый исход обеих реализаций. O(k) для сценария, память O(k).

**Частая ошибка**

Проверять только Count или одну пару Add/Take; тестировать конкретное поле Queue вместо интерфейса.

Полный исходник: [solutions/Task14.cs](solutions/Task14.cs).

```csharp
namespace Course.Lesson09;

public static class Task14
{
    public interface IIntQueue { void Add(int value); bool TryTake(out int value); }
    public sealed class FifoQueue : IIntQueue
    {
        private readonly Queue<int> data = new(); public void Add(int value) { data.Enqueue(value); }
        public bool TryTake(out int value) => data.TryDequeue(out value);
    }
    public sealed class LifoQueue : IIntQueue
    {
        private readonly Stack<int> data = new(); public void Add(int value) { data.Push(value); }
        public bool TryTake(out int value) => data.TryPop(out value);
    }
    public static bool SatisfiesContract(IIntQueue queue)
    {
        if (queue.TryTake(out int empty) || empty != 0) return false;
        queue.Add(1); queue.Add(2);
        if (!queue.TryTake(out int first) || first != 1) return false;
        queue.Add(3);
        if (!queue.TryTake(out int second) || second != 2) return false;
        if (!queue.TryTake(out int third) || third != 3) return false;
        return !queue.TryTake(out int last) && last == 0;
    }
    public static void Run()
    {
        Check.True(SatisfiesContract(new FifoQueue())); Check.True(!SatisfiesContract(new LifoQueue()));
        IIntQueue queue = new FifoQueue(); Check.True(!queue.TryTake(out int absent)); Check.Equal(0, absent);
        queue.Add(0); Check.True(queue.TryTake(out int present)); Check.Equal(0, present);
    }
}
```

**Исполняемые проверки**

- Правильная реализация проходит общий сценарий
- Неправильная LIFO отвергается
- Пустота возвращает 0
- Значение 0 не путается с отсутствием

<a id="09-15"></a>
## 09-15. Локальная служба уведомлений

Сервис только координирует интерфейсы, а тестовые реализации собираются вручную в Run.

**Ход решения**

Разделение источника, шаблона и канала позволяет проверить приложение без настоящей доставки. false — ожидаемый локальный отказ, он не должен оборвать цикл. Дубликаты сохраняются, поскольку контракт задаёт попытки, а не множество адресов. Отчёт считается по реальным результатам каждого TrySend. O(n) плюс построение текста; память зависит от тестового журнала.

**Частая ошибка**

Использовать All с коротким замыканием или return при первом false; создавать зависимости внутри сервиса; убрать дубликаты без требования.

Полный исходник: [solutions/Task15.cs](solutions/Task15.cs).

```csharp
namespace Course.Lesson09;

public static class Task15
{
    public interface IRecipients { string[] Get(); }
    public interface IMessageTemplate { string Build(string recipient); }
    public interface IChannel { bool TrySend(string recipient, string message); }
    public readonly record struct DeliverySummary(int Attempted, int Succeeded);
    public sealed class NotificationService
    {
        private readonly IRecipients recipients; private readonly IMessageTemplate template; private readonly IChannel channel;
        public NotificationService(IRecipients recipients, IMessageTemplate template, IChannel channel)
        { this.recipients = recipients; this.template = template; this.channel = channel; }
        public DeliverySummary SendAll()
        {
            string[] targets = recipients.Get(); int succeeded = 0;
            foreach (string target in targets) if (channel.TrySend(target, template.Build(target))) succeeded++;
            return new DeliverySummary(targets.Length, succeeded);
        }
    }
    private sealed class FixedRecipients : IRecipients
    {
        private readonly string[] values; public int Calls { get; private set; }
        public FixedRecipients(string[] values) { this.values = values; }
        public string[] Get() { Calls++; return (string[])values.Clone(); }
    }
    private sealed class GreetingTemplate : IMessageTemplate { public string Build(string recipient) => "Привет, " + recipient; }
    private sealed class RecordingChannel : IChannel
    {
        public List<string> Attempts { get; } = new();
        public bool TrySend(string recipient, string message) { Attempts.Add(recipient + "|" + message); return recipient != "b"; }
    }
    public static void Run()
    {
        FixedRecipients recipients = new(new[] { "a", "b", "a" }); RecordingChannel channel = new();
        DeliverySummary result = new NotificationService(recipients, new GreetingTemplate(), channel).SendAll();
        Check.Equal(new DeliverySummary(3, 2), result); Check.Equal(1, recipients.Calls);
        Check.SequenceEqual(new[] { "a|Привет, a", "b|Привет, b", "a|Привет, a" }, channel.Attempts);
        Check.Equal(new DeliverySummary(0, 0), new NotificationService(new FixedRecipients(Array.Empty<string>()), new GreetingTemplate(), new RecordingChannel()).SendAll());
    }
}
```

**Исполняемые проверки**

- Три попытки и две успешных
- Порядок и содержимое сообщений
- Get вызван один раз
- Продолжение после отказа
- Пустой список
