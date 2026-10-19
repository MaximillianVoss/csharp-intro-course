// Учебный каркас. Здесь нет алгоритмов и готовых ответов; условия — в ../TASKS.md.
namespace Course.Lesson09;

// 09-01. Общая операция разных фигур
// Полное условие: ../TASKS.md, раздел 09-01.
// TODO: Объявите IShape и требуемые фигуры; реализуйте TotalArea через объявленный контракт.
public static class Task01
{
    public interface IShape
    {
        double Area { get; }
    }

    public sealed class Rectangle : IShape
    {
        public Rectangle(double width, double height)
        {
            // TODO: Объявите IShape и требуемые фигуры; реализуйте TotalArea через объявленный контракт.
            throw new NotImplementedException("09-01: задание ещё не выполнено.");
        }

        public double Area => throw new NotImplementedException("09-01: реализуйте по условию.");
    }

    public sealed class Triangle : IShape
    {
        public Triangle(double width, double height)
        {
            // TODO: Объявите IShape и требуемые фигуры; реализуйте TotalArea через объявленный контракт.
            throw new NotImplementedException("09-01: задание ещё не выполнено.");
        }

        public double Area => throw new NotImplementedException("09-01: реализуйте по условию.");
    }

    public static double TotalArea(IShape[] shapes)
    {
        // TODO: Объявите IShape и требуемые фигуры; реализуйте TotalArea через объявленный контракт.
        throw new NotImplementedException("09-01: задание ещё не выполнено.");
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("09-01: задание ещё не выполнено.");
    }
}

// 09-02. Скрытие метода и переопределение
// Полное условие: ../TASKS.md, раздел 09-02.
// TODO: Перенесите объявления из условия, запишите прогноз и реализуйте Replay с заданными вызовами.
public static class Task02
{
    public class Printer
    {
        public string Describe()
        {
            // TODO: Перенесите объявления из условия, запишите прогноз и реализуйте Replay с заданными вызовами.
            throw new NotImplementedException("09-02: задание ещё не выполнено.");
        }

        public virtual string Render()
        {
            // TODO: Перенесите объявления из условия, запишите прогноз и реализуйте Replay с заданными вызовами.
            throw new NotImplementedException("09-02: задание ещё не выполнено.");
        }
    }

    public sealed class FancyPrinter : Printer
    {
        public new string Describe()
        {
            // TODO: Перенесите объявления из условия, запишите прогноз и реализуйте Replay с заданными вызовами.
            throw new NotImplementedException("09-02: задание ещё не выполнено.");
        }

        public override string Render()
        {
            // TODO: Перенесите объявления из условия, запишите прогноз и реализуйте Replay с заданными вызовами.
            throw new NotImplementedException("09-02: задание ещё не выполнено.");
        }
    }

    public static string[] Replay()
    {
        // TODO: Перенесите объявления из условия, запишите прогноз и реализуйте Replay с заданными вызовами.
        throw new NotImplementedException("09-02: задание ещё не выполнено.");
    }

    public static void Run()
    {
        // TODO: До запуска запишите собственный прогноз; затем воспроизведите данный фрагмент и проверьте его.
        throw new NotImplementedException("09-02: задание ещё не выполнено.");
    }
}

// 09-03. Налоговая политика как зависимость
// Полное условие: ../TASKS.md, раздел 09-03.
// TODO: Создайте ITaxPolicy, политики и Invoice с зависимостью конструктора, сохранив контракт расчёта.
public static class Task03
{
    public interface ITaxPolicy
    {
        decimal Tax(decimal net);
    }

    public sealed class NoTax : ITaxPolicy
    {
        public decimal Tax(decimal net)
        {
            // TODO: Создайте ITaxPolicy, политики и Invoice с зависимостью конструктора, сохранив контракт расчёта.
            throw new NotImplementedException("09-03: задание ещё не выполнено.");
        }
    }

    public sealed class RateTax : ITaxPolicy
    {
        public RateTax(decimal rate)
        {
            // TODO: Создайте ITaxPolicy, политики и Invoice с зависимостью конструктора, сохранив контракт расчёта.
            throw new NotImplementedException("09-03: задание ещё не выполнено.");
        }

        public decimal Tax(decimal net)
        {
            // TODO: Создайте ITaxPolicy, политики и Invoice с зависимостью конструктора, сохранив контракт расчёта.
            throw new NotImplementedException("09-03: задание ещё не выполнено.");
        }
    }

