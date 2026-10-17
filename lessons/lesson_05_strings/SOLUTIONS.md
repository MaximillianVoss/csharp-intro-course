# Урок 5. Строки, сравнение и границы Unicode: решения

Сначала выполните свою попытку по TASKS.md. Здесь приведены ответы.
Каждое решение запускается по ID; Run содержит исполняемые проверки.

<a id="05-01"></a>
## 05-01. Безопасное чтение по индексу

Проверить null и обе границы, затем присвоить char.

**Ход решения**

Length задаёт число кодовых единиц UTF-16. Последний допустимый индекс равен Length-1, а пустая строка не имеет допустимого индекса. Out присваивается нулевому char до возможного отказа, чтобы контракт был одинаков на всех путях. Проверка null обязана предшествовать чтению Length. Специальный тест с эмодзи показывает, что операция действительно возвращает одну кодовую единицу: первая часть пары является старшим суррогатом, а не самостоятельным скаляром. Это ожидаемая граница данного API. Время O(1).

**Частая ошибка**

Проверить index&lt;=Length или считать возвращаемый char всегда целым знаком.

Полный исходник: [solutions/Task01.cs](solutions/Task01.cs).

```csharp
using System;
using Course;

namespace Course.Lesson05;

public static class Task01
{
    public static bool TryChar(string? text, int index, out char value)
    {
        value = '\0';
        if (text is null || index < 0 || index >= text.Length) return false;
        value = text[index];
        return true;
    }

    public static void Run()
    {
        Check.True(TryChar("кот", 1, out char value));
        Check.Equal('о', value);
        Check.Equal(false, TryChar("кот", 3, out value));
        Check.Equal('\0', value);
        Check.Equal(false, TryChar("кот", -1, out value));
        Check.Equal(false, TryChar("", 0, out value));
        Check.Equal(false, TryChar(null, 0, out value));
        Check.Equal('\0', value);
        Check.True(TryChar("😀", 0, out value));
        Check.True(char.IsHighSurrogate(value));
    }
}
```

**Исполняемые проверки**

- Обычный индекс
- Индекс равен Length
- Отрицательный индекс
- Пустая строка
- Null
- Старший суррогат эмодзи

<a id="05-02"></a>
## 05-02. Середина строки с оговорённым алфавитом

После пустого случая определить длину фрагмента 1 или 2 и начало (Length-length)/2.

**Ход решения**

Для нечётного количества букв после удаления одной центральной оставшееся число чётно и поровну делится на левую и правую часть. Для чётного количества аналогично выделяются две центральные. Формула (Length-length)/2 даёт число букв слева и тем самым начало Substring. Пустоту проверяем до выбора длины 2, иначе возник бы отрицательный индекс. ASCII-ограничение здесь существенно: перенос того же алгоритма на произвольные UTF-16 может разрезать суррогатную пару. Проверки длин 0,1,2,4,5 покрывают оба режима и минимальные случаи. O(длины результата).

**Частая ошибка**

Для чётной строки начать с Length/2 вместо Length/2-1.

Полный исходник: [solutions/Task02.cs](solutions/Task02.cs).

```csharp
using System;
using Course;

namespace Course.Lesson05;

public static class Task02
{
    public static string Middle(string text)
    {
        if (text.Length == 0) return "";
        int length = text.Length % 2 == 0 ? 2 : 1;
        return text.Substring((text.Length - length) / 2, length);
    }

    public static void Run()
    {
        Check.Equal("", Middle(""));
        Check.Equal("A", Middle("A"));
        Check.Equal("Ab", Middle("Ab"));
        Check.Equal("bc", Middle("abcd"));
        Check.Equal("c", Middle("abcde"));
    }
}
```

**Исполняемые проверки**

- Длины 0,1,2,4,5
- Сохранение регистра

<a id="05-03"></a>
## 05-03. Replace создаёт новое значение

Replace возвращает другую строку; присваивание меняет только значение переменной copy.

**Ход решения**

Сразу после copy=original обе переменные обозначают одно строковое значение. String не допускает изменения его содержимого через Replace: вызов возвращает результат замены. Присваивание этого результата copy перенаправляет только вторую переменную. Original сохраняет прежний текст. Если не сохранить результат вызова, обе переменные останутся с исходным содержимым. Проверяем и вариант без присваивания через отдельный метод Ignored, чтобы объяснение подтверждалось выполнением. Пустая строка и отсутствие a также допустимы. Время зависит от длины строки, дополнительная память — от результата.

