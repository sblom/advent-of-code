<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>static UserQuery</Namespace>
</Query>

//#define TEST

#region preamble
#load "..\Lib\Utils"
#load "..\Lib\BFS"
#endregion

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"v...>>.vv>
.vv>>.vv..
>>.>v>...v
>>v>>.>.v.
v>v.vv.v..
>.>>..v...
.vv..>.>v.
v.v..>>v.v
....v..v.>".Replace("^", "").GetLines();
#endif

var grid = lines.Select(line => line.ToCharArray()).ToArray();

int c = 0;

//string.Join("\n",grid.Select(line => string.Join("",line))).DumpFixed();

while (true)
{
	c++;
	
	int moved = 0;
	
	var eastHerd = (from x in Enumerable.Range(0, grid[0].Length) from y in Enumerable.Range(0,grid.Length)
	where grid[y][x] == '>' && grid[y][(x + 1) % grid[0].Length] == '.' select (x,y)).ToList();
	
	moved += eastHerd.Count();
	
	//eastHerd.Dump();
	
	foreach (var cucumber in eastHerd)
	{
		grid[cucumber.y][cucumber.x] = '.';
		grid[cucumber.y][(cucumber.x + 1) % grid[0].Length] = '>';
	}

	//("\n\n" + string.Join("\n",grid.Select(line => string.Join("",line)))).DumpFixed();
	
	var southHerd = (from x in Enumerable.Range(0, grid[0].Length)
				   from y in Enumerable.Range(0, grid.Length)
				   where grid[y][x] == 'v' && grid[(y + 1) % grid.Length][x] == '.'
				   select (x, y)).ToList();
				   
	moved += southHerd.Count();

	foreach (var cucumber in southHerd)
	{
		grid[cucumber.y][cucumber.x] = '.';
		grid[(cucumber.y + 1) % grid.Length][cucumber.x] = 'v';
	}
	
	//("\n\n" + string.Join("\n",grid.Select(line => string.Join("",line)))).DumpFixed();

	if (moved == 0)
	{
		c.Dump("Part 1");
		break;
	}
}