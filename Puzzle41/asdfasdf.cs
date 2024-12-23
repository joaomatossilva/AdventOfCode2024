// // See https://aka.ms/new-console-template for more information
//
// using System.Text.RegularExpressions;
//
// var numericKeyPad = new Dictionary<char, Position>()
// {
//     { '7', new Position(0, 0) },
//     { '8', new Position(1, 0) },
//     { '9', new Position(2, 0) },
//     { '4', new Position(0, 1) },
//     { '5', new Position(1, 1) },
//     { '6', new Position(2, 1) },
//     { '1', new Position(0, 2) },
//     { '2', new Position(1, 2) },
//     { '3', new Position(2, 2) },
//     { '*', new Position(0, 3) },
//     { '0', new Position(1, 3) },
//     { 'A', new Position(2, 3) }
// };
//
// var directionalKeyPad = new Dictionary<char, Position>()
// {
//     { '*', new Position(0, 0) },
//     { '^', new Position(1, 0) },
//     { 'A', new Position(2, 0) },
//     { '<', new Position(0, 1) },
//     { 'v', new Position(1, 1) },
//     { '>', new Position(2, 1) },
// };
//
// var costs = new Dictionary<(Position, Position), long>();
//
// var initialNumericPosition = numericKeyPad['A'];
// var initialDirectionalPosition = directionalKeyPad['A'];
// int numberOfRobots = 2;
//
// var codes = File.ReadAllText("input.txt").Split(Environment.NewLine);
// //var codes = new string[] { "029A" };
//
// var robot = new Robot(initialNumericPosition, numericKeyPad);
// var directionRobots = new Robot[numberOfRobots];
// for (int i = 0; i < numberOfRobots; i++)
// {
//     directionRobots[i] = new Robot(initialDirectionalPosition, directionalKeyPad);
// }
//
// /**/
//
// var seq = GetSequences('A', '0', numericKeyPad);
// foreach (var s in seq)
// {
//     Console.WriteLine($"{new string(s.ToArray())} - {new string (GetBestSequence(1, s.ToArray()))}");
// }
//
// /**/
//
//
//
//
// // FillCosts(numericKeyPad);
// //
// //
// // long complexity = 0;
// // foreach (var c in codes)
// // {
// //    
// //     var moves = DoCode(c);
// //     var numeric = Number().Match(c);
// //     
// //     // Console.WriteLine($"Code {c}: Moves {moves.Length} * {numeric} -- {new string(moves)}");
// //     // complexity += moves.Length * int.Parse(numeric.Value);
// //     Console.WriteLine($"Code {c}: Moves {moves} * {numeric}");
// //     complexity += moves * int.Parse(numeric.Value);
// // }
// // Console.WriteLine(complexity);
//
//
// void FillCosts(Dictionary<char, Position> pad)
// {
//     foreach (var key in pad.Where(x => x.Key != '*'))
//     {
//         var pairs = pad.Where(x => x.Key != '*');
//         foreach (var pair in pairs)
//         {
//             var a = GetSequences(key.Key, pair.Key, pad)
//                 .Select(x => GetBestSequence(numberOfRobots, x.ToArray()))
//                 .Min();
//             costs.Add((key.Value, pair.Value), a.Length);
//             
//         }
//     }
// }
//
// char[] GetBestSequence(int level, char[] sequence)
// {
//     if (level == 0)
//     {
//         return sequence;
//     }
//
//     if (sequence.Length == 0)
//     {
//         return Array.Empty<char>();
//     }
//
//     List<char[]> sq = new List<char[]>();
//     
//     var start = 'A';
//     for (int a = 0; a < sequence.Length; a++)
//     {
//         var end = sequence[a];
//         var seqs = GetSequences(start, end, directionalKeyPad).ToList();
//         var deepSequencesCost = seqs
//             .Select(x => new
//             {
//                 x,
//                 cost = GetBestSequence(level - 1, x.ToArray())
//             })
//             .OrderBy(x => x.cost.Length)
//             .ToList();
//             
//         sq.Add(deepSequencesCost.Select(x => x.cost).First());
//     }
//     
//     return sq.Aggregate("", (current, s) => current + new string(s)).ToCharArray();
// }
//
// IEnumerable<IEnumerable<char>> GetSequences(char startChar, char finishChar, Dictionary<char, Position> pad)
// {
//     var start = pad[startChar];
//     var finish = pad[finishChar];
//     var star = pad['*'];
//     var diffX = finish.X - start.X;
//     var diffY = finish.Y - start.Y;
//
//     bool canMoveXFirst = diffX > 0;
//     if (start.Y == star.Y && finish.X == star.X)
//     {
//         canMoveXFirst = false;
//     }
//     
//     bool canMoveYFirst = diffY > 0;
//     if (start.X == star.X && finish.Y == star.Y)
//     { 
//         canMoveYFirst = false;
//     }
//
//     if(canMoveXFirst) yield return MoveX().Concat(MoveY()).Concat(['A']);
//     if(canMoveYFirst) yield return MoveY().Concat(MoveX()).Concat(['A']);
//
//     IEnumerable<char> MoveX()
//     {
//         for (int i = 0; i < Math.Abs(diffX); i++)
//         {
//             yield return diffX > 0 ? '>' : '<';
//         }
//     }
//     
//     IEnumerable<char> MoveY()
//     {
//         for (int i = 0; i < Math.Abs(diffY); i++)
//         {
//             yield return diffY > 0 ? 'v' : '^';
//         }
//     }
// }
//
//
//
// long DoCode(string code)
// {
//     var cost = 0L;
//     var start = directionalKeyPad['A'];
//     for (int a = 0; a < code.Length; a++)
//     {
//         cost += costs[(start, numericKeyPad[code[a]])];
//         start = numericKeyPad[code[a]];
//     }
//
//     return cost;
// }
//
// // char[] DoCode(string code)
// // {
// //     Console.WriteLine();
// //     var sequence = code.Select(x => robot.Press(x)).SelectMany(x => x);
// //     foreach (var directionRobot in directionRobots)
// //     {
// //         Console.WriteLine($"sequence : {new string(sequence.ToArray())}");
// //         
// //         sequence = sequence.Select(x => directionRobot.Press(x)).SelectMany(x => x);
// //     }
// //     
// //     return sequence.ToArray();
// // }
//
//
// public class Robot(Position position, Dictionary<char, Position> pad)
// {
//     private Position _position = position;
//     private Position _star = pad['*'];
//
//     public IEnumerable<IEnumerable<char>> Press(char key)
//     {
//         var moves = new List<char>();
//         var keyPosition = pad[key];
//         var diffX = keyPosition.X - _position.X;
//         var diffY = keyPosition.Y - _position.Y;
//
//         
//         
//         _position = keyPosition;
//         return [moves];
//     }
// }
//
//
// // public class Robot(Position position, Dictionary<char, Position> pad, bool useLong = false)
// // {
// //     private Position _position = position;
// //     private Position _star = pad['*'];
// //
// //     public IEnumerable<char> Press(char key)
// //     {
// //         // if (useLong && key == '<' && pad.Count == 5 && _position == position)
// //         // {
// //         //     _position = pad['<'];
// //         //     return new[] { '<', 'v', '<', 'A' };
// //         // }
// //         
// //         var moves = new List<char>();
// //         var keyPosition = pad[key];
// //         var diffX = keyPosition.X - _position.X;
// //         var diffY = keyPosition.Y - _position.Y;
// //
// //         bool moveXFirst = true; //diffX > 0;
// //
// //         if (diffY == 2 && diffX == 2)
// //         {
// //             moveXFirst = false;
// //         }
// //         
// //         
// //         if (keyPosition.X == _star.X && _position.Y == _star.Y)
// //         {
// //             moveXFirst = false;
// //         }
// //         if (_position.Y == _star.Y && _position.X == _star.X)
// //         {
// //             moveXFirst = false;
// //         }
// //         if (keyPosition.Y == _star.Y && _position.X == _star.X)
// //         {
// //             moveXFirst = true;
// //         }
// //         if (_position.X == _star.X && _position.Y == _star.Y)
// //         {
// //             moveXFirst = true;
// //         }
// //         
// //         if (moveXFirst)
// //         {
// //             for (int i = 0; i < Math.Abs(diffX); i++)
// //             {
// //                 moves.Add(diffX > 0 ? '>' : '<');
// //             }
// //         }
// //
// //         for (int i = 0; i < Math.Abs(diffY); i++)
// //         {
// //             moves.Add(diffY > 0 ? 'v' : '^');
// //         }
// //
// //         if (!moveXFirst)
// //         {
// //             for (int i = 0; i < Math.Abs(diffX); i++)
// //             {
// //                 moves.Add(diffX > 0 ? '>' : '<');
// //             }
// //         }
// //
// //         moves.Add('A');
// //         _position = keyPosition;
// //         return moves;
// //     }
// // }
//
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
// public partial class Program
// {
//     [GeneratedRegex("\\d+")]
//     public static partial Regex Number();
// }