**Частая ошибка**

Ожидать изменения всех переменных, содержащих тот же текст, или забыть сохранить результат Replace.

Полный исходник: [solutions/Task03.cs](solutions/Task03.cs).

```csharp
using System;
using Course;

namespace Course.Lesson05;

public static class Task03
{
    public static string Trace(string text)
    {
        string original = text;
        string copy = original;
        copy = copy.Replace('a', 'o');
        return original + ":" + copy;
    }
    public static string Ignored(string text)
    {
        text.Replace('a', 'o');
        return text;
    }

    public static void Run()
    {
        Check.Equal("banana:bonono", Trace("banana"));
        Check.Equal(":", Trace(""));
        Check.Equal("xyz:xyz", Trace("xyz"));
        Check.Equal("banana", Ignored("banana"));
    }
}
```

**Исполняемые проверки**

- Точный исходник на banana
- Пустая строка
- Отсутствие заменяемой буквы
- Вызов Replace без присваивания

<a id="05-04"></a>
## 05-04. Цифры ASCII вместо всех числовых знаков

После проверки пустоты пройти char и отвергнуть любой вне ASCII-диапазона.

**Ход решения**

Строка проверяется как последовательность допустимых кодов, а не как числовое значение. Поэтому переполнение и ведущие нули здесь не имеют отношения к успеху. Char.IsDigit распознаёт десятичные цифры Unicode разных письменностей, тогда как протокол разрешает только десять ASCII-кодов. Сравнение с диапазоном явно выражает это ограничение. Отказ можно вернуть на первом запрещённом char. Проверки содержат арабские цифры, пробел, знак и длинную последовательность нулей. В худшем случае O(n) времени, O(1) дополнительной памяти.

**Частая ошибка**

Использовать int.TryParse и отвергнуть длинную допустимую строку либо принять не-ASCII цифры.

Полный исходник: [solutions/Task04.cs](solutions/Task04.cs).

```csharp
using System;
using Course;

namespace Course.Lesson05;

public static class Task04
{
    public static bool IsAsciiDigits(string? text)
    {
        if (string.IsNullOrEmpty(text)) return false;
        foreach (char ch in text)
            if (ch < '0' || ch > '9') return false;
        return true;
    }

    public static void Run()
    {
        Check.True(IsAsciiDigits("007"));
        Check.True(IsAsciiDigits(new string('0', 1000)));
        Check.Equal(false, IsAsciiDigits(null));
        Check.Equal(false, IsAsciiDigits(""));
        Check.Equal(false, IsAsciiDigits("+12"));
        Check.Equal(false, IsAsciiDigits("12 "));
        Check.Equal(false, IsAsciiDigits("١٢"));
        Check.Equal(false, IsAsciiDigits("1a2"));
    }
}
```

**Исполняемые проверки**

- Ведущие нули
- Длинная цифровая строка
- Null и пустота
- Знак и пробел
- Арабские цифры
- Буква внутри

<a id="05-05"></a>
## 05-05. Первое вхождение без готового поиска

For от нуля до Length-1 и return при первом равенстве.

**Ход решения**

Порядок слева направо и немедленный возврат гарантируют, что найдено именно первое, а не последнее совпадение. До текущего индекса инвариант говорит: target там не найден. Если цикл завершился, просмотрена вся строка, поэтому -1 корректно обозначает отсутствие. Нельзя возвращать 0 при неуспехе: это допустимый индекс первого char. Отдельно проверяем совпадение в позиции 0, повторяющиеся буквы, последний char и пустую строку. Время O(n) в худшем случае и O(1) памяти. Это упражнение на индексирование; в обычном коде для поиска уместен IndexOf.

**Частая ошибка**

Продолжать поиск после совпадения и вернуть последнее вхождение либо использовать 0 как отсутствие.

Полный исходник: [solutions/Task05.cs](solutions/Task05.cs).

```csharp
using System;
using Course;

namespace Course.Lesson05;

public static class Task05
{
    public static int FirstIndex(string text, char target)
    {
        for (int index = 0; index < text.Length; index++)
            if (text[index] == target) return index;
        return -1;
    }

    public static void Run()
    {
        Check.Equal(1, FirstIndex("banana", 'a'));
        Check.Equal(-1, FirstIndex("banana", 'A'));
        Check.Equal(0, FirstIndex("abc", 'a'));
        Check.Equal(2, FirstIndex("abc", 'c'));
        Check.Equal(-1, FirstIndex("", 'a'));
    }
}
```

