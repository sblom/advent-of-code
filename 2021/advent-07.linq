<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

//#define TEST

#region preamble
#load "..\Lib\Utils"
#load "..\Lib\BFS"
#endregion

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"3,4,3,1,2".GetLines();
#endif

var pos = lines.First().Split(",").Select(int.Parse).ToArray();

var max = pos.Max();

int best = 0;
int min = int.MaxValue;

for (int i = pos.Min(); i <= max; i++)
{
	var fuel = pos.Sum(p => Math.Abs(p - i));

	//(fuel,i).ToString().Dump();

	if (fuel < min)
	{
		min = fuel;
		best = i;
	}
}

best.Dump();
pos.Sum(p => Math.Abs(p - best)).Dump();

best = 0;
min = int.MaxValue;

for (int i = pos.Min(); i <= max; i++)
{
	var fuel = pos.Sum(p => Math.Abs(p - i)*(Math.Abs(p - i) + 1)/2);

	//(fuel, i).ToString().Dump();

	if (fuel < min)
	{
		min = fuel;
		best = i;
	}
}

pos.Sum(p => Math.Abs(p - best)*(Math.Abs(p - best) + 1)/2).Dump();
