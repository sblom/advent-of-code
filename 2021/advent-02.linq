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
var lines = @"199
200
208
210
200
207
240
269
260
263".GetLines();
#endif

var instrs = lines.Extract<(string, int)>(@"(\w+) (\d+)").Dump();

var (x,d) = (0,0);

foreach (var instr in instrs)
{
	switch (instr)
	{
		case ("forward", int dx): x += dx; break;
		case ("down", int dd): d += dd; break;
		case ("up", int dd): d -= dd; break;
	}
}

(x * d).Dump1();

var (depth,aim,hor) = (0,0,0);

foreach (var instr in instrs)
{
	switch (instr)
	{
		case ("forward", int dx): hor += dx; depth += aim * dx; break;
		case ("down", int dd): aim += dd; break;
		case ("up", int dd): aim -= dd; break;
	}
}

(hor * depth).Dump2();