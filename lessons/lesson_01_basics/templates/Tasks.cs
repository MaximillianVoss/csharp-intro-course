using System;
using System.IO;

namespace Course.Lesson01;

// 01-01. Подпись складской ячейки
// Полное условие: ../TASKS.md, раздел 01-01.
// Run и методы ниже намеренно не решены.
public static class Task01
{
    public static string Label(string item, int count)
    {
        // TODO: Сформируйте ровно требуемую подпись, сохраняя имя товара и возвращая строку без Console.
        throw new NotImplementedException("01-01: реализуйте Label.");
    }

    public static void Run()
    {
        // TODO: Вызовите Label на своём допустимом примере; отдельно проверьте точные пробелы, знаки и нулевое количество.
        throw new NotImplementedException("Задача 01-01: добавьте свой эксперимент и проверки.");
    }
}

// 01-02. Разложение длительности
// Полное условие: ../TASKS.md, раздел 01-02.
// Run и методы ниже намеренно не решены.
public static class Task02
{
    public static string Duration(int seconds)
    {
        // TODO: Вычислите компоненты длительности в границах из условия и верните H:M:S без ведущих нулей.
        throw new NotImplementedException("01-02: реализуйте Duration.");
    }

    public static void Run()
    {
        // TODO: Запишите проверки нулевой длительности и перехода через час; полный формат сравнивайте как строку.
        throw new NotImplementedException("Задача 01-02: добавьте свой эксперимент и проверки.");
    }
}

// 01-03. Дробь исчезла до присваивания
// Полное условие: ../TASKS.md, раздел 01-03.
// Run и методы ниже намеренно не решены.
public static class Task03
{
    public static double Truncated(int total, int count)
    {
        // TODO: Перенесите исходный метод из условия без исправления; это часть эксперимента.
        throw new NotImplementedException("01-03: реализуйте Truncated.");
    }

    public static double Mean(int total, int count)
    {
        // TODO: Исправьте вычисление среднего, соблюдая требование о дробной части.
        throw new NotImplementedException("01-03: реализуйте Mean.");
    }

    public static void Run()
    {
        // TODO: До запуска запишите прогноз для исходного фрагмента; затем оформите свои сравнения двух методов без заимствования готового ответа.
        throw new NotImplementedException("Задача 01-03: добавьте свой эксперимент и проверки.");
    }
}

// 01-04. Десятичная скидка
// Полное условие: ../TASKS.md, раздел 01-04.
// Run и методы ниже намеренно не решены.
public static class Task04
{
    public static decimal Discounted(decimal unitPrice, int quantity, decimal percent)
    {
        // TODO: Верните decimal-стоимость после скидки; не добавляйте округление, которого нет в контракте.
        throw new NotImplementedException("01-04: реализуйте Discounted.");
    }

    public static void Run()
    {
        // TODO: Проверьте обычный заказ и независимые крайние случаи количества и процента; добавьте случай с долей копейки.
        throw new NotImplementedException("Задача 01-04: добавьте свой эксперимент и проверки.");
    }
}

// 01-05. Скорость с единицами измерения
// Полное условие: ../TASKS.md, раздел 01-05.
// Run и методы ниже намеренно не решены.
public static class Task05
{
    public static double KilometresPerHour(double metres, double seconds)
    {
        // TODO: Получите скорость в требуемых единицах без округления для экрана; сохраняйте вещественные входы.
        throw new NotImplementedException("01-05: реализуйте KilometresPerHour.");
    }

    public static void Run()
    {
        // TODO: Сформулируйте единицы каждого промежуточного значения и проверьте дробные данные с заданным допуском.
        throw new NotImplementedException("Задача 01-05: добавьте свой эксперимент и проверки.");
    }
}

// 01-06. Пошаговое состояние двух переменных
// Полное условие: ../TASKS.md, раздел 01-06.
// Run и методы ниже намеренно не решены.
public static class Task06
{
    public static string Trace(int start)
    {
        // TODO: Воспроизведите инструкции из условия в исходном порядке; самостоятельную таблицу запишите до выполнения.
        throw new NotImplementedException("01-06: реализуйте Trace.");
    }

    public static void Run()
    {
        // TODO: Запишите таблицу по шагам для заданного start; только затем добавьте воспроизводимый запуск и сравнение прогноза с наблюдением.
        throw new NotImplementedException("Задача 01-06: добавьте свой эксперимент и проверки.");
    }
}

// 01-07. Расширение произошло слишком поздно
// Полное условие: ../TASKS.md, раздел 01-07.
// Run и методы ниже намеренно не решены.
public static class Task07
{
    public static long BrokenArea(int width, int height)
    {
        // TODO: Перенесите ошибочный вариант из условия без исправления для воспроизводимого сравнения.
        throw new NotImplementedException("01-07: реализуйте BrokenArea.");
    }

    public static long Area(int width, int height)
    {
        // TODO: Исправьте промежуточный тип произведения, сохранив полную область допустимых размеров.
        throw new NotImplementedException("01-07: реализуйте Area.");
    }

    public static void Run()
    {
        // TODO: Сопоставьте исходник и исправление на входе, где int-произведение не помещается; затем проверьте границы контракта.
        throw new NotImplementedException("Задача 01-07: добавьте свой эксперимент и проверки.");
    }
}

// 01-08. Var не меняет тип переменной
// Полное условие: ../TASKS.md, раздел 01-08.
// Run и методы ниже намеренно не решены.
public static class Task08
{
    public static double Increased(int start)
    {
        // TODO: Исправьте объявление value и увеличение из условия, не устраняя ошибку потерей дробной части.
        throw new NotImplementedException("01-08: реализуйте Increased.");
    }

