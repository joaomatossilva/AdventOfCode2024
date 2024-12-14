// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

int maxX = 11;//101;
int maxY = 7; //103;

maxX = 101;
maxY = 103;

string input = @"p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3";

var robots = new List<Robot>();
foreach (var line in input.Split(Environment.NewLine))
{
    var match = Robot().Match(line);
    if (match.Success)
    {
        var robot = new Robot
        {
            X = int.Parse(match.Groups[1].Value),
            Y = int.Parse(match.Groups[2].Value),
            Vx = int.Parse(match.Groups[3].Value),
            Vy = int.Parse(match.Groups[4].Value),
        };
        robots.Add(robot);
    }
}

Console.WriteLine();
Print();

int seconds = 0;
do
{
    seconds++;
    foreach (var robot in robots)
    {
        Move(robot);
    }

    if (AreDiagonal())
    {
        Console.WriteLine(seconds);
        Print();
        Console.WriteLine();
        Console.ReadLine();
    }
}while (true);


void Move(Robot robot1)
{
    robot1.X += robot1.Vx;
    if (robot1.X >= maxX)
    {
        robot1.X %= maxX;
    }

    if (robot1.X < 0)
    {
        robot1.X += maxX;
    }
    
    robot1.Y += robot1.Vy;
    if (robot1.Y >= maxY)
    {
        robot1.Y %= maxY;
    }

    if (robot1.Y < 0)
    {
        robot1.Y += maxY;
    }
}

bool AreDiagonal()
{
    var robotHashes = robots.Select(x => (x.X, x.Y)).ToHashSet();
    foreach (var robot in robots)
    {
        var max = new [] { (1,1), (1, -1), (-1, 1), (-1, 1)}.Select(x => InLine((robot.X, robot.Y), x)).Max();
        if (max > 5)
            return true;
    }

    return false;
    int InLine((int x, int y) robot, (int, int) direction)
    {
        var newPos = (robot.x + direction.Item1, robot.y + direction.Item2);
        if (robotHashes.Contains(newPos))
        {
            return InLine(newPos, direction) + 1;
        }

        return 0;
    }
}

void Print()
{
    for (int y = 0; y < maxY; y++)
    {
        for (int x = 0; x < maxX; x++)
        {
            int count = 0;
            foreach (var robot in robots)
            {
                if (robot.X == x && robot.Y == y)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                Console.Write(".");
                continue;
            }

            Console.Write(count);
        }
        Console.WriteLine();
    }
}

class Robot
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public int Vx { get; set; }
    public int Vy { get; set; }
}

partial class Program
{
    [GeneratedRegex("p=(\\d+),(\\d+) v=(-?\\d+),(-?\\d+)")]
    public static partial Regex Robot();
}