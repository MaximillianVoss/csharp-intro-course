# Урок 4. Методы, декомпозиция и отладка: решения

Сначала выполните свою попытку по TASKS.md. Здесь приведены ответы.
Каждое решение запускается по ID; Run содержит исполняемые проверки.

<a id="04-01"></a>
## 04-01. Ограничение числа диапазоном

Два ранних возврата для выхода слева и справа, затем возврат value.

**Ход решения**

Сигнатура сообщает, что все данные приходят через три параметра, а наружу выходит одно int. Метод чистый: одинаковые аргументы дают одинаковый результат без изменений окружения. Число слева от диапазона ближе всего к minimum, справа — к maximum. Внутри менять его не требуется. Разность с границей вычислять не надо, поэтому крайние int безопасны. Проверки включают границы, одноточечный диапазон и int.MinValue/int.MaxValue. Число сравнений ограничено, O(1).

**Частая ошибка**

Использовать строгие границы как причину сдвинуть уже допустимое значение или перепутать минимум и максимум.

Полный исходник: [solutions/Task01.cs](solutions/Task01.cs).

```csharp
using System;
using Course;

namespace Course.Lesson04;

public static class Task01
{
    public static int Clamp(int value, int minimum, int maximum)
    {
        if (value < minimum) return minimum;
        if (value > maximum) return maximum;
        return value;
    }

    public static void Run()
    {
        Check.Equal(0, Clamp(-2, 0, 10));
        Check.Equal(4, Clamp(4, 0, 10));
        Check.Equal(10, Clamp(12, 0, 10));
        Check.Equal(0, Clamp(0, 0, 10));
        Check.Equal(10, Clamp(10, 0, 10));
        Check.Equal(7, Clamp(100, 7, 7));
        Check.Equal(int.MinValue, Clamp(int.MinValue, int.MinValue, int.MaxValue));
    }
}
```

**Исполняемые проверки**

- Левее, внутри и правее
- Обе границы
- Одноточечный диапазон
- Крайние int

<a id="04-02"></a>
## 04-02. Два исхода целого деления через out

Назначить quotient=0 до проверок; после исключения двух отказов вычислить частное и вернуть true.

**Ход решения**

Возвращаемый bool отвечает на вопрос, удалось ли выполнить операцию, а out переносит числовой результат. Значение out должно быть присвоено на каждом пути, в том числе отказном. Делитель -1 особый только вместе с int.MinValue: положительный результат 2147483648 не помещается в int. После двух проверок обычное деление безопасно. Проверяем успех с положительным и отрицательным частным, ноль-делимое и оба отказа с явным контролем q=0. O(1).

**Частая ошибка**

Забыть присвоить quotient перед false или учесть только делитель 0.

Полный исходник: [solutions/Task02.cs](solutions/Task02.cs).

```csharp
using System;
using Course;

namespace Course.Lesson04;

public static class Task02
{
    public static bool TryDivide(int dividend, int divisor, out int quotient)
    {
        quotient = 0;
        if (divisor == 0 || (dividend == int.MinValue && divisor == -1)) return false;
        quotient = dividend / divisor;
        return true;
    }

    public static void Run()
    {
        Check.True(TryDivide(7, 2, out int q));
        Check.Equal(3, q);
        Check.True(TryDivide(-7, 2, out q));
        Check.Equal(-3, q);
        Check.True(TryDivide(0, 1, out q));
        Check.Equal(0, q);
        Check.Equal(false, TryDivide(7, 0, out q));
        Check.Equal(0, q);
        Check.Equal(false, TryDivide(int.MinValue, -1, out q));
        Check.Equal(0, q);
    }
}
```

**Исполняемые проверки**

- 7/2
- -7/2
- Нулевое делимое
- Нулевой делитель и out=0
- Переполнение и out=0

<a id="04-03"></a>
## 04-03. Обмен копиями и обмен переменными

Обычные int-параметры — копии; ref связывает параметры с переменными вызывающего кода.

**Ход решения**

