<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RawScape.Wintellect.PowerCollections</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <NuGetReference>Z3.Linq</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Collections.ObjectModel</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Wintellect.PowerCollections</Namespace>
  <Namespace>Z3.Linq</Namespace>
</Query>

#region Preamble

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST
//#define CHECKED
//#define TRACE

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"".GetLines();
#endif

#if CHECKED
checked{
#endif

AoC._outputs = new DumpContainer[] { new(), new() };
Util.HorizontalRun("Part 1,Part 2", AoC._outputs).Dump();

#endregion

int acc = 1;

var components = lines.Extract<(string part, List<string> connections)>(@"(...): ((...) ?)+");
var edges = components.SelectMany(component => component.connections.Select(conn => (left: component.part, right: conn)).Concat(component.connections.Select(conn => (left: conn, right: component.part)))).ToLookup(x => x.left);

int ReachableIn(string start, int n, IImmutableSet<string> visited)
{
    if (n == 0) return 1;
    return 1 + edges[start].Where(edge => !visited.Contains(edge.right)).Select(edge => ReachableIn(edge.right, n - 1, visited.Add(edge.right))).Sum();
}

var edgesarray = edges.Select(e => e.Key).ToArray();

Dictionary<string,int> counts = new();

for (int i = 0; i < 600; i++)
{
    var from = edgesarray[Random.Shared.Next(edgesarray.Length)];
    var to = edgesarray[Random.Shared.Next(edgesarray.Length)];
    
    var path = DijkstraTheFuck(from, to);
    
    foreach (var (fn, tn) in path.Zip(path.Skip(1)))
    {
        if (!counts.ContainsKey(fn+tn)) counts[fn+tn] = 0;
        counts[fn+tn]++;
    }
}

var snipedges = counts.OrderByDescending(x => x.Value).Take(6).Select(snip => (snip.Key[0..3], snip.Key[3..6]));
var snipnodes = counts.OrderByDescending(x => x.Value).Take(6).Select(snip => snip.Key[0..3]);

edges = edges.SelectMany(edge => edge.Select(edg => (edge.Key, edg)).Where(x => !snipedges.Contains(x.edg))).ToLookup(x => x.Key, x => x.edg);

var a = snipedges.First().Item1.Dump();
var b = snipedges.First().Item2.Dump();

DijkstraTheFuck(a, "");
DijkstraTheFuck(b, "");

acc.Dump1();

IImmutableList<string> DijkstraTheFuck(string from, string to)
{
    var frontier = new Queue<IImmutableList<string>>();
    var visited = new HashSet<string>();

    frontier.Enqueue([from]);

    while (frontier.TryDequeue(out var path))
    {
        foreach (var (_,next) in edges[path.Last()])
        {
            if (next == to) return path.Add(next);
            if (!visited.Contains(next))
            {
                visited.Add(next);
                frontier.Enqueue(path.Add(next));
            }
        }
    }
    if (to == "") acc *= visited.Count();
    return ImmutableList<string>.Empty;    
}

#if CHECKED
}
#endif

//string.Join("\n",components.SelectMany(comp => comp.connections.Select(conn => $"{{source: \"{comp.part}\", target: \"{conn}\"}},"))).Dump();
//var ids = components.Select(comp => comp.part).Concat(components.SelectMany(comp => comp.connections)).ToHashSet().Select((x, i) => (x,i)).ToDictionary(x=>x.x, x=>x.i).Dump();

//Mermaid
//string.Join("\n",components.SelectMany(comp => comp.connections.Select(conn => $"    {comp.part} --> {conn}"))).Dump();

//edges.Select(edge => (edge.Key, ReachableIn(edge.Key, 6, []))).OrderBy(x => x.Item2).Dump();

