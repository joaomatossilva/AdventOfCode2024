// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

string input = @"...............................s..................
..................s..............q.............p..
.....a............................................
........c......Y.......Q..........................
............................................4.....
........Y.........y............m..........4.......
......................Y...s..........S............
.........................................S........
...............N.............y....................
...........a.......y..................1...........
................................................S.
...c........k.............q....t............S.....
.............................qM...................
........a.........................................
..................................................
..................................................
..c..........k...Q..q....P........................
5.................Q...................8...........
......yc..........................................
........................E............4............
.........6........................u..p.....4......
.........5.............P..n......1.........N......
6..............................1.........J.t......
..6..................................3.u..t.....p.
....5...k..........................u..............
.......................E..................u....x..
..................E.................x.............
...k..................P.............3.............
...........0.....9.5...........E.........31e....N.
......0.................................N.........
.................CU.....................t....x....
......7....................e......................
....0..........K......C...........................
.....6....j......M............................J...
......K.................................p.........
.....9........................U...................
............................3....n................
.............K.........2.....C..................x.
....................P........UJ...................
.....0......X...C.........T..............U........
.......M.....8j....7.............2........Q.......
9...............K.................................
....e.....8.........................2.A.m.........
..e......8.........s...n..........................
.....................................T..nm........
...................X............2.........m......A
......................X..j....................T...
.........7..M......j.............T................
....9...7....................................A....
..........................................A.......";

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

            yield return (a.x, a.y);
            yield return (b.x, b.y);

            int i = 1;
            do
            {
                (int x, int y) node = (a.x - i * deltaX, a.y - i * deltaY);
                
                if(!CheckBound(node))
                    break;
                
                yield return node;
                i++;
            } while (true);
            
            i = 2;
            do
            {
                (int x, int y) node = (a.x + i * deltaX, a.y + i * deltaY);
                
                if(!CheckBound(node))
                    break;
                
                yield return node;
                i++;
            } while (true);
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