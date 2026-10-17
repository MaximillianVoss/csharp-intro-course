# Урок 2. Ввод, условия и выбор действия: решения

Сначала выполните свою попытку по TASKS.md. Здесь приведены ответы.
Каждое решение запускается по ID; Run содержит исполняемые проверки.

<a id="02-01"></a>
## 02-01. Четыре исхода чтения количества

Проверять причины отказа в заданном порядке и возвращать отдельный статус.

**Ход решения**

EOF — это отсутствие строки, поэтому проверяем null до попытки разбора: иначе он слился бы с неверным форматом. TryParse сообщает, можно ли представить текст как int, и только при успехе значение имеет нужный смысл. Затем диапазон 0..100 отделяет допустимое количество от просто корректного целого. Пустая строка и текст с дробной точкой дают invalid, очень длинная последовательность цифр тоже invalid, поскольку не помещается в int. Переменная value предоставляется готовым вызовом с out: устройство собственных out-методов изучается позже. Время линейно по длине текста.

**Частая ошибка**

Заменить null пустой строкой или проверять диапазон после неудачного TryParse.

Полный исходник: [solutions/Task01.cs](solutions/Task01.cs).

```csharp
using System;
using Course;

namespace Course.Lesson02;

public static class Task01
{
    public static string ReadCount(string? text)
    {
        if (text is null) return "eof";
        if (!int.TryParse(text, out int value)) return "invalid";
        if (value < 0 || value > 100) return "range";
        return $"ok:{value}";
    }

    public static void Run()
    {
        Check.Equal("eof", ReadCount(null));
        Check.Equal("invalid", ReadCount(""));
        Check.Equal("ok:7", ReadCount(" 007 "));
        Check.Equal("ok:0", ReadCount("0"));
        Check.Equal("ok:100", ReadCount("100"));
        Check.Equal("range", ReadCount("-1"));
        Check.Equal("range", ReadCount("101"));
        Check.Equal("invalid", ReadCount("999999999999999999"));
        Check.Equal("invalid", ReadCount("1.5"));
    }
}
```

**Исполняемые проверки**

- EOF
- Пустая строка
- Пробелы и ведущие нули
- 0 и 100
- -1 и 101
- Слишком большое для int число
- Дробный текст

<a id="02-02"></a>
## 02-02. Три знака целого числа

Проверить value < 0, затем value > 0, затем вернуть zero.

**Ход решения**

Все целые разбиваются на три непересекающиеся группы. Если первая проверка истинна, метод может сразу вернуть negative; последующие проверки уже не нужны. Если отрицательность исключена, проверка >0 отделяет положительные. После двух отказов остаётся ровно ноль. Дополнительное сравнение ==0 допустимо, но не обязательно. Два отдельных if с общим else легко привязать неверно, поэтому здесь полезна явная структура возвратов или единая цепочка. Проверки включают int.MinValue: вычислять модуль числа для определения знака не требуется.

**Частая ошибка**

Считать ноль положительным из-за условия >=0 или брать Math.Abs(int.MinValue).

Полный исходник: [solutions/Task02.cs](solutions/Task02.cs).

```csharp
using System;
using Course;

namespace Course.Lesson02;

public static class Task02
{
    public static string Sign(int value)
    {
        if (value < 0) return "negative";
        if (value > 0) return "positive";
        return "zero";
    }

    public static void Run()
    {
        Check.Equal("negative", Sign(-1));
        Check.Equal("zero", Sign(0));
        Check.Equal("positive", Sign(1));
        Check.Equal("negative", Sign(int.MinValue));
        Check.Equal("positive", Sign(int.MaxValue));
    }
}
```

**Исполняемые проверки**

- -1, 0, 1
- int.MinValue
- int.MaxValue

<a id="02-03"></a>
## 02-03. Полуоткрытое окно приёма

Вернуть minute >= opening && minute < closing.

**Ход решения**

