<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Text.Json.Nodes</Namespace>
  <Namespace>System.Text.Json</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST

#if !TEST
var lines = (await AoC.GetLinesWeb()).ToArray();
#else
var lines = @"Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3".GetLines().ToArray();
#endif

Dictionary<(int, int), int> counts = new();

var observations = lines.Extract<(int sx, int sy, int bx, int by)>(@"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)").ToList();

for (int i = 0; i < 4000000; i++)
{
    foreach (var line in observations)
    {
        var dist = Math.Abs(line.sx - line.bx) + Math.Abs(line.sy - line.by);
        var ydist = Math.Abs(i - line.sy);
        if (ydist > dist) continue;

        var xdist = dist - ydist;
        var x1 = line.sx - xdist - 1;
        var x2 = line.sx + xdist + 1;
        
        if (!counts.ContainsKey((i,x1)))
        {
            counts[(i,x1)] = 0;
        }
        if (!counts.ContainsKey((i, x2)))
        {
            counts[(i, x2)] = 0;
        }
        
        counts[(i,x1)]++;
        counts[(i,x2)]++;
    }
    
    if (i % 10000 == 0) i.Dump();
}

counts.OrderByDescending(x => x.Value).Take(10).Dump();

//points.Count.Dump("part 1");