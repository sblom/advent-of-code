<Query Kind="Statements">
  <NuGetReference>RawScape.Wintellect.PowerCollections</NuGetReference>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>Wintellect.PowerCollections</Namespace>
</Query>

#LINQPad checked+

#load "..\Lib\Utils"

//#define TEST

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"###############
\#.......#....E#
\#.#.###.#.###.#
\#.....#.#...#.#
\#.###.#####.#.#
\#.#.#.......#.#
\#.#.#####.###.#
\#...........#.#
\###.#.#####.#.#
\#...#.....#.#.#
\#.#.#.###.#.#.#
\#.....#...#.#.#
\#.###.#.#.#.#.#
\#S..#.....#...#
\###############".Replace("\\","").GetLines();
#endif

var grid = lines.Select(x => x.ToCharArray()).ToArray();

var startRow = grid.Index().Where(x => x.Item.Contains('S')).Single().Index;
var startCol = grid[startRow].Index().Where(x => x.Item == 'S').Single().Index;

var endRow = grid.Index().Where(x => x.Item.Contains('E')).Single().Index;
var endCol = grid[endRow].Index().Where(x => x.Item == 'E').Single().Index;

var scores = new Dictionary<(int row, int col, int dy, int dx), (int score, ImmutableHashSet<(int,int)>)>();
var visited = new HashSet<(int,int,int,int)>();

scores[(startRow,startCol,0,1)] = (0,ImmutableHashSet<(int,int)>.Empty.Add((startRow,startCol)));

var next = scores.Where(kv => !visited.Contains(kv.Key));

while (next.Any())
{
    var node = next.MinBy(kv => kv.Value.score);
    visited.Add(node.Key);
    var (row, col, dy, dx) = node.Key;
    var (score, path) = node.Value;

    var newnode = (row + dy, col + dx, dy, dx);
    var newscore = 1 + score;
    if (grid[newnode.Item1][newnode.Item2] is '.' or 'E')
    {
        if (!scores.ContainsKey(newnode))
        {
            scores[newnode] = (newscore, path.Add((row + dy, col + dx)));
        }
        else if (scores[newnode].score == newscore)
        {
            scores[newnode] = (newscore, scores[newnode].Item2.Union(path));
        }
        else if (scores[newnode].score > newscore)
        {
            scores[newnode] = (newscore, path);
            visited.Remove(newnode);
        }
    }
    (dy, dx) = (-dx, dy);
    newnode = (row, col, dy, dx);
    newscore = 1000 + score;
    if (grid[row + dy][col + dx] is '.' or 'E')
    {
        if (!scores.ContainsKey(newnode))
        {
            scores[newnode] = (newscore, path);
        }
        else if (scores[newnode].score == newscore)
        {
            scores[newnode] = (newscore, scores[newnode].Item2.Union(path));
        }
        else if (scores[newnode].score > newscore)
        {
            scores[newnode] = (newscore, path);
            visited.Remove(newnode);
        }
    }
    (dy, dx) = (-dx, dy);
    newnode = (row, col, dy, dx);
    newscore = 2000 + score;
    if (grid[row + dy][col + dx] is '.' or 'E')
    {
        if (!scores.ContainsKey(newnode))
        {
            scores[newnode] = (newscore, path);
        }
        else if (scores[newnode].score == newscore)
        {
            scores[newnode] = (newscore, scores[newnode].Item2.Union(path));
        }
        else if (scores[newnode].score > newscore)
        {
            scores[newnode] = (newscore, path);
            visited.Remove(newnode);
        }
    }
    (dy, dx) = (-dx, dy);
    newnode = (row, col, dy, dx);
    newscore = 1000 + score;
    if (grid[row + dy][col + dx] is '.' or 'E')
    {
        if (!scores.ContainsKey(newnode))
        {
            scores[newnode] = (newscore, path);
        }
        else if (scores[newnode].score == newscore)
        {
            scores[newnode] = (newscore, scores[newnode].Item2.Union(path));
        }
        else if (scores[newnode].score > newscore)
        {
            scores[newnode] = (newscore, path);
            visited.Remove(newnode);
        }
    }

    next = scores.Where(kv => !visited.Contains(kv.Key));
}

scores.Where(kv => kv.Key.row == endRow && kv.Key.col == endCol).MinBy(kv => kv.Value.score).Value.Item2.Count().Dump("Part 2");
// 388 is too low