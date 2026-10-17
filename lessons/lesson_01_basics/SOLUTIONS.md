# Урок 1. Первая программа, типы и выражения: решения

Сначала выполните свою попытку по TASKS.md. Здесь приведены ответы.
Каждое решение запускается по ID; Run содержит исполняемые проверки.

<a id="01-01"></a>
## 01-01. Подпись складской ячейки

Вернуть интерполированную строку с двумя подстановками.

**Ход решения**

Сначала выделяем неизменную часть формата: двоеточие, пробел, единица измерения и точка. Затем подставляем item и count через фигурные скобки. Число остаётся int до формирования результата: не требуется вручную превращать каждую цифру в символ. Возвращаем строку, чтобы её мог проверить вызывающий код; Console.WriteLine внутри метода решал бы другую задачу. Проверяем обычное имя, ноль и текст с пробелом: последний случай выявляет попытку обрабатывать только первое слово. Время и память пропорциональны длине результата.

**Частая ошибка**

Забыть пробел или точку; перепутать возвращаемую строку с выводом в консоль.

Полный исходник: [solutions/Task01.cs](solutions/Task01.cs).

```csharp
using System;
using Course;

namespace Course.Lesson01;

public static class Task01
{
    public static string Label(string item, int count)
    {
        return $"{item}: {count} шт.";
    }

    public static void Run()
    {
        Check.Equal("Кабель: 12 шт.", Label("Кабель", 12));
        Check.Equal("Кабель: 0 шт.", Label("Кабель", 0));
        Check.Equal("Белая бумага: 1000000 шт.", Label("Белая бумага", 1_000_000));
    }
}
```

**Исполняемые проверки**

- Кабель и 12
- Нулевое количество
- Имя товара с пробелом и максимальное количество

<a id="01-02"></a>
## 01-02. Разложение длительности

Вычислить seconds / 3600, seconds / 60 % 60 и seconds % 60.

**Ход решения**

Один час содержит 3600 секунд, поэтому целое деление выделяет полные часы. Для минут нельзя просто взять seconds / 60: тогда при 3661 секундах получим 61 минуту. Остаток от деления общего числа минут на 60 оставляет минуты внутри текущего часа. Последняя операция % 60 выделяет секунды. Все величины неотрицательны, поэтому здесь остаток соответствует привычному остатку от деления натуральных чисел. Проверки около смены часа и при нуле подтверждают договорённость о формате. Операций фиксированное число, сложность O(1).

**Частая ошибка**

Вывести общее количество минут вместо минут внутри часа.

Полный исходник: [solutions/Task02.cs](solutions/Task02.cs).

```csharp
using System;
using Course;

namespace Course.Lesson01;

public static class Task02
{
    public static string Duration(int seconds)
    {
        int hours = seconds / 3600;
        int minutes = seconds / 60 % 60;
        int rest = seconds % 60;
        return $"{hours}:{minutes}:{rest}";
    }

    public static void Run()
    {
        Check.Equal("1:1:1", Duration(3661));
        Check.Equal("0:59:59", Duration(3599));
        Check.Equal("0:0:0", Duration(0));
        Check.Equal("1:0:0", Duration(3600));
        Check.Equal("277:46:40", Duration(1_000_000));
    }
}
```

**Исполняемые проверки**

- 3661 секунда
- 3599 секунд
- Ноль
- Ровно один час
- Верхняя граница

<a id="01-03"></a>
## 01-03. Дробь исчезла до присваивания

Исходник получает 3, потому что делит два int; до деления привести total к double.

**Ход решения**

Вычисление правой части происходит до присваивания. total и count имеют тип int, значит 7 / 2 даёт целое 3. Присваивание double сохраняет уже полученное 3 как 3.0; восстановить потерянную половину оно не может. Если один операнд привести к double заранее, второй тоже участвует в вещественном делении. Проверяем нечётное положительное число и отрицательное: целое деление округляет к нулю, а не вниз. Проверка точного деления нужна, но сама по себе ошибку не обнаруживает. Время O(1).

**Частая ошибка**

Писать (double)(total / count): преобразование снова происходит слишком поздно.

