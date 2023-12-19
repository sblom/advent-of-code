<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RawScape.Wintellect.PowerCollections</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>Wintellect.PowerCollections</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

#region Preamble

#load "..\Lib\Utils"
//#load "..\Lib\BFS"

//#define TEST
//#define CHECKED

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"R 6 (#70c710)
D 5 (#0dc571)
L 2 (#5713f0)
D 2 (#d2c081)
R 2 (#59c680)
D 2 (#411b91)
L 5 (#8ceee2)
U 2 (#caa173)
L 1 (#1b58a2)
U 2 (#caa171)
R 2 (#7807d2)
U 3 (#a77fa3)
L 2 (#015232)
U 2 (#7a21e3)".GetLines();
#endif

#if CHECKED
checked{
#endif

AoC._outputs = new DumpContainer[] {new(), new()};
Util.HorizontalRun("Part 1,Part 2",AoC._outputs).Dump();

#endregion

var digs = lines.Extract<(char dir, int distance, string color)>(@"(.) (\d+) \(([#0-9a-f]+)\)").Dump();

Dictionary<(int x, int y), string> map = new() {
    [(0,0)] = "#",
};

var (x, y) = (0,0);

foreach (var dig in digs)
{
    var (dx, dy) = dig.dir switch
    {
        'D' => (0,1),
        'U' => (0,-1),
        'L' => (-1,0),
        'R' => (1,0)
    };
    
    for (int i = 0; i < dig.distance; i++)
    {
        (x, y) = (x + dx, y + dy);
        map[(x,y)] = dig.color;
    }
}

var newmap = map.ToDictionary(x => x.Key, x => x.Value);

for (int i = map.Keys.Min(x => x.y); i <= map.Keys.Max(x => x.y); i++)
{
    bool inside = false;
    //Console.Write(i);
    for (int j = map.Keys.Min(x => x.x); j <= map.Keys.Max(x => x.x); j++)
    {
        //Console.Write(map.ContainsKey((j,i)) ? "#" : ".");
        if (map.ContainsKey((j,i)))
        {
            if (!map.ContainsKey((j+1,i)) && !map.ContainsKey((j-1,i)))
                inside = !inside;
            else if (map.ContainsKey((j,i - 1)))
                inside = !inside;
        }
        if (inside && !map.ContainsKey((j,i)))
            newmap[(j,i)] = "#inside";
    }
    //Console.WriteLine();
}

newmap.Keys.Count().Dump1();

for (int i = newmap.Keys.Min(x => x.y); i <= newmap.Keys.Max(x => x.y); i++)
{
    for (int j = newmap.Keys.Min(x => x.x); j <= newmap.Keys.Max(x => x.x); j++)
    {
        //Console.Write(newmap.ContainsKey((j, i)) ? "#" : ".");
    }
    //Console.WriteLine();
}

var bigdigs = digs.Select(x => (dir: x.color[^1] switch {'0' => 'R', '1' => 'D', '2' => 'L', '3' => 'U'}, int.Parse(x.color[1..^1],NumberStyles.HexNumber)));

var borders = new Dictionary<int,List<long>>();

(x, y) = (0,0);

long tot = 0;

foreach (var dig in bigdigs)
{
    tot += dig.Item2;
    
    var (dx, dy) = dig.dir switch
    {
        'D' => (0,1),
        'U' => (0,-1),
        'L' => (-1,0),
        'R' => (1,0)
    };
    
    if (dig.dir is 'L' or 'R')
    {
        (x, y) = (x + dx * dig.Item2, y + dy * dig.Item2);
    }
    else
    {
        for (int i = 0; i < dig.Item2; i++)
        {
            if (!borders.ContainsKey(y)) borders[y] = new List<long>();
            if (!borders.ContainsKey(y+1)) borders[y+1] = new List<long>();            
            
            if (dig.dir == 'U')
            {
                borders[y].Add(x);
                (x, y) = (x + dx, y + dy);
            }
            else
            {
                (x, y) = (x + dx, y + dy);
                borders[y].Add(x);
            }
        }
    }
}

borders.Keys.Max().Dump();

(tot + borders.Sum(x => x.Value.Order().Chunk(2).Sum(c => c[1] - c[0]))).Dump2();

#if CHECKED
}
#endif
