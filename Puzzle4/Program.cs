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

bool CheckSafety(List<int> level, int multiplier = 1)
{
    for (int i = 0; i < level.Count; i++)
    {
        if (CheckSafety2(level, i, multiplier))
        {
            return true;
        }
    }

    return false;
}

bool CheckSafety2(List<int> levels, int skipIndex, int multiplier)
{
    for (int i = 0; i < levels.Count - 2; i++)
    {
        var gap = (GetAtIndexAdjusted(levels, i, skipIndex) - GetAtIndexAdjusted(levels, i + 1, skipIndex)) * multiplier;
        if (gap > 0 && gap <= 3)
        {
            continue;
        }

        return false;
    }

    return true;
}

int GetAtIndexAdjusted(List<int> levels, int index, int skipIndex)
{
    var i = index;
    if (index >= skipIndex)
    {
        i++;
    }

    return levels[i];
}

Console.WriteLine(safeCount);

partial class Program
{
    [GeneratedRegex("\\D+")]
    private static partial Regex GetNonNumbers();
}