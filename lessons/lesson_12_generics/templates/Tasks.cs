// Авторский предметный каркас. Модели и API заданы; алгоритмы и проверки предстоит написать.

namespace Course.Lesson12;
// 12-01. Обмен двух значений
// Условие: ../TASKS.md, раздел 12-01.
public static class Task01
{
    public static void Swap<T>(ref T first, ref T second)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте разные типы T и два ref, обозначающих одну переменную.
        throw new NotImplementedException();
    }
}

// 12-02. Первый элемент или запасной
// Условие: ../TASKS.md, раздел 12-02.
public static class Task02
{
    public static T FirstOr<T>(IReadOnlyList<T> items, T fallback)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Сравните отсутствующий элемент и существующий default; используйте наблюдаемый список без полного обхода.
        throw new NotImplementedException();
    }
}

// 12-03. Статическое поле для каждого T
// Условие: ../TASKS.md, раздел 12-03.
public static class Task03
{
    public sealed class Box<T>
    {
        public static int Count;
    }

    public static (int IntCount, int StringCount) Observe()
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Запишите прогноз, воспроизведите фрагмент дважды и проверьте влияние создания нового Box.
        throw new NotImplementedException();
    }
}

// 12-04. Исправление обобщённого Head
// Условие: ../TASKS.md, раздел 12-04.
public static class Task04
{
    public static T Head<T>(T[] items)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Объясните ошибку компиляции исходника и проверьте идентичность возвращаемого объекта.
        throw new NotImplementedException();
    }
}

// 12-05. Поиск сущности с ограничением
// Условие: ../TASKS.md, раздел 12-05.
public static class Task05
{
    public interface IHasKey
    {
        int Key { get; }
    }

    public sealed record User(int Key, string Name) : IHasKey;
    public sealed record Ticket(int Key) : IHasKey;
    public static T? Find<T>(IReadOnlyList<T> items, int key)
        where T : class, IHasKey
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Испытайте два класса и null в хвосте после подходящего первого объекта.
        throw new NotImplementedException();
    }
}

// 12-06. Стек без потери default
// Условие: ../TASKS.md, раздел 12-06.
public static class Task06
{
    public sealed class ArrayStack<T>
    {
        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Push(T value)
        {
            throw new NotImplementedException();
        }

        public bool TryPop(out T value)
        {
            throw new NotImplementedException();
        }
    }

    public static void Run()
    {
        // TODO: Проследите Count и результат TryPop для сохранённого default и затем пустого стека.
        throw new NotImplementedException();
    }
}

// 12-07. Ограниченное копирование через вариантные интерфейсы
// Условие: ../TASKS.md, раздел 12-07.
public static class Task07
{
    public interface ISource<out T>
    {
        bool HasNext { get; }

        T Read();
    }

    public interface ISink<in T>
    {
        void Write(T value);
    }

    public class Animal
    {
        public Animal(string name)
        {
            throw new NotImplementedException();
        }

        public string Name { get; }
    }

    public sealed class Dog : Animal
    {
        public Dog(string name) : base(name)
        {
            throw new NotImplementedException();
        }
    }

    public static int CopyAtMost<T>(ISource<T> source, ISink<T> sink, int limit)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Создайте наблюдаемые source/sink и докажите отсутствие лишнего чтения после лимита и EOF.
        throw new NotImplementedException();
    }
}

// 12-08. Создание независимых экземпляров T
// Условие: ../TASKS.md, раздел 12-08.
public static class Task08
{
    public sealed class Widget
    {
        public static int Created { get; set; }
        public int Value { get; set; }

        public Widget()
        {
            throw new NotImplementedException();
        }
    }

    public sealed class Note
    {
        public string Text { get; set; } = "empty";
    }

    public static T[] CreateMany<T>(int count)
        where T : class, new()
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте счётчик конструирований и независимость изменяемых объектов, включая второй тип.
        throw new NotImplementedException();
    }
}

