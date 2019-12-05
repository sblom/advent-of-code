<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
</Query>

void Main()
{
	var lines = AoC.GetLinesWeb().Result;
	//lines = new[] { @"3,0,4,0,99"};
	//lines = new[] { @"1002,4,3,4,33" };
	//lines = new[] { @"1101,100,-1,4,0"};
	//lines = new[] { @"3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99" };

	var rx = new Regex("");

	var origmem = lines.First().Split(',').Select(x => int.Parse(x)).ToArray();

	var mem1 = (int[])origmem.Clone();
	ref int locA = ref mem1[0];
	ref int locB = ref mem1[0];
	ref int locC = ref mem1[0];

	int loc = 0;

	while (mem1[loc] != 99)
	{
		var instr = mem1[loc] % 100;
		var immA = (mem1[loc] / 100) % 10 > 0;
		var immB = (mem1[loc] / 1000) % 10 > 0;
		var immC = mem1[loc] / 10000 > 0;

		var (op, len) = ops[instr];

		if (len > 1) if (immA) locA = ref mem1[loc + 1]; else locA = ref mem1[mem1[loc + 1]];
		if (len > 2) if (immB) locB = ref mem1[loc + 2]; else locB = ref mem1[mem1[loc + 2]];
		if (len > 3) if (immC) locC = ref mem1[loc + 3]; else locC = ref mem1[mem1[loc + 3]];

		loc = op(ref locA, ref locB, ref locC, loc);
	}
}

delegate int Op(ref int A, ref int B, ref int C, int loc);

Dictionary<int, (Op op, int @params)> ops = new Dictionary<int, (Op, int)>
{
	{ 1, ((ref int A, ref int B, ref int C, int loc) => { C = A + B; return loc + 4; }, 4) },
	{ 2, ((ref int A, ref int B, ref int C, int loc) => { C = A * B; return loc + 4; }, 4) },
	{ 3, ((ref int A, ref int B, ref int C, int loc) => { A = int.Parse(Console.ReadLine()); return loc + 2; }, 2) },
	{ 4, ((ref int A, ref int B, ref int C, int loc) => { Console.Write(A); return loc + 2; }, 2) },
	{ 5, ((ref int A, ref int B, ref int C, int loc) => { if (A != 0) return B; else return loc + 3; }, 3) },
	{ 6, ((ref int A, ref int B, ref int C, int loc) => { if (A == 0) return B; else return loc + 3; }, 3) },
	{ 7, ((ref int A, ref int B, ref int C, int loc) => { if (A < B) C = 1; else C = 0; return loc + 4; }, 4) },
	{ 8, ((ref int A, ref int B, ref int C, int loc) => { if (A == B) C = 1; else C = 0; return loc + 4; }, 4) },
};