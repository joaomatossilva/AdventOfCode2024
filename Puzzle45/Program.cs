// See https://aka.ms/new-console-template for more information

var input = File.ReadAllText("input.txt")
    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

var map = new Dictionary<string, HashSet<string>>();

foreach (var link in input)
{
    var parts = link.Split("-");
    if (!map.TryAdd(parts[0], [parts[1]]))
    {
        map[parts[0]].Add(parts[1]);
    }
    
    if (!map.TryAdd(parts[1], [parts[0]]))
    {
        map[parts[1]].Add(parts[0]);
    }
}

var visited = new HashSet<(string, string, string)>();

foreach (var node in map)
{
    if (!node.Key.StartsWith("t"))
    {
        continue;
    }
    
    foreach (var connection in node.Value)
    {
        foreach (var subConnection in map[connection])
        {
            if (map[subConnection].Contains(node.Key))
            {
                var list = new List<string> { node.Key, connection, subConnection };
                list.Sort();
                visited.Add((list[0], list[1], list[2]));
            }
        }
    }
}

Console.WriteLine(visited.Count);