**Исполняемые проверки**

- Первое из повторов
- Различие регистра
- Индекс 0
- Последний индекс
- Пустота

<a id="05-06"></a>
## 05-06. Нейтральное сравнение команд

Вызвать string.Equals(text,expected,StringComparison.OrdinalIgnoreCase).

**Ход решения**

Техническая команда должна означать одно и то же при разных настройках компьютера. OrdinalIgnoreCase задаёт нечувствительное к регистру сопоставление без лингвистической сортировки. Статический string.Equals принимает null безопасно, поэтому не нужен вызов метода через потенциально отсутствующий text. Пробелы не удаляются, поскольку контракт считает их частью команды. В отличие от ToLower, сравнение не создаёт две дополнительные строки. Проверки покрывают смешанный регистр, EOF, лишние пробелы и похожий, но иной текст. Время O(n).

**Частая ошибка**

Использовать ToLower() без явной культуры или незаметно сделать Trim.

Полный исходник: [solutions/Task06.cs](solutions/Task06.cs).

```csharp
using System;
using Course;

namespace Course.Lesson05;

public static class Task06
{
    public static bool SameCommand(string? text, string expected)
    {
        return string.Equals(text, expected, StringComparison.OrdinalIgnoreCase);
    }

    public static void Run()
    {
        Check.True(SameCommand("StArT", "start"));
        Check.Equal(false, SameCommand(null, "start"));
        Check.Equal(false, SameCommand(" start ", "start"));
        Check.Equal(false, SameCommand("starter", "start"));
        Check.True(SameCommand("stop", "stop"));
    }
}
```

**Исполняемые проверки**

- Смешанный регистр
- Null
- Пробелы
- Иная команда
- Равенство без изменений

<a id="05-07"></a>
## 05-07. Разделитель остался в результате

Индекс -1 означает отсутствие, иначе фрагмент начинается на единицу правее.

**Ход решения**

IndexOf возвращает индекс самого разделителя. Substring(position) поэтому сохраняет двоеточие, а при -1 получает недопустимый индекс и вызывает ArgumentOutOfRangeException. Нужна проверка отсутствия до выделения фрагмента. Если двоеточие последнее, position+1 равен Length; Substring(Length) корректно возвращает пустую строку. Второе двоеточие принадлежит полезному хвосту и не удаляется. В Run ошибки исходника подтверждаются отдельно от исправленного поведения. Время поиска и копирования O(n), память пропорциональна хвосту.

**Частая ошибка**

Использовать последнее двоеточие либо убрать все двоеточия через Replace.

Полный исходник: [solutions/Task07.cs](solutions/Task07.cs).

```csharp
using System;
using Course;

namespace Course.Lesson05;

public static class Task07
{
    public static string Broken(string text)
    {
        int position = text.IndexOf(':');
        return text.Substring(position);
    }
    public static string AfterColon(string text)
    {
        int position = text.IndexOf(':');
        if (position < 0) return "";
        return text.Substring(position + 1);
    }

    public static void Run()
    {
        Check.Equal(":value", Broken("name:value"));
        Check.Throws<ArgumentOutOfRangeException>(() => Broken("name"));
        Check.Equal("value:tail", AfterColon("name:value:tail"));
        Check.Equal("", AfterColon("name:"));
        Check.Equal("value", AfterColon(":value"));
        Check.Equal("", AfterColon("name"));
        Check.Equal("", AfterColon(""));
    }
}
```

**Исполняемые проверки**

- Исходник сохраняет разделитель
- Исходник падает без разделителя
- Один и несколько разделителей
- Разделитель в начале и конце
- Нет разделителя

<a id="05-08"></a>
## 05-08. Нормализация пробельных промежутков

Хранить флаг отложенного разделителя и добавлять его только между двумя непустыми фрагментами.

**Ход решения**

Пробел не добавляется немедленно при встрече разделителя: неизвестно, будет ли после него ещё текст. Флаг pending сообщает, что между уже записанным результатом и следующим символом нужен один пробел. При непробельном char сначала добавляем отложенный пробел, затем сам char и сбрасываем флаг. Начальные разделители не создают pending, потому что результат пока пуст; конечные остаются только флагом и не попадают в итог. Кодовые единицы эмодзи копируются последовательно, поэтому пара сохраняется. O(n) времени и O(n) памяти под результат.

