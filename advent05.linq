<Query Kind="Statements">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
</Query>

var lines = await AoC.GetLinesWeb();
//lines = new[] { @"3,0,4,0,99"};
//lines = new[] { @"1002,4,3,4,33" };
//lines = new[] { @"1101,100,-1,4,0"};
//lines = new[] { @"3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99" };

var rx = new Regex("");

var origmem = lines.First().Split(',').Select(x => int.Parse(x)).ToArray();

var mem1 = (int[])origmem.Clone();
int loc = 0;

while (mem1[loc] != 99)
{
	var instr = mem1[loc] % 100;
	var immC = mem1[loc] / 10000 > 0;
	var immB = (mem1[loc] / 1000) % 10 > 0;
	var immA = (mem1[loc] / 100) % 10 > 0;

	switch (instr)
	{
		case 1:
			mem1[mem1[loc + 3]] = (immA ? mem1[loc + 1] : mem1[mem1[loc + 1]]) + (immB ? mem1[loc + 2] : mem1[mem1[loc + 2]]);
			loc += 4;
			break;
		case 2:
			mem1[mem1[loc + 3]] = (immA ? mem1[loc + 1] : mem1[mem1[loc + 1]]) * (immB ? mem1[loc + 2] : mem1[mem1[loc + 2]]);
			loc += 4;
			break;
		case 3:
			mem1[mem1[loc + 1]] = int.Parse(Util.ReadLine());
			loc += 2;
			break;
		case 4:
			Console.Write(immA ? mem1[loc + 1] : mem1[mem1[loc + 1]] );
			loc += 2;
			break;
		case 5:
			if ((immA ? mem1[loc + 1] : mem1[mem1[loc + 1]]) != 0)
				loc = immB ? mem1[loc + 2] : mem1[mem1[loc + 2]];
			else
				loc += 3;
			break;
		case 6:
			if ((immA ? mem1[loc + 1] : mem1[mem1[loc + 1]]) == 0)
				loc = immB ? mem1[loc + 2] : mem1[mem1[loc + 2]];
			else
				loc += 3;
			break;
		case 7:
			if ((immA ? mem1[loc + 1] : mem1[mem1[loc + 1]]) < (immB ? mem1[loc + 2] : mem1[mem1[loc + 2]]))
				mem1[mem1[loc + 3]] = 1;
			else
				mem1[mem1[loc + 3]] = 0;
			loc += 4;
			break;
		case 8:
			if ((immA ? mem1[loc + 1] : mem1[mem1[loc + 1]]) == (immB ? mem1[loc + 2] : mem1[mem1[loc + 2]]))
				mem1[mem1[loc + 3]] = 1;
			else
				mem1[mem1[loc + 3]] = 0;
			loc += 4;
			break;
	}
}

mem1.Dump();