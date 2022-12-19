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
var lines = @"Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.
Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.".GetLines().ToArray();
#endif

var blueprints = lines.Extract<(int id, int oreCost, int clayCost, (int ore, int clay) obsidianCost, (int ore, int obsidian) geodeCost)>(@"Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs ((\d+) ore and (\d+) clay). Each geode robot costs ((\d+) ore and (\d+) obsidian).");

int HowManyTurns(int needed, int generated)
{
    needed = needed < 0 ? 0 : needed;
    double neededF = 1.0 * needed;
    return (int)Math.Floor((neededF - 1) / generated) + 1;
}

Func<((int b, int r) ore, (int b, int r) clay, (int b, int r) obsidian, (int b, int r) geode, int time), IEnumerable<((int b, int r) ore, (int b, int r) clay, (int b, int r) obsidian, (int b, int r) geode, int time)>> GetNeighborFunc((int id, int oreCost, int clayCost, (int ore, int clay) obsidianCost, (int ore, int obsidian) geodeCost) blueprint)
{
    IEnumerable<((int b, int r) ore, (int b, int r) clay, (int b, int r) obsidian, (int b, int r) geode, int time)> NeighborFunc(((int b, int r) ore, (int b, int r) clay, (int b, int r) obsidian, (int b, int r) geode, int time) state)
    {
        ((int b, int r) ore, (int b, int r) clay, (int b, int r) obsidian, (int b, int r) geode, int time) = state;
        
        if (time >= 24) yield break;
        
        int rem = 24 - time;
        
        int c = 0;
        if (ore.b > 0 && obsidian.b > 0 && ore.r + (rem - 1) * ore.b >= blueprint.geodeCost.ore && obsidian.r + (rem - 1) * obsidian.b >= blueprint.geodeCost.obsidian)
        {
            c++;
            int t = Math.Max(HowManyTurns(blueprint.geodeCost.ore - ore.r, ore.b), HowManyTurns(blueprint.geodeCost.obsidian - obsidian.r, obsidian.b));
            Debug.Assert(t >= 0);
            if (time + t + 1 > 24)
            {
                (rem, time, t).ToString().Dump();
            }

            yield return ((ore.b, ore.r + ore.b * (t + 1) - blueprint.geodeCost.ore), (clay.b, clay.r + clay.b * (t + 1)), (obsidian.b, obsidian.r + obsidian.b * (t + 1) - blueprint.geodeCost.obsidian), (geode.b + 1, geode.r + geode.b * (t + 1)), time + t + 1);
        }
        if (ore.b > 0 && clay.b > 0 && ore.r + (rem - 1) * ore.b >= blueprint.obsidianCost.ore && clay.r + (rem - 1) * clay.b >= blueprint.obsidianCost.clay)
        {
            c++;
            int t = Math.Max(HowManyTurns(blueprint.obsidianCost.ore - ore.r, ore.b), HowManyTurns(blueprint.obsidianCost.clay - clay.r, clay.b));
            Debug.Assert(t >= 0);
            if (time + t + 1 > 24)
            {
                (rem, time,t).ToString().Dump();
            }
            
            yield return ((ore.b, ore.r + ore.b * (t + 1) - blueprint.obsidianCost.ore), (clay.b, clay.r + clay.b * (t + 1) - blueprint.obsidianCost.clay), (obsidian.b + 1, obsidian.r + obsidian.b * (t + 1)), (geode.b, geode.r + geode.b * (t + 1)), time + t + 1);
        }
        if (ore.b > 0 && ore.r + (rem - 1) * ore.b >= blueprint.clayCost)
        {
            c++;
            int t = HowManyTurns(blueprint.clayCost - ore.r, ore.b);
            Debug.Assert(t >= 0);
            if (time + t + 1 > 24)
            {
                (rem, time, t).ToString().Dump();
            }

            yield return ((ore.b, ore.r + ore.b * (t + 1) - blueprint.clayCost), (clay.b + 1, clay.r + clay.b * (t + 1)), (obsidian.b, obsidian.r + obsidian.b * (t + 1)), (geode.b, geode.r + geode.b * (t + 1)), time + t + 1);
        }
        if (ore.b > 0 && ore.r + (rem - 1) * ore.b >= blueprint.oreCost)
        {
            c++;
            int t = HowManyTurns(blueprint.oreCost - ore.r, ore.b);
            Debug.Assert(t >= 0);
            if (time + t + 1 > 24)
            {
                (rem, time, t).ToString().Dump();
            }

            yield return ((ore.b + 1, ore.r + ore.b * (t + 1) - blueprint.oreCost), (clay.b, clay.r + clay.b * (t + 1)), (obsidian.b, obsidian.r + obsidian.b * (t + 1)), (geode.b, geode.r + geode.b * (t + 1)), time + t + 1);
        }
        
        if (c == 0)
        {
            yield return ((ore.b, ore.r + rem * ore.b), (clay.b, clay.r + rem * clay.b), (obsidian.b, obsidian.r + rem * obsidian.b), (geode.b, geode.r + rem * geode.b), 24);
        }
    }
    
    return NeighborFunc;
}

int c = 0;

foreach (var (blueprint, i) in blueprints.Select((x,i) => (x, i)))
{
    var bfs = new BFS<((int b, int r) ore, (int b, int r) clay, (int b, int r) obsidian, (int b, int r) geode, int time)>(
        ((1,0),(0,0),(0,0),(0,0),0),
        GetNeighborFunc(blueprint),
        (s) => s.time == 24,
        (s) => true,
        null,
        (s) => 1000000000 - (s.ore.b + s.clay.b * 100 + s.obsidian.b * 10000 + s.geode.b * 1000000)
    );
    
    var max = bfs.Search().Max(x => x.geode.r);
    
    c += max * (i + 1);
    
    (i + 1, max).ToString().Dump();
}

c.Dump();