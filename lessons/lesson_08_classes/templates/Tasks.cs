// Учебный каркас. Здесь нет алгоритмов и готовых ответов; условия — в ../TASKS.md.
namespace Course.Lesson08;

// 08-01. Счётчик с верхней границей
// Полное условие: ../TASKS.md, раздел 08-01.
// TODO: Создайте состояние BoundedCounter и реализуйте TryIncrement с заданной публичной границей.
public static class Task01
{
    public sealed class BoundedCounter
    {
        public int Maximum
        {
            get
            {
                // TODO: Создайте состояние BoundedCounter и реализуйте TryIncrement с заданной публичной границей.
                throw new NotImplementedException("08-01: задание ещё не выполнено.");
            }
        }

        public int Value
        {
            get
            {
                // TODO: Создайте состояние BoundedCounter и реализуйте TryIncrement с заданной публичной границей.
                throw new NotImplementedException("08-01: задание ещё не выполнено.");
            }

            private set
            {
                // TODO: Создайте состояние BoundedCounter и реализуйте TryIncrement с заданной публичной границей.
                throw new NotImplementedException("08-01: задание ещё не выполнено.");
            }
        }

        public BoundedCounter(int maximum)
        {
            // TODO: Создайте состояние BoundedCounter и реализуйте TryIncrement с заданной публичной границей.
            throw new NotImplementedException("08-01: задание ещё не выполнено.");
        }

        public bool TryIncrement()
        {
            // TODO: Создайте состояние BoundedCounter и реализуйте TryIncrement с заданной публичной границей.
            throw new NotImplementedException("08-01: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("08-01: задание ещё не выполнено.");
    }
}

// 08-02. Размеры меняются вместе
// Полное условие: ../TASKS.md, раздел 08-02.
// TODO: Реализуйте Rectangle, его свойства и TryResize; проверьте совместное изменение размеров.
public static class Task02
{
    public sealed class Rectangle
    {
        public decimal Width
        {
            get
            {
                // TODO: Реализуйте Rectangle, его свойства и TryResize; проверьте совместное изменение размеров.
                throw new NotImplementedException("08-02: задание ещё не выполнено.");
            }

            private set
            {
                // TODO: Реализуйте Rectangle, его свойства и TryResize; проверьте совместное изменение размеров.
                throw new NotImplementedException("08-02: задание ещё не выполнено.");
            }
        }

        public decimal Height
        {
            get
            {
                // TODO: Реализуйте Rectangle, его свойства и TryResize; проверьте совместное изменение размеров.
                throw new NotImplementedException("08-02: задание ещё не выполнено.");
            }

            private set
            {
                // TODO: Реализуйте Rectangle, его свойства и TryResize; проверьте совместное изменение размеров.
                throw new NotImplementedException("08-02: задание ещё не выполнено.");
            }
        }

        public decimal Area => throw new NotImplementedException("08-02: реализуйте по условию.");

        public Rectangle(decimal width, decimal height)
        {
            // TODO: Реализуйте Rectangle, его свойства и TryResize; проверьте совместное изменение размеров.
            throw new NotImplementedException("08-02: задание ещё не выполнено.");
        }

        public bool TryResize(decimal width, decimal height)
        {
            // TODO: Реализуйте Rectangle, его свойства и TryResize; проверьте совместное изменение размеров.
            throw new NotImplementedException("08-02: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("08-02: задание ещё не выполнено.");
    }
}

// 08-03. Объект и значение при присваивании
// Полное условие: ../TASKS.md, раздел 08-03.
// TODO: Запишите прогноз для class и значимого типа, затем воспроизведите Replay из условия.
public static class Task03
{
    public sealed class Box
    {
        public int Number
        {
            get
            {
                // TODO: Запишите прогноз для class и значимого типа, затем воспроизведите Replay из условия.
                throw new NotImplementedException("08-03: задание ещё не выполнено.");
            }

            set
            {
                // TODO: Запишите прогноз для class и значимого типа, затем воспроизведите Replay из условия.
                throw new NotImplementedException("08-03: задание ещё не выполнено.");
            }
        }
    }

    public readonly record struct Point(int X, int Y);

    public static int[] Replay()
    {
        // TODO: Запишите прогноз для class и значимого типа, затем воспроизведите Replay из условия.
        throw new NotImplementedException("08-03: задание ещё не выполнено.");
    }

