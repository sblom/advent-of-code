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
var lines = $@"1
2
3
2024".Replace("\\", "").GetLines();
    //   2,4,1,4,7,5,4,1,1,4,5,5,0,3,3,0

#endif

long c = 0;

var windows = new Dictionary<int,int>();
var seen = new HashSet<int>();

foreach (var line in lines)
{
    seen.Clear();
    var x = long.Parse(line);
    
    var window = 0;
    var prev = 0;
    var diff = 0;
    
    for (int i = 0; i < 2000; i++)
    {
        var cur = (int)(x % 10);
        if (i > 0)
        {
            diff = cur - prev;
            window <<= 8; window += (diff + 10);
        }
        if (i >= 4 && !seen.Contains(window))
        {
            seen.Add(window);
            if (!windows.ContainsKey(window)) windows[window] = 0;
            windows[window] += cur;
        }
        var y = 64 * x;
        x ^= y;
        x %= 16777216;
        y = x / 32;
        x ^= y;
        x %= 16777216;    
        y = x * 2048;
        x ^= y;    
        x %= 16777216;
        
        prev = cur;
    }
    c += x;
}

c.Dump("Part 1");
//windows.Dump();
windows.MaxBy(kv => kv.Value).Value.Dump("Part 2");
//Convert.ToString(windows.MaxBy(kv => kv.Value).Key,2).Dump();


// 338 wrong
// 2284 wrong
// 2148 wrong