Полуоткрытый интервал позволяет соседним окнам соприкасаться без двойного приёма в одну минуту. Для принадлежности нужны одновременно две проверки, поэтому используем &&. Символ < справа принципиален: замена на <= меняет договорённость о закрытии. Если opening=closing, ни одна минута не может одновременно быть не меньше и строго меньше одной границы, поэтому специальная ветка не требуется. Проверяем минуту до открытия, само открытие, последнюю допустимую минуту и закрытие. Сложность O(1).

**Частая ошибка**

Соединить сравнения через || или включить правую границу.

Полный исходник: [solutions/Task03.cs](solutions/Task03.cs).

```csharp
using System;
using Course;

namespace Course.Lesson02;

public static class Task03
{
    public static bool InWindow(int minute, int opening, int closing)
    {
        return minute >= opening && minute < closing;
    }

    public static void Run()
    {
        Check.Equal(false, InWindow(539, 540, 1020));
        Check.True(InWindow(540, 540, 1020));
        Check.True(InWindow(1019, 540, 1020));
        Check.Equal(false, InWindow(1020, 540, 1020));
        Check.Equal(false, InWindow(600, 600, 600));
    }
}
```

**Исполняемые проверки**

- До открытия
- Открытие
- Внутри
- Закрытие
- Пустое окно

<a id="02-04"></a>
## 02-04. Правило доступа с запретом

Вернуть active && (owner || hasPass) && !blocked.

**Ход решения**

В правиле есть три обязательных требования: активность, наличие хотя бы одного основания доступа и отсутствие блокировки. Внутри второго требования используется ИЛИ, а между требованиями — И. Скобки сохраняют эту структуру и делают смысл заметным даже без знания приоритетов. Если написать active && owner || hasPass, пропуск сможет обойти проверку активности. Блокировка вынесена как общее обязательное условие, поэтому владелец не становится исключением. Проверки специально включают неактивного обладателя пропуска и заблокированного владельца: обычный успешный сценарий эти ошибки не выявляет.

**Частая ошибка**

Дать пропуску или владельцу обойти общий запрет из-за отсутствующих скобок.

Полный исходник: [solutions/Task04.cs](solutions/Task04.cs).

```csharp
using System;
using Course;

namespace Course.Lesson02;

public static class Task04
{
    public static bool MayOpen(bool active, bool owner, bool hasPass, bool blocked)
    {
        return active && (owner || hasPass) && !blocked;
    }

    public static void Run()
    {
        Check.True(MayOpen(true, true, false, false));
        Check.True(MayOpen(true, false, true, false));
        Check.Equal(false, MayOpen(true, false, false, false));
        Check.Equal(false, MayOpen(false, false, true, false));
        Check.Equal(false, MayOpen(true, true, true, true));
    }
}
```

**Исполняемые проверки**

- Доступ владельцу
- Доступ по пропуску
- Нет оснований
- Неактивен с пропуском
- Заблокированный владелец

<a id="02-05"></a>
## 02-05. Имя направления через switch

Каждый case возвращает свою строку, default возвращает invalid.

**Ход решения**

Switch здесь выбирает по одному целому коду, а не по произвольному диапазону. В каждом case можно сразу сделать return, тогда переход к следующему случаю не нужен. Ветка default завершает определение поведения для всех остальных int. Нельзя применять %4: это превратило бы неизвестный код в допустимое направление, хотя условие требует invalid. Проверяем все четыре кода, а затем значения с обеих сторон диапазона. Эта задача тренирует полноту конечного выбора; она не требует коллекции или массива соответствий. Сложность O(1).

**Частая ошибка**

Забыть default или добавить циклическое поведение, которого нет в условии.

Полный исходник: [solutions/Task05.cs](solutions/Task05.cs).

```csharp
using System;
using Course;

namespace Course.Lesson02;

public static class Task05
{
    public static string Direction(int code)
    {
        switch (code)
        {
            case 0: return "north";
            case 1: return "east";
            case 2: return "south";
            case 3: return "west";
            default: return "invalid";
        }
    }

    public static void Run()
    {
        Check.Equal("north", Direction(0));
        Check.Equal("east", Direction(1));
        Check.Equal("south", Direction(2));
        Check.Equal("west", Direction(3));
        Check.Equal("invalid", Direction(-1));
        Check.Equal("invalid", Direction(4));
        Check.Equal("invalid", Direction(int.MaxValue));
    }
}
```