    public sealed class Invoice
    {
        public Invoice(ITaxPolicy policy)
        {
            // TODO: Создайте ITaxPolicy, политики и Invoice с зависимостью конструктора, сохранив контракт расчёта.
            throw new NotImplementedException("09-03: задание ещё не выполнено.");
        }

        public decimal Total(decimal net)
        {
            // TODO: Создайте ITaxPolicy, политики и Invoice с зависимостью конструктора, сохранив контракт расчёта.
            throw new NotImplementedException("09-03: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("09-03: задание ещё не выполнено.");
    }
}

// 09-04. Одинаковая команда разным устройствам
// Полное условие: ../TASKS.md, раздел 09-04.
// TODO: Объявите ICommand и команды; реализуйте ExecuteAll с нужными частотой и порядком обращений.
public static class Task04
{
    public interface ICommand
    {
        string Execute();
    }

    public sealed class EchoCommand : ICommand
    {
        public EchoCommand(string text)
        {
            // TODO: Объявите ICommand и команды; реализуйте ExecuteAll с нужными частотой и порядком обращений.
            throw new NotImplementedException("09-04: задание ещё не выполнено.");
        }

        public string Execute()
        {
            // TODO: Объявите ICommand и команды; реализуйте ExecuteAll с нужными частотой и порядком обращений.
            throw new NotImplementedException("09-04: задание ещё не выполнено.");
        }
    }

    public sealed class SumCommand : ICommand
    {
        public SumCommand(int left, int right)
        {
            // TODO: Объявите ICommand и команды; реализуйте ExecuteAll с нужными частотой и порядком обращений.
            throw new NotImplementedException("09-04: задание ещё не выполнено.");
        }

        public string Execute()
        {
            // TODO: Объявите ICommand и команды; реализуйте ExecuteAll с нужными частотой и порядком обращений.
            throw new NotImplementedException("09-04: задание ещё не выполнено.");
        }
    }

    public static string[] ExecuteAll(ICommand[] commands)
    {
        // TODO: Объявите ICommand и команды; реализуйте ExecuteAll с нужными частотой и порядком обращений.
        throw new NotImplementedException("09-04: задание ещё не выполнено.");
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("09-04: задание ещё не выполнено.");
    }
}

// 09-05. Часы для повторяемого теста
// Полное условие: ../TASKS.md, раздел 09-05.
// TODO: Создайте часы и Greeting; самостоятельно подберите проверки границы и ошибочного варианта.
public static class Task05
{
    public interface IClock
    {
        DateTime Now { get; }
    }

    public sealed class FixedClock : IClock
    {
        public DateTime Now
        {
            get
            {
                // TODO: Создайте часы и Greeting; самостоятельно подберите проверки границы и ошибочного варианта.
                throw new NotImplementedException("09-05: задание ещё не выполнено.");
            }
        }

        public FixedClock(DateTime now)
        {
            // TODO: Создайте часы и Greeting; самостоятельно подберите проверки границы и ошибочного варианта.
            throw new NotImplementedException("09-05: задание ещё не выполнено.");
        }
    }

    public sealed class Greeting
    {
        public Greeting(IClock clock)
        {
            // TODO: Создайте часы и Greeting; самостоятельно подберите проверки границы и ошибочного варианта.
            throw new NotImplementedException("09-05: задание ещё не выполнено.");
        }

        public string Get()
        {
            // TODO: Создайте часы и Greeting; самостоятельно подберите проверки границы и ошибочного варианта.
            throw new NotImplementedException("09-05: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Спроектируйте различающие проверки по условию; не ограничивайтесь запуском без ошибки.
        throw new NotImplementedException("09-05: задание ещё не выполнено.");
    }
}

// 09-06. Общая рамка расчёта доставки
// Полное условие: ../TASKS.md, раздел 09-06.
// TODO: Определите абстрактную рамку ShippingPolicy и требуемые реализации, сохранив доступность членов.
public static class Task06
{
    public abstract class ShippingPolicy
    {
        protected abstract decimal BaseFee { get; }
        protected abstract decimal Minimum { get; }

        protected abstract decimal VariableCost(decimal kilograms);
        public decimal Quote(decimal kilograms)
        {
            // TODO: Определите абстрактную рамку ShippingPolicy и требуемые реализации, сохранив доступность членов.
            throw new NotImplementedException("09-06: задание ещё не выполнено.");
        }
    }

    public sealed class Ground : ShippingPolicy
    {
        protected override decimal BaseFee => throw new NotImplementedException("09-06: реализуйте по условию.");
        protected override decimal Minimum => throw new NotImplementedException("09-06: реализуйте по условию.");