Exchange получает два независимых локальных int. Присваивания меняют эти копии, а наружу void не возвращает результата. Ref требуется и в объявлении, и в вызове: таким образом виден сам факт возможного изменения переменной. Временная переменная хранит старое a, чтобы второе присваивание не потеряло его. Если a и b ссылаются на одну переменную, обе записи присвоят ей то же число. Проверки сравнивают исходник и исправление, затем испытывают алиас и максимальные значения. O(1).

**Частая ошибка**

Добавить ref только в объявление или обменять через a=a+b на крайних int.

Полный исходник: [solutions/Task03.cs](solutions/Task03.cs).

```csharp
using System;
using Course;

namespace Course.Lesson04;

public static class Task03
{
    public static void Exchange(int a, int b)
    {
        int temporary = a;
        a = b;
        b = temporary;
    }
    public static void Swap(ref int a, ref int b)
    {
        int temporary = a;
        a = b;
        b = temporary;
    }

    public static void Run()
    {
        int x = 2;
        int y = 9;
        Exchange(x, y);
        Check.Equal(2, x);
        Check.Equal(9, y);
        Swap(ref x, ref y);
        Check.Equal(9, x);
        Check.Equal(2, y);
        Swap(ref x, ref x);
        Check.Equal(9, x);
        x = int.MinValue;
        y = int.MaxValue;
        Swap(ref x, ref y);
        Check.Equal(int.MaxValue, x);
        Check.Equal(int.MinValue, y);
    }
}
```

**Исполняемые проверки**

- Исходник сохраняет x,y
- Ref-вариант меняет x,y
- Одна переменная в обоих аргументах
- Крайние int

<a id="04-04"></a>
## 04-04. Именованные и необязательные аргументы

Задать значения по умолчанию в объявлении и оставить вычисление одним выражением.

**Ход решения**

Необязательный параметр не означает неопределённость: при пропуске quantity компилятор подставляет 1, при пропуске fee — 0m. Именованный аргумент связывается с именем параметра, поэтому можно указать fee, не передавая quantity, или изменить порядок всех именованных аргументов. Сам метод не знает, как был записан вызов: он получает готовые значения. Сбор относится к заказу целиком, поэтому не умножается на quantity. Проверка нулевого количества различает эти две модели. Сложность O(1).

**Частая ошибка**

Умножить fee на quantity или предполагать, что пропущенный int автоматически равен 0.

Полный исходник: [solutions/Task04.cs](solutions/Task04.cs).

```csharp
using System;
using Course;

namespace Course.Lesson04;

public static class Task04
{
    public static decimal Total(decimal unitPrice, int quantity = 1, decimal fee = 0m)
    {
        return unitPrice * quantity + fee;
    }

    public static void Run()
    {
        Check.Equal(10m, Total(10m));
        Check.Equal(20m, Total(10m, 2));
        Check.Equal(13m, Total(10m, fee: 3m));
        Check.Equal(23m, Total(fee: 3m, quantity: 2, unitPrice: 10m));
        Check.Equal(3m, Total(10m, 0, 3m));
    }
}
```

**Исполняемые проверки**

- Один аргумент
- Два аргумента
- Пропуск quantity по имени fee
- Переставленные имена
- Нулевое количество со сбором

<a id="04-05"></a>
## 04-05. Как выбирается перегрузка

Литерал 7 и var x=7 имеют int; 7.0 и double y=7 имеют double.

**Ход решения**

Перегрузка определяется сигнатурами и типом аргумента в месте вызова. Значение 7 может храниться и как int, и как double, но равенство математических значений не делает типы одинаковыми. Для int существует точное соответствие, поэтому не требуется расширение к double. Var выводит тип из инициализатора, а явное double y=7 выполняет допустимое расширение при присваивании. InvariantCulture нужен только для стабильной текстовой демонстрации. В Run выполняются все четыре вызова и дополнительное дробное значение.

**Частая ошибка**

Выбирать перегрузку по наличию дробной части во время выполнения вместо статического типа.

Полный исходник: [solutions/Task05.cs](solutions/Task05.cs).

