<Query Kind="Statements">
  <NuGetReference>System.Interactive</NuGetReference>
  <Namespace>System.Linq</Namespace>
</Query>

//var chars = "97,167,54,178,2,11,209,174,119,248,254,0,255,1,64,190".ToCharArray().ToList()
//			.Append((char)17).Append((char)31).Append((char)73).Append((char)47).Append((char)23).Select(x => (int)x);

var chars = "97,167,54,178,2,11,209,174,119,248,254,0,255,1,64,190".ToCharArray()
			.Select(x => (int)x).Concat(new int[] {17, 31, 73, 47, 23});

var list = Enumerable.Range(0, 256).ToArray();

int loc = 0, skip = 0;

for (int round = 0; round < 64; round++)
{
	foreach (var len in chars)
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

list.Select((x,i) => new {ch = x, group = i / 16}).GroupBy(x => x.group).Select(x => x.Aggregate(0,(a, g) => a ^ g.ch)).Dump().Select(x => string.Format("{0:x02}",x)).Dump();

for (int i = 0; i < 16; i++)
{
	for (int j = 0; j < 16; j++)
	{
		dense[i] ^= list[16*i + j];
	}
}

string.Join("", dense.Select(x => string.Format("{0:x2}",x))).Dump();