Полный исходник: [solutions/Task03.cs](solutions/Task03.cs).

```csharp
using System;
using Course;

namespace Course.Lesson01;

public static class Task03
{
    public static double Truncated(int total, int count)
    {
        double mean = total / count;
        return mean;
    }
    public static double Mean(int total, int count)
    {
        return (double)total / count;
    }

    public static void Run()
    {
        Check.Equal(3.0, Truncated(7, 2));
        Check.Near(3.5, Mean(7, 2));
        Check.Near(-3.5, Mean(-7, 2));
        Check.Near(4, Mean(8, 2));
        Check.Near(0, Mean(0, 100));
    }
}
```

**Исполняемые проверки**

- Исходный код на 7/2
- Исправленный код на 7/2 и -7/2
- Точное деление
- Нулевой числитель

<a id="01-04"></a>
## 01-04. Десятичная скидка

Использовать decimal-формулу unitPrice * quantity * (1m - percent / 100m).

**Ход решения**

Процент — это доля от ста, поэтому после скидки остаётся 1 - percent / 100 исходной цены. Суффикс m фиксирует тип дробных литералов, подходящий к unitPrice. Сначала можно вычислить базовую сумму отдельной переменной для наблюдения в отладчике. По условию округление отсутствует: оно изменило бы контракт для цен с долями копейки. Нулевое количество и скидка 100 процентов дают ноль по разным причинам, поэтому проверяются отдельно. Decimal подходит для этих ограничений, но не является числом бесконечной точности для любых дробей.

**Частая ошибка**

Использовать 0.01 без m и смешать decimal с double; добавить незапрошенное округление.

Полный исходник: [solutions/Task04.cs](solutions/Task04.cs).

```csharp
using System;
using Course;

namespace Course.Lesson01;

public static class Task04
{
    public static decimal Discounted(decimal unitPrice, int quantity, decimal percent)
    {
        decimal subtotal = unitPrice * quantity;
        return subtotal * (1m - percent / 100m);
    }

    public static void Run()
    {
        Check.Equal(53.73m, Discounted(19.90m, 3, 10m));
        Check.Equal(59.70m, Discounted(19.90m, 3, 0m));
        Check.Equal(0m, Discounted(19.90m, 3, 100m));
        Check.Equal(0m, Discounted(999m, 0, 5m));
        Check.Equal(0.009m, Discounted(0.01m, 1, 10m));
    }
}
```

**Исполняемые проверки**

- Цена 19.90, три штуки, скидка 10%
- Нулевая скидка
- Полная скидка
- Нулевое количество
- Доля копейки сохраняется

<a id="01-05"></a>
## 01-05. Скорость с единицами измерения

Вычислить metres / seconds * 3.6.

**Ход решения**

Деление метров на секунды даёт м/с. За час проходит 3600 секунд, а километр содержит 1000 метров, поэтому множитель равен 3600 / 1000 = 3.6. Этот анализ единиц полезнее угадывания формулы. Аргументы уже double, значит целого деления здесь нет. Случай с дробными аргументами отличает решение от варианта, который сначала приводит всё к int. Нулевое расстояние допустимо и даёт нулевую скорость; нулевая длительность исключена контрактом, поэтому здесь не нужно придумывать специальный результат. O(1).

**Частая ошибка**

Умножить на 1000/3600 вместо 3600/1000 или преждевременно округлить.

Полный исходник: [solutions/Task05.cs](solutions/Task05.cs).

```csharp
using System;
using Course;

namespace Course.Lesson01;

public static class Task05
{
    public static double KilometresPerHour(double metres, double seconds)
    {
        return metres / seconds * 3.6;
    }

    public static void Run()
    {
        Check.Near(18, KilometresPerHour(100, 20), 1e-8);
        Check.Near(45, KilometresPerHour(1500, 120), 1e-8);
        Check.Near(0, KilometresPerHour(0, 20));
        Check.Near(9, KilometresPerHour(1.25, 0.5), 1e-8);
    }
}
```

**Исполняемые проверки**

- 100 метров за 20 секунд
- 1500 метров за 120 секунд
- Нулевое расстояние
- Дробные метры и секунды

