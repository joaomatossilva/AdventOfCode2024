// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

string input = @"3   4
4   3
2   5
1   3
3   9
3   3";

string[] lines = input.Split('\n');
var left = new List<int>();
var right = new List<int>();
foreach (string line in lines)
{
    var numbers = GetNumbers().Match(line);
    if (!numbers.Success)
    {
        continue;
    }
    left.Add(int.Parse(numbers.Groups[1].Value));
    right.Add(int.Parse(numbers.Groups[2].Value));
}

Console.WriteLine($"left: {string.Join(",", left.Select(x => x.ToString()).ToArray())}");
Console.WriteLine($"right: {string.Join(",", left.Select(x => x.ToString()).ToArray())}");

int GetSimilarity(ICollection<int> left, ICollection<int> right)
{
  
    var accumulatedSimilarity = 0;
    foreach (var leftNumber in left)
    {
        var appear = right.Count(x => x == leftNumber);
        accumulatedSimilarity += leftNumber * appear;
    }

    return accumulatedSimilarity;
}

Console.WriteLine(GetSimilarity(left, right));

partial class Program
{
    [GeneratedRegex("([0-9]+)\\s+([0-9]+)")]
    private static partial Regex GetNumbers();
}