using System;
using System.IO;
using Course;

long sum = 0;
for (int i = 1; i <= 5; i++)
{
    // Before the addition: sum contains 1..i-1.
    sum += i;
}
Check.Equal(15L, sum);

int remainder = 1203;
int digitSum = 0;
do
{
    digitSum += remainder % 10;
    remainder /= 10;
} while (remainder > 0);
Check.Equal(6, digitSum);

int a = 48;
int b = 18;
while (b != 0)
{
    int rest = a % b;
    a = b;
    b = rest;
}
Check.Equal(6, a);

// StringReader is a supplied test source with the same ReadLine contract.
using var input = new StringReader("4\nbad\n-2\n");
long streamSum = 0;
while (true)
{
    string? line = input.ReadLine();
    if (line is null) break;
    if (!int.TryParse(line, out int number)) continue;
    streamSum += number;
}
Check.Equal(2L, streamSum);
Check.Equal<string?>(null, input.ReadLine());
Console.WriteLine($"Сумма 1..5: {sum}; сумма цифр: {digitSum}; НОД: {a}; поток: {streamSum}");
Console.WriteLine("Пример 03 выполнен; циклы завершаются на детерминированных данных.");
