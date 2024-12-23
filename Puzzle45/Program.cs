// See https://aka.ms/new-console-template for more information

using SharpGraph;

var input = File.ReadAllText("input.txt")
    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

var map = new Dictionary<string, HashSet<string>>();
var listOfEdges = new List<Edge>();

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
    
    listOfEdges.Add(new Edge(parts[0], parts[1]));
}

var graph = new Graph(listOfEdges);
var cliques = graph.FindMaximalCliques();

var connectedTNodes = Denormalize3(cliques.Where(x => x.Count >= 3))
    .Where(x => x.Any(n => n.GetLabel()!.Trim().StartsWith("t")))
    .ToList();

Console.WriteLine(connectedTNodes.Count);


//For Cliques greater than 3, we need to cartesian join in sets of 3
IEnumerable<HashSet<Node>> Denormalize3(IEnumerable<HashSet<Node>> nodeList)
{
    foreach (var nodes in nodeList)
    {
        if (nodes.Count <= 3)
        {
            yield return nodes;
            continue;
        }

        var nodesArray = nodes.ToArray();
        for(int a = 0; a < nodesArray.Length; a++)
        for(int b = a + 1; b < nodesArray.Length; b++)
        for(int c = b + 1; c < nodesArray.Length; c++)
        {
            var hash = new HashSet<Node>();
            hash.Add(nodesArray[a]);
            hash.Add(nodesArray[b]);
            hash.Add(nodesArray[c]);
            yield return hash;
        }

    }
}


/* Original Answer before SharGraphLib*/
// var visited = new HashSet<(string, string, string)>();
//
// foreach (var node in map)
// {
//     if (!node.Key.StartsWith("t"))
//     {
//         continue;
//     }
//     
//     foreach (var connection in node.Value)
//     {
//         foreach (var subConnection in map[connection])
//         {
//             if (map[subConnection].Contains(node.Key))
//             {
//                 var list = new List<string> { node.Key, connection, subConnection };
//                 list.Sort();
//                 visited.Add((list[0], list[1], list[2]));
//             }
//         }
//     }
// }
//
// Console.WriteLine(visited.Count);
