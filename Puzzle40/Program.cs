string inputMap = File.ReadAllText("input.txt");
var map = inputMap.Split(Environment.NewLine);

int maxX = map[0].Length;
int maxY = map.Length;

var directions = new Dictionary<char, Vector>
{
    { '^', new Vector(0, -1) },
    { '<', new Vector(-1, 0) },
    { 'v', new Vector(0, 1) },
    { '>', new Vector(1, 0) },
};

int maxCheatLength = 20;
int minScoreCheat = 100;

var start = GetPosition('S');

var queue = new Queue<(Position, long)>();
var cache = new Dictionary<Position, long>();
var cheats = new Dictionary<(Position, Position), long>();
var cheatsQueue = new Queue<(Position, int)>();

//Fill up the cache with all circuit positions
FindExit(start!);

/*COMPLETE WASTE OF TIME!!!!!*/
// FindCheats();
//
// void FindCheats()
// {
//     foreach (var position in cache)
//     {
//         EnumerateLandings(position.Key, position.Value);
//     }
// }

var a = cache.Select(startPosition => new
    {
        startPosition, destinations = cache.Select(x => new
        {
            Distance = distance(startPosition.Key, x.Key),
            StartPosition = startPosition.Key,
            Destination = x.Key,
            Saved = x.Value - startPosition.Value - distance(startPosition.Key, x.Key)
        }).Where(x => x.Distance <= maxCheatLength && x.Saved >= minScoreCheat)
    })
    .SelectMany(x => x.destinations)
    .ToList();
    

int distance(Position p1, Position p2)
{
    return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
}

var part = a
    .OrderBy(x => x.Saved)
    .GroupBy(x => x.Saved)
    .Select(x => new { x.Key, Count = (long)x.Count() })
    .ToList();

foreach (var cheat in part)
{
    Console.WriteLine($"- There are {cheat.Count} cheats that save {cheat.Key} picoseconds");
}
Console.WriteLine($"There are {part.Sum(x => x.Count)} cheats in total");


/* COMPLETE WASTE OT TIME!!!!! */
// void EnumerateLandings(Position startPosition, long score)
// {
//     Dictionary<Position, long> backtrack = new Dictionary<Position, long>();
//     
//     (Position land, int cheatLength) node = (startPosition, 0);
//     do
//     {
//         foreach (var direction in directions)
//         {
//             var newPosition = node.land.Add(direction.Value);
//             var newCheatLength = node.cheatLength + 1;
//
//             if (newPosition.X < 0 || newPosition.X >= maxX || newPosition.Y < 0 || newPosition.Y >= maxY)
//             {
//                 continue;
//             }
//             
//             //check backtrack
//             if (backtrack.TryGetValue(newPosition, out long backtrackLength))
//             {
//                 if (backtrackLength <= newCheatLength)
//                 {
//                     continue;
//                 }
//             }
//             else
//             {
//                 backtrack.Add(newPosition, newCheatLength);
//             }
//
//             //found a path
//             if (cache.TryGetValue(newPosition, out var originalScore))
//             {
//                 var cheatedScore = score + newCheatLength;
//                 
//                 //our cheat was backwards
//                 if (originalScore <= cheatedScore)
//                 {
//                     continue;
//                 }
//
//                 if (newCheatLength > 1)
//                 {
//                     var newCheatedSkips = originalScore - cheatedScore;
//                     if (cheats.TryGetValue((startPosition, newPosition), out var cachedCheatSkips))
//                     {
//                         //we had already a cheat with same start and end position with more 
//                         if (cachedCheatSkips >= newCheatedSkips)
//                         {
//                             continue;
//                         }
//
//                         cheats[(startPosition, newPosition)] = newCheatedSkips;
//                     }
//                     else
//                     {
//                         cheats.Add((startPosition, newPosition), newCheatedSkips);
//                     }
//                 }
//             }
//             
//             if (newCheatLength < maxCheatLength)
//             {
//                 cheatsQueue.Enqueue((newPosition, newCheatLength));
//             }
//         }
//         
//         
//     }while (cheatsQueue.TryDequeue(out node));
// }




// //Print map
// Console.WriteLine();
// for (int y = 0; y < map.Length; y++)
// {
//     for (int x = 0; x < map[y].Length; x++)
//     {
//         if (cache.ContainsKey(new Position(x, y)))
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