<a id="01-06"></a>
## 01-06. Пошаговое состояние двух переменных

При start=4 состояния: x=4, y=6, x=18, y=12; возвращается 18:12.

**Ход решения**

После объявления x известно только его значение 4. Объявление y читает x и создаёт 6. Присваивание x читает текущий y=6 и заменяет x на 18; старый x дальше не участвует. Последняя арифметическая строка читает новый x=18 и прежний y=6, поэтому получает 12. Не надо одновременно решать эти строки как систему уравнений. Метод для произвольного start возвращает 3(start+2):2(start+2), но в учебном воспроизведении сохраняем последовательность, чтобы сверить трассировку с отладчиком. Три значения start дают разные начальные состояния.

**Частая ошибка**

Подставить в последнюю строку первоначальный x вместо уже обновлённого.

Полный исходник: [solutions/Task06.cs](solutions/Task06.cs).

```csharp
using System;
using Course;

namespace Course.Lesson01;

public static class Task06
{
    public static string Trace(int start)
    {
        int x = start;
        int y = x + 2;
        x = y * 3;
        y = x - y;
        return $"{x}:{y}";
    }

    public static void Run()
    {
        Check.Equal("18:12", Trace(4));
        Check.Equal("6:4", Trace(0));
        Check.Equal("0:0", Trace(-2));
    }
}
```

**Исполняемые проверки**

- Точная трасса start=4
- Нулевой start
- Start=-2 обнуляет обе переменные

<a id="01-07"></a>
## 01-07. Расширение произошло слишком поздно

Привести width к long внутри произведения.

**Ход решения**

Тип переменной area не меняет тип умножения справа. В исходнике два int дают int; явно указанный unchecked делает оборачивание предсказуемым даже при иной настройке проекта. Для 2 500 000 000 не хватает int, и в long попадает уже неверное отрицательное число. Приведение (long)width поднимает операцию до long до потери данных. Максимальное произведение двух неотрицательных int меньше long.MaxValue, поэтому выбранного типа достаточно. Проверка двух максимальных аргументов подтверждает эту границу, а ноль проверяет простейшую вырожденную площадь.

**Частая ошибка**

Написать (long)(width * height): переполнение уже случилось.

Полный исходник: [solutions/Task07.cs](solutions/Task07.cs).

```csharp
using System;
using Course;

namespace Course.Lesson01;

public static class Task07
{
    public static long BrokenArea(int width, int height)
    {
        long area = unchecked(width * height);
        return area;
    }
    public static long Area(int width, int height)
    {
        return (long)width * height;
    }

    public static void Run()
    {
        Check.Equal(-1_794_967_296L, BrokenArea(50_000, 50_000));
        Check.Equal(2_500_000_000L, Area(50_000, 50_000));
        Check.Equal(4_611_686_014_132_420_609L, Area(int.MaxValue, int.MaxValue));
        Check.Equal(0L, Area(0, int.MaxValue));
    }
}
```

**Исполняемые проверки**

- Выполнен точный исходный вариант
- Площадь 50000×50000
- Максимальные аргументы
- Нулевая сторона

<a id="01-08"></a>
## 01-08. Var не меняет тип переменной

Var выводит int по start; объявление double value = start сохраняет возможность дробного результата.

**Ход решения**

При объявлении с var компилятор один раз определяет тип инициализатора. Здесь start — int, поэтому value тоже int на всё время жизни. Выражение value * 1.5 имеет тип double; неявного обратного преобразования в int нет. Это ошибка компиляции, а не переполнение и не исключение. Нельзя проверять исполнением некомпилируемый фрагмент: в solutions находится его исправленный аналог с явным double. Для проверки обязательно берём нечётный start, иначе необязательное приведение к int могло бы скрыть ошибку. Отрицательный пример подтверждает сохранение знака.

**Частая ошибка**

Считать var аналогом динамического типа или исправить ошибку потерей дробной части.

Полный исходник: [solutions/Task08.cs](solutions/Task08.cs).

