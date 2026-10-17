using System;
using System.Globalization;
using Course;

// Strings model the result of Console.ReadLine; there is no keyboard wait.
string? text = " 42 ";
string outcome;
if (text is null) outcome = "eof";
else if (!int.TryParse(text, out int value)) outcome = "invalid";
else if (value < 0 || value > 100) outcome = "range";
else outcome = $"ok:{value}";
Check.Equal("ok:42", outcome);
Console.WriteLine(outcome);

string? endOfInput = null;
string emptyLine = "";
Check.True(endOfInput is null);
Check.Equal(0, emptyLine.Length);
Check.Equal(false, int.TryParse("3.5", out _));
Check.True(int.TryParse("-3", out int negative));
Check.True(negative < 0); // Format is valid; the count would be invalid.

int dividend = 12;
int divisor = 0;
bool divides = divisor != 0 && dividend % divisor == 0;
Check.Equal(false, divides);

bool active = false;
bool owner = false;
bool hasPass = true;
bool blocked = false;
bool mayOpen = active && (owner || hasPass) && !blocked;
Check.Equal(false, mayOpen);

int code = 1;
string direction;
switch (code)
{
    case 0: direction = "north"; break;
    case 1: direction = "east"; break;
    default: direction = "invalid"; break;
}
Check.Equal("east", direction);

const NumberStyles style = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;
Check.True(decimal.TryParse("1.5", style, CultureInfo.InvariantCulture, out decimal amount));
Check.Equal(1.5m, amount);
Check.Equal(false, decimal.TryParse("1,5", style, CultureInfo.InvariantCulture, out _));
Console.WriteLine("Пример 02 выполнен; EOF, формат и ветвления проверены.");