**Исполняемые проверки**

- Все четыре направления
- Код -1
- Код 4
- int.MaxValue

<a id="02-06"></a>
## 02-06. К какому if относится else

Else принадлежит ближайшему незавершённому if(permitted); при ready=false result не изменяется.

**Ход решения**

Сначала выполняется внешний if(ready). Если ready=false, всё вложенное предложение пропускается вместе с его else, поэтому остаётся unchanged. Если ready=true, внутренний if выбирает go или denied в зависимости от permitted. Отступы помогают человеку, но связь else определяет грамматика: ближайший if без else. Рабочий аналог добавляет скобки вокруг внешней и внутренней ветвей, сохраняя тот же выбор. Все четыре сочетания проверяются, включая ready=false, permitted=true: этот случай хорошо показывает, что разрешение само по себе не запускает внешний блок.

**Частая ошибка**

Привязать else к внешнему if только по визуальному выравниванию текста.

Полный исходник: [solutions/Task06.cs](solutions/Task06.cs).

```csharp
using System;
using Course;

namespace Course.Lesson02;

public static class Task06
{
    public static string Inspect(bool ready, bool permitted)
    {
        string result = "unchanged";
        if (ready)
        {
            if (permitted)
            {
                result = "go";
            }
            else
            {
                result = "denied";
            }
        }
        return result;
    }

    public static void Run()
    {
        Check.Equal("go", Inspect(true, true));
        Check.Equal("denied", Inspect(true, false));
        Check.Equal("unchanged", Inspect(false, true));
        Check.Equal("unchanged", Inspect(false, false));
    }
}
```

**Исполняемые проверки**

- Все четыре комбинации ready/permitted

<a id="02-07"></a>
## 02-07. Безопасный порядок короткого замыкания

Переставить проверку divisor != 0 в левый операнд &&.

**Ход решения**

Операнды && вычисляются слева направо, и правая часть пропускается только после false слева. В исходнике первым уже выполняется остаток от деления, поэтому нулевой делитель приводит к исключению раньше проверки. Правильный порядок использует false от divisor!=0 как основание не выполнять потенциально опасную операцию. Проверяем ноль-делимое при ненулевом делителе: 0 делится без остатка, и запрещать его не нужно. Отрицательный делитель также допустим. Run исполняет ошибочный вариант на нуле внутри Check.Throws и затем проверяет исправленный результат.

**Частая ошибка**

Заменить && на &, который вычисляет обе части, или запретить нулевое делимое.

Полный исходник: [solutions/Task07.cs](solutions/Task07.cs).

```csharp
using System;
using Course;

namespace Course.Lesson02;

public static class Task07
{
    public static bool Broken(int value, int divisor)
    {
        return value % divisor == 0 && divisor != 0;
    }
    public static bool Divides(int value, int divisor)
    {
        return divisor != 0 && value % divisor == 0;
    }

    public static void Run()
    {
        Check.Throws<DivideByZeroException>(() => Broken(12, 0));
        Check.True(Divides(12, 3));
        Check.Equal(false, Divides(12, 5));
        Check.Equal(false, Divides(12, 0));
        Check.True(Divides(0, 7));
        Check.True(Divides(12, -3));
    }
}
```

**Исполняемые проверки**

- Исходный вариант на нулевом делителе
- Деление с остатком и без
- Нулевой делитель
- Нулевое делимое
- Отрицательный делитель

<a id="02-08"></a>
## 02-08. Плоские проверки перед отправкой

Возвращать paused, address, payment по трём ранним проверкам; в конце send.

**Ход решения**

