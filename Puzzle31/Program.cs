// See https://aka.ms/new-console-template for more information

// string @inputMap = @"#####
// #..E#
// #...#
// #S..#
// #####";

string inputMap = @"#################
#...#...#...#..E#
#.#.#.#.#.#.#.#.#
#.#.#.#...#...#.#
#.#.#.#.###.#.#.#
#...#.#.#.....#.#
#.#.#.#.#.#####.#
#.#...#.#.#.....#
#.#.#####.#.###.#
#.#.#.......#...#
#.#.###.#####.###
#.#.#...#.....#.#
#.#.#.#####.###.#
#.#.#.........#.#
#.#.#.#########.#
#S#.............#
#################";

var directions = new Dictionary<char, Vector>
{
    { '^', new Vector(0, -1) },
    { '<', new Vector(-1, 0) },
    { 'v', new Vector(0, 1) },
    { '>', new Vector(1, 0) },
};


var map = inputMap.Split(Environment.NewLine);

var start = GetPosition('S');
var initialScore = new Score(0, new List<KeyValuePair<char, Vector>>());

var queue = new Queue<(Position, char, Score)>();
var cache = new Dictionary<(Position, char), Score>();

var cost = FindExit(start!, '>', initialScore);


var listOfMoves = cost.Moves!;

var positions = new Dictionary<Position, char>();
var lastPosition = start;
foreach (var move in listOfMoves)
{
    lastPosition = lastPosition.Add(move.Value);
    positions.TryAdd(lastPosition, move.Key);
}

Console.WriteLine();
for (int y = 0; y < map.Length; y++)
{
    for (int x = 0; x < map[y].Length; x++)
    {
        if (positions.ContainsKey(new Position(x, y)))
        {
            Console.Write(positions[new Position(x, y)]);
        }
        else
        {
            Console.Write(map[y][x]);
        }
    }
    Console.WriteLine();
}

Console.WriteLine($"Part1: {cost}");

Score FindExit(Position position1, char direction1, Score score1)
{
    Score? bestExistScore = null;

    (Position position, char direction, Score score) node = (position1, direction1, score1);
    do
    {
        if (cache.TryGetValue((node.position, node.direction), out var scoreNode))
        {
            if (scoreNode.Value <= node.score.Value)
            {
                continue;
            }
            
            cache[(node.position, node.direction)] = node.score;
        }
        else
        {
            cache.Add((node.position, node.direction), node.score);
        }
        
        if (map[node.position.Y][node.position.X] == 'E')
        {
            Console.Write($"Got a path with score {node.score}");
            
            if (bestExistScore == null || bestExistScore.Value >= node.score.Value)
            {
                bestExistScore = node.score;
                continue;
            }
            
            continue;
        }
        
        
        foreach (var nextDirection in directions)
        {
            if (nextDirection.Key == GetOppositeDirection(node.direction))
            {
                continue;
            }
        
            var newPos = node.position.Add(nextDirection.Value);
            if (map[newPos.Y][newPos.X] == '#')
            {
                continue;
            }
        
            var nextScore = node.score.Add((nextDirection.Key == node.direction ? 1 : 1001), nextDirection);
            queue.Enqueue((newPos, nextDirection.Key, nextScore));
        }
        
        
        
    }while(queue.TryDequeue(out node));


    return bestExistScore!;
}

Position? GetPosition(char c)
{
    for (int y = 0; y < map.Length; y++)
    for (int x = 0; x < map[y].Length; x++)
    {
        if (map[y][x] == c)
        {
            return new Position(x, y);
        }
    }
    return null;
}
char GetOppositeDirection(char direction)
{
    switch (direction)
    {
        case '>':
            return '<';
        case '<':
            return '>';
        case '^':
            return 'v';
        case 'v':
            return '^';
    }

    throw new ArgumentException();
}
public record Vector(int Vx, int Vy);

public record Position(int X, int Y)
{
    public Position Add(Vector v)
    {
        return new Position ( X + v.Vx, Y + v.Vy );
    }
}

public record Score(int Value, IList<KeyValuePair<char, Vector>>? Moves)
{
    public Score Add(int cost, KeyValuePair<char, Vector> move)
    {
        var newMoves = new List<KeyValuePair<char, Vector>>(Moves!) { move };
        return new Score(Value + cost, newMoves);
    }
}