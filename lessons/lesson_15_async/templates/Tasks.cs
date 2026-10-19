// Авторский предметный каркас. Модели и API заданы; алгоритмы и проверки предстоит написать.
// Заглушки Task возвращают faulted Task, async-итератор падает при продвижении; это ещё не решение.
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Course.Lesson15;
// 15-01. Ожидание результата функции
// Условие: ../TASKS.md, раздел 15-01.
public static class Task01
{
    public static Task<int> TransformAsync(int value, Func<int, Task<int>> transform)
    {
        return Task.FromException<int>(new NotImplementedException());
    }

    public static Task Run()
    {
        // TODO: Управляйте завершением через TaskCompletionSource и проверьте состояние внешнего Task до разрешения gate.
        return Task.FromException(new NotImplementedException());
    }
}

// 15-02. Асинхронный файловый round trip
// Условие: ../TASKS.md, раздел 15-02.
public static class Task02
{
    public static Task<string> RoundTripAsync(string path, string text, CancellationToken token = default)
    {
        return Task.FromException<string>(new NotImplementedException());
    }

    public static Task Run()
    {
        // TODO: Сравните исходный и прочитанный текст и проверьте сохранность старого файла при неверном Unicode.
        return Task.FromException(new NotImplementedException());
    }
}

// 15-03. До и после незавершённого await
// Условие: ../TASKS.md, раздел 15-03.
public static class Task03
{
    public static Task<string[]> ObserveAsync()
    {
        return Task.FromException<string[]>(new NotImplementedException());
    }

    public static Task Run()
    {
        // TODO: Предскажите весь порядок trace, затем воспроизведите его с незавершённым gate без Task.Delay.
        return Task.FromException(new NotImplementedException());
    }
}

// 15-04. Исправление блокирующего чтения результата
// Условие: ../TASKS.md, раздел 15-04.
public static class Task04
{
    public static Task<int> DoubleAsync(Task<int> input)
    {
        return Task.FromException<int>(new NotImplementedException());
    }

    public static Task Run()
    {
        // TODO: Проверьте незавершённый input и переполнение; возврат Task должен происходить до разрешения gate.
        return Task.FromException(new NotImplementedException());
    }
}

// 15-05. Отмена до и после загрузки
// Условие: ../TASKS.md, раздел 15-05.
public static class Task05
{
    public static Task<int> LengthAsync(Func<CancellationToken, Task<string>> load, CancellationToken token = default)
    {
        return Task.FromException<int>(new NotImplementedException());
    }

    public static Task Run()
    {
        // TODO: Отмените токен внутри зависимости, которая всё же возвращает успешную строку, и проверьте внешний результат.
        return Task.FromException(new NotImplementedException());
    }
}

// 15-06. Все результаты в исходном порядке
// Условие: ../TASKS.md, раздел 15-06.
public static class Task06
{
    public static Task<int[]> RunAllAsync(Func<Task<int>>[] factories)
    {
        return Task.FromException<int[]>(new NotImplementedException());
    }

    public static Task Run()
    {
        // TODO: Завершите задачи в обратном порядке и проверьте, что синхронная ошибка одной фабрики не мешает запустить следующую.
        return Task.FromException(new NotImplementedException());
    }
}

// 15-07. Первая завершившаяся операция
// Условие: ../TASKS.md, раздел 15-07.
public static class Task07
{
    public static Task<string> FirstAsync(Task<string> first, Task<string> second)
    {
        return Task.FromException<string>(new NotImplementedException());
    }

    public static Task Run()
    {
        // TODO: Проверьте уже завершённую ничью и ошибочного победителя; завершите оставшиеся тестовые задачи сами.
        return Task.FromException(new NotImplementedException());
    }
}

// 15-08. Граница ожидаемой ошибки
// Условие: ../TASKS.md, раздел 15-08.
public static class Task08
{
    public static Task<(bool Success, int Value)> TryReadAsync(Func<Task<int>> read)
    {
        return Task.FromException<(bool Success, int Value)>(new NotImplementedException());
    }