```csharp
using System;
using Course;
using System.Globalization;

namespace Course.Lesson04;

public static class Task05
{
    public static string Describe(int value) { return $"int:{value}"; }
    public static string Describe(double value)
    {
        return "double:" + value.ToString(CultureInfo.InvariantCulture);
    }

    public static void Run()
    {
        Check.Equal("int:7", Describe(7));
        Check.Equal("double:7", Describe(7.0));
        var x = 7;
        double y = 7;
        Check.Equal("int:7", Describe(x));
        Check.Equal("double:7", Describe(y));
        Check.Equal("double:1.25", Describe(1.25));
    }
}
```

**Исполняемые проверки**

- Литерал int
- Литерал double
- Переменная var от int
- Double-переменная с целым значением
- Дробный аргумент

<a id="04-06"></a>
## 04-06. Возврат на каждом пути

Вернуть value % 2 == 0 напрямую.

**Ход решения**

Если число нечётное, условие ложно и управление достигает закрывающей скобки bool-метода без return. Компилятор отвергает такой путь до запуска. Само сравнение value%2==0 уже даёт требуемый bool для каждого числа, поэтому отдельное ветвление избыточно. У отрицательных чисел остаток может быть отрицательным, но чётность проверяется равенством нулю и от знака не зависит. Проверяем отрицательные чётное и нечётное, ноль и минимальное int. Невозможно выполнить исходный некомпилируемый метод; исполняется исправленный аналог.

**Частая ошибка**

Считать, что отсутствие return автоматически означает false.

Полный исходник: [solutions/Task06.cs](solutions/Task06.cs).

```csharp
using System;
using Course;

namespace Course.Lesson04;

public static class Task06
{
    public static bool IsEven(int value)
    {
        return value % 2 == 0;
    }

    public static void Run()
    {
        Check.True(IsEven(-4));
        Check.Equal(false, IsEven(-3));
        Check.True(IsEven(0));
        Check.Equal(false, IsEven(7));
        Check.True(IsEven(int.MinValue));
    }
}
```

**Исполняемые проверки**

- Отрицательные чётное и нечётное
- Ноль
- Положительное нечётное
- int.MinValue

<a id="04-07"></a>
## 04-07. Чек без смешивания расчёта и печати

Отдельно вычислять базовую стоимость и ставку; в Receipt округлить скидку и оформить нейтральный текст.

**Ход решения**

Subtotal отвечает только за произведение, DiscountRate — за порог количества. Receipt использует оба метода и формирует результат, который можно проверить без перехвата консоли. Скидку округляем до копеек до вычитания: так итоговая сумма согласована с напечатанными составляющими. При заданной цене subtotal уже имеет не более двух знаков. Формат 0.00 вместе с InvariantCulture фиксирует точку и два разряда. Проверки 9 и 10 отделяют порог, а цена 0.01 при 15 штуках даёт скидку 0.015→0.02 и проверяет правило округления.

**Частая ошибка**

Считать скидку до достижения десяти штук или напечатать несогласованный итог из неокруглённой скидки.

Полный исходник: [solutions/Task07.cs](solutions/Task07.cs).

```csharp
using System;
using Course;
using System.Globalization;

namespace Course.Lesson04;

public static class Task07
{
    public static decimal Subtotal(decimal price, int quantity) { return price * quantity; }
    public static decimal DiscountRate(int quantity) { return quantity >= 10 ? 0.10m : 0m; }
    public static string Receipt(decimal price, int quantity)
    {
        decimal subtotal = Subtotal(price, quantity);
        decimal discount = Math.Round(subtotal * DiscountRate(quantity), 2, MidpointRounding.AwayFromZero);
        decimal total = subtotal - discount;
        return "subtotal=" + subtotal.ToString("0.00", CultureInfo.InvariantCulture) +
               ";discount=" + discount.ToString("0.00", CultureInfo.InvariantCulture) +
               ";total=" + total.ToString("0.00", CultureInfo.InvariantCulture);
    }

    public static void Run()
    {
        Check.Equal(30m, Subtotal(10m, 3));
        Check.Equal(0m, DiscountRate(9));
        Check.Equal(0.10m, DiscountRate(10));
        Check.Equal("subtotal=90.00;discount=0.00;total=90.00", Receipt(10m, 9));
        Check.Equal("subtotal=100.00;discount=10.00;total=90.00", Receipt(10m, 10));
        Check.Equal("subtotal=0.00;discount=0.00;total=0.00", Receipt(10m, 0));
        Check.Equal("subtotal=0.15;discount=0.02;total=0.13", Receipt(0.01m, 15));
    }
}
```