Первое препятствие в исходном дереве — paused, оно имеет приоритет над всеми остальными. Если остановки нет, исходник сначала требует адрес и только затем оплату. Значит плоская версия должна повторить этот порядок, иначе одновременно неверный адрес и отсутствие оплаты получат другой статус. После раннего return соответствующая проблемная ветка уже завершена, поэтому else не нужен. Оставшийся путь означает отсутствие всех препятствий. Проверяем восемь комбинаций bool, чтобы подтвердить эквивалентность дерева и плоской последовательности полностью, а не только для успешной отправки.

**Частая ошибка**

Переставить проверки оплаты и адреса и тем самым изменить приоритет ошибки.

Полный исходник: [solutions/Task08.cs](solutions/Task08.cs).

```csharp
using System;
using Course;

namespace Course.Lesson02;

public static class Task08
{
    public static string Status(bool paused, bool addressValid, bool paid)
    {
        if (paused) return "paused";
        if (!addressValid) return "address";
        if (!paid) return "payment";
        return "send";
    }

    public static void Run()
    {
        Check.Equal("paused", Status(true, false, false));
        Check.Equal("paused", Status(true, false, true));
        Check.Equal("paused", Status(true, true, false));
        Check.Equal("paused", Status(true, true, true));
        Check.Equal("address", Status(false, false, false));
        Check.Equal("address", Status(false, false, true));
        Check.Equal("payment", Status(false, true, false));
        Check.Equal("send", Status(false, true, true));
    }
}
```

**Исполняемые проверки**

- Все восемь сочетаний трёх флагов

<a id="02-09"></a>
## 02-09. Правило високосного года

Вернуть year % 400 == 0 || (year % 4 == 0 && year % 100 != 0).

**Ход решения**

Есть две достаточные причины високосности. Первая — кратность 400, она сразу решает особый случай. Вторая — обычная кратность 4 при отсутствии столетнего исключения. Связываем две причины через ||, а две части обычной причины через &&. Годы 2023 и 2024 проверяют базовую кратность 4. Год 1900 показывает необходимость исключения по 100, а 2000 — необходимость восстановления по 400. Одна проверка текущего високосного года не подтверждает всё правило. Диапазон положительных годов позволяет не обсуждать нумерацию до нашей эры. O(1).

**Частая ошибка**

Использовать только %4 или отменить все столетние годы, включая 2000.

Полный исходник: [solutions/Task09.cs](solutions/Task09.cs).

```csharp
using System;
using Course;

namespace Course.Lesson02;

public static class Task09
{
    public static bool IsLeap(int year)
    {
        return year % 400 == 0 || (year % 4 == 0 && year % 100 != 0);
    }

    public static void Run()
    {
        Check.Equal(false, IsLeap(2023));
        Check.True(IsLeap(2024));
        Check.Equal(false, IsLeap(1900));
        Check.True(IsLeap(2000));
        Check.Equal(false, IsLeap(1));
        Check.Equal(false, IsLeap(9999));
    }
}
```

**Исполняемые проверки**

- 2023 и 2024
- 1900 и 2000
- Год 1
- Год 9999

<a id="02-10"></a>
## 02-10. Ступенчатый тариф без повторной оплаты

Выбрать ступень и добавить уже оплаченную стоимость предыдущих ступеней.

**Ход решения**

До 10 единиц достаточно units*2. На второй ступени первые десять уже стоят 20, поэтому добавляется (units-10)*3. После двадцати стоимость первых двух ступеней фиксирована: 20+30=50, остаток стоит (units-20)*5. Ошибочный вариант units*ставка применял бы новую цену ко всему объёму. Проверки 10,11,20,21 расположены по обе стороны каждого перехода и выявляют как неверный порог, так и забытое накопление. Верхняя граница даёт 4 999 950, что помещается в int. O(1).

**Частая ошибка**

На второй ступени умножить весь объём на 3 или дважды оплатить первые 10 единиц.

Полный исходник: [solutions/Task10.cs](solutions/Task10.cs).

