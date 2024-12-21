// See https://aka.ms/new-console-template for more information

string inputMap = File.ReadAllText("input.txt");

var directions = new Dictionary<char, Vector>
{
    { '^', new Vector(0, -1) },
    { '<', new Vector(-1, 0) },
    { 'v', new Vector(0, 1) },
    { '>', new Vector(1, 0) },
};


var map = inputMap.Split(Environment.NewLine);

var maxX = map[0].Length;
var maxY = map.Length;

var start = GetPosition('S')!;
var initialScore = new Score(0, 0, null, null);

var cheats = new Dictionary<(Position, Position), int>();
var queue = new Queue<(Position, char, Score)>();
var cache = new Dictionary<Position, Score>();
var minCheatForResult = 0; // 100;

var baseLine = FindExit(start!, initialScore, 0);
var baseLineScore = baseLine.First();
var costs = FindExit(start!, initialScore, baseLineScore.Value);

var solutions = cheats/*.Where(x => x.Value)*/
    .GroupBy(x => x.Value)
    .OrderBy(x => x.Key)
    .Select(x => new
    {
        Moves = x.Key,
        MovesCount = x.Count(),
    })
    .Where(x => x.Moves >= minCheatForResult)
    .ToList();
foreach (var score in solutions)
{
    Console.WriteLine($"Got {score.MovesCount} Solves with less {score.Moves} moves");
}
Console.WriteLine($"Part1: {solutions.Sum(x => x.MovesCount)}");

// Console.WriteLine();
// for (int y = 0; y < map.Length; y++)
// {
//     for (int x = 0; x < map[y].Length; x++)
//     {
//         if (positions.ContainsKey(new Position(x, y)))
//         {
//             Console.Write("O");
//         }
//         else
//         {
//             Console.Write(map[y][x]);
//         }
//     }
//     Console.WriteLine();
// }


IEnumerable<Score> FindExit(Position position1, Score score1, int baseLine = 0)
{
    var bestExistScores = new List<Score>();

    (Position position, char direction, Score score) node = (position1, '>', score1);
    do
    {
        if (baseLine > 0 && node.score.Value > baseLine)
        {
            continue;
        }

        if (map[node.position.Y][node.position.X] == 'E')
        {
            Console.WriteLine($"Got a path with score {node.score}");
            
            if (bestExistScores.Count == 0)
            {
                bestExistScores.Add(node.score);
                continue;
            }
            
            if(!cheats.TryGetValue((node.score.StartCheat, node.score.EndCheat), out _))
            {
                var cheatScore = node.score.Value - bestExistScores.First().Value;
                Console.WriteLine($"Got cheat to the end with {cheatScore}");
                cheats.Add((node.score.StartCheat, node.score.EndCheat), cheatScore);
            }

            bestExistScores.Add(node.score);
            
            continue;
        }
        
        if (cache.TryGetValue(node.position, out var scoreNode))
        {
            if (scoreNode.Value < node.score.Value)
            {
                continue;
            }

            if (node.score.Cheats == 0)
            {
                cache[node.position] = node.score;
            }
            else
            {
                if(!cheats.TryGetValue((node.score.StartCheat, node.score.EndCheat), out _))
                {
                    var cheatScore = scoreNode.Value - node.score.Value;
                    Console.WriteLine($"Got an expected cheat with {cheatScore}");
                    cheats.Add((node.score.StartCheat, node.score.EndCheat), cheatScore);
                    
                    continue;
                }
            }
            
        }
        else
        {
            if (node.score.Cheats == 0)
            {
                cache.Add(node.position, node.score);
            }
            else
            {
                Console.WriteLine($"Got an unexpected cheat with {node.score}");
            }
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
               
                if (newPos.X < 1 || newPos.X >= maxX - 1 || newPos.Y < 1 || newPos.Y >= maxY - 1)
                {
                    continue;
                }
                
                if (baseLine > 0 && node.score.Cheats == 0)
                {
                    //double step
                    var endCheatPos = newPos.Add(nextDirection.Value);
                    if (map[endCheatPos.Y][endCheatPos.X] == '#') // endCheat must be track
                    {
                        continue;
                    }
                    
                    var nextScoreWithCheat = node.score.Add(1, 1, newPos, endCheatPos)
                        .Add(1, 1, newPos, endCheatPos);
                    queue.Enqueue((endCheatPos, nextDirection.Key, nextScoreWithCheat));
                }
                
                continue;
            }
        
            var nextScore = node.score.Add(1, 0, node.score.StartCheat, node.score.EndCheat);
            queue.Enqueue((newPos, nextDirection.Key, nextScore));
        }
        
    }while(queue.TryDequeue(out node));
    
    return bestExistScores;
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

public record Score(int Value, int Cheats, Position StartCheat, Position EndCheat)
{
    public Score Add(int cost, int cheats, Position startCheat, Position endCheat)
    {
        return new Score(Value + cost, Cheats + cheats, startCheat, endCheat);
    }
}