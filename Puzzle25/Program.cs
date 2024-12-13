// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

string input = @"Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=8400, Y=5400

Button A: X+26, Y+66
Button B: X+67, Y+21
Prize: X=12748, Y=12176

Button A: X+17, Y+86
Button B: X+84, Y+37
Prize: X=7870, Y=6450

Button A: X+69, Y+23
Button B: X+27, Y+71
Prize: X=18641, Y=10279";

var prizes = new List<Prize>();
var buttons = new List<Button>();
foreach (string line in input.Split(Environment.NewLine))
{
    
    if (line.Contains("Button"))
    {
        var match = Button().Match(line);
        if (match.Success)
        {
            var button = new Button(
                match.Groups[1].Value[0],
                int.Parse(match.Groups[2].Value),
                int.Parse(match.Groups[3].Value));
            buttons.Add(button);
        }
    }

    if (line.Contains("Prize"))
    {
        var match = Prize().Match(line);
        if (match.Success)
        {
            var prize = new Prize(
                int.Parse(match.Groups[1].Value),
                int.Parse(match.Groups[2].Value),
                buttons.ToArray());
            prizes.Add(prize);
            buttons.Clear();
        }
    }
}

int prizesCount = 0;
int totalCost = 0;
foreach (var prize in prizes)
{
    (int a, int b) = CalculatePresses(prize);
    if (a == 0 && b == 0)
    {
        continue;
    }

    var cost = a * 3 + b;
    prizesCount++;
    totalCost += cost;
    Console.WriteLine($"a: {a}, b: {b} = {cost}");
}

Console.WriteLine($"Total prizes: {prizesCount}, Total cost: {totalCost}");

(int a, int b) CalculatePresses(Prize prize)
{
    for (int a = 0; a < 100; a++)
    {
        for (int b = 0; b < 100; b++)
        {
            var deltaX = prize.Buttons[0].X * a;
            var deltaY = prize.Buttons[0].Y * a;
            
            deltaX += prize.Buttons[1].X * b;
            deltaY += prize.Buttons[1].Y * b;

            if (deltaX > prize.X || deltaY > prize.Y)
            {
                break;
            }

            if (deltaX == prize.X && deltaY == prize.Y)
            {
                return (a, b);
            }
        }
    }
    
    return (0, 0);
}

record Button(char Name, int X, int Y);
record Prize(int X, int Y, Button[] Buttons);

partial class Program
{
    [GeneratedRegex("Button (A|B): X\\+(\\d+), Y\\+(\\d+)")]
    public static partial Regex Button();
    
    [GeneratedRegex("Prize: X=(\\d+), Y=(\\d+)")]
    public static partial Regex Prize();
}