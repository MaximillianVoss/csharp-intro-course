# Урок 3. Циклы, состояния и численные алгоритмы: решения

Сначала выполните свою попытку по TASKS.md. Здесь приведены ответы.
Каждое решение запускается по ID; Run содержит исполняемые проверки.

<a id="03-01"></a>
## 03-01. Сумма префикса натурального ряда

Начать с нулевой long-суммы и пройти i от 1 до n включительно.

**Ход решения**

До первого шага сумма пустого префикса равна нулю. Перед шагом i накоплены числа от 1 до i-1, после прибавления i — от 1 до i. Когда i становится n+1, в накопителе ровно нужный префикс. Для n=0 тело не выполняется. На максимальном n результат 500000500000, поэтому int непригоден. Проверка n=1 выявляет неверное начало, n=4 — пропуск последнего слагаемого, верхняя граница — неверный тип. Время O(n), дополнительная память O(1).

**Частая ошибка**

Писать i&lt;n вместо i&lt;=n или хранить сумму в int.

Полный исходник: [solutions/Task01.cs](solutions/Task01.cs).

```csharp
using System;
using Course;

namespace Course.Lesson03;

public static class Task01
{
    public static long SumTo(int n)
    {
        long sum = 0;
        for (int i = 1; i <= n; i++) sum += i;
        return sum;
    }

    public static void Run()
    {
        Check.Equal(0L, SumTo(0));
        Check.Equal(1L, SumTo(1));
        Check.Equal(10L, SumTo(4));
        Check.Equal(500_000_500_000L, SumTo(1_000_000));
    }
}
```

**Исполняемые проверки**

- n=0
- n=1
- n=4
- Максимальный n

<a id="03-02"></a>
## 03-02. Сколько десятичных цифр у int

В long безопасно взять модуль и выполнить do/while с делением на 10.

**Ход решения**

Одна операция /10 удаляет последнюю десятичную цифру неотрицательного числа. Количество таких удалений до нуля и есть число цифр. Для исходного нуля всё равно нужна одна итерация, поэтому do/while выражает правило прямо. Сначала переносим int в long и лишь затем отрицаем отрицательное значение; обратный порядок переполнил бы int.MinValue. Ведущие нули числа как значения не существуют. Проверяем переходы 9→10, ноль, отрицательное число с нулями и минимальный int. Время O(log10(|value|+1)), память O(1).

**Частая ошибка**

Использовать while без особого нуля или Math.Abs(int.MinValue).

Полный исходник: [solutions/Task02.cs](solutions/Task02.cs).

```csharp
using System;
using Course;

namespace Course.Lesson03;

public static class Task02
{
    public static int DigitCount(int value)
    {
        long rest = value;
        if (rest < 0) rest = -rest;
        int count = 0;
        do
        {
            count++;
            rest /= 10;
        } while (rest != 0);
        return count;
    }

    public static void Run()
    {
        Check.Equal(1, DigitCount(0));
        Check.Equal(1, DigitCount(9));
        Check.Equal(2, DigitCount(10));
        Check.Equal(4, DigitCount(-1200));
        Check.Equal(10, DigitCount(int.MinValue));
        Check.Equal(10, DigitCount(int.MaxValue));
    }
}
```

**Исполняемые проверки**

- 0
- 9 и 10
- Отрицательное число с нулями
- int.MinValue и int.MaxValue

<a id="03-03"></a>
## 03-03. Факториал с единицей пустого произведения

Перемножить целые от 2 до n с начальным результатом 1.

**Ход решения**

Нейтральное значение умножения — единица: пустое произведение должно её сохранить. Начало с нуля сделало бы все дальнейшие результаты нулевыми. Перед шагом i уже хранится (i-1)!, после умножения — i!. При n=0 и n=1 итераций нет и возвращается 1. Диапазон ограничен 20, потому что 20! помещается в long, а 21! уже нет. Checked делает случайное расширение диапазона заметным. Проверки включают оба пустых произведения, обычное значение и максимально допустимый факториал. O(n) времени и O(1) памяти.

**Частая ошибка**

Начать произведение с 0 или умножить на 0 первой итерацией.

