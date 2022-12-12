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
var lines = @"zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw".GetLines();
#endif

var line = lines.Single().ToCharArray();

char a = ' ', b = ' ',c = ' ',d = ' ',e = ' ',f = ' ', g = ' ', h = ' ', i = ' ', j = ' ', k = ' ', l = ' ', m = ' ', n = ' ';

for (int ii = 0; ii < line.Length; ii++)
{
    if (new[] {a,b,c,d, e, f, g, h, i, j, k, l, m, n}.Distinct().Count() == 14) ii.Dump();
    (a, b, c, d, e, f, g, h, i, j, k, l, m ) = (b, c, d, e, f, g, h, i, j, k, l, m, n);
    n = line[ii];
}