        protected override decimal VariableCost(decimal kilograms)
        {
            // TODO: Определите абстрактную рамку ShippingPolicy и требуемые реализации, сохранив доступность членов.
            throw new NotImplementedException("09-06: задание ещё не выполнено.");
        }
    }

    public sealed class Air : ShippingPolicy
    {
        protected override decimal BaseFee => throw new NotImplementedException("09-06: реализуйте по условию.");
        protected override decimal Minimum => throw new NotImplementedException("09-06: реализуйте по условию.");

        protected override decimal VariableCost(decimal kilograms)
        {
            // TODO: Определите абстрактную рамку ShippingPolicy и требуемые реализации, сохранив доступность членов.
            throw new NotImplementedException("09-06: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("09-06: задание ещё не выполнено.");
    }
}

// 09-07. Перегрузка не заменила виртуальный метод
// Полное условие: ../TASKS.md, раздел 09-07.
// TODO: Перенесите данный дефект из условия и реализуйте исправленный Dog с требуемым контрактом.
public static class Task07
{
    public class Animal
    {
        public virtual string Speak()
        {
            // TODO: Перенесите данный дефект из условия и реализуйте исправленный Dog с требуемым контрактом.
            throw new NotImplementedException("09-07: задание ещё не выполнено.");
        }
    }

    public sealed class BrokenDog : Animal
    {
        public string Speak(string suffix)
        {
            // TODO: Перенесите данный дефект из условия и реализуйте исправленный Dog с требуемым контрактом.
            throw new NotImplementedException("09-07: задание ещё не выполнено.");
        }
    }

    public sealed class Dog : Animal
    {
        public override string Speak()
        {
            // TODO: Перенесите данный дефект из условия и реализуйте исправленный Dog с требуемым контрактом.
            throw new NotImplementedException("09-07: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("09-07: задание ещё не выполнено.");
    }
}

// 09-08. Узкий интерфейс для читателя
// Полное условие: ../TASKS.md, раздел 09-08.
// TODO: Объявите читающий и пишущий контракты и модели из условия; реализуйте BuildHeading без лишней зависимости.
public static class Task08
{
    public interface IReadName
    {
        string Read();
    }

    public interface IWriteName
    {
        void Write(string value);
    }

    public sealed class MemoryName : IReadName, IWriteName
    {
        public MemoryName(string initial)
        {
            // TODO: Объявите читающий и пишущий контракты и модели из условия; реализуйте BuildHeading без лишней зависимости.
            throw new NotImplementedException("09-08: задание ещё не выполнено.");
        }

        public string Read()
        {
            // TODO: Объявите читающий и пишущий контракты и модели из условия; реализуйте BuildHeading без лишней зависимости.
            throw new NotImplementedException("09-08: задание ещё не выполнено.");
        }

        public void Write(string value)
        {
            // TODO: Объявите читающий и пишущий контракты и модели из условия; реализуйте BuildHeading без лишней зависимости.
            throw new NotImplementedException("09-08: задание ещё не выполнено.");
        }
    }

    public sealed class ReadOnlyName : IReadName
    {
        public ReadOnlyName(string value)
        {
            // TODO: Объявите читающий и пишущий контракты и модели из условия; реализуйте BuildHeading без лишней зависимости.
            throw new NotImplementedException("09-08: задание ещё не выполнено.");
        }

        public string Read()
        {
            // TODO: Объявите читающий и пишущий контракты и модели из условия; реализуйте BuildHeading без лишней зависимости.
            throw new NotImplementedException("09-08: задание ещё не выполнено.");
        }
    }

    public static string BuildHeading(IReadName source)
    {
        // TODO: Объявите читающий и пишущий контракты и модели из условия; реализуйте BuildHeading без лишней зависимости.
        throw new NotImplementedException("09-08: задание ещё не выполнено.");
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("09-08: задание ещё не выполнено.");
    }
}

// 09-09. Интерфейс не отменяет владение данными
// Полное условие: ../TASKS.md, раздел 09-09.
// TODO: Создайте IIntRepository и MemoryRepository; проверьте владение при сохранении и при чтении.
public static class Task09
{
    public interface IIntRepository
    {
        void Save(int[] values);
        int[] Load();
    }

    public sealed class MemoryRepository : IIntRepository
    {
        public void Save(int[] values)
        {
            // TODO: Создайте IIntRepository и MemoryRepository; проверьте владение при сохранении и при чтении.
            throw new NotImplementedException("09-09: задание ещё не выполнено.");
        }

