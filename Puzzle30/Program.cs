﻿// See https://aka.ms/new-console-template for more information

string @inputMap = @"##########
#..O..O.O#
#......O.#
#.OO..O.O#
#..O@..O.#
#O#..O...#
#O..O..O.#
#.OO.O.OO#
#....O...#
##########";

string @inputMoves = @"<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^";

var directions = new Dictionary<char, Vector>
{
    { '^', new Vector(0, -1) },
    { '>', new Vector(1, 0) },
    { '<', new Vector(-1, 0) },
    { 'v', new Vector(0, 1) }
};

Position robot = new Position();
var boxes = new HashSet<Position>();

inputMap = inputMap.Select(x =>
{
    if (x == '#')
    {
        return "##";
    }

    if (x == 'O')
    {
        return "[]";
    }

    if (x == '.')
    {
        return "..";
    }

    if (x == '@')
    {
        return "@.";
    }
    
    return x.ToString();
}).Aggregate("", (agg, curr) => agg + curr);

var map = inputMap.Split(Environment.NewLine);
int maxX = map[0].Length;
int maxY = map.Length;

for (int y = 0; y < maxY; y++)
{
    for (int x = 0; x < maxX; x++)
    {
        if (map[y][x] == '@')
        {
            robot = new Position { X = x, Y = y };
        }

        if (map[y][x] == '[')
        {
            boxes.Add(new Position { X = x, Y = y });
        }
    }
}

Console.WriteLine($"Robot: {robot.X}, {robot.Y}");
foreach (var box in boxes)
{
    Console.WriteLine($"Box: {box.X}, {box.Y}");
}

PrintMap();
foreach (var move in inputMoves)
{
    if (!directions.TryGetValue(move, out var direction))
    {
        continue;
    }

    MoveRobot(direction);
    
    Console.WriteLine();
    Console.WriteLine($"Move: {move}");
    //PrintMap();
    // int a = 0;
}

PrintMap();
PintSum();


void MoveRobot(Vector direction)
{
    //find next free space
    var newRobot = robot.Add(direction);

    var backUpBoxes = BackupBoxes();
    if (MoveBox(direction, newRobot))
    {
        robot = newRobot;
    }
    else // revert boxes
    {
        boxes = backUpBoxes;
    }
}

bool MoveBox(Vector direction, Position pos)
{
    if (map[pos.Y][pos.X] == '#')
    {
        return false;
    }

    // move h
    if (direction.Vy == 0)
    {
        if (!boxes.TryGetValue(pos, out var box))
        {
            boxes.TryGetValue(pos with { X = pos.X - 1 }, out box);
        }
        
        if(box != null)
        {
            if (MoveBox(direction, pos.BoxAdd(direction)))
            {
                boxes.Remove(box);
                boxes.Add(box.Add(direction));
                return true;
            }
            
            return false;
        }
    }

    // move v
    if (direction.Vx == 0)
    {
        if (!boxes.TryGetValue(pos, out var box))
        {
            boxes.TryGetValue(pos with { X = pos.X - 1 }, out box);
        }

        if (box != null)
        {
            if (MoveBox(direction, box.Add(direction)) && MoveBox(direction, box.Add(new Vector(1, 0)).Add(direction)))
            {
                boxes.Remove(box);
                boxes.Add(box.Add(direction));
                return true;
            }
           
            return false;
        }
    }
    
    
    //
    //
    //
    // if (boxes.Contains(pos) || boxes.Contains(pos with { X = pos.X - 1 }))
    // {
    //     if (MoveBox(direction, pos.Add(direction)))
    //     {
    //         boxes.Remove(pos);
    //         boxes.Add(pos.Add(direction));
    //         return true;
    //     }
    //     return false;
    // }
    
    return true;
}

void PintSum()
{
    long sum = 0;
    for (int y = 0; y < maxY; y++)
    {
        for (int x = 0; x < maxX; x++)
        {
            if (boxes.Contains(new Position { X = x, Y = y }))
            {
                sum += y * 100 + x;
            }
        }
    }
    Console.WriteLine($"Sum {sum}");
}

void PrintMap()
{
    for (int y = 0; y < maxY; y++)
    {
        for (int x = 0; x < maxX; x++)
        {
            
            if (boxes.Contains(new Position { X = x, Y = y }))
            {
                Console.Write("[");
            }
            else
            if (boxes.Contains(new Position { X = x-1, Y = y }))
            {
                Console.Write("]");
            }
            else
            if (robot.X == x && robot.Y == y)
            {
                Console.Write("@");
            }
            else if (map[y][x] == '#')
            {
                Console.Write("#");
            }
            else
            {
                Console.Write(".");
            }
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

HashSet<Position> BackupBoxes()
{
    HashSet<Position> newBoxes = new HashSet<Position>(boxes.Count);
    foreach (var position in boxes)
    {
        newBoxes.Add(position);
    }

    return newBoxes;
}

public record Vector(int Vx, int Vy);

public record Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position Add(Vector v)
    {
        return new Position { X = X + v.Vx, Y = Y + v.Vy };
    }

    public Position BoxAdd(Vector v)
    {
        if (X == 1)
        {
            return new Position { X = X + v.Vx + 1, Y = Y + v.Vy };
        }
        
        return new Position { X = X + v.Vx, Y = Y + v.Vy };
    }
}