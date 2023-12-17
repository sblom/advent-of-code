<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

var lines = AoC.GetLines().ToArray();

int[] sgn = {-1,0,1};

var neighbors3 = from dx in sgn from dy in sgn from dz in sgn where dx != 0 || dy != 0 || dz != 0 select (dx,dy,dz,0);
var neighbors4 = from dw in sgn from dx in sgn from dy in sgn from dz in sgn where dw != 0 || dx != 0 || dy != 0 || dz != 0 select (dw,dx,dy,dz);

var activeCubes = new HashSet<(int, int, int, int)>();

for (int y = 0; y < lines.Length; y++)
{
	for (int x = 0; x < lines[0].Length; x++)
	{
		if (lines[y][x] == '#') activeCubes.Add((0, x, y, 0));
	}
}

for (int c = 0; c < 6; c++)
{
	var nextActiveCubes = new HashSet<(int,int,int,int)>();	
	
	var (low, hiw, lox, hix, loy, hiy, loz, hiz) = MinMax(activeCubes);

	int w = 0;
	for (int x = lox - 1; x <= hix + 1; x++)
	{
		for (int y = loy - 1; y <= hiy + 1; y++)
		{
			for (int z = loz - 1; z <= hiz + 1; z++)
			{
				var nc = neighbors4.Count(dir => activeCubes.Contains((w + dir.dw, x + dir.dx, y + dir.dy, z + dir.dz)));
				
				if ((activeCubes.Contains((w,x,y,z)) && (nc == 2 || nc == 3)) || (!activeCubes.Contains((w,x,y,z)) && nc == 3))
				{
					nextActiveCubes.Add((w,x,y,z));
				}
			}
		}
	}
	activeCubes = nextActiveCubes;
}

activeCubes.Count.Dump("Part 1");

activeCubes = new HashSet<(int, int, int, int)>();

for (int y = 0; y < lines.Length; y++)
{
	for (int x = 0; x < lines[0].Length; x++)
	{
		if (lines[y][x] == '#') activeCubes.Add((0, x, y, 0));
	}
}

for (int c = 0; c < 6; c++)
{
	var nextActiveCubes = new HashSet<(int, int, int, int)>();

	var (low, hiw, lox, hix, loy, hiy, loz, hiz) = MinMax(activeCubes);

	for (int w = low - 1; w <= hiw + 1; w++)
	{
		for (int x = lox - 1; x <= hix + 1; x++)
		{
			for (int y = loy - 1; y <= hiy + 1; y++)
			{
				for (int z = loz - 1; z <= hiz + 1; z++)
				{
					var nc = neighbors4.Count(dir => activeCubes.Contains((w + dir.dw, x + dir.dx, y + dir.dy, z + dir.dz)));

					if ((activeCubes.Contains((w, x, y, z)) && (nc == 2 || nc == 3)) || (!activeCubes.Contains((w, x, y, z)) && nc == 3))
					{
						nextActiveCubes.Add((w, x, y, z));
					}
				}
			}
		}
	}
	activeCubes = nextActiveCubes;
}

activeCubes.Count.Dump("Part 2");

(int, int, int, int, int, int, int, int) MinMax(IEnumerable<(int,int,int,int)> coords)
{

	int low = int.MaxValue, hiw = int.MinValue, lox = int.MaxValue, hix = int.MinValue, loy = int.MaxValue, hiy = int.MinValue, loz = int.MaxValue, hiz = int.MinValue;

	foreach (var coord in coords)
	{
		var (w,x,y,z) = coord;
		(low, hiw, lox, hix, loy, hiy, loz, hiz) = (Min(low,w), Max(hiw,w), Min(lox,x), Max(hix,x), Min(loy,y), Max(hiy,y), Min(loz,z), Max(hiz,z));
	}

	return (low, hiw, lox, hix, loy, hiy, loz, hiz);
}