    public static void Run()
    {
        // TODO: До запуска запишите собственный прогноз; затем воспроизведите данный фрагмент и проверьте его.
        throw new NotImplementedException("08-03: задание ещё не выполнено.");
    }
}

// 08-04. Снятие без отрицательного баланса
// Полное условие: ../TASKS.md, раздел 08-04.
// TODO: Реализуйте Wallet с закрытым изменением Balance и ожидаемым отказом TrySpend.
public static class Task04
{
    public sealed class Wallet
    {
        public decimal Balance
        {
            get
            {
                // TODO: Реализуйте Wallet с закрытым изменением Balance и ожидаемым отказом TrySpend.
                throw new NotImplementedException("08-04: задание ещё не выполнено.");
            }

            private set
            {
                // TODO: Реализуйте Wallet с закрытым изменением Balance и ожидаемым отказом TrySpend.
                throw new NotImplementedException("08-04: задание ещё не выполнено.");
            }
        }

        public Wallet(decimal initialBalance)
        {
            // TODO: Реализуйте Wallet с закрытым изменением Balance и ожидаемым отказом TrySpend.
            throw new NotImplementedException("08-04: задание ещё не выполнено.");
        }

        public bool TrySpend(decimal amount)
        {
            // TODO: Реализуйте Wallet с закрытым изменением Balance и ожидаемым отказом TrySpend.
            throw new NotImplementedException("08-04: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("08-04: задание ещё не выполнено.");
    }
}

// 08-05. Граница ёмкости под тестом
// Полное условие: ../TASKS.md, раздел 08-05.
// TODO: Определите Tank и спроектируйте проверки, различающие допустимую границу и ошибочный предикат.
public static class Task05
{
    public sealed class Tank
    {
        public int Capacity
        {
            get
            {
                // TODO: Определите Tank и спроектируйте проверки, различающие допустимую границу и ошибочный предикат.
                throw new NotImplementedException("08-05: задание ещё не выполнено.");
            }
        }

        public int Volume
        {
            get
            {
                // TODO: Определите Tank и спроектируйте проверки, различающие допустимую границу и ошибочный предикат.
                throw new NotImplementedException("08-05: задание ещё не выполнено.");
            }

            private set
            {
                // TODO: Определите Tank и спроектируйте проверки, различающие допустимую границу и ошибочный предикат.
                throw new NotImplementedException("08-05: задание ещё не выполнено.");
            }
        }

        public Tank(int capacity)
        {
            // TODO: Определите Tank и спроектируйте проверки, различающие допустимую границу и ошибочный предикат.
            throw new NotImplementedException("08-05: задание ещё не выполнено.");
        }

        public bool TryFill(int amount)
        {
            // TODO: Определите Tank и спроектируйте проверки, различающие допустимую границу и ошибочный предикат.
            throw new NotImplementedException("08-05: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Спроектируйте различающие проверки по условию; не ограничивайтесь запуском без ошибки.
        throw new NotImplementedException("08-05: задание ещё не выполнено.");
    }
}

// 08-06. Изменение записи через with
// Полное условие: ../TASKS.md, раздел 08-06.
// TODO: Объявите Product и Reprice; проверьте состав записи и независимость недеструктивного изменения.
public static class Task06
{
    public sealed record Product(string Code, string Title, decimal Price);

    public static Product? Reprice(Product original, decimal newPrice)
    {
        // TODO: Объявите Product и Reprice; проверьте состав записи и независимость недеструктивного изменения.
        throw new NotImplementedException("08-06: задание ещё не выполнено.");
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("08-06: задание ещё не выполнено.");
    }
}

// 08-07. Корзина состоит из строк
// Полное условие: ../TASKS.md, раздел 08-07.
// TODO: Создайте CartLine и Cart; реализуйте объявленные свойства и TryAdd без раскрытия внутренней коллекции.
public static class Task07
{
    public sealed record CartLine(string Code, decimal UnitPrice, int Quantity)
    {
        public decimal Total => throw new NotImplementedException("08-07: реализуйте по условию.");
    }

    public sealed class Cart
    {
        public int Count => throw new NotImplementedException("08-07: реализуйте по условию.");

        public decimal Total
        {
            get
            {
                // TODO: Создайте CartLine и Cart; реализуйте объявленные свойства и TryAdd без раскрытия внутренней коллекции.
                throw new NotImplementedException("08-07: задание ещё не выполнено.");
            }
        }

        public bool TryAdd(string? code, decimal price, int quantity)
        {
            // TODO: Создайте CartLine и Cart; реализуйте объявленные свойства и TryAdd без раскрытия внутренней коллекции.
            throw new NotImplementedException("08-07: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("08-07: задание ещё не выполнено.");
    }
}

// 08-08. Одна температура, две шкалы
// Полное условие: ../TASKS.md, раздел 08-08.
// TODO: Реализуйте Temperature и согласованные показания; проверьте допустимость перевода на краях диапазона.
public static class Task08
{
    public sealed class Temperature
    {
        public decimal Kelvin
        {
            get
            {
                // TODO: Реализуйте Temperature и согласованные показания; проверьте допустимость перевода на краях диапазона.
                throw new NotImplementedException("08-08: задание ещё не выполнено.");
            }

            private set
            {
                // TODO: Реализуйте Temperature и согласованные показания; проверьте допустимость перевода на краях диапазона.
                throw new NotImplementedException("08-08: задание ещё не выполнено.");
            }
        }

