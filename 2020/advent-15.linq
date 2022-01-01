<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

var lines = AoC.GetLines().ToArray();
var line = lines[0];

//line = "1,2,3";

var nums = line.Split(",").Select(int.Parse).ToArray();

int cur = nums.Last();
int turn = nums.Count() + 1;
var history = nums.Select((n, i) => (n, i)).ToDictionary(x => x.n, x => x.i + 1);

int next = 0;

for (; turn < 30000000; turn++)
{
	cur = next;
	
	if (history.ContainsKey(cur))
	{
		next = turn - history[cur];
	}
	else
	{
		next = 0;
	}
	
	history[cur] = turn;
	
	if (turn == 2019) next.Dump("Part 1");
}

next.Dump("Part 2");