using System;
using System.Globalization;
using System.Text;
using Course;

string original = "banana";
string copy = original;
copy = copy.Replace('a', 'o');
Check.Equal("banana", original);
Check.Equal("bonono", copy);
Check.True(string.Equals("StArT", "start", StringComparison.OrdinalIgnoreCase));

string emoji = "😀";
int scalars = 0;
foreach (var rune in emoji.EnumerateRunes())
{
    scalars++;
    Check.Equal("😀", rune.ToString());
}
Check.Equal(2, emoji.Length);
Check.Equal(1, scalars);
Check.True(char.IsHighSurrogate(emoji[0]));
Check.True(char.IsLowSurrogate(emoji[1]));

string accented = "e\u0301";
var accentInfo = new StringInfo(accented);
Check.Equal(2, accented.Length);
Check.Equal(1, accentInfo.LengthInTextElements);
Check.Equal(accented, accentInfo.SubstringByTextElements(0, 1));
Check.Equal("é", accented.Normalize(NormalizationForm.FormC));

string family = "👨‍👩‍👧‍👦";
int familyScalars = 0;
foreach (var rune in family.EnumerateRunes()) familyScalars++;
Check.Equal(11, family.Length);
Check.Equal(7, familyScalars);
Check.Equal(1, new StringInfo(family).LengthInTextElements);

const NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;
Check.True(decimal.TryParse("1,5", styles, CultureInfo.GetCultureInfo("ru-RU"), out decimal amount));
Check.Equal(1.5m, amount);
Check.Equal(false, decimal.TryParse("1,5", styles, CultureInfo.GetCultureInfo("en-US"), out _));
Console.WriteLine($"Эмодзи: UTF-16={emoji.Length}, скаляры={scalars}, элементы=1");
Console.WriteLine($"Семейная последовательность: UTF-16={family.Length}, скаляры={familyScalars}, элементы=1");
Console.WriteLine("Пример 05 выполнен; строки, культуры и Unicode-границы проверены.");
