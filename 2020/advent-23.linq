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
var lines = @"".GetLines();
#endif

int[] nextcup = new int[1_000_001];

var order = lines.First();

var numcups = order.Length;

foreach (var i in Enumerable.Range(0,numcups))
{
	nextcup[order[i] - '0'] = order[(i + 1) % order.Length] - '0';
}

int current = order[0] - '0';

for (int c = 0; c < 100; c++)
{
	int firstcup = nextcup[current];
	int secondcup = nextcup[firstcup];
	int thirdcup = nextcup[secondcup];
	
	nextcup[current] = nextcup[thirdcup];
	
	int destination = (current + numcups - 2) % numcups + 1;
	while (destination == firstcup || destination == secondcup || destination == thirdcup)
	{
		destination = (destination + numcups - 2) % numcups + 1;
	}
	
	nextcup[thirdcup] = nextcup[destination];
	nextcup[destination] = firstcup;
	
	current = nextcup[current];
}

ShowCups().Dump("Part 1");

// Part 2

numcups = 1_000_000;

foreach (var i in Enumerable.Range(0, order.Length - 1))
{
	nextcup[order[i] - '0'] = order[(i + 1) % order.Length] - '0';
}

nextcup[order[order.Length - 1] - '0'] = order.Length + 1;

for (int i = order.Length; i < 1_000_000; i++)
{
	nextcup[i] = i + 1;
}

nextcup[1_000_000] = order[0] - '0';

current = order[0] - '0';

for (int c = 0; c < 10_000_000; c++)
{
	int firstcup = nextcup[current];
	int secondcup = nextcup[firstcup];
	int thirdcup = nextcup[secondcup];

	nextcup[current] = nextcup[thirdcup];

	int destination = (current + numcups - 2) % numcups + 1;
	while (destination == firstcup || destination == secondcup || destination == thirdcup)
	{
	destination = (destination + numcups - 2) % numcups + 1;
	}

	nextcup[thirdcup] = nextcup[destination];
	nextcup[destination] = firstcup;

	current = nextcup[current];
}

((long)nextcup[1] * (long)nextcup[nextcup[1]]).Dump("Part 2");

string ShowCups()
{
	int current = nextcup[1];
	string result = "";
	
	do
	{
		result += (char)(current + '0');
		current = nextcup[current];
	} while (current != 1);
	
	return result;
}