**Исполняемые проверки**

- Помощники проверены отдельно
- Количество 9 и 10
- Нулевой заказ
- Скидка на половине копейки

<a id="04-08"></a>
## 04-08. Локальный помощник для расстояния

Преобразовать первый операнд каждой разности к long, локально взять модули и сложить.

**Ход решения**

Расстояние не зависит от направления, поэтому разности координат берём по модулю. Два int могут различаться на 4294967295: вычислять такую разность в int нельзя. Long-приведение до вычитания сохраняет точный результат. Локальная функция Absolute описывает маленькую деталь только этого метода; static показывает, что ей не нужны внешние переменные. Здесь её входы ограничены разностями int, поэтому отрицание не встретит long.MinValue. Проверяем совпадение точек, обратный порядок и максимальную разность по обеим осям. O(1).

**Частая ошибка**

Написать (long)(x1-x2), когда вычитание уже переполнилось.

Полный исходник: [solutions/Task08.cs](solutions/Task08.cs).

```csharp
using System;
using Course;

namespace Course.Lesson04;

public static class Task08
{
    public static long Manhattan(int x1, int y1, int x2, int y2)
    {
        static long Absolute(long value) { return value < 0 ? -value : value; }
        long dx = (long)x1 - x2;
        long dy = (long)y1 - y2;
        return Absolute(dx) + Absolute(dy);
    }

    public static void Run()
    {
        Check.Equal(7L, Manhattan(1, 2, 4, -2));
        Check.Equal(0L, Manhattan(5, 5, 5, 5));
        Check.Equal(7L, Manhattan(4, -2, 1, 2));
        Check.Equal(8_589_934_590L, Manhattan(int.MinValue, int.MinValue, int.MaxValue, int.MaxValue));
    }
}
```

**Исполняемые проверки**

- Обычное расстояние
- Совпадающие точки
- Обратный порядок
- Крайние координаты

<a id="04-09"></a>
## 04-09. Рекурсивная сумма цифр

При rest=0 вернуть 0, иначе rest%10 + Sum(rest/10).

**Ход решения**

Каждый вызов решает задачу для десятичного префикса без последней цифры и добавляет эту цифру. Деление неотрицательного rest на 10 строго уменьшает его до нуля, что даёт завершение. Для int максимум десять цифр, поэтому глубина ограничена примерно одиннадцатью вызовами и не опасна для стека. Расширение перед модулем обрабатывает int.MinValue. Ноль естественно попадает в базу и возвращает 0. Это пример рекурсии для обучения механизму вызовов, а не обещание преимущества перед циклом. Время и стек O(числа цифр).

**Частая ошибка**

Забыть базовый случай, передать rest без уменьшения или взять модуль в int.

Полный исходник: [solutions/Task09.cs](solutions/Task09.cs).

```csharp
using System;
using Course;

namespace Course.Lesson04;

public static class Task09
{
    public static int DigitSum(int value)
    {
        static int Sum(long nonNegative)
        {
            if (nonNegative == 0) return 0;
            return (int)(nonNegative % 10) + Sum(nonNegative / 10);
        }
        long rest = value;
        if (rest < 0) rest = -rest;
        return Sum(rest);
    }

    public static void Run()
    {
        Check.Equal(0, DigitSum(0));
        Check.Equal(6, DigitSum(-1203));
        Check.Equal(1, DigitSum(1000));
        Check.Equal(47, DigitSum(int.MinValue));
        Check.Equal(46, DigitSum(int.MaxValue));
    }
}
```

**Исполняемые проверки**

- Ноль
- Отрицательное число
- Цифры с нулями
- int.MinValue
- int.MaxValue

<a id="04-10"></a>
## 04-10. Трасса возврата из рекурсии

База возвращает 0, Trace(1) оборачивает её в 1[ ]1, Trace(2) — в 2[ ]2.

**Ход решения**

