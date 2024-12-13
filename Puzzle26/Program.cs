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

long increment = 10000000000000;
//long increment = 0;

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
                long.Parse(match.Groups[2].Value),
                long.Parse(match.Groups[3].Value));
            buttons.Add(button);
        }
    }

    if (line.Contains("Prize"))
    {
        var match = Prize().Match(line);
        if (match.Success)
        {
            var prize = new Prize(
                long.Parse(match.Groups[1].Value) + increment,
                long.Parse(match.Groups[2].Value) + increment,
                buttons.ToArray());
            prizes.Add(prize);
            buttons.Clear();
        }
    }
}

long prizesCount = 0;
long totalCost = 0;
foreach (var prize in prizes)
{
    (long a, long b) = CalculatePresses(prize);
    if (a == 0 && b == 0)
    {
        Console.WriteLine("Not found");
        continue;
    }

    var cost = a * 3 + b;
    prizesCount++;
    totalCost += cost;
    Console.WriteLine($"a: {a}, b: {b} = {cost}");
}

Console.WriteLine($"Total prizes: {prizesCount}, Total cost: {totalCost}");

(long a, long b) CalculatePresses(Prize prize)
{
    //Deconstructed into equations system on paper to get the formula
    var aCount = (prize.Y * prize.Buttons[1].X - prize.X * prize.Buttons[1].Y) / (prize.Buttons[1].X * prize.Buttons[0].Y - prize.Buttons[1].Y * prize.Buttons[0].X);
    var bCount = (prize.X - prize.Buttons[0].X * aCount) / prize.Buttons[1].X;

    var deltaX = aCount * prize.Buttons[0].X + bCount * prize.Buttons[1].X;
    var deltaY = aCount * prize.Buttons[0].Y + bCount * prize.Buttons[1].Y;

    if (deltaX == prize.X && deltaY == prize.Y)
    {
        return (aCount, bCount);
    }
    
    // for (long aX = 0; aX < prize.Buttons[0].X * prize.Buttons[1].X; aX++)
    // {
    //     for (long bX = 0; bX < prize.Buttons[0].X * prize.Buttons[1].X; bX++)
    //     {
    //         var deltaX = prize.Buttons[0].X * aX + prize.Buttons[1].X;
    //
    //         if (deltaX == 0)
    //         {
    //             continue;
    //         }
    //         
    //         var reminderX = prize.X % deltaX;
    //         if (reminderX == 0)
    //         {
    //             var deltaY = prize.Buttons[0].Y * aX + prize.Buttons[1].Y;
    //             var reminderY = prize.Y % deltaY;
    //             if (reminderY == 0)
    //             {
    //                 var factor = prize.X / deltaX;
    //             
    //                 Console.WriteLine($"with {factor}");
    //             
    //                 return (aX * factor, bX * factor);
    //             }
    //             
    //             //Console.WriteLine($"Found X but not Y");
    //         }
    //     }
    // }
    // return (0, 0);
    
    
    //
    //
    //
    // for (long a = 0; a < 100; a++)
    // {
    //     for (long b = 0; b < 100; b++)
    //     {
    //         var deltaX = prize.Buttons[0].X * a;
    //         var deltaY = prize.Buttons[0].Y * a;
    //         
    //         deltaX += prize.Buttons[1].X * b;
    //         deltaY += prize.Buttons[1].Y * b;
    //
    //         if (deltaX == 0 || deltaY == 0)
    //         {
    //             continue;
    //         }
    //
    //         var remainderX = prize.X % deltaX;
    //         var remainderY = prize.Y % deltaY;
    //         
    //         if (remainderX == 0 && remainderY == 0)
    //         {
    //             var factor = prize.X / deltaX;
    //             
    //             Console.WriteLine($"with {factor}");
    //             
    //             return (a * factor, b * factor);
    //         }
    //     }
    // }
    //
    return (0, 0);
}

record Button(char Name, long X, long Y);
record Prize(long X, long Y, Button[] Buttons);

partial class Program
{
    [GeneratedRegex("Button (A|B): X\\+(\\d+), Y\\+(\\d+)")]
    public static partial Regex Button();
    
    [GeneratedRegex("Prize: X=(\\d+), Y=(\\d+)")]
    public static partial Regex Prize();
}