Полный исходник: [solutions/Task03.cs](solutions/Task03.cs).

```csharp
using System;
using Course;

namespace Course.Lesson03;

public static class Task03
{
    public static long Factorial(int n)
    {
        long result = 1;
        for (int factor = 2; factor <= n; factor++) result = checked(result * factor);
        return result;
    }

    public static void Run()
    {
        Check.Equal(1L, Factorial(0));
        Check.Equal(1L, Factorial(1));
        Check.Equal(120L, Factorial(5));
        Check.Equal(2_432_902_008_176_640_000L, Factorial(20));
    }
}
```

**Исполняемые проверки**

- 0!
- 1!
- 5!
- 20!

<a id="03-04"></a>
## 03-04. Ближайшая достаточная степень двойки

Хранить long power=1 и удваивать при power&lt;requested.

**Ход решения**

В любой момент power является степенью двойки. Пока power меньше запроса, оно заведомо недостаточно и можно переходить к следующей степени. Когда условие ложно, текущая степень достаточна, а предыдущая была меньше requested; этим доказана минимальность. Равенство должно останавливать цикл: &lt;= выделило бы лишний размер. Значение удваивается, поэтому итераций логарифмически мало. Максимальный запрос требует 1073741824, помещающийся и в int, но long делает смысл запаса явным. Проверяем точную степень, число между степенями, ноль и верхний запрос.

**Частая ошибка**

Начать с 0, который при удвоении не растёт; использовать &lt;= и пропустить точное совпадение.

Полный исходник: [solutions/Task04.cs](solutions/Task04.cs).

```csharp
using System;
using Course;

namespace Course.Lesson03;

public static class Task04
{
    public static long Capacity(int requested)
    {
        long power = 1;
        while (power < requested) power *= 2;
        return power;
    }

    public static void Run()
    {
        Check.Equal(1L, Capacity(0));
        Check.Equal(1L, Capacity(1));
        Check.Equal(8L, Capacity(8));
        Check.Equal(16L, Capacity(9));
        Check.Equal(1_073_741_824L, Capacity(1_000_000_000));
    }
}
```

**Исполняемые проверки**

- 0 и 1
- Точная степень 8
- Запрос 9
- Максимальный запрос

<a id="03-05"></a>
## 03-05. Пропуск запрещённых номеров

В for сначала отсеять i%2==0 или i%5==0, затем увеличить счётчик.

**Ход решения**

Для каждого номера нужно принять ровно одно решение. Запрещённость определяется дизъюнкцией: достаточно быть чётным либо кратным 5. Continue перескакивает к шагу for, после которого снова проверяется условие; счётчик принятых не меняется, но номер продвигается. В while с ручным шагом аналогичное размещение continue могло бы создать бесконечный цикл. Тесты включают запрет по нечётной пятёрке, разрешённое число после неё и полный десяток, чтобы обе причины фильтрации были проверены. O(limit) времени, O(1) памяти.

**Частая ошибка**

Применить && вместо || к двум причинам запрета или поставить счётчик до continue.

Полный исходник: [solutions/Task05.cs](solutions/Task05.cs).

```csharp
using System;
using Course;

namespace Course.Lesson03;

public static class Task05
{
    public static int AcceptedCount(int limit)
    {
        int accepted = 0;
        for (int i = 1; i <= limit; i++)
        {
            if (i % 2 == 0 || i % 5 == 0) continue;
            accepted++;
        }
        return accepted;
    }

    public static void Run()
    {
        Check.Equal(0, AcceptedCount(0));
        Check.Equal(1, AcceptedCount(1));
        Check.Equal(2, AcceptedCount(5));
        Check.Equal(4, AcceptedCount(10));
        Check.Equal(400_000, AcceptedCount(1_000_000));
    }
}
```

**Исполняемые проверки**

- Пустой диапазон
- Один разрешённый номер
- Граница 5
- Полный десяток
- Верхняя граница

<a id="03-06"></a>
## 03-06. Итерация do/while для нулевого старта

Тело выполняется минимум один раз; при старте 8 нужны четыре целых деления на 2.

**Ход решения**

