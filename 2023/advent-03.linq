<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#region Preamble

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST
#define CHECKED

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..
".GetLines();
#endif

#if CHECKED
checked{
#endif

#endregion

var grid = lines.Select(L => L.ToCharArray()).ToArray();

int sum = 0;

var dirs = from x in new[]{-1,0,1} from y in new[]{-1,0,1} where (x != 0 || y != 0) select (x,y);
for (int i = 0; i < grid.Length; i++)
{
    for (int j = 0; j < grid[0].Length; j++)
    {
        if (grid[i][j] is >= '0' and <= '9')
        {
            int accum = 0;
            int initx = j;
            while (j < grid[0].Length && grid[i][j] is >= '0' and <= '9')
            {
                accum *= 10;
                accum += grid[i][j] - '0';
                j++;
            }
            int finalx = j - 1;
            
            for (int z = initx - 1; z <= finalx + 1; z++)
                {
                    try
                    {
                        if (!(grid[i][z] is (>= '0' and <= '9') or '.'))
                        {
                            sum += accum;

                            break;
                        }
                    }
                    catch {}
                    try{
                    if (!(grid[i+1][z] is (>= '0' and <= '9') or '.'))
                    {
                        sum += accum;
                        break;
                        }
                    }
                    catch { }
                    try
                    {

                        if (!(grid[i-1][z] is (>= '0' and <= '9') or '.'))
                    {
                        sum += accum;
                        break;
                    }

                }
                catch {}
            }
        }
    }
}

sum.Dump();

    sum = 0;
    
    var gearnums = new Dictionary<(int,int),List<int>>();

        for (int i = 0; i < grid.Length; i++)
    {
        for (int j = 0; j < grid[0].Length; j++)
        {
            if (grid[i][j] is >= '0' and <= '9')
            {
                int accum = 0;
                int initx = j;
                while (j < grid[0].Length && grid[i][j] is >= '0' and <= '9')
                {
                    accum *= 10;
                    accum += grid[i][j] - '0';
                    j++;
                }
                int finalx = j - 1;

                var nums = new List<int>();

                for (int z = initx - 1; z <= finalx + 1; z++)
                {
                    try
                    {
                        if (grid[i][z] == '*')
                        {
                            if (!gearnums.ContainsKey((i,z)))
                            {
                                gearnums[(i,z)] = new List<int>();
                            }
                            gearnums[(i,z)].Add(accum);

                            break;
                        }
                    }
                    catch { }
                    try
                    {
                        if (grid[i + 1][z] == '*')
                        {
                            if (!gearnums.ContainsKey((i + 1, z)))
                            {
                                gearnums[(i + 1, z)] = new List<int>();
                            }
                            gearnums[(i + 1, z)].Add(accum);
                            break;
                        }
                    }
                    catch { }
                    try
                    {

                        if (grid[i - 1][z] == '*')
                        {
                            if (!gearnums.ContainsKey((i - 1, z)))
                            {
                                gearnums[(i - 1, z)] = new List<int>();
                            }
                            gearnums[(i - 1, z)].Add(accum);
                            break;
                        }

                    }
                    catch { }
                }
            }
        }
    }
    
    gearnums.Where(x => (x.Value.Count == 2)).Select(x => x.Value.Aggregate((x,y) => x * y)).Sum().Dump();


#if CHECKED
}
#endif
