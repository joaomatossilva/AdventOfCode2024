// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

string input = @"RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE";

string[] lines = input.Split(Environment.NewLine);
var maxX = lines[0].Length;
var maxY = lines.Length;

List<Crop> crops = new List<Crop>();
HashSet<Point> points = new HashSet<Point>();

for (int y = 0; y < maxY; y++)
{
    for (int x = 0; x < maxX; x++)
    {
        var point = new Point(x, y);
        if (!points.Contains(point))
        {
            var type = lines[y][x];
            var crop = GetCrop(type, point);
            crops.Add(crop);
        }
    }
}

Console.WriteLine($"Crops: {crops.Count}");
var count = crops.Sum(x => x.Area * x.Perimeter);
Console.WriteLine($"cost: {count}");

Crop GetCrop(char type, Point point)
{
    var crop = new Crop(type);
    points.Add(point);
    ProcessCrop(crop, point);
    return crop;
}

void ProcessCrop(Crop crop, Point point)
{
    int perimeter = 4;
    var neighbors = GetNeighbors(point.X, point.Y)
        .Where(neighbor => IsInBounds(neighbor));
    foreach (var neighbor in neighbors)
    {
        if (crop.Type == lines[neighbor.Y][neighbor.X])
        {
            perimeter--;
            if (points.Add(neighbor))
            {
                ProcessCrop(crop, neighbor);   
            }
        }
    }
    
    crop.Perimeter += perimeter;
    crop.Area++;
}

IEnumerable<Point> GetNeighbors(int x, int y)
{
    yield return new Point(x - 1, y);
    yield return new Point(x, y - 1);
    yield return new Point(x, y + 1);
    yield return new Point(x + 1, y);
}

bool IsInBounds(Point p)
{
    return 0 <= p.X && p.X < maxX && 0 <= p.Y && p.Y < maxY;
}

record Crop(char Type)
{
    public int Area { get; set; }
    public int Perimeter { get; set; }
    HashSet<Point> Points = new();
}

record Point(int X, int Y);