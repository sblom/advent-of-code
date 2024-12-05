<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
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
var lines = @"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9".GetLines();
#endif

//lines.Extract<List<(int?,string)>>(@"(?=([0-9]|one|two|three|four|five|six|seven|eight|nine).*?)+").Dump();

int a = 0, c = 0;

foreach (var line in lines)
{    
    var data = line.Extract<List<int>>(@"((\d+)\s*)+");

    for (int s = -1; s < data.Count; s++)
    {
        var working = data.ToList();
        if (s != -1) working.RemoveAt(s);

        for (int i = 0; i < working.Count - 1; i++)
        {
            if (working[i] - working[i + 1] is 1 or 2 or 3) continue;
            goto maybe;
        }
        if (s == -1) a++;
        goto yep;
maybe:
        working = data.ToList();
        if (s != -1) working.RemoveAt(s);
        for (int i = 0; i < working.Count - 1; i++)
        {
            if (working[i + 1] - working[i] is 1 or 2 or 3) continue;
            goto notthisone;
        }
        if (s == -1) a++;
        goto yep;
notthisone:
        continue;
    }
    goto nope;
yep:
    c++;
    continue;
nope: 
    continue;
}
a.Dump("Part 1");
c.Dump("Part 2");