// 12-09. Типобезопасный результат
// Условие: ../TASKS.md, раздел 12-09.
public static class Task09
{
    public sealed class Result<T>
    {
        private Result(bool success, T value, string? error)
        {
            throw new NotImplementedException();
        }

        public bool IsSuccess { get; }
        public string? Error { get; }

        public T Value
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public static Result<T> Ok(T value)
        {
            throw new NotImplementedException();
        }

        public static Result<T> Fail(string error)
        {
            throw new NotImplementedException();
        }
    }

    public static void Run()
    {
        // TODO: Проверьте успешные default-значения и невозможность чтения Value у неуспеха.
        throw new NotImplementedException();
    }
}

// 12-10. Что означает копия T[]
// Условие: ../TASKS.md, раздел 12-10.
public static class Task10
{
    public sealed class Box
    {
        public int Value { get; set; }
    }

    public static T[] Copy<T>(T[] source)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Отдельно проверьте массив, его ячейку и объект, на который эта ячейка ссылается.
        throw new NotImplementedException();
    }
}

// 12-11. Мультисловарь
// Условие: ../TASKS.md, раздел 12-11.
public static class Task11
{
    public sealed class MultiMap<TKey, TValue>
        where TKey : notnull
    {
        public MultiMap(IEqualityComparer<TKey>? comparer = null)
        {
            throw new NotImplementedException();
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public TValue[] Get(TKey key)
        {
            throw new NotImplementedException();
        }
    }

    public static void Run()
    {
        // TODO: Проверьте пользовательское равенство ключей и независимость возвращённого массива.
        throw new NotImplementedException();
    }
}

// 12-12. Первый индекс двоичным поиском
// Условие: ../TASKS.md, раздел 12-12.
public static class Task12
{
    public static int FirstIndex<T>(IReadOnlyList<T> sorted, T target)
        where T : IComparable<T>
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте первое из равных и число сравнений собственным типом с наблюдаемым CompareTo.
        throw new NotImplementedException();
    }
}

// 12-13. Чтение коллекции базового типа
// Условие: ../TASKS.md, раздел 12-13.
public static class Task13
{
    public class Animal
    {
        public Animal(string name)
        {
            throw new NotImplementedException();
        }

        public string Name { get; }
    }

    public sealed class Cat : Animal
    {
        public Cat(string name) : base(name)
        {
            throw new NotImplementedException();
        }
    }

    public static string[] Names(IReadOnlyList<Animal> animals)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Объясните строки A/B отдельно; проверьте компилируемый вызов Names с List<Cat>.
        throw new NotImplementedException();
    }
}

// 12-14. Кольцевой буфер последних значений
// Условие: ../TASKS.md, раздел 12-14.
public static class Task14
{
    public sealed class RingBuffer<T>
    {
        public RingBuffer(int capacity)
        {
            throw new NotImplementedException();
        }

        public int Count { get; private set; }

        public void Add(T value)
        {
            throw new NotImplementedException();
        }

        public T[] Snapshot()
        {
            throw new NotImplementedException();
        }
    }

    public static void Run()
    {
        // TODO: Проследите порядок при неполном буфере, заполнении и нескольких оборотах кольца.
        throw new NotImplementedException();
    }
}

// 12-15. Типобезопасная прямоугольная сетка
// Условие: ../TASKS.md, раздел 12-15.
public static class Task15
{
    public sealed class Grid<T>
    {
        public Grid(int rows, int columns, T initial)
        {
            throw new NotImplementedException();
        }

        public int Rows { get; }
        public int Columns { get; }

        public T this[int row, int column]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public T[] CopyRow(int row)
        {
            throw new NotImplementedException();
        }
    }

    public static void Run()
    {
        // TODO: Используйте неквадратную сетку и проверьте каждую координату отдельно от линейного индекса.
        throw new NotImplementedException();
    }
}