**Частая ошибка**

Добавить пробел сразу и затем забыть удалить последний или учитывать только ASCII-пробел.

Полный исходник: [solutions/Task08.cs](solutions/Task08.cs).

```csharp
using System;
using Course;
using System.Text;

namespace Course.Lesson05;

public static class Task08
{
    public static string CollapseWhitespace(string text)
    {
        var builder = new StringBuilder();
        bool pending = false;
        foreach (char ch in text)
        {
            if (char.IsWhiteSpace(ch))
            {
                if (builder.Length > 0) pending = true;
                continue;
            }
            if (pending) builder.Append(' ');
            builder.Append(ch);
            pending = false;
        }
        return builder.ToString();
    }

    public static void Run()
    {
        Check.Equal("a b", CollapseWhitespace("  a\t\nb  "));
        Check.Equal("", CollapseWhitespace(" \t\n"));
        Check.Equal("", CollapseWhitespace(""));
        Check.Equal("a b", CollapseWhitespace("a\u00A0b"));
        Check.Equal("😀 text", CollapseWhitespace(" 😀  text "));
        Check.Equal("a b", CollapseWhitespace("a b"));
    }
}
```

**Исполняемые проверки**

- Пробелы, табуляция и перевод строки
- Только разделители
- Пустота
- Неразрывный пробел
- Эмодзи сохраняется
- Уже нормализованный текст

<a id="05-09"></a>
## 05-09. Неперекрывающиеся вхождения фрагмента

После проверки непустого образца искать с позиции start и сдвигать её на position+needle.Length.

**Ход решения**

IndexOf возвращает -1, если в оставшемся хвосте совпадений нет. Иначе найденный фрагмент целиком засчитывается, а начало следующего поиска переносится за его конец. Поскольку needle непустой, start строго растёт: бесконечного цикла нет. Сдвиг только на один char считал бы перекрытия, что меняет результат на aaaaa и aa. Ordinal делает сопоставление технического фрагмента независимым от культурных правил. Проверяем образец длиннее текста, пустой образец, совпадения подряд и различие регистра. Дополнительная память O(1); стоимость поиска определяется библиотечным IndexOf и длинами текста/образца.

**Частая ошибка**

Не обработать пустой образец или сдвигаться на 1 и считать перекрытия.

Полный исходник: [solutions/Task09.cs](solutions/Task09.cs).

```csharp
using System;
using Course;

namespace Course.Lesson05;

public static class Task09
{
    public static int CountPieces(string text, string needle)
    {
        if (needle.Length == 0) return 0;
        int start = 0;
        int count = 0;
        while (start <= text.Length - needle.Length)
        {
            int position = text.IndexOf(needle, start, StringComparison.Ordinal);
            if (position < 0) break;
            count++;
            start = position + needle.Length;
        }
        return count;
    }

    public static void Run()
    {
        Check.Equal(2, CountPieces("aaaaa", "aa"));
        Check.Equal(1, CountPieces("AaA", "a"));
        Check.Equal(0, CountPieces("a", "aa"));
        Check.Equal(0, CountPieces("abc", ""));
        Check.Equal(0, CountPieces("", "a"));
        Check.Equal(1, CountPieces("abc", "abc"));
    }
}
```

**Исполняемые проверки**

- Повторяющиеся неперекрывающиеся фрагменты
- Регистр
- Образец длиннее текста
- Пустой образец
- Пустой текст
- Совпадение с целой строкой

<a id="05-10"></a>
## 05-10. Поле в кавычках с удвоенным экранированием

Идти по внутренним индексам; при парной кавычке добавлять одну и пропускать второй char.

**Ход решения**

Внешняя пара определяет границы содержимого и не попадает в результат. Внутри кавычка не может копироваться как обычный char: она требует следующую такую же, причём следующая должна оставаться внутри поля. После успешной пары сдвигаем индекс ещё раз, чтобы не обработать вторую кавычку повторно. Результат собирается локально и назначается out только после полного успеха, поэтому поздняя ошибка не оставляет частичное содержимое. Проверки четырёх кавычек дают одну кавычку; три кавычки оставляют непарную внутреннюю и отвергаются. O(n) времени и памяти.