```csharp
using System;
using Course;

namespace Course.Lesson01;

public static class Task08
{
    public static double Increased(int start)
    {
        double value = start;
        value = value * 1.5;
        return value;
    }

    public static void Run()
    {
        Check.Near(4.5, Increased(3));
        Check.Near(-4.5, Increased(-3));
        Check.Near(0, Increased(0));
        Check.Near(1_500_000, Increased(1_000_000));
    }
}
```

**Исполняемые проверки**

- Нечётный положительный start
- Отрицательный нечётный start
- Ноль
- Верхняя граница

<a id="01-09"></a>
## 01-09. Выделение повторяющегося выражения

Записать shifted = x + 1; вернуть shifted * shifted.

**Ход решения**

По формуле квадрата суммы x² + 2x + 1 равно (x+1)². Имя shifted сообщает, что промежуточное значение — сдвинутая координата, а не случайная временная переменная. В заданном диапазоне все промежуточные значения обоих вариантов помещаются в long, поэтому равенство школьной алгебры действительно сохраняет поведение. При произвольных машинных целых преобразование выражений может изменить место переполнения, что требует отдельного анализа. Проверяем -1, где результат обнуляется, отрицательное значение и обе границы; исходную формулу тоже исполняем для сопоставления.

**Частая ошибка**

Не анализировать промежуточные значения и считать алгебраическое равенство безусловным для любых типов.

Полный исходник: [solutions/Task09.cs](solutions/Task09.cs).

```csharp
using System;
using Course;

namespace Course.Lesson01;

public static class Task09
{
    public static long Original(long x)
    {
        return x * x + 2 * x + 1;
    }
    public static long Evaluate(long x)
    {
        long shifted = x + 1;
        return shifted * shifted;
    }

    public static void Run()
    {
        Check.Equal(0L, Evaluate(-1));
        Check.Equal(16L, Evaluate(3));
        Check.Equal(4L, Evaluate(-3));
        Check.Equal(Original(-1_000_000), Evaluate(-1_000_000));
        Check.Equal(Original(1_000_000), Evaluate(1_000_000));
    }
}
```

**Исполняемые проверки**

- Корень -1
- Положительное и отрицательное значения
- Сравнение с исходной формулой на обеих границах

<a id="01-10"></a>
## 01-10. Остаток со знаком

Частное округляется к нулю; ненулевой остаток имеет знак делимого a.

**Ход решения**

Число -7/3 лежит между -3 и -2; целочисленное деление C# выбирает -2. Чтобы восстановилось -7 = (-2)*3 + остаток, остаток должен равняться -1. Для 7/(-3) частное тоже -2, но остаток 1. Для -7/(-3) получаем частное 2 и остаток -1. Поэтому % нельзя без дополнительного решения задачи считать всегда неотрицательным математическим модулем. Исходный метод исполняется буквально, а проверка тождества отдельно подтверждает смысл результата. Ограничение b≠0 предотвращает деление на ноль.

**Частая ошибка**

Округлять отрицательное частное вниз или назначать остатку знак делителя.

Полный исходник: [solutions/Task10.cs](solutions/Task10.cs).

```csharp
using System;
using Course;

namespace Course.Lesson01;

public static class Task10
{
    public static string Parts(int a, int b)
    {
        return $"{a / b}:{a % b}";
    }

    public static void Run()
    {
        Check.Equal("-2:-1", Parts(-7, 3));
        Check.Equal("-2:1", Parts(7, -3));
        Check.Equal("2:-1", Parts(-7, -3));
        Check.Equal("0:0", Parts(0, 3));
        Check.Equal(-7, (-7 / 3) * 3 + (-7 % 3));
    }
}
```

**Исполняемые проверки**

- Все три сочетания знаков
- Делимое ноль
- Тождество восстановления для -7 и 3

<a id="01-11"></a>
## 01-11. Тест округления на половине

Использовать Math.Round(amount, 2, MidpointRounding.AwayFromZero) и числа ±2.345 как различающие.

**Ход решения**

На 1.234 оба режима дают 1.23, поэтому такой тест подтверждает только число разрядов. Для 2.345 середина находится между 2.34 и 2.35: требуемый режим выбирает 2.35, а стандартный режим к чётному выбирает 2.34. Отрицательная середина должна уйти от нуля к -2.35, а не к -2.34. Дополняем точным количеством копеек и нулём, чтобы убедиться в отсутствии лишних изменений. Проверка ошибочного варианта выполняется реально: сравниваются его результат и требуемый, и обнаруженное различие утверждается через Check.True.

