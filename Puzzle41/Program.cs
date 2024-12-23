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


var initialNumericPosition = numericKeyPad['A'];
var initialDirectionalPosition = directionalKeyPad['A'];
int numberOfRobots = 2;

var codes = File.ReadAllText("input.txt").Split(Environment.NewLine);
//var codes = new string[] { "029A" };

var robot = new Robot(initialNumericPosition, numericKeyPad);
var directionRobots = new Robot[numberOfRobots];
for (int i = 0; i < numberOfRobots; i++)
{
    directionRobots[i] = new Robot(initialDirectionalPosition, directionalKeyPad);
}


long complexity = 0;
foreach (var c in codes)
{
   
    var moves = DoCode(c)
        .OrderBy(x => x.Length).First();
    var numeric = Number().Match(c);
    var movesLenght = moves.Length;
    
    Console.WriteLine($"Code {c}: Moves {movesLenght} * {numeric} -- {moves}");
    
    complexity +=movesLenght * int.Parse(numeric.Value);
}
Console.WriteLine(complexity);




string[] DoCode(string code)
{
    Console.WriteLine();
    var presses = robot.DoCode(code);
    foreach (var directionRobot in directionRobots)
    {
        // foreach (var pressesCode in presses)
        // {
        //     Console.WriteLine(pressesCode);
        // }
        presses = directionRobot.DoCode(presses);
    }
    
    return presses.ToArray();
}


public class Robot(Position position, Dictionary<char, Position> pad, bool useLong = false)
{
    private Position _position = position;
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