        public decimal Celsius => throw new NotImplementedException("08-08: реализуйте по условию.");

        public Temperature(decimal kelvin)
        {
            // TODO: Реализуйте Temperature и согласованные показания; проверьте допустимость перевода на краях диапазона.
            throw new NotImplementedException("08-08: задание ещё не выполнено.");
        }

        public bool TrySetCelsius(decimal celsius)
        {
            // TODO: Реализуйте Temperature и согласованные показания; проверьте допустимость перевода на краях диапазона.
            throw new NotImplementedException("08-08: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("08-08: задание ещё не выполнено.");
    }
}

// 08-09. Общее поле вместо состояния экземпляра
// Полное условие: ../TASKS.md, раздел 08-09.
// TODO: Перенесите данный BrokenMeter из условия для эксперимента и реализуйте отдельный исправленный Meter.
public static class Task09
{
    public sealed class BrokenMeter
    {
        public int Value => throw new NotImplementedException("08-09: реализуйте по условию.");

        public void Tick()
        {
            // TODO: Перенесите данный BrokenMeter из условия для эксперимента и реализуйте отдельный исправленный Meter.
            throw new NotImplementedException("08-09: задание ещё не выполнено.");
        }
    }

    public sealed class Meter
    {
        public int Value => throw new NotImplementedException("08-09: реализуйте по условию.");

        public void Tick()
        {
            // TODO: Перенесите данный BrokenMeter из условия для эксперимента и реализуйте отдельный исправленный Meter.
            throw new NotImplementedException("08-09: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("08-09: задание ещё не выполнено.");
    }
}

// 08-10. Каталог не отдаёт свои внутренности
// Полное условие: ../TASKS.md, раздел 08-10.
// TODO: Создайте Catalog с заданными операциями, сохранив закрытое владение коллекцией.
public static class Task10
{
    public sealed class Catalog
    {
        public int Count => throw new NotImplementedException("08-10: реализуйте по условию.");

        public bool TryAdd(string? title)
        {
            // TODO: Создайте Catalog с заданными операциями, сохранив закрытое владение коллекцией.
            throw new NotImplementedException("08-10: задание ещё не выполнено.");
        }

        public string[] GetTitles()
        {
            // TODO: Создайте Catalog с заданными операциями, сохранив закрытое владение коллекцией.
            throw new NotImplementedException("08-10: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("08-10: задание ещё не выполнено.");
    }
}

// 08-11. Структура с общей ссылкой внутри
// Полное условие: ../TASKS.md, раздел 08-11.
// TODO: Запишите прогноз для структуры с массивом; затем реализуйте Replay и отдельный DeepCopy.
public static class Task11
{
    public struct Snapshot
    {
        public int Revision;
        public int[] Values;
    }

    public static int[] Replay()
    {
        // TODO: Запишите прогноз для структуры с массивом; затем реализуйте Replay и отдельный DeepCopy.
        throw new NotImplementedException("08-11: задание ещё не выполнено.");
    }

    public static Snapshot DeepCopy(Snapshot source)
    {
        // TODO: Запишите прогноз для структуры с массивом; затем реализуйте Replay и отдельный DeepCopy.
        throw new NotImplementedException("08-11: задание ещё не выполнено.");
    }

    public static void Run()
    {
        // TODO: До запуска запишите собственный прогноз; затем воспроизведите данный фрагмент и проверьте его.
        throw new NotImplementedException("08-11: задание ещё не выполнено.");
    }
}

// 08-12. Интервал создаётся корректным
// Полное условие: ../TASKS.md, раздел 08-12.
// TODO: Реализуйте TimeSlot с фабрикой Create и Overlaps по заданной модели границ.
public static class Task12
{
    public sealed class TimeSlot
    {
        public DateTime Start
        {
            get
            {
                // TODO: Реализуйте TimeSlot с фабрикой Create и Overlaps по заданной модели границ.
                throw new NotImplementedException("08-12: задание ещё не выполнено.");
            }
        }

        public DateTime End
        {
            get
            {
                // TODO: Реализуйте TimeSlot с фабрикой Create и Overlaps по заданной модели границ.
                throw new NotImplementedException("08-12: задание ещё не выполнено.");
            }
        }

        private TimeSlot(DateTime start, DateTime end)
        {
            // TODO: Реализуйте TimeSlot с фабрикой Create и Overlaps по заданной модели границ.
            throw new NotImplementedException("08-12: задание ещё не выполнено.");
        }