Проверка x&gt;0 расположена после тела, поэтому даже x=0 сначала делится и увеличивает steps. При старте 1 первое деление сразу даёт ноль, ответ тоже 1. Для 8 последовательно получаются 4,2,1,0: цикл останавливается после четвёртого тела. Число проверок условия здесь совпадает с числом итераций, но оно не совпадает с результатом похожего while(x&gt;0) при нуле. Выполняем именно данный код, поскольку задача на чтение требует подтвердить его поведение, а не предложить более привычный вариант. Прогресс обеспечен уменьшением положительного x.

**Частая ошибка**

Считать, что старт 0 даёт ноль итераций, как у while.

Полный исходник: [solutions/Task06.cs](solutions/Task06.cs).

```csharp
using System;
using Course;

namespace Course.Lesson03;

public static class Task06
{
    public static int Steps(int start)
    {
        int x = start;
        int steps = 0;
        do
        {
            x /= 2;
            steps++;
        } while (x > 0);
        return steps;
    }

    public static void Run()
    {
        Check.Equal(1, Steps(0));
        Check.Equal(1, Steps(1));
        Check.Equal(2, Steps(2));
        Check.Equal(4, Steps(8));
        Check.Equal(4, Steps(9));
    }
}
```

**Исполняемые проверки**

- Старты 0, 1, 2, 8 и 9

<a id="03-07"></a>
## 03-07. Простой квадрат и неверная граница

Включить квадратный корень и заменить произведение безопасным d&lt;=n/d.

**Ход решения**

У составного числа есть делитель не больше квадратного корня: иначе произведение двух делителей было бы больше самого числа. Равенство обязательно, поскольку у 49 единственный малый нетривиальный делитель 7. В исходнике d*d может также выйти за int при больших n. Для положительных d условие d&lt;=n/d задаёт ту же целую границу без умножения. Значения меньше 2 исключаем заранее. Проверки 4 и 49 выявляют пропуск квадрата, большое простое int.MaxValue проверяет безопасное завершение цикла. Время O(sqrt(n)), память O(1).

**Частая ошибка**

Исправить только &lt; на &lt;=, оставив переполняющееся d*d.

Полный исходник: [solutions/Task07.cs](solutions/Task07.cs).

```csharp
using System;
using Course;

namespace Course.Lesson03;

public static class Task07
{
    public static bool Broken(int n)
    {
        if (n < 2) return false;
        for (int d = 2; d * d < n; d++)
            if (n % d == 0) return false;
        return true;
    }
    public static bool IsPrime(int n)
    {
        if (n < 2) return false;
        for (int d = 2; d <= n / d; d++)
            if (n % d == 0) return false;
        return true;
    }

    public static void Run()
    {
        Check.True(Broken(49));
        Check.Equal(false, IsPrime(0));
        Check.Equal(false, IsPrime(1));
        Check.True(IsPrime(2));
        Check.Equal(false, IsPrime(4));
        Check.Equal(false, IsPrime(49));
        Check.True(IsPrime(97));
        Check.True(IsPrime(int.MaxValue));
    }
}
```

**Исполняемые проверки**

- Исходник ошибается на 49
- 0 и 1 не простые
- 2 простое
- 4 и 49 составные
- 97 и int.MaxValue простые

<a id="03-08"></a>
## 03-08. Одно место чтения строки

Перенести чтение внутрь while(true), сразу проверить null и завершить break.

**Ход решения**

У исходника одинаковое чтение расположено до цикла и в конце тела. При добавлении continue легко забыть второе место и зациклиться на одной строке. В новой структуре каждая итерация начинается чтением, null немедленно завершает цикл, а прочая строка либо добавляет число, либо игнорируется. Так отбрасывание неверных строк не мешает продвижению. Long выдерживает сумму тысячи любых int. В Run независимые StringReader предоставляются обоим вариантам: один и тот же уже прочитанный поток повторно использовать нельзя. Память O(1), время пропорционально суммарной длине входа.

**Частая ошибка**

Читать вторую строку в условии и пропускать каждую чётную; заменять EOF повторной попыткой.

Полный исходник: [solutions/Task08.cs](solutions/Task08.cs).

