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
var lines = @"Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II".GetLines().ToArray();
#endif

var valves = lines.Extract<(string valve, int rate, List<string> tunnels)>(@"Valve (\w+) has flow rate=(\d+); tunnels? leads? to valves? (?:(\w+)(?:, )?)+").ToDictionary(x => x.valve, x => (x.rate, x.tunnels)) as IReadOnlyDictionary<string, (int rate, List<string> tunnels)>;

var bfs = new BFS<(int score, string loc, ImmutableHashSet<string> open, int countdown)>(
    (0, "AA", ImmutableHashSet<string>.Empty, 30),
    neighbors,
    (state) => (state.countdown == 0),
    (state) => (state.countdown == 0),
    null,
    (state) => 5000 - state.score
);

IEnumerable<(int score, string loc, ImmutableHashSet<string> open, int countdown)> neighbors((int score, string loc, ImmutableHashSet<string> open, int countdown) state)
{
    if (valves[state.loc].rate > 0 && !state.open.Any(x => x == state.loc)) yield return (state.score + valves[state.loc].rate * (state.countdown - 1), state.loc, state.open.Add(state.loc), state.countdown - 1);
    foreach (var exit in valves[state.loc].tunnels.Select(x => (state.score, x, state.open, state.countdown - 1)))
    {
        yield return exit;
    }
}

int Score((int score, string loc, ImmutableHashSet<string> open, int countdown) state)
{
    IEnumerable<(int score, int steps, string loc)> neighborsMax((int score, int steps, string loc) st)
    {
        return valves[st.loc].tunnels.Select(x => (st.score + (100 - st.steps) * (state.open.Contains(st.loc) ? 0 : valves[state.loc].rate), st.steps + 1, x).Dump());
    }

    var scorebfs = new BFS<(int steps, int score, string loc)>(
        (0,0,state.loc),
        neighborsMax,
        state => state.steps == valves.Count,
        state => state.steps == valves.Count,
        state => state.loc
    );
    
    return scorebfs.Search().Select(x => x.score).First();
}

bfs.Search().Aggregate(0, (max, state) => { if (state.score > max) { state.score.Dump(); return state.score; } else return max;}).Dump();