        public static TimeSlot? Create(DateTime start, DateTime end)
        {
            // TODO: Реализуйте TimeSlot с фабрикой Create и Overlaps по заданной модели границ.
            throw new NotImplementedException("08-12: задание ещё не выполнено.");
        }

        public bool Overlaps(TimeSlot other)
        {
            // TODO: Реализуйте TimeSlot с фабрикой Create и Overlaps по заданной модели границ.
            throw new NotImplementedException("08-12: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("08-12: задание ещё не выполнено.");
    }
}

// 08-13. Заказ проходит разрешённые состояния
// Полное условие: ../TASKS.md, раздел 08-13.
// TODO: Объявите OrderState и Order; реализуйте только разрешённые переходы по контракту.
public static class Task13
{
    public enum OrderState
    {
        Draft,
        Submitted,
        Cancelled
    }

    public sealed class Order
    {
        public OrderState State
        {
            get
            {
                // TODO: Объявите OrderState и Order; реализуйте только разрешённые переходы по контракту.
                throw new NotImplementedException("08-13: задание ещё не выполнено.");
            }

            private set
            {
                // TODO: Объявите OrderState и Order; реализуйте только разрешённые переходы по контракту.
                throw new NotImplementedException("08-13: задание ещё не выполнено.");
            }
        }

        public bool TrySubmit()
        {
            // TODO: Объявите OrderState и Order; реализуйте только разрешённые переходы по контракту.
            throw new NotImplementedException("08-13: задание ещё не выполнено.");
        }

        public bool TryCancel()
        {
            // TODO: Объявите OrderState и Order; реализуйте только разрешённые переходы по контракту.
            throw new NotImplementedException("08-13: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("08-13: задание ещё не выполнено.");
    }
}

// 08-14. Нормализованная дробь
// Полное условие: ../TASKS.md, раздел 08-14.
// TODO: Создайте Fraction с закрытым конструктором и фабрикой Create; проверьте каноническую форму.
public static class Task14
{
    public sealed class Fraction
    {
        public int Numerator
        {
            get
            {
                // TODO: Создайте Fraction с закрытым конструктором и фабрикой Create; проверьте каноническую форму.
                throw new NotImplementedException("08-14: задание ещё не выполнено.");
            }
        }

        public int Denominator
        {
            get
            {
                // TODO: Создайте Fraction с закрытым конструктором и фабрикой Create; проверьте каноническую форму.
                throw new NotImplementedException("08-14: задание ещё не выполнено.");
            }
        }

        private Fraction(int numerator, int denominator)
        {
            // TODO: Создайте Fraction с закрытым конструктором и фабрикой Create; проверьте каноническую форму.
            throw new NotImplementedException("08-14: задание ещё не выполнено.");
        }

        public static Fraction? Create(int numerator, int denominator)
        {
            // TODO: Создайте Fraction с закрытым конструктором и фабрикой Create; проверьте каноническую форму.
            throw new NotImplementedException("08-14: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("08-14: задание ещё не выполнено.");
    }
}

// 08-15. Перенос между контейнерами
// Полное условие: ../TASKS.md, раздел 08-15.
// TODO: Реализуйте Bin и TryTransfer; проверьте два независимых объекта и отдельно одинаковую ссылку.
public static class Task15
{
    public sealed class Bin
    {
        public int Capacity
        {
            get
            {
                // TODO: Реализуйте Bin и TryTransfer; проверьте два независимых объекта и отдельно одинаковую ссылку.
                throw new NotImplementedException("08-15: задание ещё не выполнено.");
            }
        }

        public int Quantity
        {
            get
            {
                // TODO: Реализуйте Bin и TryTransfer; проверьте два независимых объекта и отдельно одинаковую ссылку.
                throw new NotImplementedException("08-15: задание ещё не выполнено.");
            }

            private set
            {
                // TODO: Реализуйте Bin и TryTransfer; проверьте два независимых объекта и отдельно одинаковую ссылку.
                throw new NotImplementedException("08-15: задание ещё не выполнено.");
            }
        }

        public Bin(int capacity, int quantity)
        {
            // TODO: Реализуйте Bin и TryTransfer; проверьте два независимых объекта и отдельно одинаковую ссылку.
            throw new NotImplementedException("08-15: задание ещё не выполнено.");
        }

        public static bool TryTransfer(Bin source, Bin target, int amount)
        {
            // TODO: Реализуйте Bin и TryTransfer; проверьте два независимых объекта и отдельно одинаковую ссылку.
            throw new NotImplementedException("08-15: задание ещё не выполнено.");
        }
    }

    public static void Run()
    {
        // TODO: Добавьте свои проверки контракта, границ и сохранности состояния.
        throw new NotImplementedException("08-15: задание ещё не выполнено.");
    }
}
