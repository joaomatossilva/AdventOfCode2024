// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

string input = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";

string[] lines = input.Split('\n');
var maxX = lines[0].Length;
var maxY = lines.Length;
var steps = 0;
var guards = new char[] { 'v', '>', '<', '^' };
var stepmap = new HashSet<(int, int)>();
var (x,y, guard) = IndexOfGuard();
Console.WriteLine($"Starting guard... ({x},{y}) is {guard}");

while(true)
{
    (x, y, guard) = Step(x, y, guard);
    if (x == -1 || y == -1)
    {
        break;
    }

    if (!stepmap.Contains((x, y)))
    {
        steps++;
        stepmap.Add((x, y));
    }
    
    Console.WriteLine($"Step {steps}... ({x},{y}) is {guard}");
}

Console.WriteLine(steps);

(int, int, char) IndexOfGuard()
{
    
    for (int i = 0; i < lines.Length; i++)
    {
        var index = lines[i].IndexOfAny(guards);
        if (index >= 0)
        {
            return (index, i, lines[i][index]);
        }
    }

    return (-1, -1, '.');
}

(int, int, char) Step(int x, int y, char currentGuard)
{
    var nextX = x;
    var nextY = y;
    switch (currentGuard)
    {
        case 'v':
            nextY++;
            break;
        case '>':
            nextX++;
            break;
        case '<':
            nextX--;
            break;
        case '^':
            nextY--;
            break;
        default:
            throw new Exception("Invalid guard.");
    }

    //check out of bounds
    if (nextX >= maxX || nextY >= maxY || x < 0 || y < 0)
    {
        return (-1, -1, currentGuard);
    }

    var nextPosition = GetCharAt(nextX, nextY);
    if (nextPosition == '.' || nextPosition == '^')
    {
        return (nextX, nextY, currentGuard);
    }

    //rotate if blocked
    var nextGuard = currentGuard;
    switch (currentGuard)
    {
        case 'v':
            nextGuard = '<';
            break;
        case '>':
            nextGuard = 'v';
            break;
        case '<':
            nextGuard = '^';
            break;
        case '^':
            nextGuard = '>';
            break;
        default:
            throw new Exception("Invalid guard.");
    }
    
    return Step(x, y, nextGuard);
}

char GetCharAt(int x, int y)
{
    if (x < 0 || y < 0 || y >= maxY || x >= maxX)
    {
        throw new Exception("Invalid Positions.");
    }

    return lines[y][x];
}