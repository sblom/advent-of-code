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
//#define CHECKED

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"Time:      7  15   30
Distance:  9  40  200".GetLines();
#endif

#if CHECKED
checked{
#endif

#endregion

var times = lines.First().Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();
var distances = lines.Skip(1).First().Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();

int tot = 1;

for (int i = 0; i < times.Length; i++)
{
    int c = 0;
    for (int n = 0; n < times[i]; n++)
    {
        if ((times[i] - n) * n > distances[i]) c++;
    }
    tot *= c;
}

tot.Dump("Part 1");

var time = long.Parse(string.Join("",lines.First().Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1)));
var distance = long.Parse(string.Join("",lines.Skip(1).First().Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1)));


int c0 = 0;
for (long n = 0; n < time; n++)
{
    if ((time - n) * n > distance) c0++;
}

c0.Dump("Part 2");

#if CHECKED
}
#endif
