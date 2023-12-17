<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
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
var lines = @"2,2,2
1,2,2
3,2,2
2,1,2
2,3,2
2,2,1
2,2,3
2,2,4
2,2,6
1,2,5
3,2,5
2,1,5
2,3,5".GetLines().ToArray();
#endif

var cubes = lines.Extract<(int x,int y,int z)>(@"(\d+),(\d+),(\d+)").ToHashSet();

var minx = cubes.Min(c => c.x);
var maxx = cubes.Max(c => c.x);
var miny = cubes.Min(c => c.y);
var maxy = cubes.Max(c => c.y);
var minz = cubes.Min(c => c.z);
var maxz = cubes.Max(c => c.z);

(int dx, int dy, int dz)[] dirs = new[] {(-1,0,0), (1,0,0), (0,-1,0), (0,1,0), (0,0,1), (0,0,-1) };

(from cube in cubes from dir in dirs where !cubes.Contains((cube.x + dir.dx, cube.y + dir.dy, cube.z + dir.dz)) select 1).Count().Dump("Part 1");

IEnumerable<(int,int,int)> neighbors((int x, int y, int z) c) {
    foreach (var dir in dirs)
    {
        var (x,y,z) = (c.x + dir.dx, c.y + dir.dy, c.z + dir.dz);
        if (x >= minx - 1 && x <= maxx + 1 && y >= miny - 1 && y <= maxy + 1 && z >= minz - 1 && z <= maxz + 1 && !cubes.Contains((x,y,z)))
            yield return (c.x + dir.dx, c.y + dir.dy, c.z + dir.dz);
    }
}

var reachable = new BFS<(int x,int y,int z)>(
    (0,0,0),
    neighbors,
    (d) => false,
    (d) => true
);

var outerbounds = reachable.Search().ToHashSet();
outerbounds.Count().Dump();

(from cube in cubes from dir in dirs where outerbounds.Contains((cube.x + dir.dx, cube.y + dir.dy, cube.z + dir.dz)) select 1).Count().Dump("Part 2");