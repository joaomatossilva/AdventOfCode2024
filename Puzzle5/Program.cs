// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

string intput = @"xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";

var matches = GetNonNumbers().Matches(intput);

var accumulated = 0;
foreach (Match match in matches)
{
    if (match.Success)
    {
        Console.WriteLine($"found {match.Value} with  {match.Groups[1].Value}: {match.Groups[2].Value}");
        accumulated += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
    }
}

Console.WriteLine(accumulated);

partial class Program
{
    [GeneratedRegex("mul\\((\\d{1,3}),(\\d{1,3})\\)")]
    private static partial Regex GetNonNumbers();
}