// See https://aka.ms/new-console-template for more information

int maxX = 7; //71;
int maxY = 7; //71;

var directions = new Dictionary<char, Vector>
{
    { '^', new Vector(0, -1) },
    { '<', new Vector(-1, 0) },
    { 'v', new Vector(0, 1) },
    { '>', new Vector(1, 0) },
};

var start = new Position(0, 0);
var exit = new Position(maxX-1, maxY-1);
var initialScore = new Score(0, new List<KeyValuePair<char, Vector>>());

var allObstacles = File.ReadAllLines("input.txt")
    .Select(x => x.Split(","))
    .Select(x => new Position(int.Parse(x[0]), int.Parse(x[1])))
    .ToArray();

Score? lastScore = null;
HashSet<Position> lastPositions = new HashSet<Position>();

int startObstacles = 12; //1024; // we know from part 1 1024 is still possible
do
{
    //Are we putting an obstacle on last found path? If not, no need to recalculate
    if (lastScore != null)
    {
        var lastObstacle = allObstacles[startObstacles -1];
        if (!lastPositions.Contains(lastObstacle))
        {
            startObstacles++;
            continue;
        }
    }
    
    var obstacles = allObstacles
        .Take(startObstacles)
        .ToHashSet();
    
    var queue = new Queue<(Position, Score)>();
    var cache = new Dictionary<Position, Score>();
    var costs = FindExit(start!, '>', initialScore, obstacles, queue, cache);

    if (!costs.Any())
    {
        break;
    }
    
    //Update last found score cache and value so we can skip new obstacles not in the way
    var thisScore = costs.First();
    if (lastScore == null || lastScore.Value != thisScore.Value)
    {
        lastScore = thisScore;
        lastPositions.Clear();
        var lastPosition = start;
        foreach (var move in thisScore.Moves!)
        {
            lastPosition = lastPosition.Add(move.Value);
            lastPositions.Add(lastPosition);
        }
    }

    startObstacles++;
} while (startObstacles < 5000);


Console.WriteLine();
Console.WriteLine($"Part2: {allObstacles[startObstacles-1]} - {startObstacles}");

IEnumerable<Score> FindExit(Position position1, char direction1, Score score1, HashSet<Position> obstacles, Queue<(Position, Score)> queue, Dictionary<Position, Score> cache)
{
    var bestExistScores = new List<Score>();

    (Position position, Score score) node = (position1, score1);
    do
    {
        if (node.position == exit)
        {
            if (bestExistScores.Count == 0)
            {
                bestExistScores.Add(node.score);
                continue;
            }

            if (bestExistScores.First().Value > node.score.Value)
            {
                bestExistScores.Clear();
                bestExistScores.Add(node.score);
            }

            continue;
        }
        
        if (cache.TryGetValue(node.position, out var scoreNode))
        {
            if (scoreNode.Value <= node.score.Value)
            {
                continue;
            }
            
            cache[node.position] = node.score;
        }
        else
        {
            cache.Add(node.position, node.score);
        }
        
        
        foreach (var nextDirection in directions)
        {
            if (node.score?.Moves?.Count > 0 && nextDirection.Key == GetOppositeDirection(node.score?.Moves?.Last().Key ?? direction1))
            {
                continue;
            }
        
            var newPos = node.position.Add(nextDirection.Value);
            if (obstacles.Contains(newPos))
            {
                continue;
            }

            if (newPos.X < 0 || newPos.X >= maxX || newPos.Y < 0 || newPos.Y >= maxY)
            {
                continue;
            }
        
            var nextScore = node.score.Add(1, nextDirection);
            queue.Enqueue((newPos, nextScore));
        }
        
        
        
    }while(queue.TryDequeue(out node));


    return bestExistScores;
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