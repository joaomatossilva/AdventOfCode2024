// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

string rulesString = @"47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13";

string pagesString = @"75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47";

var rules = new List<Rule>();

var rulesMatches = GetRules().Matches(rulesString);
for (int i = 0; i < rulesMatches.Count; i++)
{
    if (rulesMatches[i].Success)
    {
        rules.Add(new Rule(int.Parse(rulesMatches[i].Groups[1].Value), int.Parse(rulesMatches[i].Groups[2].Value)));
    }
}

var prints = pagesString.Split('\n').Select(x => x.Split(',').Select(int.Parse).ToArray());

var accumulator = 0;
foreach (var pagesToUpdate in prints)
{
    if (IsSatisfiedBy(rules, pagesToUpdate))
    {
        var middle = GetMiddleValue(pagesToUpdate);
        accumulator += pagesToUpdate[middle];
    }
}

Console.WriteLine(accumulator);

static bool IsSatisfiedBy(List<Rule> rules, int[] pagesToUpdate)
{
    if (pagesToUpdate[0] == 75)
    {
        int a = 0;
    }
    
    foreach (var rule in rules)
    {
        
        var firstIndex = IndexOf(pagesToUpdate, rule.First);
        var secondIndex = IndexOf(pagesToUpdate, rule.Second);

        //Console.WriteLine($"First: {firstIndex}, Second: {secondIndex}");
        
        if (secondIndex >= 0 && firstIndex >= 0)
        {
            if (secondIndex <= firstIndex)
            {
                return false;
            }
        }
    }
    
    Console.WriteLine($"Satified " + string.Join(", ", pagesToUpdate));
    
    return true;
}

static int IndexOf(int[] list, int value)
{
    for (int i = 0; i < list.Length; i++)
    {
        if (list[i] == value)
        {
            return i;
        }
    }

    return -1;
}

static int GetMiddleValue(int[] pagesToUpdate)
{
    return pagesToUpdate.Length / 2;
}

record Rule(int First, int Second);

partial class Program
{
    [GeneratedRegex("(\\d+)\\|(\\d+)")]
    private static partial Regex GetRules();
}