//Console.WriteLine($"Part1: {cost}");

void FindExit(Position position1)
{
    Score? bestExistScore = null;

    (Position position, long score) node = (position1, 0);
    do
    {
        if (cache.TryGetValue(node.position, out var cachedScore))
        {
            if (cachedScore <= node.score)
            {
                continue;
            }
            
            cache[node.position] = node.score;
        }
        else
        {
            cache.Add(node.position, node.score);
        }
        
        if (map[node.position.Y][node.position.X] == 'E')
        {
            // Console.WriteLine($"Found Exit with score {node.score}");
            return;
        }
        
        
        foreach (var nextDirection in directions)
        {
            var newPos = node.position.Add(nextDirection.Value);

            if (newPos.X < 0 || newPos.X >= maxX || newPos.Y < 0 || newPos.Y >= maxY)
            {
                continue;
            }
            
            if (map[newPos.Y][newPos.X] == '#')
            {
                continue;
            }
        
            var nextScore = node.score +1;
            queue.Enqueue((newPos, nextScore));
        }
        
        
        
    }while(queue.TryDequeue(out node));
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


/***  EVEN MORE COMPLETE WASTE OF TIME (8 hours!!!! until a complete rewrite) **/
// // See https://aka.ms/new-console-template for more information
//
// using System.Drawing;
// using System.Text.RegularExpressions;
//
// string inputMap = File.ReadAllText("input.txt");
//
// var directions = new Dictionary<char, Vector>
// {
//     { '^', new Vector(0, -1) },
//     { '<', new Vector(-1, 0) },
//     { 'v', new Vector(0, 1) },
//     { '>', new Vector(1, 0) },
// };
//
//
// var map = inputMap.Split(Environment.NewLine);
//
// var maxX = map[0].Length;
// var maxY = map.Length;
//
// var start = GetPosition('S')!;
// var initialScore = new Score(0, 0, null, null);
//
// var cheats = new Dictionary<(Position, Position), int>();
// var queue = new Queue<(Position, char, Score)>();
// var cache = new Dictionary<Position, Score>();
// var cheatCache = new Dictionary<(Position, Position), Score>();
// var maxCheatDuration = 20;
// var minCheatForResult = 50;
//
// var baseLine = FindExit(start!, initialScore, 0);
// var baseLineScore = baseLine.First();
// var costs = FindExit(start!, initialScore, baseLineScore.Value);
//
// var solutions = cheats/*.Where(x => x.Value)*/
//     .GroupBy(x => x.Value)
//     .OrderBy(x => x.Key)
//     .Select(x => new
//     {
//         Moves = x.Key,
//         MovesCount = (long)x.Count(),
//     })
//     .Where(x => x.Moves >= minCheatForResult)
//     .ToList();
//
// var allOf = cheats.Where(x => x.Value == 76).ToArray();
//
// var filtered = cheats.Where(x => x.Value >= minCheatForResult).ToArray();
//
// foreach (var score in solutions)
// {
//     Console.WriteLine($"Got {score.MovesCount} Solves with less {score.Moves} moves");
// }
// Console.WriteLine($"Part1: {filtered.LongLength}");
//
// // Console.WriteLine();
// // for (int y = 0; y < map.Length; y++)
// // {
// //     for (int x = 0; x < map[y].Length; x++)
// //     {
// //         if (positions.ContainsKey(new Position(x, y)))
// //         {
// //             Console.Write("O");
// //         }
// //         else
// //         {
// //             Console.Write(map[y][x]);
// //         }
// //     }
// //     Console.WriteLine();
// // }
//
//
// IEnumerable<Score> FindExit(Position position1, Score score1, int baseLine = 0)
// {
//     var bestExistScores = new List<Score>();
//
//     (Position position, char direction, Score score) node = (position1, '>', score1);
//     do
//     {
//         if (baseLine > 0 && node.score.Value > baseLine)
//         {
//             continue;
//         }
//
//         if (map[node.position.Y][node.position.X] == 'E')
//         {
//             //Console.WriteLine($"Got a path with score {node.score}");
//             
//             if (bestExistScores.Count == 0)
//             {
//                 bestExistScores.Add(node.score);
//                 continue;
//             }
//             
//             if(node.score.Cheats > 0 && !cheats.TryGetValue((node.score.StartCheat, node.score.EndCheat), out _))
//             {
//                 if (node.score.StartCheat.X == 1 && node.score.StartCheat.Y == 8 && node.score.EndCheat.X == 5 &&
//                     node.score.EndCheat.Y == 7)
//                 {
//                     int b = 2;
//                 }
//                 
//                 var cheatScore = node.score.Value - baseLine;
//                 //Console.WriteLine($"Got cheat to the end with {cheatScore}");
//                 cheats.Add((node.score.StartCheat, node.score.EndCheat), cheatScore);
//             }
//
//             bestExistScores.Add(node.score);
//             
//             continue;
//         }
//         
//         if (cache.TryGetValue(node.position, out var scoreNode))
//         {
//             if (scoreNode.Value < node.score.Value)
//             {
//                 continue;
//             }
//
//             if (node.score.Cheats == 0)
//             {
//                 cache[node.position] = node.score;
//             }
//             else
//             {
//                 if(!cheats.TryGetValue((node.score.StartCheat, node.score.EndCheat), out var currentCheatScore))
//                 {
//                     var cheatScore = scoreNode.Value - node.score.Value;
//                     if (cheatScore == 0)
//                     {
//                         int a = 1;
//                         continue;
//                     }
//
//                     if (node.score.StartCheat.X == 1 && node.score.StartCheat.Y == 3 && node.score.EndCheat.X == 3 &&
//                         node.score.EndCheat.Y == 7)
//                     {
//                         int b = 2;
//                     }
//                     
//                     //Console.WriteLine($"Got an expected cheat with {cheatScore}");
//                     cheats.Add((node.score.StartCheat, node.score.EndCheat), cheatScore);
//                     
//                     continue;
//                 }
//                 else
//                 {
//                     if (node.score.StartCheat.X == 1 && node.score.StartCheat.Y == 3 && node.score.EndCheat.X == 3 &&
//                         node.score.EndCheat.Y == 7)
//                     {
//                         int b = 2;
//                     }
//                     
//                     var cheatScore = scoreNode.Value - node.score.Value;
//                     if (cheatScore == 0)
//                     {
//                         int a = 1;
//                         continue;
//                     }
//                     if (currentCheatScore < cheatScore)
//                     {
//                         cheats[(node.score.StartCheat, node.score.EndCheat)] = cheatScore;
//                     }
//                     continue;
//                 }
//             }
//             
//         }
//         else
//         {
//             if (node.score.Cheats == 0)
//             {
//                 cache.Add(node.position, node.score);
//             }
//         }
//         
//         foreach (var nextDirection in directions)
//         {
//             if (nextDirection.Key == GetOppositeDirection(node.direction))
//             {
//                 continue;
//             }
//         
//             var newPos = node.position.Add(nextDirection.Value);
//             
//             
//             if (map[newPos.Y][newPos.X] == '#')
//             {
//                
//                 if (newPos.X < 1 || newPos.X >= maxX - 1 || newPos.Y < 1 || newPos.Y >= maxY - 1)
//                 {
//                     continue;
//                 }
//                 
//                 if (baseLine > 0 && node.score.Cheats == 0)
//                 {
//                     //double step
//                     // var endCheatPos = newPos.Add(nextDirection.Value);
//                     // if (map[endCheatPos.Y][endCheatPos.X] == '#') // endCheat must be track
//                     // {
//                     //     continue;
//                     // }
//                     //
//                     // var nextScoreWithCheat = node.score.Add(1, 1, newPos, endCheatPos)
//                     //     .Add(1, 1, newPos, endCheatPos);
//                     // queue.Enqueue((endCheatPos, nextDirection.Key, nextScoreWithCheat));
//
//                     if (newPos.X == 1 && newPos.Y == 8)
//                     {
//                         int b = 1;
//                     }
//                     
//                     var nextCheatScore = node.score.Add(1, 1, node.position, node.score.EndCheat);
//                     StartCheat(newPos, nextCheatScore, nextDirection.Key);
//                 }
//                 
//                 continue;
//             }
//         
//             //var nextScore = node.score.Add(1, nextDirection, !isBaseLine && node.score.Cheats == 1 ? 1 : 0, node.score.StartCheat, node.score.Cheats == 1 ? newPos : node.score.EndCheat);
//             var nextScore = node.score.Add(1, 0, node.score.StartCheat, node.score.EndCheat);
//             queue.Enqueue((newPos, nextDirection.Key, nextScore));
//         }
//         
//     }while(queue.TryDequeue(out node));
//     
//     return bestExistScores;
// }
//
// void StartCheat(Position position, Score score, char direction, int moves = 1)
// {
//     if (score.Cheats >= maxCheatDuration)
//     {
//         return;
//     }
//     
//     foreach (var nextDirection in directions)
//     {
//         if (nextDirection.Key == GetOppositeDirection(direction))
//         {
//             continue;
//         }
//
//         var newPos = position.Add(nextDirection.Value);
//         
//         if (newPos.X < 1 || newPos.X >= maxX - 1 || newPos.Y < 1 || newPos.Y >= maxY - 1)
//         {
//             continue;
//         }
//         
//         //if we had processed already that cheat
//         if(cheats.ContainsKey((score.StartCheat, newPos)))
//         {
//             continue;
//         }
//
//         var nextScore = score.Add(1, 1, score.StartCheat, newPos);
//         //if we're warping to an already normal path
//         if (cache.TryGetValue(newPos, out var scoreNode) && scoreNode.Value <= score.Value)
//         {
//             continue;
//         }
//         
//         
//         
//         if (map[newPos.Y][newPos.X] != '#')
//         {
//             
//             //if we had processed already that cheat on cache
//             if(cheatCache.TryGetValue((score.StartCheat, newPos), out var cacheCheatValue))
//             {
//                 if (cacheCheatValue.Cheats >= nextScore.Cheats)
//                 {
//                     continue;
//                 }
//
//                 cheatCache[(score.StartCheat, newPos)] = nextScore;
//             }
//             else
//             {
//                 cheatCache.Add((score.StartCheat, newPos), nextScore);
//             }
//             
//             queue.Enqueue((newPos, nextDirection.Key, nextScore));
//         }
//
//         StartCheat(newPos, nextScore, nextDirection.Key, moves + 1);
//     }
//
//
//     // for (int cheatY = newPos.Y - maxCheatDuration; cheatY <= newPos.Y + maxCheatDuration; cheatY++)
//     // {
//     //     var deltaY = Math.Abs(cheatY - newPos.Y);
//     //     for (int cheatX = newPos.X - maxCheatDuration + deltaY;
//     //          cheatX <= newPos.X + maxCheatDuration - deltaY;
//     //          cheatX++)
//     //     {
//     //         if (cheatX == newPos.X && cheatY == newPos.Y)
//     //         {
//     //             continue;
//     //         }
//     //                         
//     //         if (cheatX < 1 || cheatX >= maxX - 1 || cheatY < 1 || cheatY >= maxY - 1)
//     //         {
//     //             continue;
//     //         }
//     //                         
//     //         if (map[cheatY][cheatX] == '#')
//     //         {
//     //             continue;
//     //         }
//     //                         
//     //         var deltaX = Math.Abs(cheatX - newPos.X);
//     //         var endCheat = new Position(cheatX, cheatY);
//     //         var nextScoreWithCheat = score.Add(1 + deltaX + deltaY, 1 + cheatX + deltaY, newPos, endCheat);
//     //         queue.Enqueue((endCheat, direction, nextScoreWithCheat));
//     //     }
//     // }
// }
//
// Position? GetPosition(char c)
// {
//     for (int y = 0; y < map.Length; y++)
//     for (int x = 0; x < map[y].Length; x++)
//     {
//         if (map[y][x] == c)
//         {
//             return new Position(x, y);
//         }
//     }
//     return null;
// }
// char GetOppositeDirection(char direction)
// {
//     switch (direction)
//     {
//         case '>':
//             return '<';
//         case '<':
//             return '>';
//         case '^':
//             return 'v';
//         case 'v':
//             return '^';
//     }
//
//     throw new ArgumentException();
// }
// public record Vector(int Vx, int Vy);
//
// public record Position(int X, int Y)
// {
//     public Position Add(Vector v)
//     {
//         return new Position ( X + v.Vx, Y + v.Vy );
//     }
// }
//
// public record Score(int Value, int Cheats, Position StartCheat, Position EndCheat)
// {
//     public Score Add(int cost, int cheats, Position startCheat, Position endCheat)
//     {
//         return new Score(Value + cost, Cheats + cheats, startCheat, endCheat);
//     }
// }