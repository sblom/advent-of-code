<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#LINQPad checked+

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = $@"###############
\#...#...#.....#
\#.#.#.#.#.###.#
\#S#...#.#.#...#
\#######.#.#.###
\#######.#.#...#
\#######.#.###.#
\###..E#...#...#
\###.#######.###
\#...###...#...#
\#.#####.#.###.#
\#.#...#.#.#...#
\#.#.#.#.#.#.###
\#...#...#...###
\###############".Replace("\\", "").GetLines();
    //   2,4,1,4,7,5,4,1,1,4,5,5,0,3,3,0

#endif

var grid = lines.Select(x => x.ToCharArray()).ToArray();

var startRow = grid.Index().Where(x => x.Item.Contains('S')).Single().Index;
var startCol = grid[startRow].Index().Where(x => x.Item == 'S').Single().Index;

var endRow = grid.Index().Where(x => x.Item.Contains('E')).Single().Index;
var endCol = grid[endRow].Index().Where(x => x.Item == 'E').Single().Index;

var dirs = new (int y, int x)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

var scores = new Dictionary<(int,int),int>();
var scores2 = new Dictionary<(int,int),int>();

var bfs = new BFS<(int y, int x, int score, bool canCheat)>((startRow,startCol,0,true),GetNeighbors,x => false,x => x.x == endCol && x.y == endRow, x => (x.x, x.y));
var bfs2 = new BFS<(int y, int x, int score, bool canCheat)>((endRow,endCol,84,true),GetNeighbors2,x => false,x => x.x == startCol && x.y == startRow, x => (x.x, x.y));

bfs.Search().Dump();
bfs2.Search().Dump();

var savings = new List<int>();
var savings2 = new HashSet<(int,int,int,int,int)>();

for (int i = 0; i < grid.Length; i++)
{
    for (int j = 0; j < grid[0].Length; j++)
    {
        if (scores.ContainsKey((i,j)) && scores.ContainsKey((i + 2,j))) savings.Add(Math.Abs(scores[(i,j)] - scores[(i + 2, j)]) - 2);
        if (scores.ContainsKey((i,j)) && scores.ContainsKey((i,j + 2))) savings.Add(Math.Abs(scores[(i,j)] - scores[(i, j + 2)]) - 2);
    }
}

//savings.GroupBy(x => x).Dump();

//savings.OrderByDescending(x => x).Where(x => x >= 100).Count().Dump();

savings.Clear();

for (int i = 0; i < grid.Length; i++)
{
    for (int j = 0; j < grid[0].Length; j++)
    {
        for (int m = 0; m <= 20; m++)
        {
            for (int n = -20 + Math.Abs(m); n <= 20 - Math.Abs(m); n++)
            {
                // I had this line earlier, but apparently was doing something stupid.
                if (m == 0 && n < 0) continue;
                if (scores.ContainsKey((i, j)) && scores.ContainsKey((i + m, j + n)))
                {
                    savings.Add(Math.Abs(scores[(i,j)] - scores[(i+m, j+n)]) - (Math.Abs(m)+Math.Abs(n)));
                    savings2.Add((i,j,i+m,j+n,Math.Abs(scores[(i,j)] - scores[(i+m, j+n)]) - (Math.Abs(m)+Math.Abs(n))));
                }
            }
        }
    }
}

//scores.Dump();

savings.Where(x => x >= 100).Count().Dump();

(savings2.Where(x => x.Item5 >= 100).Count()).Dump();
//savings2.GroupBy(x => x.Item5).OrderByDescending(x => x.Key).Select(x => (x.Count(),x.Key)).Dump();

IEnumerable<(int y, int x, int score, bool canCheat)> GetNeighbors((int y, int x, int score, bool canCheat) state)
{
    if (!scores.ContainsKey((state.y,state.x))) scores[(state.y,state.x)] = state.score;
    foreach (var (dx, dy) in dirs)
    {
        var (y, x, score, canCheat) = state;
        (y, x) = (y + dy, x + dx);
        if (x >= 0 && x < grid[0].Length && y >= 0 && y < grid.Length && (grid[y][x] == '.' || grid[y][x] == 'E' || grid[y][x] == 'S'))
        {
            yield return (y, x, score + 1, canCheat);
        }
        //else if (canCheat && x >= 0 && x < grid[0].Length && y >= 0 && y < grid.Length && grid[y][x] == '#')
        //{
        //    yield return (y, x, score + 1, false);
        //}
    }
}

IEnumerable<(int y, int x, int score, bool canCheat)> GetNeighbors2((int y, int x, int score, bool canCheat) state)
{
    if (!scores2.ContainsKey((state.y, state.x))) scores2[(state.y, state.x)] = state.score;
    foreach (var (dx, dy) in dirs)
    {
        var (y, x, score, canCheat) = state;
        (y, x) = (y + dy, x + dx);
        if (x >= 0 && x < grid[0].Length && y >= 0 && y < grid.Length && (grid[y][x] == '.' || grid[y][x] == 'E' || grid[y][x] == 'S'))
        {
            yield return (y, x, score - 1, canCheat);
        }
        //else if (canCheat && x >= 0 && x < grid[0].Length && y >= 0 && y < grid.Length && grid[y][x] == '#')
        //{
        //    yield return (y, x, score + 1, false);
        //}
    }
}

// 1332