**Частая ошибка**

Назвать любой дробный тест достаточным для различения двух режимов.

Полный исходник: [solutions/Task11.cs](solutions/Task11.cs).

```csharp
using System;
using Course;

namespace Course.Lesson01;

public static class Task11
{
    public static decimal RoundMoney(decimal amount)
    {
        return Math.Round(amount, 2, MidpointRounding.AwayFromZero);
    }
    public static decimal Wrong(decimal amount)
    {
        return Math.Round(amount, 2);
    }

    public static void Run()
    {
        Check.Equal(1.23m, RoundMoney(1.234m));
        Check.Equal(2.35m, RoundMoney(2.345m));
        Check.Equal(-2.35m, RoundMoney(-2.345m));
        Check.Equal(12.30m, RoundMoney(12.30m));
        Check.Equal(0m, RoundMoney(0m));
        Check.Equal(2.34m, Wrong(2.345m));
        Check.True(Wrong(2.345m) != RoundMoney(2.345m));
    }
}
```

**Исполняемые проверки**

- Обычная дробь
- Положительная и отрицательная середины
- Точные копейки
- Ноль
- Ошибочный вариант различён на 2.345

<a id="01-12"></a>
## 01-12. Явный контроль переполнения суммы

Checked сообщает о выходе за диапазон, unchecked оставляет младшие биты; нормальные суммы совпадают.

**Ход решения**

Диапазон int конечен, поэтому математический результат int.MaxValue+1 в него не помещается. В явно заданном checked происходит OverflowException. В unchecked результат оборачивается к int.MinValue. Аналогично вычитание единицы из int.MinValue оборачивается к int.MaxValue. Это не увеличение точности и не полезный отрицательный ответ: допустимость такого поведения определяется задачей. Обычная сумма 20+22 одинакова в обоих режимах. В Run проверяются и значения, и два ожидаемых исключения; исключения перехватывает инфраструктура курса, поэтому демонстрация не аварийно завершает проверку.

**Частая ошибка**

Предполагать, что checked автоматически расширяет тип до long.

Полный исходник: [solutions/Task12.cs](solutions/Task12.cs).

```csharp
using System;
using Course;

namespace Course.Lesson01;

public static class Task12
{
    public static int CheckedSum(int a, int b)
    {
        return checked(a + b);
    }
    public static int WrappedSum(int a, int b)
    {
        return unchecked(a + b);
    }

    public static void Run()
    {
        Check.Equal(42, CheckedSum(20, 22));
        Check.Equal(42, WrappedSum(20, 22));
        Check.Throws<OverflowException>(() => CheckedSum(int.MaxValue, 1));
        Check.Equal(int.MinValue, WrappedSum(int.MaxValue, 1));
        Check.Throws<OverflowException>(() => CheckedSum(int.MinValue, -1));
        Check.Equal(int.MaxValue, WrappedSum(int.MinValue, -1));
    }
}
```

**Исполняемые проверки**

- Нормальная сумма обоих вариантов
- Положительное переполнение обоих вариантов
- Отрицательное переполнение обоих вариантов

<a id="01-13"></a>
## 01-13. Сравнение измерения с допуском

Вернуть Math.Abs(measured - expected) <= tolerance.

**Ход решения**

Разность может быть положительной или отрицательной, но расстояние между измерениями должно быть неотрицательным. Модуль превращает обе стороны отклонения в одну величину. Затем сравнение <= включает пограничное расстояние по условию. Проверяем оба направления ошибки: реализация без Math.Abs ошибочно пропустит любое достаточно маленькое measured. Для точно представимой границы берём половину, а не 0.1: тогда проверка не зависит от неожиданного двоичного представления десятичной дроби. При tolerance=0 сравниваются сами хранимые значения double. Сложность O(1).

**Частая ошибка**

Проверить только measured - expected <= tolerance без модуля или исключить границу знаком <.

