// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

string input = @"............
........0...
.....0......
.......0....
....0.......
......A.....
............
............
........A...
.........A..
............
............";

string[] lines = input.Split(Environment.NewLine);
var maxX = lines[0].Length;
var maxY = lines.Length;
long accumulator = 0;

var allFrequencies = GetFrequencies().Matches(input.ReplaceLineEndings("")).Select(x => new Frquency(
        x.Index % maxX,
        x.Index / maxX,
        x.Value[0]))
    .ToList();

HashSet<(int, int)> antinodes = new HashSet<(int, int)>();

foreach (var uniqueFrquency in allFrequencies.DistinctBy(x => x.name))
{
    var pairs = allFrequencies.Where(x => x.name == uniqueFrquency.name)
        .Select(x => allFrequencies.Where(y => y.name == x.name && y != x).Select(z => (x, z)))
        .SelectMany(x => x);

    foreach ((Frquency a, Frquency b) in pairs)
    {
        var pairingNodes = GetAntiNodes(a, b);

        foreach (var antiNode in pairingNodes)
        {
            Console.WriteLine($"for {a} and {b} got {antiNode}");

            antinodes.Add(antiNode);
        }
        
        IEnumerable<(int, int)> GetAntiNodes(Frquency a, Frquency b)
        {
            var deltaX = (b.x - a.x);
            var deltaY = (b.y - a.y);

            (int x, int y) first = (a.x - deltaX, a.y - deltaY);
            if (CheckBound(first))
            {
                yield return (first.x, first.y);
            }

            (int x, int y) second = (b.x + deltaX, b.y + deltaY);
            if (CheckBound(second))
            {
                yield return (second.x, second.y);
            }
            bool CheckBound((int x, int y) node)
            {
                if (node.x >= 0 && node.x < maxX && node.y >= 0 && node.y < maxY)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
Console.WriteLine($"Answer: {antinodes.Count}");

for (int y = 0; y < maxY; y++)
{
    for (int x = 0; x < maxX; x++)
    {
        Console.Write(antinodes.Contains((x,y)) ? "#" : lines[y][x].ToString());
    }
    Console.Write(Environment.NewLine);
}


record Frquency(int x, int y, char name);

partial class Program
{
    [GeneratedRegex("([a-zA-Z0-9]{1})")]
    private static partial Regex GetFrequencies();
}