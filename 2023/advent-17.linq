<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RawScape.Wintellect.PowerCollections</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>Wintellect.PowerCollections</Namespace>
</Query>

#region Preamble

#load "..\Lib\Utils"
//#load "..\Lib\BFS"

//#define TEST
#define CHECKED

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"2413432311323
3215453535623
3255245654254
3446585845452
4546657867536
1438598798454
4457876987766
3637877979653
4654967986887
4564679986453
1224686865563
2546548887735
4322674655533".GetLines();
#endif

#if CHECKED
checked{
#endif

AoC._outputs = new DumpContainer[] {new(), new()};
Util.HorizontalRun("Part 1,Part 2",AoC._outputs).Dump();

#endregion

DoSearch(1,3).Dump1();
DoSearch(4, 10).Dump2();

int DoSearch(int min, int max)
{
    var grid = lines.Select(line => line.ToCharArray()).ToArray();
    var Y = grid.Length;
    var X = grid[0].Length;

    var mins = Enumerable.Range(0, Y).Select(y => Enumerable.Range(0, X).Select(x => new[] { int.MaxValue, int.MaxValue }).ToArray()).ToArray();

    (int dx, int dy)[] dirs = [(0, 1), (0, -1), (1, 0), (-1, 0)];

    var next = MakeNext(min, max);

    var frontier = new OrderedBag<(int x, int y, int dx, int dy, int loss)>(Comparer<(int x, int y, int dx, int dy, int loss)>.Create((a, b) => a.loss - b.loss));
    frontier.Add((0, 0, -1, 0, 0));
    frontier.Add((0, 0, 0, -1, 0));

    while (frontier.Any())
    {
        var (x, y, dx, dy, loss) = frontier.RemoveFirst();
        if ((x, y) == (X - 1, Y - 1))
        {
            return loss;
        }

        if (loss < mins[y][x][Math.Abs(dy)])
        {
            frontier.AddMany(next((x, y, dx, dy, loss)));
            mins[y][x][Math.Abs(dy)] = loss;
        }
    }
    
    return -1;

    Func<(int x, int y, int dx, int dy, int loss), IEnumerable<(int x, int y, int dx, int dy, int loss)>> MakeNext(int min, int max)
    {
        return UltraNext;
        IEnumerable<(int x, int y, int dx, int dy, int loss)> UltraNext((int x, int y, int dx, int dy, int loss) state)
        {
            var (x, y, dx, dy, loss) = state;

            for (int j = -1; j <= 1; j += 2)
            {
                var dir = (dx: dy * j, dy: dx * j);

                for (int i = min; i <= max; i++)
                {
                    if (x + i * dir.dx >= 0 && x + i * dir.dx < X && y + i * dir.dy >= 0 && y + i * dir.dy < Y)
                    {
                        yield return (x + i * dir.dx, y + i * dir.dy, dir.dx, dir.dy, loss + Enumerable.Range(1, i).Select(k => grid[y + k * dir.dy][x + k * dir.dx] - '0').Sum());
                    }
                }
            }
        }
    }
}

#if CHECKED
}
#endif