Полный исходник: [solutions/Task13.cs](solutions/Task13.cs).

```csharp
using System;
using Course;

namespace Course.Lesson01;

public static class Task13
{
    public static bool Within(double measured, double expected, double tolerance)
    {
        return Math.Abs(measured - expected) <= tolerance;
    }

    public static void Run()
    {
        Check.True(Within(9.5, 10, 0.5));
        Check.True(Within(10.5, 10, 0.5));
        Check.Equal(false, Within(9.49, 10, 0.5));
        Check.Equal(false, Within(-100, 10, 0.5));
        Check.True(Within(3, 3, 0));
        Check.True(Within(0.1 + 0.2, 0.3, 1e-12));
    }
}
```

**Исполняемые проверки**

- Обе стороны границы 0.5
- Выход за допуск
- Нулевой допуск
- Двоичная погрешность 0.1+0.2 с ненулевым допуском

<a id="01-14"></a>
## 01-14. Сужение числа до байта

Вернуть checked((byte)value).

**Ход решения**

Сужающее преобразование потенциально теряет старшие биты, поэтому C# требует явного приведения. Если добавить checked к приведению, компилятор сгенерирует контроль диапазона назначения. Для byte допустимы обе границы 0 и 255. Значения -1 и 256 проверяют две разные стороны отказа; одна такая проверка не заменяет другую. Ошибочный unchecked-вариант превратил бы 256 в 0, а -1 в 255 и тем самым создал бы внешне допустимые, но неверные данные. Возврат byte сообщает вызывающему коду реальный диапазон результата. Время O(1).

**Частая ошибка**

Поставить checked только вокруг присваивания вызывающего кода, а не самого преобразования.

Полный исходник: [solutions/Task14.cs](solutions/Task14.cs).

```csharp
using System;
using Course;

namespace Course.Lesson01;

public static class Task14
{
    public static byte Channel(int value)
    {
        return checked((byte)value);
    }

    public static void Run()
    {
        Check.Equal((byte)0, Channel(0));
        Check.Equal((byte)128, Channel(128));
        Check.Equal((byte)255, Channel(255));
        Check.Throws<OverflowException>(() => Channel(-1));
        Check.Throws<OverflowException>(() => Channel(256));
        Check.Throws<OverflowException>(() => Channel(int.MaxValue));
    }
}
```

**Исполняемые проверки**

- 0, 128 и 255
- Отказ для -1
- Отказ для 256
- Отказ для int.MaxValue

<a id="01-15"></a>
## 01-15. План покупки краски

Найти площадь, затем Math.Ceiling(area / coverage), затем checked-приведение к int.

**Ход решения**

Два первых аргумента задают длины, их произведение — площадь в м². После деления на покрытие единицы сокращаются до числа банок. Дробную банку купить нельзя, а округление к ближайшему могло бы оставить часть стены непокрашенной, поэтому нужен Ceiling. Это иной смысл округления, чем округление цены до копеек. При максимальной площади и минимальном покрытии нужно 1 000 000 000 банок — результат помещается в int. Проверки охватывают точное деление, необходимость ещё одной банки, малую положительную площадь, ноль и верхнюю границу. Сложность O(1).

**Частая ошибка**

Привести отношение к int до Ceiling или применять обычное округление к ближайшему.

Полный исходник: [solutions/Task15.cs](solutions/Task15.cs).

```csharp
using System;
using Course;

namespace Course.Lesson01;

public static class Task15
{
    public static int Cans(double width, double height, double coverage)
    {
        double area = width * height;
        double required = Math.Ceiling(area / coverage);
        return checked((int)required);
    }

    public static void Run()
    {
        Check.Equal(4, Cans(4, 2.5, 3));
        Check.Equal(2, Cans(3, 2, 3));
        Check.Equal(1, Cans(0.1, 0.1, 100));
        Check.Equal(0, Cans(0, 10, 3));
        Check.Equal(1_000_000_000, Cans(10_000, 10_000, 0.1));
    }
}
```

**Исполняемые проверки**

- Дробное число банок
- Ровно целое число банок
- Очень маленькая положительная площадь
- Нулевая площадь
- Максимальный результат