        public int[] Load()
        {
            // TODO: Создайте IIntRepository и MemoryRepository; проверьте владение при сохранении и при чтении.
            throw new NotImplementedException("09-09: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("09-09: задание ещё не выполнено.");
    }
}

// 09-10. Один контракт для листа и папки
// Полное условие: ../TASKS.md, раздел 09-10.
// TODO: Создайте INode, FileNode и FolderNode; реализуйте размер в рамках заданного дерева.
public static class Task10
{
    public interface INode
    {
        long Size { get; }
    }

    public sealed class FileNode : INode
    {
        public long Size
        {
            get
            {
                // TODO: Создайте INode, FileNode и FolderNode; реализуйте размер в рамках заданного дерева.
                throw new NotImplementedException("09-10: задание ещё не выполнено.");
            }
        }

        public FileNode(long size)
        {
            // TODO: Создайте INode, FileNode и FolderNode; реализуйте размер в рамках заданного дерева.
            throw new NotImplementedException("09-10: задание ещё не выполнено.");
        }
    }

    public sealed class FolderNode : INode
    {
        public void Add(INode node)
        {
            // TODO: Создайте INode, FileNode и FolderNode; реализуйте размер в рамках заданного дерева.
            throw new NotImplementedException("09-10: задание ещё не выполнено.");
        }

        public long Size
        {
            get
            {
                // TODO: Создайте INode, FileNode и FolderNode; реализуйте размер в рамках заданного дерева.
                throw new NotImplementedException("09-10: задание ещё не выполнено.");
            }
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("09-10: задание ещё не выполнено.");
    }
}

// 09-11. Добавление поведения обёрткой
// Полное условие: ../TASKS.md, раздел 09-11.
// TODO: Объявите ITextTransform и требуемые обёртки; проверьте оба порядка композиции.
public static class Task11
{
    public interface ITextTransform
    {
        string Transform(string text);
    }

    public sealed class Identity : ITextTransform
    {
        public string Transform(string text)
        {
            // TODO: Объявите ITextTransform и требуемые обёртки; проверьте оба порядка композиции.
            throw new NotImplementedException("09-11: задание ещё не выполнено.");
        }
    }

    public sealed class TrimDecorator : ITextTransform
    {
        public TrimDecorator(ITextTransform inner)
        {
            // TODO: Объявите ITextTransform и требуемые обёртки; проверьте оба порядка композиции.
            throw new NotImplementedException("09-11: задание ещё не выполнено.");
        }

        public string Transform(string text)
        {
            // TODO: Объявите ITextTransform и требуемые обёртки; проверьте оба порядка композиции.
            throw new NotImplementedException("09-11: задание ещё не выполнено.");
        }
    }

    public sealed class PrefixDecorator : ITextTransform
    {
        public PrefixDecorator(ITextTransform inner, string prefix)
        {
            // TODO: Объявите ITextTransform и требуемые обёртки; проверьте оба порядка композиции.
            throw new NotImplementedException("09-11: задание ещё не выполнено.");
        }

        public string Transform(string text)
        {
            // TODO: Объявите ITextTransform и требуемые обёртки; проверьте оба порядка композиции.
            throw new NotImplementedException("09-11: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("09-11: задание ещё не выполнено.");
    }
}

// 09-12. Резервный источник вызывается по необходимости
// Полное условие: ../TASKS.md, раздел 09-12.
// TODO: Создайте источники IValueSource и FallbackSource; проверьте наличие, пустое значение и частоту обращений.
public static class Task12
{
    public interface IValueSource
    {
        bool TryGet(string key, out string value);
    }

    public sealed class DictionarySource : IValueSource
    {
        public DictionarySource(Dictionary<string, string> data)
        {
            // TODO: Создайте источники IValueSource и FallbackSource; проверьте наличие, пустое значение и частоту обращений.
            throw new NotImplementedException("09-12: задание ещё не выполнено.");
        }

        public bool TryGet(string key, out string value)
        {
            // TODO: Создайте источники IValueSource и FallbackSource; проверьте наличие, пустое значение и частоту обращений.
            throw new NotImplementedException("09-12: задание ещё не выполнено.");
        }
    }

    public sealed class FallbackSource : IValueSource
    {
        public FallbackSource(IValueSource primary, IValueSource backup)
        {
            // TODO: Создайте источники IValueSource и FallbackSource; проверьте наличие, пустое значение и частоту обращений.
            throw new NotImplementedException("09-12: задание ещё не выполнено.");
        }

