// See https://aka.ms/new-console-template for more information

var input = File.ReadAllText("input.txt").Split(Environment.NewLine + Environment.NewLine);
var patterns = input[0].Split(", ");

var designs = input[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

var lengths = patterns.Select(x => x.Length).Distinct().ToList();
var minPat = lengths.Min();

var hashesNotPossible = new HashSet<int>();  

var arePossible = designs
    .Select(x => CanMatchPatters(x))
    .Count(x => x);


Console.WriteLine(arePossible);

bool CanMatchPatters(ReadOnlySpan<char> design)
{
    var availableChars = design.Length;
    if (availableChars <= 0)
    {
        return true;
    }

    var designHash = Hash(design);
    if (hashesNotPossible.Contains(designHash))
    {
        return false;
    }
    
    if (availableChars < minPat)
    {
        hashesNotPossible.Add(designHash);
        return false;
    }
    
    foreach (var pattern in patterns)
    {
        if (RightIsInLeft(design, pattern))
        {
            if (CanMatchPatters(design.Slice(pattern.Length)))
            {
                return true;
            };
        }
    }
    
    hashesNotPossible.Add(designHash);
    return false;
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