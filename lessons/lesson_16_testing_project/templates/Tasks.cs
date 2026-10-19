namespace Course.Lesson16;

// 16-01. Границы и мутация условия
// Условие: ../TASKS.md, раздел 16-01.
public static class Task01
{
    public static bool IsPassing(int score) => throw new NotImplementedException();

    public static void Run()
    {
        // TODO: Составьте таблицу границ и найдите вход, на котором строгий мутант отличается.
        throw new NotImplementedException("Задача 16-01 ещё не решена.");
    }
}

// 16-02. Общее состояние тестов
// Условие: ../TASKS.md, раздел 16-02.
public static class Task02
{
    public sealed class Counter
    {
        // TODO: состояние должно принадлежать экземпляру.
        public int Next() => throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Сначала запишите прогноз фрагмента, затем проверьте два независимых Counter.
        throw new NotImplementedException("Задача 16-02 ещё не решена.");
    }
}

// 16-03. Табличный тест парсера порта
// Условие: ../TASKS.md, раздел 16-03.
public static class Task03
{
    public static bool TryPort(string? text, out int port) => throw new NotImplementedException();

    public static void Run()
    {
        // TODO: Заполните таблицу вход/успех/out; различите грамматику и числовой диапазон.
        throw new NotImplementedException("Задача 16-03 ещё не решена.");
    }
}

// 16-04. Регрессия переполнения процента
// Условие: ../TASKS.md, раздел 16-04.
public static class Task04
{
    public static double Percent(int done, int total) => throw new NotImplementedException();

    public static void Run()
    {
        // TODO: Воспроизведите оба независимых дефекта и сохраните две регрессионные проверки.
        throw new NotImplementedException("Задача 16-04 ещё не решена.");
    }
}

// 16-05. Сообщение после сохранения
// Условие: ../TASKS.md, раздел 16-05.
public static class Task05
{
    public static void Commit(Action save, TextWriter output) => throw new NotImplementedException();

    public static void Run()
    {
        // TODO: Проверьте порядок эффектов, ошибку save, ошибку writer и отсутствие повтора.
        throw new NotImplementedException("Задача 16-05 ещё не решена.");
    }
}

// 16-06. Часы как явная зависимость
// Условие: ../TASKS.md, раздел 16-06.
public static class Task06
{
    public static bool IsOverdue(DateOnly due, DateOnly today) => throw new NotImplementedException();

    public static void Run()
    {
        // TODO: Проверьте даты относительно переданного today, включая переход года.
        throw new NotImplementedException("Задача 16-06 ещё не решена.");
    }
}

// 16-07. Место наблюдения async-ошибки
// Условие: ../TASKS.md, раздел 16-07.
public static class Task07
{
    // Work взят из условия; изменять этот тестовый вход нельзя.
    public static Task<int> Work() => Task.FromException<int>(new IOException("read"));
    public static Task<string> Observe() => throw new NotImplementedException();

    public static void Run()
    {
        // TODO: Сначала запишите прогноз; затем дождитесь Observe и проверьте точку наблюдения ошибки.
        throw new NotImplementedException("Задача 16-07 ещё не решена.");
    }
}

// 16-08. Свойства сортировки вместо одного примера
// Условие: ../TASKS.md, раздел 16-08.
public static class Task08
{
    public static bool IsValidSort(int[] original, int[] result) => throw new NotImplementedException();

    public static void Run()
    {
        // TODO: Проверьте порядок, кратности и неизменность обоих массивов.
        throw new NotImplementedException("Задача 16-08 ещё не решена.");
    }
}

// 16-09. Переводы строк в сравнении вывода
// Условие: ../TASKS.md, раздел 16-09.
public static class Task09
{
    public static string NormalizeNewlines(string text) => throw new NotImplementedException();

    public static void Run()
    {
        // TODO: Проверьте разные переводы строк без потери пробелов и пустых строк.
        throw new NotImplementedException("Задача 16-09 ещё не решена.");
    }
}

// 16-10. Грамматика двух CLI-опций
// Условие: ../TASKS.md, раздел 16-10.
public static class Task10
{
    public sealed record Options(int Limit, bool Reverse);
    public static Options Parse(string[] args) => throw new NotImplementedException();

    public static void Run()
    {
        // TODO: Проверьте оба порядка опций, значения по умолчанию и ошибки грамматики.
        throw new NotImplementedException("Задача 16-10 ещё не решена.");
    }
}

// 16-11. Поток вывода вместо Console
// Условие: ../TASKS.md, раздел 16-11.
public static class Task11
{
    public static void WriteReport(IReadOnlyDictionary<string,int> rows, TextWriter writer) => throw new NotImplementedException();

    public static void Run()
    {
        // TODO: Используйте свой StringWriter, задайте NewLine и проверьте Ordinal-порядок.
        throw new NotImplementedException("Задача 16-11 ещё не решена.");
    }
}

// 16-12. Модель очереди и ошибочный стек
// Условие: ../TASKS.md, раздел 16-12.
public static class Task12
{
    // Только входная последовательность из условия; null обозначает извлечение.
    public static int?[] Operations => [4,7,null,9,null,null];

    public static void Run()
    {
        // TODO: Реализуйте тестовую последовательность с моделью List, проверкой Count и мутантом Stack.
        throw new NotImplementedException("Задача 16-12 ещё не решена.");
    }
}

// 16-13. Отсутствие JSON-флага и false
// Условие: ../TASKS.md, раздел 16-13.
public static class Task13
{
    // TODO: способ представления отсутствующего поля выберите самостоятельно.
    public static bool ReadFlag(string json) => throw new NotImplementedException();

    public static void Run()
    {
        // TODO: Различите отсутствующий флаг, false, неверный тип и повреждённый JSON.
        throw new NotImplementedException("Задача 16-13 ещё не решена.");
    }
}

// 16-14. Пакет обновлений без частичного применения
// Условие: ../TASKS.md, раздел 16-14.
public static class Task14
{
    public static void Apply(Dictionary<string,int> stock, (string Product,int Quantity)[] batch) => throw new NotImplementedException();

    public static void Run()
    {
        // TODO: Проверьте отказ на позднем элементе и полную неизменность stock.
        throw new NotImplementedException("Задача 16-14 ещё не решена.");
    }
}

// 16-15. Приёмка собранного приложения
// Условие: ../TASKS.md, раздел 16-15.
public static class Task15
{
    public static Task ScenarioAsync() => throw new NotImplementedException();

    public static void Run()
    {
        // TODO: Создайте свой temp-каталог; вызовите публичный API несколькими новыми экземплярами.
        throw new NotImplementedException("Задача 16-15 ещё не решена.");
    }
}