Вызовы идут 2→1→0, но окончательные строки становятся известны в обратном порядке. Внешний вызов не может закончить интерполяцию, пока не получил результат внутреннего. После базы имеем 0, затем 1[0]1, затем 2[1[0]1]2. Цифры после закрывающей скобки показывают обратный ход, который теряется при печати только перед рекурсивным вызовом. Каждый уровень хранит своё n. Область n≤5 ограничивает объём наглядной трассы. Проверка n=3 подтверждает повторяемость механизма, а не один заученный ответ.

**Частая ошибка**

Собрать только 210 или считать, что n одной переменной меняется одновременно во всех кадрах стека.

Полный исходник: [solutions/Task10.cs](solutions/Task10.cs).

```csharp
using System;
using Course;

namespace Course.Lesson04;

public static class Task10
{
    public static string Trace(int n)
    {
        if (n == 0) return "0";
        return $"{n}[{Trace(n - 1)}]{n}";
    }

    public static void Run()
    {
        Check.Equal("0", Trace(0));
        Check.Equal("1[0]1", Trace(1));
        Check.Equal("2[1[0]1]2", Trace(2));
        Check.Equal("3[2[1[0]1]2]3", Trace(3));
    }
}
```

**Исполняемые проверки**

- Точные строки для n=0,1,2,3

<a id="04-11"></a>
## 04-11. Проверять bool и out вместе

Разбирать во временную переменную и назначать out только при успехе; различающий вход -5.

**Ход решения**

Wrong действительно возвращает false для -5, но int.TryParse уже записал в out значение -5. Это нарушает договорённость об отказе. Для неверного формата TryParse сам запишет 0, поэтому тест только с bad ошибку не обнаружит. Правильный метод сначала назначает value=0, разбирает во временный parsed и копирует его только после проверки положительности. В Run проверяются пары bool/значение для успеха, нуля, отрицательного числа, EOF и переполнения. Затем непосредственно фиксируется ложный out ошибочного варианта. Время O(длины текста).

**Частая ошибка**

Считать, что false автоматически обнуляет out независимо от предыдущих операций.

Полный исходник: [solutions/Task11.cs](solutions/Task11.cs).

```csharp
using System;
using Course;

namespace Course.Lesson04;

public static class Task11
{
    public static bool TryPositive(string? text, out int value)
    {
        value = 0;
        if (!int.TryParse(text, out int parsed) || parsed <= 0) return false;
        value = parsed;
        return true;
    }
    public static bool Wrong(string? text, out int value)
    {
        return int.TryParse(text, out value) && value > 0;
    }

    public static void Run()
    {
        Check.True(TryPositive("7", out int value));
        Check.Equal(7, value);
        Check.Equal(false, TryPositive("0", out value));
        Check.Equal(0, value);
        Check.Equal(false, TryPositive("-5", out value));
        Check.Equal(0, value);
        Check.Equal(false, TryPositive(null, out value));
        Check.Equal(0, value);
        Check.Equal(false, TryPositive("999999999999999999", out value));
        Check.Equal(0, value);
        Check.Equal(false, Wrong("-5", out value));
        Check.Equal(-5, value);
    }
}
```

**Исполняемые проверки**

- Успех и выходное число
- Ноль
- Отрицательное число
- EOF
- Переполнение
- Wrong имеет неверный out при false

<a id="04-12"></a>
## 04-12. Быстрая целая степень

Хранить result, factor и remaining; после деления remaining пополам квадрат нужен только при remaining>0.

**Ход решения**

Инвариант: result * factor^remaining равен исходной степени как математическое выражение. Нечётный remaining позволяет вынести один factor в result. После удаления этого множителя оставшийся показатель чётный; деление показателя на два и квадрат factor сохраняют значение. Когда remaining=0, result готов. Ненужный квадрат в самом конце нарушил бы безопасность checked для 10^18, хотя он не участвует в ответе. Заданная область гарантирует помещаемость нужных произведений. Проверяем отрицательное основание с чётным и нечётным показателем, нулевые случаи и 10^18. O(log exponent) времени, O(1) памяти.

**Частая ошибка**

Всегда вычислять factor*factor в конце тела и получить лишнее переполнение.

Полный исходник: [solutions/Task12.cs](solutions/Task12.cs).

