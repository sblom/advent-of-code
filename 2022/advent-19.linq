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

#define TEST

#if !TEST
var lines = (await AoC.GetLinesWeb()).ToArray();
#else
var lines = @"Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.
Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.".GetLines().ToArray();
#endif

var blueprints = lines.Extract<(int id, int oreCost, int clayCost, (int ore, int clay) obsidianCost, (int ore, int obsidian) geodeCost)>(@"Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs ((\d+) ore and (\d+) clay). Each geode robot costs ((\d+) ore and (\d+) obsidian).");

Func<((int b, int r) ore, (int b, int r) clay, (int b, int r) obsidian, (int b, int r) geode, int time), IEnumerable<((int b, int r) ore, (int b, int r) clay, (int b, int r) obsidian, (int b, int r) geode, int time)>> GetNeighborFunc((int id, int oreCost, int clayCost, (int ore, int clay) obsidianCost, (int ore, int obsidian) geodeCost) blueprint)
{
    IEnumerable<((int b, int r) ore, (int b, int r) clay, (int b, int r) obsidian, (int b, int r) geode, int time)> NeighborFunc(((int b, int r) ore, (int b, int r) clay, (int b, int r) obsidian, (int b, int r) geode, int time) state)
    {
        int c = 0;
        
        if (state.ore.r >= blueprint.geodeCost.ore && state.obsidian.r >= blueprint.geodeCost.obsidian)
        {
            c++;
            yield return (
                (state.ore.b, state.ore.r + state.ore.b - blueprint.geodeCost.ore),
                (state.clay.b, state.clay.r + state.clay.b),
                (state.obsidian.b, state.obsidian.r + state.obsidian.b - blueprint.geodeCost.obsidian),
                (state.geode.b + 1, state.geode.r + state.geode.b),
                state.time + 1);
        }
        if (state.ore.r >= blueprint.obsidianCost.ore && state.clay.r >= blueprint.obsidianCost.clay)
        {
            c++;
            yield return (
                (state.ore.b, state.ore.r + state.ore.b - blueprint.obsidianCost.ore),
                (state.clay.b, state.clay.r + state.clay.b - blueprint.obsidianCost.clay),
                (state.obsidian.b + 1, state.obsidian.r + state.obsidian.b),
                (state.geode.b, state.geode.r + state.geode.b),
                state.time + 1);
        }
        if (state.ore.r >= blueprint.clayCost)
        {
            c++;
            yield return (
                (state.ore.b, state.ore.r + state.ore.b - blueprint.clayCost),
                (state.clay.b + 1, state.clay.r + state.clay.b),
                (state.obsidian.b, state.obsidian.r + state.obsidian.b),
                (state.geode.b, state.geode.r + state.geode.b),
                state.time + 1);
        }
        if (state.ore.r >= blueprint.oreCost)
        {
            c++;
            yield return (
                (state.ore.b + 1, state.ore.r + state.ore.b - blueprint.oreCost),
                (state.clay.b, state.clay.r + state.clay.b),
                (state.obsidian.b, state.obsidian.r + state.obsidian.b),
                (state.geode.b, state.geode.r + state.geode.b),
                state.time + 1);
        }        
        yield return ((state.ore.b, state.ore.r + state.ore.b), (state.clay.b, state.clay.r + state.clay.b), (state.obsidian.b, state.obsidian.r + state.obsidian.b), (state.geode.b, state.geode.r + state.geode.b), state.time + 1);
    }
    
    return NeighborFunc;
}

foreach (var blueprint in blueprints)
{
    var bfs = new BFS<((int b, int r) ore, (int b, int r) clay, (int b, int r) obsidian, (int b, int r) geode, int time)>(
        ((1,0),(0,0),(0,0),(0,0),0),
        GetNeighborFunc(blueprint),
        (s) => s.time == 24,
        (s) => true,
        null,
        (s) => 1000000000 - (s.ore.b + s.clay.b * 100 + s.obsidian.b * 10000 + s.geode.b * 1000000)
    );
    
    bfs.Search().Dump();
}