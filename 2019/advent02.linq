<Query Kind="Statements">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
</Query>

#load "..\lib\Utils"

var lines = await AoC.GetLinesWeb();
//lines = new[] { @"1,1,1,4,99,5,6,0,99"};

var rx = new Regex("");

var origmem = lines.First().Split(',').Select(x => int.Parse(x)).ToArray();

var mem1 = (int[])origmem.Clone();
mem1[1] = 12;
mem1[2] = 2;
int loc = 0;

while (mem1[loc] != 99)
{
	switch (mem1[loc])
	{
		case 1:
			mem1[mem1[loc + 3]] = mem1[mem1[loc + 1]] + mem1[mem1[loc + 2]];
			loc += 4;
			break;

		case 2:
			mem1[mem1[loc + 3]] = mem1[mem1[loc + 1]] * mem1[mem1[loc + 2]];
			loc += 4;
			break;
	}
}

mem1[0].Dump("Part 1");

for (int j = 0; j < 100; j++)
{
	for (int i = 0; i < 100; i++)
	{

		var mem = (int[])origmem.Clone();
		mem[1] = i;
		mem[2] = j;
		loc = 0;

		while (mem[loc] != 99)
		{
			switch (mem[loc])
			{
				case 1:
					mem[mem[loc + 3]] = mem[mem[loc + 1]] + mem[mem[loc + 2]];
					loc += 4;
					break;

				case 2:
					mem[mem[loc + 3]] = mem[mem[loc + 1]] * mem[mem[loc + 2]];
					loc += 4;
					break;
			}
		}
		
		if (mem[0] == 19690720)
		{
			(i * 100 + j).Dump("Part 2");
			return;
		}
	}
}