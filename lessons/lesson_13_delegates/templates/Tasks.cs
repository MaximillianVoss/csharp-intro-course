// Авторский предметный каркас. Модели и API заданы; алгоритмы и проверки предстоит написать.

namespace Course.Lesson13;
// 13-01. Один вызов преобразования
// Условие: ../TASKS.md, раздел 13-01.
public static class Task01
{
    public static int Apply(int value, Func<int, int> operation)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Используйте счётчик и заранее созданное исключение, чтобы проверить вызов ровно один раз.
        throw new NotImplementedException();
    }
}

// 13-02. Композиция в заданном порядке
// Условие: ../TASKS.md, раздел 13-02.
public static class Task02
{
    public static Func<T, T> Compose<T>(Func<T, T> first, Func<T, T> second)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Подберите операции, для которых перестановка меняет результат; проверьте время первого вызова.
        throw new NotImplementedException();
    }
}

// 13-03. Когда читается захваченная переменная
// Условие: ../TASKS.md, раздел 13-03.
public static class Task03
{
    public static int[] Observe()
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Запишите прогноз до выполнения и сравните захват переменной с отдельно сохранённым значением.
        throw new NotImplementedException();
    }
}

// 13-04. Исправление захвата в for
// Условие: ../TASKS.md, раздел 13-04.
public static class Task04
{
    public static Func<int>[] Build(int count)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Вызывайте полученные делегаты после цикла; сравните с буквальным ошибочным вариантом.
        throw new NotImplementedException();
    }
}

// 13-05. Предикат диапазона
// Условие: ../TASKS.md, раздел 13-05.
public static class Task05
{
    public static Predicate<int> InRange(int minimum, int maximum)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте соседние значения, точечный диапазон и независимость двух созданных предикатов.
        throw new NotImplementedException();
    }
}

// 13-06. Отбор без LINQ
// Условие: ../TASKS.md, раздел 13-06.
public static class Task06
{
    public static T[] Filter<T>(IEnumerable<T> source, Predicate<T> accept)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Наблюдайте число вызовов, продвижение источника и его освобождение при ошибке предиката.
        throw new NotImplementedException();
    }
}

// 13-07. Составное сравнение имён
// Условие: ../TASKS.md, раздел 13-07.
public static class Task07
{
    public static string[] SortNames(string[] names)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте обе ступени сравнения и независимость от текущей культуры, сохранив исходный массив.
        throw new NotImplementedException();
    }
}

// 13-08. Наблюдатель накопленной суммы
// Условие: ../TASKS.md, раздел 13-08.
public static class Task08
{
    public static long Accumulate(IReadOnlyList<int> values, Action<long> report)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Запишите все уведомления и проверьте, какое состояние видит callback после каждого элемента.
        throw new NotImplementedException();
    }
}

// 13-09. Конвейер с внедрёнными действиями
// Условие: ../TASKS.md, раздел 13-09.
public static class Task09
{
    public static string Process(string text, Func<string, string> normalize, Action<string> log)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Используйте общий журнал порядка и проверяйте отсутствие эффектов при неверной конфигурации.
        throw new NotImplementedException();
    }
}

// 13-10. Тест числа вызовов предиката
// Условие: ../TASKS.md, раздел 13-10.
public static class Task10
{
    public static int CountPassing(int[] values, Predicate<int> accept)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Напишите проверку, различающую реализации даже при одинаковом итоговом количестве.
        throw new NotImplementedException();
    }
}

// 13-11. Многоадресный делегат и исключение
// Условие: ../TASKS.md, раздел 13-11.
public static class Task11
{
    public static List<Exception> InvokeAll(Action[] handlers)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Сравните стандартный multicast с отдельным вызовом каждого обработчика и проверьте порядок ошибок.
        throw new NotImplementedException();
    }
}

// 13-12. Событие пересечения порога
// Условие: ../TASKS.md, раздел 13-12.
public static class Task12
{
    public sealed class Counter
    {
        public Counter(int threshold)
        {
            throw new NotImplementedException();
        }

        public int Value { get; private set; }

        // TODO: Организуйте хранение подписок; допустимо заменить эти заглушки обычным event.
        public event Action<int>? Reached
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public void Add(int delta)
        {
            throw new NotImplementedException();
        }
    }

    public static void Run()
    {
        // TODO: Проверьте переход, повторное пересечение и то значение Counter, которое наблюдает подписчик.
        throw new NotImplementedException();
    }
}

// 13-13. Мемоизация успешных вычислений
// Условие: ../TASKS.md, раздел 13-13.
public static class Task13
{
    public static Func<int, int> Memoize(Func<int, int> compute)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Используйте счётчики для успеха, нуля, исключения и двух независимых экземпляров кэша.
        throw new NotImplementedException();
    }
}

// 13-14. Составное правило с коротким замыканием
// Условие: ../TASKS.md, раздел 13-14.
public static class Task14
{
    public static Predicate<T> All<T>(params Predicate<T>[] rules)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Отдельно проверьте порядок вызовов и влияние замены ячейки исходного массива после создания правила.
        throw new NotImplementedException();
    }
}

// 13-15. Реестр команд
// Условие: ../TASKS.md, раздел 13-15.
public static class Task15
{
    public sealed class CommandRouter
    {
        public void Register(string name, Action<IReadOnlyList<string>> handler)
        {
            throw new NotImplementedException();
        }

        public bool Execute(string name, IReadOnlyList<string> args)
        {
            throw new NotImplementedException();
        }
    }

    public static void Run()
    {
        // TODO: Проверьте регистрацию, поиск другого регистра и сохранение старого обработчика при повторе.
        throw new NotImplementedException();
    }
}
