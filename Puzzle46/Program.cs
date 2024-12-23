// See https://aka.ms/new-console-template for more information

using System.Runtime.InteropServices;
using SharpGraph;

var input = File.ReadAllText("input.txt")
    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

var map = new Dictionary<string, HashSet<string>>();
List<Edge> list = new List<Edge> ();

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
    
    list.Add(new Edge(parts[0], parts[1]));
}

var graph = new Graph(list);

var a =graph.FindMaximalCliques()
    .OrderByDescending(x => x.Count)
    .First();

var sorted = a.Select(x => x.ToString().Trim()).OrderBy(x => x);
Console.WriteLine(string.Join(",", sorted));

