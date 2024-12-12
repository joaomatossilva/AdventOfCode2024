// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

string input = @"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9";

string[] lines = input.Split('\n');
int safeCount = 0;
foreach (string line in lines)
{
    var level = new List<int>();
    var numbers = GetNonNumbers().Split(line);


    for (int i = 0; i < numbers.Length; i++)
    {
        if (numbers[i].Length == 0)
        {
            continue;
        }
        
        level.Add(int.Parse(numbers[i]));
    }

    Console.WriteLine($"level: {string.Join(",", level.Select(x => x.ToString()).ToArray())}");

    if (level.Count == 0)
    {
        continue;
    }
    
    if (CheckSafety(level) || CheckSafety(level, -1))
    {
        Console.WriteLine("safe");
        safeCount++;
    }
}


bool CheckSafety(List<int> levels, int multiplier = 1)
{
    for (int i = 1; i < levels.Count; i++)
    {
        var gap = (levels[i - 1] - levels[i]) * multiplier;
        if (gap <= 0 || gap > 3)
        {
            return false;
        }
    }

    return true;
}

Console.WriteLine(safeCount);

partial class Program
{
    [GeneratedRegex("\\D+")]
    private static partial Regex GetNonNumbers();
}