**Частая ошибка**

Разрешить последнюю внешнюю кавычку в качестве второй части внутреннего escape или вернуть частичный out.

Полный исходник: [solutions/Task10.cs](solutions/Task10.cs).

```csharp
using System;
using Course;
using System.Text;

namespace Course.Lesson05;

public static class Task10
{
    public static bool TryUnquote(string? token, out string value)
    {
        value = "";
        if (token is null || token.Length < 2 || token[0] != '"' || token[token.Length - 1] != '"')
            return false;
        var builder = new StringBuilder();
        for (int index = 1; index < token.Length - 1; index++)
        {
            char ch = token[index];
            if (ch == '"')
            {
                if (index + 1 >= token.Length - 1 || token[index + 1] != '"') return false;
                index++;
            }
            builder.Append(ch);
        }
        value = builder.ToString();
        return true;
    }

    public static void Run()
    {
        Check.True(TryUnquote("\"abc\"", out string value));
        Check.Equal("abc", value);
        Check.True(TryUnquote("\"\"", out value));
        Check.Equal("", value);
        Check.True(TryUnquote("\"a\"\"b\"", out value));
        Check.Equal("a\"b", value);
        Check.Equal(false, TryUnquote("\"a\"b\"", out value));
        Check.Equal("", value);
        Check.Equal(false, TryUnquote("\"\"\"", out value));
        Check.True(TryUnquote("\"\"\"\"", out value));
        Check.Equal("\"", value);
        Check.Equal(false, TryUnquote(null, out value));
        Check.Equal(false, TryUnquote("abc", out value));
        Check.Equal("", value);
    }
}
```

**Исполняемые проверки**

- Обычное и пустое поле
- Удвоенная кавычка
- Одиночная внутренняя
- Три и четыре кавычки
- Null и отсутствие внешних кавычек
- Отказ очищает out

<a id="05-11"></a>
## 05-11. Различающий тест для первого скаляра

Вернуть первый элемент EnumerateRunes как строку; эмодзи различает правильный и ошибочный варианты.

**Ход решения**

У BMP-буквы один char и один скаляр, поэтому тест на a не обнаружит неверное Substring(0,1). Эмодзи 😀 кодируется двумя char, но одним Rune: ошибочный вариант вернёт только старший суррогат. В Run это подтверждается длиной 1 и char.IsHighSurrogate, а правильный результат сравнивается с целым эмодзи. Комбинируемая последовательность e+акцент содержит два скаляра, поэтому требуемый первый скаляр — только e. Так тест ограничивает обещание метода и не смешивает Rune с графемой. Время получения первого скаляра O(1), результат до двух char.

**Частая ошибка**

Назвать Rune видимым знаком и ожидать, что он включает следующий комбинируемый акцент.

Полный исходник: [solutions/Task11.cs](solutions/Task11.cs).

```csharp
using System;
using Course;
using System.Text;

namespace Course.Lesson05;

public static class Task11
{
    public static string FirstScalar(string text)
    {
        foreach (var rune in text.EnumerateRunes()) return rune.ToString();
        return "";
    }
    public static string Wrong(string text)
    {
        return text.Length == 0 ? "" : text.Substring(0, 1);
    }

    public static void Run()
    {
        Check.Equal("", FirstScalar(""));
        Check.Equal("a", FirstScalar("abc"));
        Check.Equal("😀", FirstScalar("😀x"));
        Check.Equal("e", FirstScalar("e\u0301"));
        string broken = Wrong("😀x");
        Check.Equal(1, broken.Length);
        Check.True(char.IsHighSurrogate(broken[0]));
        Check.True(broken != FirstScalar("😀x"));
    }
}
```

**Исполняемые проверки**

- Пустота
- BMP-буква
- Эмодзи вне BMP
- Комбинируемый акцент
- Wrong реально возвращает половину пары

<a id="05-12"></a>
## 05-12. Префикс из целых текстовых элементов

Ограничить количество минимумом limit и LengthInTextElements, затем выделить префикс по элементам.

**Ход решения**

Rune учитывает суррогатную пару, но комбинируемый акцент всё ещё остаётся отдельным скаляром. StringInfo группирует последовательность в текстовые элементы по Unicode-правилам среды выполнения. Поэтому начинаем с определения числа элементов, а не Length. Для нулевого количества возвращаем пустую строку явно. SubstringByTextElements получает начальный номер элемента и их количество; API самостоятельно переводит это в границы UTF-16. Проверяем акцент, один эмодзи вне BMP, семейную ZWJ-последовательность, превышающий лимит и ноль. Правила Unicode версионируются; тестовые ожидания курса привязаны к .NET 10.

