<Query Kind="Statements">
  <NuGetReference>System.Collections.Immutable</NuGetReference>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

int[] knothash(string str)
{
	var lens = str.Select(x => (int)x).Concat(new int[] {17, 31, 73, 47, 23});
	var list = Enumerable.Range(0, 256).ToArray();

	int loc = 0, skip = 0;

	for (int round = 0; round < 64; round++)
	{
		foreach (var len in lens)
		{
			int x0 = loc, y0 = (loc + len - 1) % 256;

			for (int x = x0, y = y0; x != y && x != (y + 1) % 256; x = (x + 1) % 256, y = (y + 255) % 256)
			{
				(list[x], list[y]) = (list[y], list[x]);
			}

			loc = (loc + len + (skip++)) % 256;
		}
	}

	var dense = new int[16];

	return list.Select((x, i) => new { ch = x, group = i / 16 }).GroupBy(x => x.group).Select(x => x.Aggregate(0, (a, g) => a ^ g.ch)).ToArray();
}

int countbits(int[] n)
{
	int count = 0;
	foreach (var m in n)
	{
		var mm = m;
		while (mm > 0)
		{
			if (mm % 2 > 0)
				count++;
			mm /= 2;
		}
	}
	
	return count;
}

var key = "xlqgujun";

int[,] mat = new int[128,128];

int c = 0;

for (int i = 0; i < 128; i++)
{
	var hash = knothash(key + "-" + i.ToString());
	c += countbits(hash);
	for (int j = 0; j < 16; j++)
	{
		for (int k = 0; k < 8; k++)
		{
			if ((hash[j] & (128 >> k)) > 0)
			mat[i,j*8+k] = 1;
		}
	}
}

bool findslot(out (int x, int y) coord)
{
	for (int i = 0; i < 128; i++)
	{
		for (int j = 0; j < 128; j++)
		{
			if (mat[i,j] == 1)
			{
				coord = (i,j);
				return true;
			}
		}
	}
	coord = (-1,-1);
	return false;
}

void squash(int x, int y, IImmutableSet<(int, int)> set = null)
{
	if (set == null) set = ImmutableHashSet<(int, int)>.Empty;

	mat[x, y] = 0;

	foreach (var (dx, dy) in new[] { (0, 1), (0, -1), (-1, 0), (1, 0) })
	{
		var (nx, ny) = (x + dx, y + dy);
		if (0 <= nx && nx < 128 && 0 <= ny && ny < 128)
		{
			if (mat[nx, ny] == 1)
				set = set.Add((nx, ny));
		}
	}

	try
	{
		var (gx, gy) = set.First();

		squash(gx, gy, set.Remove((gx, gy)));
	}
	catch { }
}

var cc = 0;

while (findslot(out var coord))
{
	var (x,y) = coord;
	
	squash(x,y);
	
	cc++;
}



c.Dump();
cc.Dump();