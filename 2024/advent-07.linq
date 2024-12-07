<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
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
var lines = @"190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20".GetLines();
#endif

var calibrations = lines.Extract<(long target,List<long> nums)>(@"(\d+): ((\d+) ?)+").ToList();
long t = 0;

foreach (var cal in calibrations)
{
    for (int i = 0; i < 1 << cal.nums.Count - 1; i++)
    {
        var c = cal.nums.Count - 1;
        var ops = i;
        var list = cal.nums.ToList();
        while (c > 0)
        {
            var val = (ops % 2) switch
            {
                0 => list[0] = list[0] + list[1],
                1 => list[0] = list[0] * list[1]
            };
            list.RemoveAt(1);
            ops >>= 1;
            c--;
        }
        if (list[0] == cal.target) goto found;
    }
    continue;
found:
    t += cal.target;    
}

t.Dump();

t = 0;

foreach (var cal in calibrations)
{
    for (int i = 0; i < Math.Pow(3, cal.nums.Count - 1); i++)
    {
        var c = cal.nums.Count - 1;
        var ops = i;
        var list = cal.nums.ToList();
        while (c > 0)
        {
            var val = (ops % 3) switch
            {
                0 => list[0] = list[0] + list[1],
                1 => list[0] = list[0] * list[1],
                2 => list[0] = long.Parse(list[0].ToString() + list[1].ToString())
            };
            list.RemoveAt(1);
            ops /= 3;
            c--;
        }
        if (list[0] == cal.target) goto found;
    }
    continue;
found:
    t += cal.target;
}
t.Dump();