**Частая ошибка**

Передать число элементов в обычный Substring или объявить графемную длину экранной шириной.

Полный исходник: [solutions/Task12.cs](solutions/Task12.cs).

```csharp
using System;
using Course;
using System.Globalization;

namespace Course.Lesson05;

public static class Task12
{
    public static string TakeElements(string text, int limit)
    {
        var information = new StringInfo(text);
        int count = Math.Min(limit, information.LengthInTextElements);
        if (count == 0) return "";
        return information.SubstringByTextElements(0, count);
    }

    public static void Run()
    {
        Check.Equal("", TakeElements("abc", 0));
        Check.Equal("", TakeElements("", 10));
        Check.Equal("abc", TakeElements("abc", 10));
        Check.Equal("e\u0301", TakeElements("e\u0301x", 1));
        Check.Equal("😀", TakeElements("😀x", 1));
        Check.Equal("👨‍👩‍👧‍👦", TakeElements("👨‍👩‍👧‍👦x", 1));
    }
}
```

**Исполняемые проверки**

- Нулевой лимит
- Пустой текст
- Лимит больше длины
- Комбинируемый акцент
- Эмодзи вне BMP
- Семейная ZWJ-последовательность

<a id="05-13"></a>
## 05-13. Каноническое равенство меток

Привести обе записи к канонической форме композиции и сравнить их точно.

**Ход решения**

Один текстовый знак может иметь предсоставленную запись é или последовательность e+комбинируемый акцент. Ordinal без нормализации видит разные кодовые единицы; FormC приводит канонически эквивалентные записи к общему представлению. Нормализация не должна превращать задачу в поиск похожих символов: окружённая цифра ① и обычная 1 не канонически эквивалентны, поэтому остаются разными. FormKC решила бы другой, более широкий контракт. Регистр и пробелы по условию значимы. Тесты отделяют все эти правила, а не только один успешный акцент. Время и память зависят от длины нормализованных строк.

**Частая ошибка**

Подменить каноническое равенство совместимостью FormKC или визуальным сходством.

Полный исходник: [solutions/Task13.cs](solutions/Task13.cs).

```csharp
using System;
using Course;
using System.Text;

namespace Course.Lesson05;

public static class Task13
{
    public static bool SameLabel(string first, string second)
    {
        string normalizedFirst = first.Normalize(NormalizationForm.FormC);
        string normalizedSecond = second.Normalize(NormalizationForm.FormC);
        return string.Equals(normalizedFirst, normalizedSecond, StringComparison.Ordinal);
    }

    public static void Run()
    {
        Check.True(SameLabel("é", "e\u0301"));
        Check.Equal(false, SameLabel("A", "a"));
        Check.Equal(false, SameLabel("a", "a "));
        Check.Equal(false, SameLabel("①", "1"));
        Check.True(SameLabel("", ""));
    }
}
```

**Исполняемые проверки**

- Предсоставленный и комбинируемый акцент
- Различие регистра
- Различие пробелов
- Совместимый знак не приравнен
- Пустые строки

<a id="05-14"></a>
## 05-14. Число в явно выбранной культуре

Передать CultureInfo.GetCultureInfo(cultureName) в TryParse, проверить диапазон во временной переменной.

**Ход решения**

Десятичная запятая может быть пользовательским форматом, но это должно определяться явной культурой. При en-US запятая не становится автоматически допустимой группировкой, потому что AllowThousands не включён. Простая замена символа не проверила бы другие правила и скрыла бы неверный исходный формат. Out остаётся нулём при синтаксическом и смысловом отказе. Проверяем обе культуры, обе формы разделителя, пробелы, запрещённую группировку, EOF и выход за диапазон. Заданный cultureName исключает необходимость обрабатывать неизвестную культуру в этом упражнении. Время O(длины текста).

**Частая ошибка**

Использовать NumberStyles.Number и принять чужой разделитель как группировку тысяч.

Полный исходник: [solutions/Task14.cs](solutions/Task14.cs).