        public bool TryGet(string key, out string value)
        {
            // TODO: Создайте источники IValueSource и FallbackSource; проверьте наличие, пустое значение и частоту обращений.
            throw new NotImplementedException("09-12: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("09-12: задание ещё не выполнено.");
    }
}

// 09-13. Два представления одного отчёта
// Полное условие: ../TASKS.md, раздел 09-13.
// TODO: Объявите два интерфейса и Report; реализуйте требуемые представления и BothViews.
public static class Task13
{
    public interface ICompactReport
    {
        string Render();
    }

    public interface IDetailedReport
    {
        string Render();
    }

    public sealed class Report : ICompactReport, IDetailedReport
    {
        public Report(string title, int count)
        {
            // TODO: Объявите два интерфейса и Report; реализуйте требуемые представления и BothViews.
            throw new NotImplementedException("09-13: задание ещё не выполнено.");
        }

        string ICompactReport.Render()
        {
            // TODO: Объявите два интерфейса и Report; реализуйте требуемые представления и BothViews.
            throw new NotImplementedException("09-13: задание ещё не выполнено.");
        }

        string IDetailedReport.Render()
        {
            // TODO: Объявите два интерфейса и Report; реализуйте требуемые представления и BothViews.
            throw new NotImplementedException("09-13: задание ещё не выполнено.");
        }
    }

    public static string[] BothViews(Report report)
    {
        // TODO: Объявите два интерфейса и Report; реализуйте требуемые представления и BothViews.
        throw new NotImplementedException("09-13: задание ещё не выполнено.");
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("09-13: задание ещё не выполнено.");
    }
}

// 09-14. Совпадение сигнатур ещё не очередь
// Полное условие: ../TASKS.md, раздел 09-14.
// TODO: Реализуйте два адаптера и самостоятельно напишите SatisfiesContract с различающим поведением.
public static class Task14
{
    public interface IIntQueue
    {
        void Add(int value);
        bool TryTake(out int value);
    }

    public sealed class FifoQueue : IIntQueue
    {
        public void Add(int value)
        {
            // TODO: Реализуйте два адаптера и самостоятельно напишите SatisfiesContract с различающим поведением.
            throw new NotImplementedException("09-14: задание ещё не выполнено.");
        }

        public bool TryTake(out int value)
        {
            // TODO: Реализуйте два адаптера и самостоятельно напишите SatisfiesContract с различающим поведением.
            throw new NotImplementedException("09-14: задание ещё не выполнено.");
        }
    }

    public sealed class LifoQueue : IIntQueue
    {
        public void Add(int value)
        {
            // TODO: Реализуйте два адаптера и самостоятельно напишите SatisfiesContract с различающим поведением.
            throw new NotImplementedException("09-14: задание ещё не выполнено.");
        }

        public bool TryTake(out int value)
        {
            // TODO: Реализуйте два адаптера и самостоятельно напишите SatisfiesContract с различающим поведением.
            throw new NotImplementedException("09-14: задание ещё не выполнено.");
        }
    }

    public static bool SatisfiesContract(IIntQueue queue)
    {
        // TODO: Реализуйте два адаптера и самостоятельно напишите SatisfiesContract с различающим поведением.
        throw new NotImplementedException("09-14: задание ещё не выполнено.");
    }

    public static void Run()
    {
        // TODO: Спроектируйте различающие проверки по условию; не ограничивайтесь запуском без ошибки.
        throw new NotImplementedException("09-14: задание ещё не выполнено.");
    }
}

// 09-15. Локальная служба уведомлений
// Полное условие: ../TASKS.md, раздел 09-15.
// TODO: Создайте контракты, отчёт и NotificationService; тестовые реализации подготовьте в памяти без сети.
public static class Task15
{
    public interface IRecipients
    {
        string[] Get();
    }

    public interface IMessageTemplate
    {
        string Build(string recipient);
    }

    public interface IChannel
    {
        bool TrySend(string recipient, string message);
    }

    public readonly record struct DeliverySummary(int Attempted, int Succeeded);

    public sealed class NotificationService
    {
        public NotificationService(IRecipients recipients, IMessageTemplate template, IChannel channel)
        {
            // TODO: Создайте контракты, отчёт и NotificationService; тестовые реализации подготовьте в памяти без сети.
            throw new NotImplementedException("09-15: задание ещё не выполнено.");
        }

        public DeliverySummary SendAll()
        {
            // TODO: Создайте контракты, отчёт и NotificationService; тестовые реализации подготовьте в памяти без сети.
            throw new NotImplementedException("09-15: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("09-15: задание ещё не выполнено.");
    }
}
