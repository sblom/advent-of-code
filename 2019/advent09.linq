<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <NuGetReference>System.Collections.Immutable</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	var lines = await AoC.GetLinesWeb();

	//lines = "3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0".Split("\n");
	//lines = "3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0".Split("\n");
	//lines = "3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0".Split("\n");
	//lines = "3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5".Split("\n");
	var mem = lines.First().Split(',').Select(n => long.Parse(n)).ToArray();
	Array.Resize(ref mem, 1_000_000);

	var io = new VMInOut { Inputs = new List<long> { 1, 2 } };
	var vm = new IntCodeVM(mem, io);
	
	while (vm.ExecuteNext());
	vm.Reset();
	while (vm.ExecuteNext());	
	
	io.Outputs.Dump();
	
	//for (int loc = 0; loc < mem.Length; )
	//{
	//	var (str, len) = vm.PrettyInstruction(loc);
	//	str.DumpFixed();
	//	loc += len;
	//}

	
}

public IEnumerable<IEnumerable<T>> Permute<T>(IEnumerable<T> @in)
{
	var items = ImmutableList.CreateRange(@in);
	var stack = ImmutableStack<(ImmutableList<T> cur, int pos, ImmutableList<T> acc)>.Empty;
	
	var (curitems, pos, acc) = (items, 0, ImmutableList<T>.Empty);
	
	while (true)
	{
		if (pos >= curitems.Count())
		{
			if (stack.Count() == 0) yield break;
			else if (curitems.Count() == 0) yield return acc;
			
			(curitems, pos, acc) = stack.Peek();
			pos = pos + 1;
			stack = stack.Pop();
		}
		else
		{
			stack = stack.Push((curitems,pos,acc));
			(curitems, pos, acc) = (curitems.RemoveAt(pos), 0, acc.Add(curitems[pos]));
		}
	}
}

public interface IInOut {
	public long Read();
	public void Write(long output);
}

public class VMInOut : IInOut
{
	public List<long> Inputs { get; set; }
	public List<long> Outputs { get; }
	
	public VMInOut()
	{
		Outputs = new List<long>();
	}
	
	public long Read()
	{
		var val = Inputs[0];
		Inputs.RemoveAt(0);
		return val;
	}

	public void Write(long output)
	{
		Outputs.Add(output);
	}
}

public class IntCodeVM
{
	private IInOut io;
	private long[] origmem;
	private long[] mem;
	
	private int ip = 0;
	private int rb = 0;
	
	delegate int Op(ref long A, ref long B, ref long C, int ip);
	
	private Dictionary<int, (string mnem, Op op, int instrLen)> ops;

	public IntCodeVM(long[] mem, IInOut io)
	{
		this.mem = (long[])mem.Clone();
		this.origmem = (long[])mem.Clone();
		this.io = io;

		ops = new Dictionary<int, (string, Op, int)>
		{
			{ 1,  ("add",  (ref long A, ref long B, ref long C, int ip) => { C = A + B; return ip + 4; }, 4) },
			{ 2,  ("mul",  (ref long A, ref long B, ref long C, int ip) => { C = A * B; return ip + 4; }, 4) },
			{ 3,  ("in",   (ref long A, ref long B, ref long C, int ip) => { A = io.Read(); return ip + 2; }, 2) },
			{ 4,  ("out",  (ref long A, ref long B, ref long C, int ip) => { io.Write(A); return ip + 2; }, 2) },
			{ 5,  ("jnz",  (ref long A, ref long B, ref long C, int ip) => { if (A != 0) return (int)B; else return ip + 3; }, 3) },
			{ 6,  ("jz",   (ref long A, ref long B, ref long C, int ip) => { if (A == 0) return (int)B; else return ip + 3; }, 3) },
			{ 7,  ("lt",   (ref long A, ref long B, ref long C, int ip) => { if (A < B) C = 1; else C = 0; return ip + 4; }, 4) },
			{ 8,  ("eq",   (ref long A, ref long B, ref long C, int ip) => { if (A == B) C = 1; else C = 0; return ip + 4; }, 4) },
			{ 9,  ("chrb", (ref long A, ref long B, ref long C, int ip) => { rb += (int)A; return ip + 2; }, 2) },

			{ 99, ("halt", (ref long A, ref long B, ref long C, int ip) => { return -1; }, 1) },
		};
	}
	
	public (string, int) PrettyInstruction(int loc)
	{
		var instr = (int)(mem[loc] % 100);
		bool[] imms = new bool[] {
			(mem[loc] / 100) % 10 > 0,
			(mem[loc] / 1000) % 10 > 0,
			mem[loc] / 10000 > 0
		};

		var (mnem, op, instrLen) = ops[instr];
		
		StringBuilder sb = new StringBuilder();
		
		sb.Append(string.Format("{0:0000}", loc));
		sb.Append(" ");
		sb.Append(string.Format("{0,-4}", mnem));
		for (int i = 1; i < instrLen; ++i)
		{
			sb.Append(" ");
			if (!imms[i-1]) sb.Append("*");
			else sb.Append(" ");
			sb.Append(string.Format("{0:0000}", mem[loc + i]));
		}
		
		return (sb.ToString(), instrLen);
	}

	public void Reset()
	{
		rb = 0;
		ip = 0;
		mem = (long[])origmem.Clone();
	}

	public bool ExecuteNext()
	{
		long crap = 0;
		ref long locA = ref crap;
		ref long locB = ref crap;
		ref long locC = ref crap;

		var instr = (int)(mem[ip] % 100);
		var modeA = (mem[ip] / 100) % 10;
		var modeB = (mem[ip] / 1000) % 10;
		var modeC = mem[ip] / 10000;

		var (mnem, op, instrLen) = ops[instr];

		if (instrLen > 1) if (modeA == 1) locA = ref mem[ip + 1]; else if (modeA == 2) locA = ref mem[rb + mem[ip + 1]]; else locA = ref mem[mem[ip + 1]];
		if (instrLen > 2) if (modeB == 1) locB = ref mem[ip + 2]; else if (modeB == 2) locB = ref mem[rb + mem[ip + 2]]; else locB = ref mem[mem[ip + 2]];
		if (instrLen > 3) if (modeC == 1) locC = ref mem[ip + 3]; else if (modeC == 2) locC = ref mem[rb + mem[ip + 3]]; else locC = ref mem[mem[ip + 3]];

		ip = op(ref locA, ref locB, ref locC, ip);
		
		return (ip > 0 && ip < mem.Length);
	}
}