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

int p1 = int.Parse(lines.First());
int p2 = int.Parse(lines.Skip(1).First());
int c = 0, c1 = 0, c2 = 0;

long v = 1;

while (c1 == 0 || c2 == 0)
{
	v = v * 7;
	v = v % 20201227;
	if (v == p1)
	{
		v = p2;
		for (int i = 0; i < c; i++)
		{
			v = v * p2;
			v = v % 20201227;
		}
		break;
	}
	if (v == p2)
	{
		v = p1;
		for (int i = 0; i < c; i++)
		{
			v = v * p1;
			v = v % 20201227;
		}
		break;
	}

	c++;
}

v.Dump("Part 1");