```csharp
using System;
using Course;

namespace Course.Lesson04;

public static class Task12
{
    public static long Power(long basis, int exponent)
    {
        long result = 1;
        long factor = basis;
        int remaining = exponent;
        while (remaining > 0)
        {
            if (remaining % 2 != 0) result = checked(result * factor);
            remaining /= 2;
            if (remaining > 0) factor = checked(factor * factor);
        }
        return result;
    }

    public static void Run()
    {
        Check.Equal(1L, Power(0, 0));
        Check.Equal(1L, Power(7, 0));
        Check.Equal(0L, Power(0, 10));
        Check.Equal(-32L, Power(-2, 5));
        Check.Equal(16L, Power(-2, 4));
        Check.Equal(1_000_000_000_000_000_000L, Power(10, 18));
    }
}
```

**Исполняемые проверки**

- Нулевая степень
- 0 в положительной степени
- Отрицательное основание с обеими чётностями
- 10^18 без лишнего квадрата

<a id="04-13"></a>
## 04-13. Вывод через предоставленный приёмник

Цикл пишет в аргумент output; тест создаёт StringWriter с фиксированным NewLine.

**Ход решения**

Если метод сам обращается к Console, проверка вынуждена менять глобальный вывод процесса. Переданный TextWriter делает зависимость явной: при запуске это может быть консоль, а в тесте — строковый приёмник. Мы не создаём и не закрываем чужой output, потому что его срок жизни определяет вызывающий код. Границы маленькие, поэтому увеличение счётчика не переполнится. Тест фиксирует NewLine и сравнивает полный текст, включая завершающий перевод строки; отдельный тест проверяет, что после вызова приёмник остаётся доступным. O(числа выведенных чисел).

**Частая ошибка**

Печатать в Console вместо output или закрыть переданный приёмник.

Полный исходник: [solutions/Task13.cs](solutions/Task13.cs).

```csharp
using System;
using Course;
using System.IO;

namespace Course.Lesson04;

public static class Task13
{
    public static void PrintRange(TextWriter output, int from, int to)
    {
        for (int value = from; value <= to; value++) output.WriteLine(value);
    }

    public static void Run()
    {
        using var output = new StringWriter();
        output.NewLine = "\n";
        PrintRange(output, -1, 1);
        Check.Equal("-1\n0\n1\n", output.ToString());
        using var empty = new StringWriter();
        PrintRange(empty, 2, 1);
        Check.Equal("", empty.ToString());
        using var single = new StringWriter();
        single.NewLine = "\n";
        PrintRange(single, 5, 5);
        Check.Equal("5\n", single.ToString());
        single.Write("still-open");
        Check.Equal("5\nstill-open", single.ToString());
    }
}
```

**Исполняемые проверки**

- Диапазон -1..1 и точные переводы строк
- Пустой диапазон
- Один элемент
- Приёмник доступен после метода

<a id="04-14"></a>
## 04-14. Часы: совместное изменение двух ref-параметров

После валидации вычислить total=(hours*60+minutes+deltaMinutes)%1440 и записать total/60 и total%60.

**Ход решения**

Время суток удобно представить одним числом минут от начала суток. Только после проверки всех трёх входов вычисляем сумму и остаток по 1440; он гарантированно неотрицателен при допустимых входах. Деление на 60 даёт новые часы, остаток — минуты. Все временные вычисления выполняются до присваивания ref-параметрам, поэтому отказ не оставляет частично обновлённое состояние. Запрет алиаса двух ref нужен: одна переменная не может одновременно хранить обе составляющие. Проверки включают полночь, ровно сутки, нулевую добавку, обе стороны диапазонов и int.MaxValue. Сложность O(1).

**Частая ошибка**

Исправлять hours или minutes до завершения валидации либо прибавлять непроверенный int с переполнением.

Полный исходник: [solutions/Task14.cs](solutions/Task14.cs).

