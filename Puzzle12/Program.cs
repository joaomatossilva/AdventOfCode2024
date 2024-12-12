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

string[] lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
var maxX = lines[0].Length;
var maxY = lines.Length;
var variations = 0;
var guards = new char[] { 'v', '>', '<', '^' };

var (startX,startY, startGuard) = IndexOfGuard();
Console.WriteLine($"Starting guard... ({startX},{startY}) is {startGuard}");
var impostorX = 0;
var impostorY = 0;

for (; impostorX < maxX; impostorX++)
{
    for (impostorY = 0; impostorY < maxY; impostorY++)
    {
        var pos = GetCharAt(impostorX, impostorY);
        if (pos != '.')
        {
            continue;
        }

        Console.WriteLine($"try with impostor ({impostorX},{impostorY})");
        if (Execute())
        {
            Console.WriteLine($"impostor found ({impostorX},{impostorY})");
            variations++;
        }
    }
}


Console.WriteLine(variations);

bool Execute()
{
    int x = startX;
    int y = startY;
    char guard = startGuard;
    var posMap = new HashSet<(int, int, char)>();
    while(true)
    {
        (x, y, guard) = Step(x, y, guard);
        if (x == -1 || y == -1)
        {
            return false;
        }

        if (posMap.Contains((x, y, guard)))
        {
            return true;
        }

        posMap.Add((x, y, guard));
    }
}

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
    if (nextX >= maxX || nextY >= maxY || nextX < 0 || nextY < 0)
    {
        return (-1, -1, currentGuard);
    }

    var nextPosition = GetCharAt(nextX, nextY);
    if (!(nextX == impostorX && nextY == impostorY))
    {
        if (nextPosition == '.' || nextPosition == '^')
        {
            return (nextX, nextY, currentGuard);
        }
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
        throw new Exception($"Invalid Positions. ({x},{y})");
    }

    return lines[y][x];
}