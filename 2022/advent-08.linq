<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"30373
25512
65332
33549
35390".GetLines();
#endif

var grid = lines.Select(x => x.Select(x => x - '0').ToArray()).ToArray();

var treesVisible = new HashSet<(int,int)>();

for (int x = 0; x < grid[0].Length; x++)
{
    int height = -1;
    int y = 0;
    while (height < 9 && y < grid.Length)
    {
        if (grid[y][x] > height) treesVisible.Add((x,y));
        height = int.Max(height, grid[y][x]);        
        y++;
    }
    
    height = -1;
    y = grid.Length - 1;
    while (height < 9 && y >= 0)
    {
        if (grid[y][x] > height) treesVisible.Add((x,y));
        height = int.Max(height, grid[y][x]);
        y--;
    }
}

for (int y = 0; y < grid.Length; y++)
{
    int height = -1;
    int x = 0;
    while (height < 9 && x < grid[0].Length)
    {
        if (grid[y][x] > height) treesVisible.Add((x,y));
        height = int.Max(height, grid[y][x]);
        x++;
    }

    height = -1;
    x = grid[0].Length - 1;
    while (height < 9 && x >= 0)
    {
        if (grid[y][x] > height) treesVisible.Add((x,y));
        height = int.Max(height, grid[y][x]);
        x--;
    }
}

treesVisible.Count.Dump();

int maxScore = -1;

for (int i = 1; i < grid[0].Length - 1; i++)
{
    for (int j = 1; j < grid.Length - 1; j++)
    {
        
        int score = 1;
        int selfheight = grid[j][i];
        
        int height = -1;
        int dist = 1;
        int vis = 0;
        
        int y = j + dist;
        while (height < selfheight && y < grid.Length)
        {
            if (grid[y][i] > height) vis++;
            height = int.Max(height, grid[y][i]);
            dist++;
            y = j + dist;
        }
        
        score *= (dist - 1);

        height = -1;
        dist = 1;
        vis = 0;
        
        y = j - dist;
        while (height < selfheight && y >= 0)
        {
            if (grid[y][i] > height) vis++;
            height = int.Max(height, grid[y][i]);
            dist++;
            y = j - dist;
        }
        
        score *= (dist - 1);

        height = -1;
        dist = 1;
        vis = 0;

        int x = i + dist;
        while (height < selfheight && x < grid[0].Length)
        {
            if (grid[j][x] > height) vis++;
            height = int.Max(height, grid[j][x]);
            dist++;
            x = i + dist;
        }
        
        score *= (dist - 1);

        height = -1;
        dist = 1;
        vis = 0;

        x = i - dist;
        while (height < selfheight && x >= 0)
        {
            if (grid[j][x] > height) vis++;
            height = int.Max(height, grid[j][x]);
            dist++;
            x = i - dist;
        }
        
        score *= (dist - 1);
        
        maxScore = int.Max(maxScore, score);
    }
}

maxScore.Dump();