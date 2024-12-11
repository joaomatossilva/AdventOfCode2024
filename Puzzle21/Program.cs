// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text.RegularExpressions;

string input = @"872027 227 18 9760 0 4 67716 9245696";

var numbers = GetNumbers().Matches(input);
int stoneIndex = numbers.Count - 1;

Stone stoneTail = new Stone { Number = long.Parse(numbers[stoneIndex].Value)};
Stone stoneHead = stoneTail;
int count = numbers.Count;
var stopwatch = Stopwatch.StartNew();

while (stoneIndex > 0)
{
    stoneIndex--;
    stoneHead = new Stone { Number = long.Parse(numbers[stoneIndex].Value), Next = stoneHead};
}

Print(-1);
for (int i = 0; i < 25; i++)
{
    Blink();
    Print(i);
}

void Blink()
{
    var stone = stoneHead;
    do
    {
        Process();
        stone = stone.Next;
    } while (stone != null);
    
    void Process()
    {
        if (stone.Number == 0)
        {
            stone.Number = 1;
            return;
        }

        var fullLength = GetLenght(stone.Number);
        if (fullLength % 2 == 0)
        {
            var lenght = fullLength / 2;

            var number = stone.Number;
            var firstPart = number / (long)Math.Pow(10, lenght);
            stone.Number = firstPart; //  first stone number
            
            var secondPart = number % (long)Math.Pow(10, lenght);
            var newStone = new Stone { Number = secondPart, Next = stone.Next };
            stone.Next = newStone;
            stone = newStone;

            count++;
            
            return;
        }
        
        stone.Number *= 2024;
    }
}

void Print(int i)
{
    Console.WriteLine("");
    Console.WriteLine($"{i}=={count}== {stopwatch.Elapsed}");
    // var stone = stoneHead;
    // while (stone != null)
    // {
    //     Console.Write($" {stone.Number} ");
    //     
    //     stone = stone.Next;
    // }
}

int GetLenght(long number)
{
    for (int i = 15; i >= 0; i--)
    {
        if (number / (long)Math.Pow(10, i) > 0)
        {
            return i + 1;
        }
    }
    
    throw new Exception();
}

partial class Program
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex GetNumbers();
}

class Stone
{

    public long Number { get; set; }
   
    public Stone? Next { get; set; }
};