```csharp
using System;
using Course;

namespace Course.Lesson04;

public static class Task14
{
    public static bool TryAdvanceClock(ref int hours, ref int minutes, int deltaMinutes)
    {
        if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59 ||
            deltaMinutes < 0 || deltaMinutes > 1440) return false;
        int total = (hours * 60 + minutes + deltaMinutes) % 1440;
        hours = total / 60;
        minutes = total % 60;
        return true;
    }

    public static void Run()
    {
        int hours = 23;
        int minutes = 50;
        Check.True(TryAdvanceClock(ref hours, ref minutes, 20));
        Check.Equal(0, hours);
        Check.Equal(10, minutes);
        Check.True(TryAdvanceClock(ref hours, ref minutes, 1440));
        Check.Equal(0, hours);
        Check.Equal(10, minutes);
        Check.True(TryAdvanceClock(ref hours, ref minutes, 0));
        Check.Equal(0, hours);
        Check.Equal(10, minutes);
        hours = 0;
        minutes = 59;
        Check.True(TryAdvanceClock(ref hours, ref minutes, 1));
        Check.Equal(1, hours);
        Check.Equal(0, minutes);
        static void Refusal(int originalHours, int originalMinutes, int delta)
        {
            int h = originalHours;
            int m = originalMinutes;
            Check.Equal(false, TryAdvanceClock(ref h, ref m, delta));
            Check.Equal(originalHours, h);
            Check.Equal(originalMinutes, m);
        }
        Refusal(-1, 10, 1);
        Refusal(24, 10, 1);
        Refusal(12, -1, 1);
        Refusal(12, 60, 1);
        Refusal(12, 30, -1);
        Refusal(12, 30, 1441);
        Refusal(12, 30, int.MaxValue);
    }
}
```

**Исполняемые проверки**

- 23:50 плюс 20
- Ровно сутки
- Нулевая добавка
- Переход 00:59 на следующий час
- Недопустимые часы, минуты и добавки сохраняют оба ref
- Очень большая добавка отвергается до арифметики

<a id="04-15"></a>
## 04-15. Проверка номера с контрольной цифрой

Выделить проверку диапазона и расчёт цифры; основной метод выбирает диагноз.

**Ход решения**

Первая функция отвечает только за допустимый диапазон номера. Вторая получает уже проверенный трёхзначный префикс и выделяет цифры делением и остатком. Веса 3,2,1 применяются в заданном порядке, последующий %10 ограничивает результат одной цифрой. Основной метод сначала проверяет диапазон, чтобы не нарушить предусловие ExpectedDigit, затем сравнивает code%10 с результатом для code/10. Помощники тестируются отдельно, что позволяет локализовать ошибку: неверное выделение цифр отличается от неверного порядка диагностики. Проверки 1003 и 9994 покрывают края номера; это учебная контрольная сумма, а не стандарт внешней системы.

**Частая ошибка**

Применить веса к цифрам в обратном порядке или вызвать ExpectedDigit до проверки диапазона.

Полный исходник: [solutions/Task15.cs](solutions/Task15.cs).

```csharp
using System;
using Course;

namespace Course.Lesson04;

public static class Task15
{
    public static bool IsFourDigits(int code) { return code >= 1000 && code <= 9999; }
    public static int ExpectedDigit(int prefix)
    {
        int a = prefix / 100;
        int b = prefix / 10 % 10;
        int c = prefix % 10;
        return (3 * a + 2 * b + c) % 10;
    }
    public static string InspectCode(int code)
    {
        if (!IsFourDigits(code)) return "range";
        return code % 10 == ExpectedDigit(code / 10) ? "valid" : "checksum";
    }

    public static void Run()
    {
        Check.Equal(false, IsFourDigits(999));
        Check.True(IsFourDigits(1000));
        Check.True(IsFourDigits(9999));
        Check.Equal(0, ExpectedDigit(123));
        Check.Equal(7, ExpectedDigit(900));
        Check.Equal("valid", InspectCode(1230));
        Check.Equal("checksum", InspectCode(1234));
        Check.Equal("valid", InspectCode(1003));
        Check.Equal("valid", InspectCode(9994));
        Check.Equal("range", InspectCode(-1230));
        Check.Equal("range", InspectCode(10000));
    }
}
```

**Исполняемые проверки**

- Диапазон отдельно
- Расчёт для 123 и 900
- Правильный и неверный номер
- Нижний и верхний допустимые номера
- Отрицательный и пятизначный ввод
