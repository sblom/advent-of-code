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
var lines = @"00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010".GetLines();
#endif

var segs = lines.Extract<(int,int,int,int)>(@"(\d+),(\d+) -> (\d+),(\d+)");

Dictionary<(int,int),int> coverage = new();

foreach (var line in segs)
{
	switch (line)
	{
		case var (x1,y1,x2,y2) when x1 == x2:
			for (int y = Math.Min(y1,y2); y <= Math.Max(y1,y2); y++)
			{
				if (!coverage.ContainsKey((x1,y)))
					coverage[(x1,y)] = 0;
				
				coverage[(x1,y)]++;
			}
		break;
		case var (x1,y1,x2,y2) when y1 == y2:
			for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
			{
				if (!coverage.ContainsKey((x, y1)))
					coverage[(x, y1)] = 0;

				coverage[(x, y1)]++;
			}
		break;
	}
}

coverage.Where(x => x.Value >= 2).Count().Dump();

coverage = new();

foreach ((int x1,int y1, int x2, int y2) in segs)
{
	var dx = Math.Sign(x2 - x1);
	var dy = Math.Sign(y2 - y1);	
	
	for (int d = 0; ; d++)
	{
		var (x,y) = (x1 + dx * d, y1 + dy * d);
		
		if (!coverage.ContainsKey((x,y)))
			coverage[(x,y)] = 0;

		coverage[(x,y)]++;
		
		if (x1 + dx * d == x2 && y1 + dy * d == y2) break;
	}
}

coverage.Where(x => x.Value >= 2).Count().Dump();