```csharp
using System;
using Course;
using System.IO;

namespace Course.Lesson03;

public static class Task08
{
    public static long Original(TextReader reader)
    {
        long sum = 0;
        string? line = reader.ReadLine();
        while (line is not null)
        {
            if (int.TryParse(line, out int value)) sum += value;
            line = reader.ReadLine();
        }
        return sum;
    }
    public static long SumLines(TextReader reader)
    {
        long sum = 0;
        while (true)
        {
            string? line = reader.ReadLine();
            if (line is null) break;
            if (int.TryParse(line, out int value)) sum += value;
        }
        return sum;
    }

    public static void Run()
    {
        using var original = new StringReader("3\nbad\n-2\n");
        using var revised = new StringReader("3\nbad\n-2\n");
        Check.Equal(1L, SumLines(revised));
        Check.Equal(1L, Original(original));
        using var empty = new StringReader("");
        Check.Equal(0L, SumLines(empty));
        using var invalid = new StringReader("bad\n\n");
        Check.Equal(0L, SumLines(invalid));
        using var large = new StringReader("2147483647\n2147483647\n");
        Check.Equal(4_294_967_294L, SumLines(large));

        // Review 2026-10-19: reader remains caller owned
        Check.Equal<string?>(null, revised.ReadLine());
    }
}
```

**Исполняемые проверки**

- Смешанный поток сравнивается с исходником
- Пустой поток
- Неверная и пустая строки
- Сумма двух int.MaxValue
- После SumLines источник остаётся доступен вызывающему коду: следующее ReadLine корректно возвращает EOF

<a id="03-09"></a>
## 03-09. Наибольший общий делитель

Пока b не ноль, сохранить остаток, присвоить a=b, b=остаток.

**Ход решения**

Общие делители a и b совпадают с общими делителями b и a%b: вычитание целого числа копий b не меняет делимость. При положительном b остаток лежит от 0 до b-1, поэтому вторая величина строго уменьшается до нуля. После остановки НОД(a,0)=a. Важно сохранить остаток до перезаписи a, иначе выражение будет вычислено уже для другой пары. Нулевые входы обрабатываются тем же алгоритмом. Проверки включают равные числа, взаимно простые и оба нуля. Время логарифмическое по большему аргументу, память O(1).

**Частая ошибка**

Обновить a до вычисления остатка или брать остаток при b=0.

Полный исходник: [solutions/Task09.cs](solutions/Task09.cs).

```csharp
using System;
using Course;

namespace Course.Lesson03;

public static class Task09
{
    public static int Gcd(int a, int b)
    {
        while (b != 0)
        {
            int remainder = a % b;
            a = b;
            b = remainder;
        }
        return a;
    }

    public static void Run()
    {
        Check.Equal(6, Gcd(48, 18));
        Check.Equal(1, Gcd(17, 13));
        Check.Equal(7, Gcd(0, 7));
        Check.Equal(7, Gcd(7, 0));
        Check.Equal(0, Gcd(0, 0));
        Check.Equal(int.MaxValue, Gcd(int.MaxValue, int.MaxValue));
    }
}
```

**Исполняемые проверки**

- 48 и 18
- Взаимно простые
- Ноль с каждой стороны
- Оба нуля
- Равные максимальные числа

<a id="03-10"></a>
## 03-10. Вклад с ежегодным округлением

Обновлять decimal-состояние максимум maxYears раз и проверять порог после каждого начисления.

**Ход решения**

На шаге year хранится сумма именно после year ежегодных начислений. Начальную проверку выполняем до цикла, чтобы не приписывать лишний год уже достигнутой цели. Округление производится на каждом шаге и влияет на следующий, поэтому заменить цикл общим множителем нельзя. При initial=0 или очень малом initial рост может отсутствовать из-за округления; ограничение maxYears гарантирует завершение независимо от достижения цели. Проверка initial=0.01,target=0.02 выявляет эффект округления: 0.011 каждый год снова становится 0.01. Время O(maxYears), память O(1).

**Частая ошибка**

Округлять только окончательный результат или ждать роста без ограничения при нулевом балансе.