```csharp
using System;
using Course;

namespace Course.Lesson02;

public static class Task10
{
    public static int Charge(int units)
    {
        if (units <= 10) return units * 2;
        if (units <= 20) return 20 + (units - 10) * 3;
        return 50 + (units - 20) * 5;
    }

    public static void Run()
    {
        Check.Equal(0, Charge(0));
        Check.Equal(20, Charge(10));
        Check.Equal(23, Charge(11));
        Check.Equal(26, Charge(12));
        Check.Equal(50, Charge(20));
        Check.Equal(55, Charge(21));
        Check.Equal(4_999_950, Charge(1_000_000));
    }
}
```

**Исполняемые проверки**

- 0
- 10 и 11
- 12
- 20 и 21
- Верхняя граница units

<a id="02-11"></a>
## 02-11. Испытания существования треугольника

Использовать > для каждой суммы и проверить все три стороны, а не только c.

**Ход решения**

У треугольника каждая сторона меньше суммы двух других. Равенство даёт отрезок, поэтому >= неверно. Проверка только a+b>c не замечает ситуацию, где самой длинной стороной оказалась a или b. Набор тестов должен разнести эти причины: (1,2,3) выявляет нестрогое сравнение, (10,1,2) — пропущенную проверку длинной a. Перестановки (1,10,2) и (1,2,10) подтверждают симметрию правильного правила. Нулевые и отрицательные стороны отдельно проверяют входную политику. Суммы ограничены двумя миллионами и безопасны для int. Run реально показывает положительные результаты Wrong там, где правильный ответ false.

**Частая ошибка**

Ограничиться примером 3,4,5, который проходит и у ошибочной реализации.

Полный исходник: [solutions/Task11.cs](solutions/Task11.cs).

```csharp
using System;
using Course;

namespace Course.Lesson02;

public static class Task11
{
    public static bool IsTriangle(int a, int b, int c)
    {
        return a > 0 && b > 0 && c > 0 && a + b > c && a + c > b && b + c > a;
    }
    public static bool Wrong(int a, int b, int c)
    {
        return a > 0 && b > 0 && c > 0 && a + b >= c;
    }

    public static void Run()
    {
        Check.True(IsTriangle(3, 4, 5));
        Check.True(IsTriangle(2, 2, 2));
        Check.Equal(false, IsTriangle(1, 2, 3));
        Check.Equal(false, IsTriangle(10, 1, 2));
        Check.Equal(false, IsTriangle(1, 10, 2));
        Check.Equal(false, IsTriangle(1, 2, 10));
        Check.Equal(false, IsTriangle(0, 2, 2));
        Check.Equal(false, IsTriangle(-1, 2, 2));
        Check.True(Wrong(1, 2, 3));
        Check.True(Wrong(10, 1, 2));
    }
}
```

**Исполняемые проверки**

- Обычный и равносторонний треугольники
- Вырожденный случай
- Три расположения слишком длинной стороны
- Нулевая и отрицательная стороны
- Две ошибки Wrong реально обнаружены

<a id="02-12"></a>
## 02-12. Точка, ось или четверть

Проверить обе нулевые координаты, оси, затем комбинации положительности.

**Ход решения**

Начало принадлежало бы обеим осям, поэтому получает собственную первую ветку. После исключения нуля y можно определить x-axis; после исключения нуля x — y-axis. На оставшемся пути обе координаты ненулевые: знак x выбирает левую или правую полуплоскость, знак y — верхнюю или нижнюю. Не нужно умножать координаты ради определения общего знака: это теряет информацию о конкретной четверти и создаёт риск переполнения. Восемь проверок охватывают все семь категорий и экстремальные значения int. Классификация выполняется за O(1).

**Частая ошибка**

Определить ось раньше начала или искать четверть только по знаку произведения.

Полный исходник: [solutions/Task12.cs](solutions/Task12.cs).

