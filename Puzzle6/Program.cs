// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

string intput = @"xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";

var accumulated = 0;
var enabled = true;

    var regex = GetNonNumbers();
    var matches = regex.Matches(intput);

    foreach (Match match in matches)
    {
        if (!match.Success)
        {
            break;
        }
        
        Console.WriteLine($"found {match.Value} at {match.Index}");
        
        if (match.Value == "do()")
        {
            enabled = true;
        }
        else if (match.Value == "don't()")
        {
            enabled = false;
        }
        
        if (enabled && match.Value.StartsWith("mul"))
        {
            accumulated += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
        }
    }

Console.WriteLine(accumulated);

partial class Program
{
    [GeneratedRegex("mul\\((\\d{1,3}),(\\d{1,3})\\)|do\\(\\)|don't\\(\\)")]
    private static partial Regex GetNonNumbers();
}