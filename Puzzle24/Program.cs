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
var count = crops.Sum(x => x.Area * x.Sides);
Console.WriteLine($"cost: {count}");

Crop GetCrop(char type, Point point)
{
    var crop = new Crop(type);
    points.Add(point);
    crop.Points.Add(point);
    ProcessCrop(crop, point);
    CalculateSides(crop);
    return crop;
}

void ProcessCrop(Crop crop, Point point)
{
    var neighbors = GetNeighbors(point.X, point.Y)
        .Where(neighbor => IsInBounds(neighbor));
    foreach (var neighbor in neighbors)
    {
        if (crop.Type == lines[neighbor.Y][neighbor.X])
        {
            crop.Points.Add(neighbor);
            if (points.Add(neighbor))
            {
                ProcessCrop(crop, neighbor);   
            }
        }
    }
    
    crop.Area++;
}

void CalculateSides(Crop crop)
{
    int minX = Int32.MaxValue, minY = Int32.MaxValue;
    int maxX = 0, maxY = 0;
    foreach (var point in crop.Points)
    {
        minX = Math.Min(minX, point.X);
        maxX = Math.Max(maxX, point.X);
        minY = Math.Min(minY, point.Y);
        maxY = Math.Max(maxY, point.Y);
    }

    var sides = 0;
    for (int y = minY; y <= maxY; y++)
    {
        var topSided = new List<int>();
        var bottomSided = new List<int>();
        
        for (int x = minX; x <= maxX; x++)
        {
            var point = new Point(x, y);
            if (!crop.Points.Contains(point))
            {
                continue;
            }
            
            var top = point with { Y = y - 1 };
            if (!crop.Points.Contains(top))
            {
                topSided.Add(point.X);
            }
            
            var bottom = point with { Y = y + 1 };
            if (!crop.Points.Contains(bottom))
            {
                bottomSided.Add(point.X);
            }
        }

        sides += GetContiguous(topSided);
        sides += GetContiguous(bottomSided);
    }
    
    
    for (int x = minX; x <= maxX; x++)
    {
        var leftSided = new List<int>();
        var rightSided = new List<int>();
        
        for (int y = minY; y <= maxY; y++)
        {
            var point = new Point(x, y);
            if (!crop.Points.Contains(point))
            {
                continue;
            }
            
            var left = point with { X = x - 1 };
            if (!crop.Points.Contains(left))
            {
                leftSided.Add(point.Y);
            }
            
            var right = point with { X = x + 1 };
            if (!crop.Points.Contains(right))
            {
                rightSided.Add(point.Y);
            }
        }

        sides += GetContiguous(leftSided);
        sides += GetContiguous(rightSided);
    }
    
    crop.Sides = sides;
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

int GetContiguous(IEnumerable<int> list)
{
    var contiguous = 0;
    var expected = -1;
    foreach (var x in list)
    {
        if (x != expected)
        {
            contiguous++;
        }
            
        expected = x + 1;
    }

    return contiguous;
}

record Crop(char Type)
{
    public int Area { get; set; }
    public int Sides { get; set; }
    public HashSet<Point> Points = new();
}

record Point(int X, int Y);