```csharp
using System;
using Course;

namespace Course.Lesson02;

public static class Task12
{
    public static string Region(int x, int y)
    {
        if (x == 0 && y == 0) return "origin";
        if (y == 0) return "x-axis";
        if (x == 0) return "y-axis";
        if (x > 0) return y > 0 ? "I" : "IV";
        return y > 0 ? "II" : "III";
    }

    public static void Run()
    {
        Check.Equal("origin", Region(0, 0));
        Check.Equal("x-axis", Region(-3, 0));
        Check.Equal("y-axis", Region(0, -7));
        Check.Equal("I", Region(2, 3));
        Check.Equal("II", Region(-2, 3));
        Check.Equal("III", Region(-2, -3));
        Check.Equal("IV", Region(2, -3));
        Check.Equal("II", Region(int.MinValue, int.MaxValue));
    }
}
```

**Исполняемые проверки**

- Начало
- Обе оси
- Все четыре четверти
- Крайние значения int без произведения

<a id="02-13"></a>
## 02-13. Сравнение рациональных чисел без потери точности

До умножения расширить числители до long; сравнить перекрёстные произведения ветвлениями.

**Ход решения**

Положительные знаменатели позволяют домножить обе стороны сравнения на b*d без изменения направления. Получается сравнение a*d и c*b, выполняемое точно в целых. Произведения могут достигать по модулю 10^12, поэтому int недостаточно, но long подходит. Приведение должно стоять до умножения. Равные дроби с разными записями проверяют математическое равенство, а близкие большие дроби выявляют попытку целочисленного деления или переполнение. Отрицательные числители подтверждают правильность направления сравнения. Число операций фиксировано, O(1).

**Частая ошибка**

Сравнить a/b и c/d как int или сохранить произведения в long после int-умножения.

Полный исходник: [solutions/Task13.cs](solutions/Task13.cs).

```csharp
using System;
using Course;

namespace Course.Lesson02;

public static class Task13
{
    public static int CompareFractions(int a, int b, int c, int d)
    {
        long left = (long)a * d;
        long right = (long)c * b;
        if (left < right) return -1;
        if (left > right) return 1;
        return 0;
    }

    public static void Run()
    {
        Check.Equal(0, CompareFractions(1, 3, 2, 6));
        Check.Equal(-1, CompareFractions(1, 2, 2, 3));
        Check.Equal(1, CompareFractions(2, 3, 1, 2));
        Check.Equal(-1, CompareFractions(-3, 4, -1, 2));
        Check.Equal(1, CompareFractions(999_999, 1_000_000, 999_998, 999_999));
    }
}
```

**Исполняемые проверки**

- Равные дроби
- Меньше и больше
- Отрицательные числители
- Большие близкие дроби

<a id="02-14"></a>
## 02-14. Переход состояния по одной команде

Использовать switch по строке и условный выбор ответа для каждого известного действия.

**Ход решения**

Одинаковая команда может означать реальный переход или повтор уже выполненного действия. Поэтому одного соответствия текст→ответ недостаточно: start и stop зависят от running. Команда status ничего не меняет и отражает текущее значение. Null отделяем до switch, чтобы EOF нельзя было спутать с неизвестной командой. По условию пробелы и регистр значимы: Trim или ToLower здесь самовольно расширили бы допустимый язык. Проверяем обе исходные ситуации для трёх известных команд и несколько отказов. Возвращаемая строка — протокол каркаса, не сериализованное состояние приложения.

**Частая ошибка**

Незаметно нормализовать команды или считать повтор start новым успешным запуском.

Полный исходник: [solutions/Task14.cs](solutions/Task14.cs).

```csharp
using System;
using Course;

namespace Course.Lesson02;

public static class Task14
{
    public static string Command(bool running, string? command)
    {
        if (command is null) return "eof";
        switch (command)
        {
            case "start": return running ? "already-running" : "started";
            case "stop": return running ? "stopped" : "already-stopped";
            case "status": return running ? "running" : "stopped";
            default: return "invalid";
        }
    }

    public static void Run()
    {
        Check.Equal("started", Command(false, "start"));
        Check.Equal("already-running", Command(true, "start"));
        Check.Equal("stopped", Command(true, "stop"));
        Check.Equal("already-stopped", Command(false, "stop"));
        Check.Equal("running", Command(true, "status"));
        Check.Equal("stopped", Command(false, "status"));
        Check.Equal("eof", Command(false, null));
        Check.Equal("invalid", Command(false, ""));
        Check.Equal("invalid", Command(false, "Start"));
        Check.Equal("invalid", Command(false, " start "));
    }
}
```