    public static void Run()
    {
        // TODO: Сначала объясните ошибку компиляции исходника; в проверках исправления обязательно используйте значение с дробным результатом.
        throw new NotImplementedException("Задача 01-08: добавьте свой эксперимент и проверки.");
    }
}

// 01-09. Выделение повторяющегося выражения
// Полное условие: ../TASKS.md, раздел 01-09.
// Run и методы ниже намеренно не решены.
public static class Task09
{
    public static long Original(long x)
    {
        // TODO: Воспроизведите исходную формулу из условия для сопоставления результатов.
        throw new NotImplementedException("01-09: реализуйте Original.");
    }

    public static long Evaluate(long x)
    {
        // TODO: Перепишите формулу в заданной форме через shifted, сохраняя допустимую область.
        throw new NotImplementedException("01-09: реализуйте Evaluate.");
    }

    public static void Run()
    {
        // TODO: Сравните исходную и переработанную версии на характерных значениях и границах; поясните оценку промежуточных значений.
        throw new NotImplementedException("Задача 01-09: добавьте свой эксперимент и проверки.");
    }
}

// 01-10. Остаток со знаком
// Полное условие: ../TASKS.md, раздел 01-10.
// Run и методы ниже намеренно не решены.
public static class Task10
{
    public static string Parts(int a, int b)
    {
        // TODO: Воспроизведите исходный фрагмент; не заменяйте остаток собственной нормализацией.
        throw new NotImplementedException("01-10: реализуйте Parts.");
    }

    public static void Run()
    {
        // TODO: Сначала вычислите частное и остаток для каждого знакового случая и проверьте восстановление исходного делимого; затем выполните метод.
        throw new NotImplementedException("Задача 01-10: добавьте свой эксперимент и проверки.");
    }
}

// 01-11. Тест округления на половине
// Полное условие: ../TASKS.md, раздел 01-11.
// Run и методы ниже намеренно не решены.
public static class Task11
{
    public static decimal RoundMoney(decimal amount)
    {
        // TODO: Реализуйте указанное в условии денежное правило округления.
        throw new NotImplementedException("01-11: реализуйте RoundMoney.");
    }

    public static decimal Wrong(decimal amount)
    {
        // TODO: Перенесите контрпример из условия или восстановите явно описанный ошибочный вариант, не исправляя его.
        throw new NotImplementedException("01-11: реализуйте Wrong.");
    }

    public static void Run()
    {
        // TODO: Спроектируйте свои различающие случаи и объясните каждый; исполните их на правильной и ошибочной версиях, не ограничиваясь комментариями.
        throw new NotImplementedException("Задача 01-11: добавьте свой эксперимент и проверки.");
    }
}

// 01-12. Явный контроль переполнения суммы
// Полное условие: ../TASKS.md, раздел 01-12.
// Run и методы ниже намеренно не решены.
public static class Task12
{
    public static int CheckedSum(int a, int b)
    {
        // TODO: Воспроизведите первое выражение из условия буквально.
        throw new NotImplementedException("01-12: реализуйте CheckedSum.");
    }

    public static int WrappedSum(int a, int b)
    {
        // TODO: Воспроизведите второе выражение из условия буквально.
        throw new NotImplementedException("01-12: реализуйте WrappedSum.");
    }

    public static void Run()
    {
        // TODO: До запуска составьте прогноз для каждого вызова; самостоятельно выберите проверку значения или отказа и сравните оба контекста.
        throw new NotImplementedException("Задача 01-12: добавьте свой эксперимент и проверки.");
    }
}

// 01-13. Сравнение измерения с допуском
// Полное условие: ../TASKS.md, раздел 01-13.
// Run и методы ниже намеренно не решены.
public static class Task13
{
    public static bool Within(double measured, double expected, double tolerance)
    {
        // TODO: Реализуйте проверку абсолютного допуска с включённой границей; округление показа не используйте как сравнение.
        throw new NotImplementedException("01-13: реализуйте Within.");
    }

    public static void Run()
    {
        // TODO: Проверьте оба направления отклонения, точное попадание на границу и нулевой допуск; ожидаемые bool определите из контракта.
        throw new NotImplementedException("Задача 01-13: добавьте свой эксперимент и проверки.");
    }
}

// 01-14. Сужение числа до байта
// Полное условие: ../TASKS.md, раздел 01-14.
// Run и методы ниже намеренно не решены.
public static class Task14
{
    public static byte Channel(int value)
    {
        // TODO: Реализуйте проверяемое преобразование в byte с указанным отказом вне диапазона.
        throw new NotImplementedException("01-14: реализуйте Channel.");
    }

    public static void Run()
    {
        // TODO: Проверьте границы byte и значения с каждой стороны; используйте предоставленный Check.Throws для договорённого отказа.
        throw new NotImplementedException("Задача 01-14: добавьте свой эксперимент и проверки.");
    }
}

// 01-15. План покупки краски
// Полное условие: ../TASKS.md, раздел 01-15.
// Run и методы ниже намеренно не решены.
public static class Task15
{
    public static int Cans(decimal width, decimal height, decimal coverage)
    {
        // TODO: Постройте decimal-расчёт минимального целого числа банок на заданной десятичной сетке, не округляя промежуточную площадь.
        throw new NotImplementedException("01-15: реализуйте Cans.");
    }

    public static void Run()
    {
        // TODO: Добавьте проверки точной потребности и соседних размеров на десятичной сетке, нулевой площади и максимально допустимого результата.
        throw new NotImplementedException("Задача 01-15: добавьте свой эксперимент и проверки.");
    }
}
