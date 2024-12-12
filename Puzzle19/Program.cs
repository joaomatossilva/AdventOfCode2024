// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

string input = @"0123
1234
8765
9876";

string[] lines = input.Split(Environment.NewLine);
var maxX = lines[0].Length;
var maxY = lines.Length;

long accumulator = 0;

for (var y = 0; y < lines.Length; y++)
{
    for (int x = 0; x < maxX; x++)
    {
        var heigth = lines[y][x];
        if (heigth == '0')
        {
            accumulator += GetScore(x, y);
        }
    }
}

Console.WriteLine(accumulator);

int GetScore(int x, int y, int height = 0, HashSet<(int, int)> visited = null)
{
    if (visited == null)
    {
        visited = new HashSet<(int, int)>();
    }
    
    if (height == 9)
    {
        if (visited.Contains((x, y)))
        {
            return 0;
        }
        
        visited.Add((x, y));
        return 1;
    }
    
    var next = height + 1;
    var score = GetNeighbors(x, y)
        .Where(pos => IsInBounds(pos.x, pos.y))
        .Where(pos => lines[pos.y][pos.x] == '0' + next)
        .Sum(pos => GetScore(pos.x, pos.y, next, visited));

    return score;
}

IEnumerable<(int x, int y)> GetNeighbors(int x, int y)
{
    yield return (x - 1, y);
    yield return (x, y - 1);
    yield return (x, y + 1);
    yield return (x + 1, y);
}

bool IsInBounds(int x, int y)
{
    return 0 <= x && x < maxX && 0 <= y && y < maxY;
}