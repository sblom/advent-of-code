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
//#define PART2

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"..............
..............
.......#......
.....###.#....
...#...#.#....
....#...##....
...#.###......
...##.#.##....
....#..#......
..............
..............
..............".Replace("\t", "    ").Replace("\r", "").Replace("\\","").Split("\n");
#endif

var grid = lines.Select(x => x.ToArray()).ToArray();

var elves = new HashSet<(int x, int y)>();
var moves = new Dictionary<(int x, int y), (int x, int y)?>();

int curRule = 0;
var rules = new Func<(int x0,int y0),(int x,int y)?>[] {
    (loc) => !elves.Contains((loc.x0, loc.y0 - 1)) && !elves.Contains((loc.x0 - 1, loc.y0 - 1)) && !elves.Contains((loc.x0 + 1, loc.y0 - 1)) ? (loc.x0, loc.y0 - 1) : null,
    (loc) => !elves.Contains((loc.x0, loc.y0 + 1)) && !elves.Contains((loc.x0 - 1, loc.y0 + 1)) && !elves.Contains((loc.x0 + 1, loc.y0 + 1)) ? (loc.x0, loc.y0 + 1) : null,
    (loc) => !elves.Contains((loc.x0 - 1, loc.y0)) && !elves.Contains((loc.x0 - 1, loc.y0 - 1)) && !elves.Contains((loc.x0 - 1, loc.y0 + 1)) ? (loc.x0 - 1, loc.y0) : null,
    (loc) => !elves.Contains((loc.x0 + 1, loc.y0)) && !elves.Contains((loc.x0 + 1, loc.y0 - 1)) && !elves.Contains((loc.x0 + 1, loc.y0 + 1)) ? (loc.x0 + 1, loc.y0) : null,
};

for (int i = 0; i < grid.Length; i++)
{
    for (int j = 0; j < grid[0].Length; j++)
    {
        if (grid[i][j] == '#') elves.Add((j,i));
    }
}

for (int r = 1; ; r++)
{
    moves.Clear();
    
    foreach (var elf in elves)
    {
        var alone = (from dx in new[] {-1, 0, 1} from dy in new[] {-1,0,1} select elves.Contains((elf.x + dx, elf.y + dy)) ? 1 : 0).Sum() == 1;
        if (alone) continue;
        
        for (int rn = 0; rn < 4; rn++)
        {
            var rule = rules[(curRule + rn) % 4];
            var proposal = rule(elf);
            if (rule(elf) != null)
            {
                if (moves.ContainsKey(proposal.Value))
                {
                    moves[proposal.Value] = null;
                }
                else
                {
                    moves[proposal.Value] = elf;
                }
                break;
            }
        }
    }
    
    var moved = false;
    foreach (var move in moves)
    {
        if (move.Value == null)
        {
            continue;
        }
        else
        {
            moved = true;
            elves.Remove(move.Value.Value);
            elves.Add(move.Key);
        }
    }

    if (!moved)
    {
        r.Dump("Part 2");
        break;
    }
    
    //elves.OrderBy(x => x.y).ThenBy(x => x.x).Dump();
    
    curRule++;

    if (r == 10)
    {
        var xmin = elves.Min(x => x.x);
        var xmax = elves.Max(x => x.x);
        var ymin = elves.Min(x => x.y);
        var ymax = elves.Max(x => x.y);

        ((xmax - xmin + 1) * (ymax - ymin + 1) - elves.Count()).Dump("Part 1");
    }
}