    public static Task Run()
    {
        // TODO: Поместите ошибку формата после незавершённого await и отдельно проверьте ошибки, которые нельзя скрывать.
        return Task.FromException(new NotImplementedException());
    }
}

// 15-09. Подсчёт байтов без владения потоком
// Условие: ../TASKS.md, раздел 15-09.
public static class Task09
{
    public static Task<long> CountBytesAsync(Stream input, CancellationToken token = default)
    {
        return Task.FromException<long>(new NotImplementedException());
    }

    public static Task Run()
    {
        // TODO: Сделайте поток с короткими чтениями до EOF и проверьте, что он остаётся открытым.
        return Task.FromException(new NotImplementedException());
    }
}

// 15-10. Тест жизненного цикла сохранения
// Условие: ../TASKS.md, раздел 15-10.
public static class Task10
{
    public static Task SaveThenNotifyAsync(Func<Task> save, Action notify)
    {
        return Task.FromException(new NotImplementedException());
    }

    public static Task Run()
    {
        // TODO: Наблюдайте внешний Task и счётчик уведомлений до и после разрешения save.
        return Task.FromException(new NotImplementedException());
    }
}

// 15-11. Ограниченное число операций
// Условие: ../TASKS.md, раздел 15-11.
public static class Task11
{
    public static Task<int[]> MapBoundedAsync(int[] values, int maximum, Func<int, Task<int>> work, CancellationToken token = default)
    {
        return Task.FromException<int[]>(new NotImplementedException());
    }

    public static Task Run()
    {
        // TODO: Закройте первые слоты gate-ами; отдельно проверьте отмену ожидания и завершение остальных операций после ошибки.
        return Task.FromException(new NotImplementedException());
    }
}

// 15-12. Ограниченные повторные попытки
// Условие: ../TASKS.md, раздел 15-12.
public static class Task12
{
    public static Task<string> RetryAsync(Func<CancellationToken, Task<string>> load, int attempts, CancellationToken token = default)
    {
        return Task.FromException<string>(new NotImplementedException());
    }

    public static Task Run()
    {
        // TODO: Используйте счётчик запусков и разные типы ошибок; проверьте отмену между разрешёнными попытками.
        return Task.FromException(new NotImplementedException());
    }
}

// 15-13. Асинхронный поток непустых строк
// Условие: ../TASKS.md, раздел 15-13.
public static class Task13
{
    public static async IAsyncEnumerable<string> NonBlankLinesAsync(TextReader reader, [EnumeratorCancellation] CancellationToken token = default)
    {
        yield return await Task.FromException<string>(new NotImplementedException());
    }

    public static Task Run()
    {
        // TODO: Продвиньте перечислитель вручную один раз, отмените токен и проверьте оставшуюся строку reader.
        return Task.FromException(new NotImplementedException());
    }
}

// 15-14. Освобождение асинхронного ресурса
// Условие: ../TASKS.md, раздел 15-14.
public static class Task14
{
    public static Task<int> RunWithLeaseAsync(IAsyncDisposable lease, Func<CancellationToken, Task<int>> work, CancellationToken token = default)
    {
        return Task.FromException<int>(new NotImplementedException());
    }

    public static Task Run()
    {
        // TODO: Сделайте DisposeAsync незавершённым и проверьте, что внешний Task ещё не завершён.
        return Task.FromException(new NotImplementedException());
    }
}

// 15-15. Асинхронный импорт проверенного снимка
// Условие: ../TASKS.md, раздел 15-15.
public static class Task15
{
    public sealed class Item
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public static Task<int> ImportAsync(Stream source, Func<Item, CancellationToken, Task> save, CancellationToken token = default)
    {
        return Task.FromException<int>(new NotImplementedException());
    }

    public static Task Run()
    {
        // TODO: Закройте первый save gate-ом и убедитесь, что второй ещё не вызван; отдельно проверьте позднюю ошибку данных и отмену после последнего save.
        return Task.FromException(new NotImplementedException());
    }
}
