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

	var mem = (int[])origmem.Clone();
	ref int locA = ref mem[0];
	ref int locB = ref mem[0];
	ref int locC = ref mem[0];

	int ip = 0;

	while (ip >= 0 && ip < mem.Length)
	{
		var instr = mem[ip] % 100;
		var immA = (mem[ip] / 100) % 10 > 0;
		var immB = (mem[ip] / 1000) % 10 > 0;
		var immC = mem[ip] / 10000 > 0;

		var (mnem, op, instrLen) = ops[instr];

		if (instrLen > 1) if (immA) locA = ref mem[ip + 1]; else locA = ref mem[mem[ip + 1]];
		if (instrLen > 2) if (immB) locB = ref mem[ip + 2]; else locB = ref mem[mem[ip + 2]];
		if (instrLen > 3) if (immC) locC = ref mem[ip + 3]; else locC = ref mem[mem[ip + 3]];

		ip = op(ref locA, ref locB, ref locC, ip);
	}
}

delegate int Op(ref int A, ref int B, ref int C, int ip);

Dictionary<int, (string mnem, Op op, int instrLen)> ops = new Dictionary<int, (string, Op, int)>
{
	{ 1,  ("add",  (ref int A, ref int B, ref int C, int ip) => { C = A + B; return ip + 4; }, 4) },
	{ 2,  ("mul",  (ref int A, ref int B, ref int C, int ip) => { C = A * B; return ip + 4; }, 4) },
	{ 3,  ("in",   (ref int A, ref int B, ref int C, int ip) => { A = int.Parse(Console.ReadLine()); return ip + 2; }, 2) },
	{ 4,  ("out",  (ref int A, ref int B, ref int C, int ip) => { Console.WriteLine(A); return ip + 2; }, 2) },
	{ 5,  ("jnz",  (ref int A, ref int B, ref int C, int ip) => { if (A != 0) return B; else return ip + 3; }, 3) },
	{ 6,  ("jz",   (ref int A, ref int B, ref int C, int ip) => { if (A == 0) return B; else return ip + 3; }, 3) },
	{ 7,  ("lt",   (ref int A, ref int B, ref int C, int ip) => { if (A < B) C = 1; else C = 0; return ip + 4; }, 4) },
	{ 8,  ("eq",   (ref int A, ref int B, ref int C, int ip) => { if (A == B) C = 1; else C = 0; return ip + 4; }, 4) },
	{ 99, ("halt", (ref int A, ref int B, ref int C, int ip) => { return -1; }, 1) },
};