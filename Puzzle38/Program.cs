// See https://aka.ms/new-console-template for more information

var input = File.ReadAllText("input.txt").Split(Environment.NewLine + Environment.NewLine);
var patterns = input[0].Split(", ");

var designs = input[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

var lengths = patterns.Select(x => x.Length).Distinct().ToList();
var minPat = lengths.Min();

var hashes = new Dictionary<int, long>();  

var arePossible = designs
    .Select(x => CanMatchPatters(x))
    .Sum(x => (long)x);


Console.WriteLine(arePossible);

long CanMatchPatters(ReadOnlySpan<char> design)
{
    var availableChars = design.Length;
    if (availableChars <= 0)
    {
        return 1;
    }

    var designHash = Hash(design);
    if (hashes.TryGetValue(designHash, out var value))
    {
        return value;
    }
    
    if (availableChars < minPat)
    {
        hashes.Add(designHash, 0);
        return 0;
    }
    
    var matchesCount = 0L;
    foreach (var pattern in patterns)
    {
        if (RightIsInLeft(design, pattern))
        {
            matchesCount += CanMatchPatters(design.Slice(pattern.Length));
        }
    }

    hashes.Add(designHash, matchesCount);
    return matchesCount;
}

bool RightIsInLeft(ReadOnlySpan<char> left, ReadOnlySpan<char> right)
{
    if (left.Length < right.Length)
    {
        return false;
    }
    
    for (int i = 0; i < right.Length; i++)
    {
        if (left[i] != right[i])
        {
            return false;
        }
    }
    
    return true;
}

int Hash(ReadOnlySpan<char> span)
{
    HashCode hash = new();
    foreach (var c in span)
        hash.Add(c);
    return hash.ToHashCode();
}