<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
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


var fish = lines.First().Split(",").Select(int.Parse).ToList().Dump();

for (int i = 0; i < 80; i++)
{
	var newfish = fish.ToList();
	var len = newfish.Count;
	for (int j = 0; j < len; j++)
	{
		if (newfish[j] == 0)
		{
			newfish[j] = 6;
			newfish.Add(8);
		}
		else
		{
			newfish[j]--;
		}
	}
	
	fish = newfish;
}

fish.Count().Dump();

var groups = lines.First().Split(",").Select(long.Parse).GroupBy(fish => fish).ToDictionary(x => x.Key, x => (long)x.Count());

for (int i = 0; i <= 8; i++)
{
	if (!groups.ContainsKey(i)) groups[i] = 0;
}

for (int i = 0; i < 256; i++)
{
	groups = new(){
		[0] = groups[1],
		[1] = groups[2],
		[2] = groups[3],
		[3] = groups[4],
		[4] = groups[5],
		[5] = groups[6],
		[6] = groups[7] + groups[0],
		[7] = groups[8],
		[8] = groups[0]
	};
}

groups.Values.Sum().Dump();