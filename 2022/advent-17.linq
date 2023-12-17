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
var lines = (await AoC.GetLinesWeb()).ToArray();
#else
var lines = @">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>".GetLines().ToArray();
#endif

var moves = lines.Single();

Dictionary<(int,long,string),long> cycles = new();

var shapes = new[] {
    new []{"..@@@@.".ToCharArray()},
    new []{"...@...".ToCharArray(),
           "..@@@..".ToCharArray(),
           "...@...".ToCharArray()},
    new []{"....@..".ToCharArray(),
           "....@..".ToCharArray(),
           "..@@@..".ToCharArray()},
    new []{"..@....".ToCharArray(),
           "..@....".ToCharArray(),
           "..@....".ToCharArray(),
           "..@....".ToCharArray()},
    new []{"..@@...".ToCharArray(),
           "..@@...".ToCharArray()}
};

(int,long,string) cyclecheck = default;
long r_0 = 0;
int h_0 = 0;

List<char[]> grid = new();

#if PART2
grid = "......................................#......#.....##.....##...####.....###.....#....####...###....###....#.#.#.####.#..#.###.####......#......#......#..#.###..#.######..###..".Chunk(7).ToList();
grid.Dump();
#endif

int move = 0;

#if PART2
move = 372;
#endif

#if PART2
for (long r = 4; r < 956; r++)
#else
for (long r = 0; ; r++)
#endif
{
    int top = 0;
    
    var blanks = grid.TakeWhile(x => x.All(x => x == '.')).Count();
    for (int i = 0; i < 3 - blanks; i++)
        grid.Insert(0, ".......".ToCharArray());
    while (blanks-- > 3)
        grid.RemoveAt(0);
        
    foreach (var row in shapes[r % shapes.Length].Reverse())
    {
        grid.Insert(0,row.ToArray());
    }

    while (true)
    {
        // Process move
        if (moves[(move++) % moves.Length] == '<')
        {
            for (int i = Math.Min(grid.Count - 1, top + 5); i >= top; i--)
            {
                for (int j = 0; j < grid[0].Length; j++)
                {
                    if (grid[i][j] == '@' && (j == 0 || grid[i][j - 1] == '#'))
                    {
                        goto move_done;
                    }
                }
            }
            for (int i = Math.Min(grid.Count - 1, top + 5); i >= top; i--)
            {
                for (int j = 0; j < grid[0].Length - 1; j++)
                {
                    if (grid[i][j + 1] == '@')
                    {
                        grid[i][j] = '@';
                        grid[i][j + 1] = '.';
                    }
                }
            }
        }
        else
        {
            for (int i = Math.Min(grid.Count - 1, top + 5); i >= top; i--)
            {
                for (int j = grid[0].Length - 1; j >= 0; j--)
                {
                    if (grid[i][j] == '@' && (j == grid[0].Length - 1 || grid[i][j + 1] == '#'))
                    {
                        goto move_done;
                    }
                }
            }
            for (int i = Math.Min(grid.Count - 1, top + 5); i >= top; i--)
            {
                for (int j = grid[0].Length - 1; j >= 1; j--)
                {
                    if (grid[i][j - 1] == '@')
                    {
                        grid[i][j] = '@';
                        grid[i][j - 1] = '.';
                    }
                }
            }
        }
        move_done:;
        //grid.Dump();
        
        // Process fall        
        for (int i = Math.Min(grid.Count - 1, top + 5); i >= top; i--)
        {
            for (int j = 0; j < grid[0].Length; j++)
            {
                if (grid[i][j] == '@' && (i == grid.Count - 1 || grid[i + 1][j] == '#'))
                {
                    for (int ii = top; ii < Math.Min(top + 5, grid.Count); ii++)
                    {
                        for (int jj = 0; jj < grid[0].Length; jj++)
                        {
                            if (grid[ii][jj] == '@') grid[ii][jj] = '#';
                        }
                    }
                    goto next_shape;
                }
            }
        }
        for (int i = Math.Min(grid.Count - 1, top + 5); i >= top + 1; i--)
        {
            for (int j = 0; j < grid[0].Length; j++)
            {
                if (grid[i - 1][j] == '@')
                {
                    grid[i - 1][j] = '.';
                    grid[i][j] = '@';
                }
            }
        }
        top++;
    }    
    next_shape:;
    
    var sb = new StringBuilder();
    for (int i = 0; i < Math.Min(25, grid.Count); i++)
    {
        for (int j = 0; j < grid[0].Length; j++)
        {
            sb.Append(grid[i][j]);
        }
    }
    
    var check = ((move - 1) % moves.Length, r % shapes.Length, sb.ToString());

    if (check == cyclecheck)
    {
        check.Dump();
        long r_n = r - r_0;
        long h_n = grid.Count - h_0;
        
        long t = (1000000000000L - r_0) / r_n;
        long r_t = r_n * t + r_0;
        long h_t = t * h_n + h_0;
        
        (h_t + 1500).Dump("Part 2");

        long r_rem = 1000000000000L - r_t;
    }

        //1585673352428 - 1585673352422

    if (cyclecheck.Item1 == 0 && cyclecheck.Item2 == 0 && cycles.ContainsKey(check))
    {
        cyclecheck = check;
        r_0 = r;
        h_0 = grid.Count;
    }
    
    cycles[check] = r;
}

grid.Dump();