Полный исходник: [solutions/Task10.cs](solutions/Task10.cs).

```csharp
using System;
using Course;

namespace Course.Lesson03;

public static class Task10
{
    public static int Years(decimal initial, decimal target, int maxYears)
    {
        decimal balance = initial;
        if (balance >= target) return 0;
        for (int year = 1; year <= maxYears; year++)
        {
            balance = Math.Round(balance * 1.10m, 2, MidpointRounding.AwayFromZero);
            if (balance >= target) return year;
        }
        return -1;
    }

    public static void Run()
    {
        Check.Equal(2, Years(100m, 121m, 5));
        Check.Equal(0, Years(100m, 99m, 0));
        Check.Equal(-1, Years(100m, 101m, 0));
        Check.Equal(-1, Years(0m, 1m, 30));
        Check.Equal(-1, Years(0.01m, 0.02m, 30));
        Check.Equal(-1, Years(100m, 200m, 2));
    }
}
```

**Исполняемые проверки**

- Достижение на втором году
- Цель уже достигнута
- Ноль лет
- Нулевой баланс
- Малая сумма не растёт
- Недостижение за лимит

<a id="03-11"></a>
## 03-11. Тесты границы повторного вычитания

Цикл должен продолжаться и при равенстве остатка делителю: условие &gt;=.

**Ход решения**

Каждое вычитание соответствует ещё одной полной группе размера divisor. Если остаток равен divisor, одна группа ещё есть, поэтому &gt; завершает цикл на шаг раньше. Неточное деление вроде 14/3 эту ошибку не показывает: последний остаток 2, и оба варианта успеют сделать четыре шага. Поэтому ключевые случаи — dividend=divisor и точное кратное. Ноль проверяет пустой цикл, меньше делителя — невозможность первой группы, делитель 1 — максимальное число шагов. Run подтверждает ошибочный результат Wrong(12,3)=3, а правильный метод возвращает 4. Время O(dividend/divisor), память O(1).

**Частая ошибка**

Проверить только деление с остатком и объявить границы покрытыми.

Полный исходник: [solutions/Task11.cs](solutions/Task11.cs).

```csharp
using System;
using Course;

namespace Course.Lesson03;

public static class Task11
{
    public static int Quotient(int dividend, int divisor)
    {
        int count = 0;
        while (dividend >= divisor)
        {
            dividend -= divisor;
            count++;
        }
        return count;
    }
    public static int Wrong(int dividend, int divisor)
    {
        int count = 0;
        while (dividend > divisor)
        {
            dividend -= divisor;
            count++;
        }
        return count;
    }

    public static void Run()
    {
        Check.Equal(0, Quotient(0, 3));
        Check.Equal(0, Quotient(2, 3));
        Check.Equal(1, Quotient(3, 3));
        Check.Equal(4, Quotient(12, 3));
        Check.Equal(4, Quotient(14, 3));
        Check.Equal(10_000, Quotient(10_000, 1));
        Check.Equal(3, Wrong(12, 3));
        Check.True(Wrong(12, 3) != Quotient(12, 3));
    }
}
```

**Исполняемые проверки**

- Нулевое делимое
- Меньше делителя
- Равенство
- Точное кратное
- Деление с остатком
- Делитель 1
- Wrong реально различён

<a id="03-12"></a>
## 03-12. Неупорядоченные пары с заданной суммой

Для каждого a начинать b с a+1 и проверять сумму.

**Ход решения**

Ограничение a&lt;b одновременно устраняет равные элементы и симметричные повторы. Внутренний цикл начинает b=a+1, а не 1: тогда каждая допустимая пара посещается ровно один раз. Для n=0 и n=1 пар нет естественным образом. На (5,6) ошибочный полный квадрат перебора дал бы лишние зеркальные пары и (3,3), поэтому пример хорошо различает структуру. Дополнительно проверяем невозможную сумму и крайние пары. Время O(n²), дополнительная память O(1). Математическая формула допустима в обсуждении, но учебная реализация должна продемонстрировать вложенный цикл.

**Частая ошибка**

Начать b с a или с 1 и получить равные элементы либо двойной счёт.