```csharp
using System;
using Course;
using System.Globalization;

namespace Course.Lesson05;

public static class Task14
{
    public static bool TryAmount(string? text, string cultureName, out decimal amount)
    {
        amount = 0m;
        const NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite |
                                    NumberStyles.AllowTrailingWhite | NumberStyles.AllowDecimalPoint;
        if (!decimal.TryParse(text, styles, CultureInfo.GetCultureInfo(cultureName), out decimal parsed))
            return false;
        if (parsed < -1_000_000m || parsed > 1_000_000m) return false;
        amount = parsed;
        return true;
    }

    public static void Run()
    {
        Check.True(TryAmount("1,5", "ru-RU", out decimal value));
        Check.Equal(1.5m, value);
        Check.Equal(false, TryAmount("1,5", "en-US", out value));
        Check.Equal(0m, value);
        Check.True(TryAmount(" -1.5 ", "en-US", out value));
        Check.Equal(-1.5m, value);
        Check.Equal(false, TryAmount("1.5", "ru-RU", out value));
        Check.Equal(false, TryAmount("1,000", "en-US", out value));
        Check.Equal(false, TryAmount(null, "ru-RU", out value));
        Check.Equal(false, TryAmount("1000001", "en-US", out value));
        Check.Equal(0m, value);
    }
}
```

**Исполняемые проверки**

- Русская запятая
- Запятая отвергнута en-US
- Английская точка с пробелами
- Точка отвергнута ru-RU
- Группировка запрещена
- Null
- Выход за диапазон с out=0

<a id="05-15"></a>
## 05-15. Разбор присваивания с проверкой ключа

Сначала собрать локальные части и проверить ключ; переносить их в out после полного успеха.

**Ход решения**

IndexOf первого = сохраняет всё остальное как значение, поэтому a=b=c разбирается в ключ a и значение b=c. Trim удаляет только окружающие пробельные символы; внутренние пробелы в значении остаются. Грамматика ключа техническая и намеренно ASCII: похожая кириллическая буква не должна незаметно стать допустимой. Отдельный IsKey удобно испытывать независимо от срезов. Out назначаются после проверок, чтобы даже ошибка в конце ключа не оставляла частичного результата. Проверки охватывают пустое значение, знак равенства в значении, неверный первый char, длину 21 и EOF. Время O(n), память O(n) под строки.

**Частая ошибка**

Разделить по всем = и потерять часть значения; использовать char.IsLetter и принять иной алфавит.

Полный исходник: [solutions/Task15.cs](solutions/Task15.cs).

```csharp
using System;
using Course;

namespace Course.Lesson05;

public static class Task15
{
    public static bool IsKey(string text)
    {
        static bool Letter(char ch)
        {
            return (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z');
        }
        if (text.Length < 1 || text.Length > 20 || !Letter(text[0])) return false;
        for (int index = 1; index < text.Length; index++)
        {
            char ch = text[index];
            if (!Letter(ch) && !(ch >= '0' && ch <= '9') && ch != '_') return false;
        }
        return true;
    }
    public static bool TryAssignment(string? line, out string key, out string value)
    {
        key = "";
        value = "";
        if (line is null) return false;
        int separator = line.IndexOf('=');
        if (separator < 0) return false;
        string candidate = line.Substring(0, separator).Trim();
        if (!IsKey(candidate)) return false;
        string content = line.Substring(separator + 1).Trim();
        key = candidate;
        value = content;
        return true;
    }

    public static void Run()
    {
        Check.True(TryAssignment(" mode = a=b ", out string key, out string value));
        Check.Equal("mode", key);
        Check.Equal("a=b", value);
        Check.True(TryAssignment("A_2=", out key, out value));
        Check.Equal("A_2", key);
        Check.Equal("", value);
        Check.Equal(false, TryAssignment("2mode=x", out key, out value));
        Check.Equal("", key);
        Check.Equal("", value);
        Check.Equal(false, IsKey("Мode"));
        Check.True(IsKey(new string('a', 20)));
        Check.Equal(false, IsKey(new string('a', 21)));
        Check.Equal(false, TryAssignment("mode", out key, out value));
        Check.Equal(false, TryAssignment(null, out key, out value));
    }
}
```

**Исполняемые проверки**

- Первый разделитель сохраняет хвост
- Пустое значение
- Неверный первый char
- Кириллическая буква в ключе
- Длина ключа 20 и 21
- Нет разделителя и EOF
- Оба out очищены при отказе
