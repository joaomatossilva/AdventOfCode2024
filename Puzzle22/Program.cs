// See https://aka.ms/new-console-template for more information

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

string input = @"125 17";

var numbers = GetNumbers().Matches(input);
int stoneIndex = numbers.Count - 1;

int[] pows = new int [] {1, 10, 100, 1000, 10000, 100000, 1000000, 100000000, 1000000000};

Dictionary<long, long> stones = new Dictionary<long, long>();

for (int i = 0; i < numbers.Count; i++)
{
    stones.Add(long.Parse(numbers[i].Value), 1);
}

var stopwatch = Stopwatch.StartNew();

for (int i = 0; i < 75; i++)
{

    var blink = stones.Select(x => Process(x.Key, x.Value))
        .SelectMany(x => x)
        .GroupBy(x => x.Item1, x => x.Item2)
        .ToDictionary(x => x.Key, x => x.Sum());

    stones = blink;
    
    Console.WriteLine($"partial: {stones.Sum(x => x.Value)}");
}

var count = stones.Sum(x => x.Value);
Console.WriteLine($"Total stones: {count}");
Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");

IEnumerable<(long, long)> Process(long number, long numberCount)
{
    if (numberCount == 0)
    {
        yield break;
    }
    
    if (number == 0)
    {
        yield return (1, numberCount);
        yield break;
    }

    var fullLength = GetLenght(number);
    if (fullLength % 2 == 0)
    {
        var pow = GetPow(fullLength / 2);
        
        var first = number / pow;
        yield return (first, numberCount);
        
        var second = number % pow;
        yield return (second, numberCount);
        yield break;
    }


    var newNumber = number * 2024;
    yield return (newNumber, numberCount);
}

int GetLenght(long number)
{
    //avoiding converting to string
    
    if (number < 10)
    {
        return 1;
    }
    if (number < 100)
    {
        return 2;
    }
    if (number < 1000)
    {
        return 3;
    }
    if (number < 10000)
    {
        return 4;
    }
    if (number < 100000)
    {
        return 5;
    }
    if (number < 1000000)
    {
        return 6;
    }
    if (number < 10000000)
    {
        return 7;
    }
    if (number < 100000000)
    {
        return 8;
    }
    if (number < 1000000000)
    {
        return 9;
    }
    if (number < 10000000000)
    {
        return 10;
    }
    if (number < 100000000000)
    {
        return 11;
    }
    if (number < 1000000000000)
    {
        return 12;
    }
    if (number < 10000000000000)
    {
        return 13;
    }
    if (number < 100000000000000)
    {
        return 14;
    }
    if (number < 1000000000000000)
    {
        return 15;
    }
    
    throw new Exception();

}


int GetPow(int pow)
{
    //return pows[pow];
    return (int)Math.Pow(10, pow);
    // switch (pow)
    // {
    //     case 0:
    //         return 1;
    //     case 1:
    //         return 10;
    //     case 2:
    //         return 100;
    //     case 3:
    //         return 1000;
    //     case 4:
    //         return 10000;
    //     case 5:
    //         return 100000;
    //     case 6:
    //         return 1000000;
    //     case 7:
    //         return 10000000;
    //     case 8:
    //         return 100000000;
    //     case 9:
    //         return 1000000000;
    //     // case 10:
    //     //     return 10000000000;
    //     // case 11:
    //     //     return 100000000000;
    //     // case 12:
    //     //     return 1000000000000;
    //     // case 13:
    //     //     return 10000000000000;
    //     // case 14:
    //     //     return 100000000000000;
    //     // case 15:
    //     //     return 1000000000000000;
    //     default:
    //         throw new Exception();
    // }
}

partial class Program
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex GetNumbers();
}

public static class Programex
{
    public static long GetOrDefault(this IDictionary<long, long> dictionary, long key)
    {
        return dictionary.TryGetValue(key, out var val) ? val : default;
    }
}