**Исполняемые проверки**

- Start/stop/status из обоих состояний
- EOF
- Пустая команда
- Другой регистр
- Окружающие пробелы

<a id="02-15"></a>
## 02-15. Один расчёт из трёх строк

Последовательно выполнить проверки по указанному приоритету и выбрать операцию switch.

**Ход решения**

Порядок проверок является частью протокола: EOF имеет приоритет, затем формат обоих чисел, затем их диапазон. InvariantCulture и ограниченные флаги делают точку единственным десятичным разделителем и не допускают группировку тысяч: 1,5 не превращается в 15. Произведение ограничено 10^12, но деление на очень малое ненулевое decimal может дать результат вне диапазона, хотя оба входа допустимы. Делитель проверяется перед /; OverflowException арифметики преобразуется готовой оболочкой в статус overflow. Результат возвращается через InvariantCulture. Два теста деления на 10^-28 различают допустимый результат 10^28 и переполнение 10^34; проверка только больших целых делителей этой границы не охватывает.

**Частая ошибка**

Использовать NumberStyles.Number и принять запятую как разделитель групп; выполнить деление до проверки нуля.

Полный исходник: [solutions/Task15.cs](solutions/Task15.cs).

```csharp
using System;
using Course;
using System.Globalization;

namespace Course.Lesson02;

public static class Task15
{
    public static string Calculate(string? leftText, string? operation, string? rightText)
    {
        if (leftText is null || operation is null || rightText is null) return "eof";
        const NumberStyles style = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;
        if (!decimal.TryParse(leftText, style, CultureInfo.InvariantCulture, out decimal left) ||
            !decimal.TryParse(rightText, style, CultureInfo.InvariantCulture, out decimal right))
            return "invalid";
        if (left < -1_000_000m || left > 1_000_000m || right < -1_000_000m || right > 1_000_000m)
            return "range";
        try
        {
            decimal result;
            switch (operation)
            {
                case "+": result = left + right; break;
                case "-": result = left - right; break;
                case "*": result = left * right; break;
                case "/":
                    if (right == 0m) return "zero";
                    result = left / right;
                    break;
                default: return "operation";
            }
            return result.ToString(CultureInfo.InvariantCulture);
        }
        catch (OverflowException)
        {
            return "overflow";
        }
    }

    public static void Run()
    {
        Check.Equal("3.5", Calculate("1.5", "+", "2"));
        Check.Equal("-3", Calculate("2", "-", "5"));
        Check.Equal("-12", Calculate("-3", "*", "4"));
        Check.Equal("2.5", Calculate("5", "/", "2"));
        Check.Equal("zero", Calculate("4", "/", "0"));
        Check.Equal("eof", Calculate(null, "?", "bad"));
        Check.Equal("invalid", Calculate("1,5", "+", "2"));
        Check.Equal("invalid", Calculate(" 1", "+", "2"));
        Check.Equal("range", Calculate("1000001", "+", "2"));
        Check.Equal("operation", Calculate("1", "?", "2"));
        Check.Equal("invalid", Calculate("bad", "+", "1000001"));
        Check.Equal("10000000000000000000000000000", Calculate("1", "/", "0.0000000000000000000000000001"));
        Check.Equal("overflow", Calculate("1000000", "/", "0.0000000000000000000000000001"));
    }
}
```

**Исполняемые проверки**

- Сложение, вычитание, умножение, деление
- Деление на ноль
- EOF с неверной операцией
- Запятая и пробелы запрещены
- Ошибка диапазона
- Неизвестная операция
- Неверный формат при втором поле вне диапазона
- Очень малый делитель: успех и overflow
