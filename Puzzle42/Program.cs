// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var numericKeyPad = new Dictionary<char, Position>()
{
    { '7', new Position(0, 0) },
    { '8', new Position(1, 0) },
    { '9', new Position(2, 0) },
    { '4', new Position(0, 1) },
    { '5', new Position(1, 1) },
    { '6', new Position(2, 1) },
    { '1', new Position(0, 2) },
    { '2', new Position(1, 2) },
    { '3', new Position(2, 2) },
    { '*', new Position(0, 3) },
    { '0', new Position(1, 3) },
    { 'A', new Position(2, 3) }
};

var directionalKeyPad = new Dictionary<char, Position>()
{
    { '*', new Position(0, 0) },
    { '^', new Position(1, 0) },
    { 'A', new Position(2, 0) },
    { '<', new Position(0, 1) },
    { 'v', new Position(1, 1) },
    { '>', new Position(2, 1) },
};

var costs = new Dictionary<(char, char), (string, long)>();

var cache = new Dictionary<string, (List<string>, long)>();
var cache2 = new Dictionary<(string, int), long>();

int numberOfRobots = 2;
int numberOfRobotsEx = 3;

var codes = File.ReadAllText("input.txt").Split(Environment.NewLine);
//var codes = new string[] { "029A" };

var robot = new Robot(numericKeyPad);
var directionRobots = new Robot[numberOfRobots];
for (int i = 0; i < numberOfRobots; i++)
{
    directionRobots[i] = new Robot(directionalKeyPad);
}

List<string> temp = new List<string>();

FillCosts(directionalKeyPad);

long complexity = 0;
foreach (var c in codes)
{
   //  Console.WriteLine();
   // Console.WriteLine(c);
   //
   var movesStr = DoCode1(c);

   var moves = DoCode2(movesStr, numberOfRobotsEx);
   
    var numeric = Number().Match(c);
    //var movesLenght = moves.Select(x => x.Length).Min();

    //temp.Reverse();
    // foreach (var t in temp)
    // {
    //     Console.Write(t);
    // }
    temp.Clear();
    
    Console.WriteLine();
    Console.WriteLine($"Code {c}: Moves {moves} * {numeric}");
    
    complexity +=moves * int.Parse(numeric.Value);
}
Console.WriteLine(complexity);



void FillCosts(Dictionary<char, Position> pad)
{
    var robot = new Robot(directionalKeyPad);
    foreach (var key in pad.Where(x => x.Key != '*'))
    {
        var pairs = pad.Where(x => x.Key != '*');
        foreach (var pair in pairs)
        {

            var codesTemp = robot.Press(pair.Key, key.Key)
                .Select(code =>
                {
                    IEnumerable<string> temp = [code];
                    for (int i = 0; i < 3; i++)
                    {
                        temp = robot.DoCode(temp);
                    }

                    return new
                    {
                        code,
                        lenght = temp.Select(x => x.Length).Min(),
                    };
                })
                .OrderBy(x => x.lenght);
            
            var codes = codesTemp.First();
            costs.Add((key.Key, pair.Key), (codes.code, codes.code.Length));
        }
    }
}

string DoCode1(string code)
{
    // Console.WriteLine();
    var presses = robot.DoCode(code)
    
        .Select(code =>
        {
            IEnumerable<string> temp = [code];
            foreach (var directionRobot in directionRobots)
            {
                // foreach (var pressesCode in presses)
                // {
                //     Console.WriteLine(pressesCode);
                // }
                temp = directionRobot.DoCode(temp);
            }

            return new
            {
                code,
                lenght = temp.Select(x => x.Length).Min(),
            };
        })
        .OrderBy(x => x.lenght);
    
    
    // foreach (var directionRobot in directionRobots)
    // {
    //     // foreach (var pressesCode in presses)
    //     // {
    //     //     Console.WriteLine(pressesCode);
    //     // }
    //     presses = directionRobot.DoCode(presses);
    // }
    
    return presses.Select(x => x.code).First();
}

