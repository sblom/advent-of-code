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
var lines = @"".GetLines();
#endif

int cur = 0;
int max = 0;
List<int> cals = new();

foreach (var line in lines)
{
   var L = line.Trim();
   if (L == "")
   {
       cals.Add(cur);
       if (cur > max) max = cur;
       cur = 0;       
   }
   else{
       cur += int.Parse(L);       
   }
}

cals.Add(cur);
if (cur > max) max = cur;
max.Dump();

cals.OrderByDescending(x => x).Take(3).Sum().Dump();