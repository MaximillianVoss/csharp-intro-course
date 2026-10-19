// Авторский предметный каркас. Модели и API заданы; алгоритмы и проверки предстоит написать.

namespace Course.Lesson14;
// 14-01. Фильтр и проекция
// Условие: ../TASKS.md, раздел 14-01.
public static class Task01
{
    public static int[] EvenSquares(IEnumerable<int> source)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте неверное нечётное значение и источник, запрещающий второе перечисление.
        throw new NotImplementedException();
    }
}

// 14-02. Рейтинг с устойчивыми равенствами
// Условие: ../TASKS.md, раздел 14-02.
public static class Task02
{
    public sealed record Student(string Name, int Score);
    public static Student[] Rank(IEnumerable<Student> students)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Для одинаковых ключей используйте разные объекты и проверяйте их исходный порядок по ссылкам.
        throw new NotImplementedException();
    }
}

// 14-03. Запрос или снимок
// Условие: ../TASKS.md, раздел 14-03.
public static class Task03
{
    public static (int[] Query, int[] Snapshot) Observe()
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Предскажите два вывода до запуска и объясните момент, когда создаётся каждый массив.
        throw new NotImplementedException();
    }
}

// 14-04. Все сдали, но группа пуста
// Условие: ../TASKS.md, раздел 14-04.
public static class Task04
{
    public static bool AllPassed(IEnumerable<int> scores, int threshold)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Разделите смысл пустого All и правило непустой группы; проверьте плохую оценку после неудовлетворительной.
        throw new NotImplementedException();
    }
}

// 14-05. Ноль или отсутствие совпадения
// Условие: ../TASKS.md, раздел 14-05.
public static class Task05
{
    public static int? FirstEven(IEnumerable<int> numbers)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Сделайте источник с опасным хвостом после первого совпадения и проверьте, что он не читается.
        throw new NotImplementedException();
    }
}

// 14-06. Частоты нормализованных слов
// Условие: ../TASKS.md, раздел 14-06.
public static class Task06
{
    public sealed record WordCount(string Word, int Count);
    public static WordCount[] Frequencies(IEnumerable<string> words)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте объединение вариантов одного слова и порядок групп независимо от порядка входа.
        throw new NotImplementedException();
    }
}

// 14-07. Внутреннее соединение заказов
// Условие: ../TASKS.md, раздел 14-07.
public static class Task07
{
    public sealed record Customer(int Id, string Name);
    public sealed record Order(int Id, int CustomerId);
    public sealed record Row(int OrderId, string CustomerName);
    public static Row[] JoinOrders(IEnumerable<Customer> customers, IEnumerable<Order> orders)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте неизвестного клиента и дубликаты независимо с каждой стороны соединения.
        throw new NotImplementedException();
    }
}

// 14-08. Разворачивание рядов с координатами
// Условие: ../TASKS.md, раздел 14-08.
public static class Task08
{
    public sealed record Seat(int Row, int Column, string Label);
    public static Seat[] Flatten(string[][] rows)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Используйте пустой ряд между непустыми и проверьте исходные координаты после разворачивания.
        throw new NotImplementedException();
    }
}

// 14-09. Агрегация корзины без повторного обхода
// Условие: ../TASKS.md, раздел 14-09.
public static class Task09
{
    public sealed record Line(int Quantity, decimal UnitPrice);
    public static (long Units, decimal Total) Totals(IEnumerable<Line> lines)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте пустую корзину, сумму за пределами int и одноразовое перечисление.
        throw new NotImplementedException();
    }
}

// 14-10. Уникальность имени: Single или First
// Условие: ../TASKS.md, раздел 14-10.
public static class Task10
{
    public sealed record User(string Name);
    public static User? FindUnique(IEnumerable<User> users, string name)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Подберите два совпадения разного регистра и покажите, почему FirstOrDefault пропускает дефект.
        throw new NotImplementedException();
    }
}

// 14-11. Словарь последних версий
// Условие: ../TASKS.md, раздел 14-11.
public static class Task11
{
    public sealed record Revision(string Key, int Version, string Value);
    public static Dictionary<string, Revision> Latest(IEnumerable<Revision> source)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте позднюю меньшую версию и последнюю из одинаковых максимальных версий отдельно.
        throw new NotImplementedException();
    }
}

// 14-12. Страница последовательности
// Условие: ../TASKS.md, раздел 14-12.
public static class Task12
{
    public static T[] Page<T>(IEnumerable<T> source, int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте неполную последнюю страницу и наблюдаемый источник при переполнении смещения.
        throw new NotImplementedException();
    }
}

// 14-13. Клиенты без заказов тоже в отчёте
// Условие: ../TASKS.md, раздел 14-13.
public static class Task13
{
    public sealed record Customer(int Id);
    public sealed record Purchase(int CustomerId, decimal Amount);
    public sealed record Summary(int CustomerId, int Orders, decimal Total);
    public static Summary[] CustomerTotals(Customer[] customers, Purchase[] purchases)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте клиента без покупок и переполнение суммы при допустимых отдельных покупках.
        throw new NotImplementedException();
    }
}

// 14-14. Одноразовый источник
// Условие: ../TASKS.md, раздел 14-14.
public static class Task14
{
    public static (int Count, long Sum) Analyze(IEnumerable<int> source)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Сделайте одноразовый источник и выполните обе версии, не заменяя наблюдение оценкой времени.
        throw new NotImplementedException();
    }
}

// 14-15. Отчёт по журналу запросов
// Условие: ../TASKS.md, раздел 14-15.
public static class Task15
{
    public sealed record LogEntry(string Path, int Status, double Milliseconds);
    public sealed record Endpoint(string Path, int Requests, int Failures, double AverageMs);
    public static Endpoint[] Report(IEnumerable<LogEntry> entries, int top)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте, что успешные запросы участвуют в среднем, а top=0 не отключает валидацию.
        throw new NotImplementedException();
    }
}