Полный исходник: [solutions/Task12.cs](solutions/Task12.cs).

```csharp
using System;
using Course;

namespace Course.Lesson03;

public static class Task12
{
    public static int PairCount(int n, int target)
    {
        int count = 0;
        for (int a = 1; a <= n; a++)
            for (int b = a + 1; b <= n; b++)
                if (a + b == target) count++;
        return count;
    }

    public static void Run()
    {
        Check.Equal(0, PairCount(0, 0));
        Check.Equal(0, PairCount(1, 2));
        Check.Equal(2, PairCount(5, 6));
        Check.Equal(1, PairCount(5, 9));
        Check.Equal(0, PairCount(5, 20));
        Check.Equal(50, PairCount(100, 101));
    }
}
```

**Исполняемые проверки**

- Пустой и одноэлементный диапазоны
- Сумма 6 на 1..5
- Единственная крайняя пара
- Невозможная сумма
- Верхний диапазон

<a id="03-13"></a>
## 03-13. Максимум потока без ложного нуля

Использовать bool found и присвоить максимум первому числу, затем сравнивать следующие.

**Ход решения**

Ноль не является универсальным начальным максимумом: поток может содержать только отрицательные числа. Флаг found сообщает, имеет ли числовое значение смысл. Первое корректное число назначается максимумом независимо от знака; остальные заменяют его только при большем значении. Неверная строка не изменяет ни флаг, ни максимум. После EOF флаг отделяет настоящий максимум 0 от отсутствия данных. Проверки включают все отрицательные, int.MinValue как единственное число, пустой поток и смешанный ввод. Время линейно по входу, память O(1).

**Частая ошибка**

Начать maximum=0 и не отслеживать отсутствие чисел.

Полный исходник: [solutions/Task13.cs](solutions/Task13.cs).

```csharp
using System;
using Course;
using System.IO;

namespace Course.Lesson03;

public static class Task13
{
    public static string Maximum(TextReader reader)
    {
        bool found = false;
        int maximum = 0;
        while (true)
        {
            string? line = reader.ReadLine();
            if (line is null) break;
            if (!int.TryParse(line, out int value)) continue;
            if (!found || value > maximum) maximum = value;
            found = true;
        }
        return found ? maximum.ToString() : "empty";
    }

    public static void Run()
    {
        using var negatives = new StringReader("bad\n-8\n-3\n");
        Check.Equal("-3", Maximum(negatives));
        using var empty = new StringReader("");
        Check.Equal("empty", Maximum(empty));
        using var invalid = new StringReader("bad\n\n");
        Check.Equal("empty", Maximum(invalid));
        using var minimum = new StringReader("-2147483648");
        Check.Equal("-2147483648", Maximum(minimum));
        using var mixed = new StringReader("1\n10\nx\n0\n");
        Check.Equal("10", Maximum(mixed));

        // Review 2026-10-19: maximum preserves reader ownership
        Check.Equal<string?>(null, minimum.ReadLine());

        // Review 2026-10-19: zero maximum is present data
        using var zeroMaximum = new StringReader("bad\n0\n-2\n");
        Check.Equal("0", Maximum(zeroMaximum));
    }
}
```

**Исполняемые проверки**

- Все числа отрицательные
- Пустой поток
- Только неверные строки
- Единственное int.MinValue
- Максимум в середине смешанного потока
- Maximum не закрывает переданный reader: после обработки можно снова прочитать EOF
- Настоящий нулевой максимум отличается от отсутствия чисел

<a id="03-14"></a>
## 03-14. Целый корень двоичным поиском

Начать left=0, right=(long)n+1 и сужать интервал по квадрату середины.

**Ход решения**

Левая граница является допустимым ответом, правая — заведомо слишком большой. Для n=0 это 0 и 1; иначе квадрат n+1 тоже больше n. Середину вычисляем как left+(right-left)/2. Если её квадрат допустим, сдвигаем left; иначе right. Пока расстояние больше единицы, середина строго внутри, поэтому интервал сокращается. В конце между границами нет другого целого, и left максимален. Приведение n к long нужно до +1; максимальный квадрат промежуточной середины укладывается в long. Проверки около квадратов и верхнего int выявляют ошибки границ. O(log n) времени, O(1) памяти.