long DoCode2(string code, int level)
{
    if (level == 0)
    {
        return code.Length;
    }

    if (cache2.TryGetValue((code, level), out var resultLevel))
    {
        return resultLevel;
    }
    
    if (cache.TryGetValue(code, out var result))
    {
        return result.Item1.Aggregate(0L, (i, s) => i + DoCode2(s, level - 1));
    }
    
    long cost = 0;
    char start = 'A';
    List<string> b = new List<string>();
    for (int i = 0; i < code.Length; i++)
    {
        char end = code[i];
        var a = CalculateCost(start, end, level - 1);
        b.Add( a.Item1);
        cost += a.Item2;
        start = end;
    }
    
    cache.TryAdd(code, (b, cost));
    cache2.TryAdd((code, level), cost);
    return cost;
}

(string, long) CalculateCost(char start, char end, int level)
{
    var node = costs[(start, end)];
    //Console.WriteLine($"{level} - {node.Item1}");
    
    if (level == 0)
    {
        //temp.Add(node.Item1);
        return (node.Item1, node.Item2);
    }
    
    return (node.Item1,DoCode2(node.Item1, level));
}


public class Robot(Dictionary<char, Position> pad)
{
    private Position _star = pad['*'];

    public IEnumerable<string> DoCode(IEnumerable<string> code)
    {
        foreach (var c in code)
        {
            foreach (var t in DoCode(c))
            {
                yield return t;
            }
        }
    }

    public IEnumerable<string> DoCode(string code)
    {
        var temp = DoCodePrv(code);
        return Program.EnumeratePars1(temp.Select(x => x.Item2));
    }
    
    private IEnumerable<(char, IEnumerable<string>)> DoCodePrv(string code)
    {
        var start = 'A';
        for (int a = 0; a < code.Length; a++)
        {
            var end = code[a];
            var alt = Press(end, start);

            yield return (end, alt);
            
            start = end;
        }
    }
    
    public IEnumerable<string> Press(char key, char startChar)
    {
        var finish = pad[key];
        var start = pad[startChar];
        var diffX = finish.X - start.X;
        var diffY = finish.Y - start.Y;

        if (diffX == 0 && diffY == 0)
        {
            yield return "A";
        }
        
        bool canMoveXFirst = diffX != 0;
        if (start.Y == _star.Y && finish.X == _star.X)
        {
            canMoveXFirst = false;
        }
         
        bool canMoveYFirst = diffY != 0;
        if (start.X == _star.X && finish.Y == _star.Y)
        { 
            canMoveYFirst = false;
        }

        if (canMoveXFirst)
        {
            yield return new string(MoveXFirst(diffX, diffY).ToArray());
        }
        if (canMoveYFirst)
        {
            yield return new string(MoveYFirst(diffX, diffY).ToArray());
        }
         
    }

    IEnumerable<char> MoveXFirst(int diffX, int diffY)
    {
        for (int i = 0; i < Math.Abs(diffX); i++)
        {
            yield return diffX > 0 ? '>' : '<';
        }
        for (int i = 0; i < Math.Abs(diffY); i++)
        {
            yield return  diffY > 0 ? 'v' : '^';
        }

        yield return 'A';
    }
    
    IEnumerable<char> MoveYFirst(int diffX, int diffY)
    {
        for (int i = 0; i < Math.Abs(diffY); i++)
        {
            yield return  diffY > 0 ? 'v' : '^';
        }
        for (int i = 0; i < Math.Abs(diffX); i++)
        {
            yield return diffX > 0 ? '>' : '<';
        }
        
        yield return 'A';
    }
}

public record Vector(int Vx, int Vy);

public record Position(int X, int Y)
{
    public Position Add(Vector v)
    {
        return new Position ( X + v.Vx, Y + v.Vy );
    }
}

public partial class Program
{
    [GeneratedRegex("\\d+")]
    public static partial Regex Number();
    
    public static IEnumerable<string> EnumeratePars1(IEnumerable<IEnumerable<string>> parts)
    {
        var d = new Dictionary<int, int>();
        var current = (IEnumerable<string>)new string[] { String.Empty };
        foreach (var part in parts)
        {
            current = EnumerateParts(current, part);
        }

        return current;
    }

    public static IEnumerable<string> EnumerateParts(IEnumerable<string> currents, IEnumerable<string> parts)
    {
        foreach (var current in currents)
        {
            foreach (var part in parts)
            {
                yield return current + part;
            }
        }
    }
}