**Частая ошибка**

Вычислить n+1 в int или остановиться на расстоянии 0 и зациклиться при соседних границах.

Полный исходник: [solutions/Task14.cs](solutions/Task14.cs).

```csharp
using System;
using Course;

namespace Course.Lesson03;

public static class Task14
{
    public static int FloorSqrt(int n)
    {
        long left = 0;
        long right = (long)n + 1;
        while (right - left > 1)
        {
            long middle = left + (right - left) / 2;
            if (middle * middle <= n) left = middle;
            else right = middle;
        }
        return (int)left;
    }

    public static void Run()
    {
        Check.Equal(0, FloorSqrt(0));
        Check.Equal(1, FloorSqrt(1));
        Check.Equal(3, FloorSqrt(15));
        Check.Equal(4, FloorSqrt(16));
        Check.Equal(4, FloorSqrt(17));
        Check.Equal(46_340, FloorSqrt(2_147_395_600));
        Check.Equal(46_340, FloorSqrt(int.MaxValue));
    }
}
```

**Исполняемые проверки**

- 0 и 1
- Числа до, на и после квадрата
- Максимальный квадрат int
- int.MaxValue

<a id="03-15"></a>
## 03-15. Ограниченные попытки и ранний выход

Цикл for нумерует попытки, внутри две проверки раннего возврата, после цикла locked.

**Ход решения**

Номер попытки увеличивается только после реально прочитанной неверной строки. Null не является неверной попыткой: это конец источника, поэтому возвращаем eof сразу. Равенство строк сохраняет ведущий ноль; int.TryParse здесь разрушил бы формат и принял 420. После return цикл полностью прекращается, а после исчерпания limit возвращается locked без дополнительного чтения. В Run проверяем хвост через следующее ReadLine: это отличает правильный ранний выход от реализации, которая прочитала лишнюю строку. Пустая строка специально считается попыткой. Время O(limit) чтений, память O(1).

**Частая ошибка**

Читать limit+1 строку для проверки завершения либо парсить код как число.

Полный исходник: [solutions/Task15.cs](solutions/Task15.cs).

```csharp
using System;
using Course;
using System.IO;

namespace Course.Lesson03;

public static class Task15
{
    public static string TryCode(TextReader reader, int limit)
    {
        for (int attempt = 1; attempt <= limit; attempt++)
        {
            string? line = reader.ReadLine();
            if (line is null) return "eof";
            if (line == "0420") return $"ok:{attempt}";
        }
        return "locked";
    }

    public static void Run()
    {
        using var first = new StringReader("0420\n");
        Check.Equal("ok:1", TryCode(first, 3));
        using var second = new StringReader("bad\n0420\nnext\n");
        Check.Equal("ok:2", TryCode(second, 3));
        Check.Equal("next", second.ReadLine());
        using var locked = new StringReader("1\n2\n3\n0420\n");
        Check.Equal("locked", TryCode(locked, 3));
        Check.Equal("0420", locked.ReadLine());
        using var eof = new StringReader("bad\n");
        Check.Equal("eof", TryCode(eof, 3));
        using var emptyLine = new StringReader("\n0420\n");
        Check.Equal("locked", TryCode(emptyLine, 1));
        using var numeric = new StringReader("420\n");
        Check.Equal("locked", TryCode(numeric, 1));

        // Review 2026-10-19: exact attempt boundary and immediate EOF
        using var noLines = new StringReader("");
        Check.Equal("eof", TryCode(noLines, 3));
        using var lastAttempt = new StringReader("bad\nalso-bad\n0420\nuntouched\n");
        Check.Equal("ok:3", TryCode(lastAttempt, 3));
        Check.Equal("untouched", lastAttempt.ReadLine());
    }
}
```

**Исполняемые проверки**

- Успех на первой попытке
- Успех на второй и хвост сохранён
- Лимит и хвост сохранён
- EOF
- Пустая строка расходует попытку
- 420 не равно 0420
- EOF до первой попытки
- Успех ровно на последней разрешённой попытке и сохранение хвоста
