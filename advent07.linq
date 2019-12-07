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
	var mem = lines.First().Split(',').Select(n => int.Parse(n)).ToArray();

	foreach (var perm in Permute(Enumerable.Range(0,5))
	{
		var in = 0;
		for (int i = 0; i < 5; ++i)
		{
			new IntCodeVM(mem, new VMInOut { Inputs = new List<int> { perm[0], 0} }),
		}
	}
}

public IEnumerable<IEnumerable<T>> Permute<T>(IEnumerable<T> @in)
{
	var items = ImmutableList.Create(@in);
	var stack = ImmutableStack<(ImmutableList<T>,int)>.Empty;
	
	var (curitems, pos) = (items, 0);
	
	while (true)
	{
		if (pos > curitems.Count())
		{
			(curitems, pos) = stack.Peek();
			stack = stack.Pop();
		}
	}
}

public interface IInOut {
	public int Read();
	public void Write(int output);
}

public class VMInOut : IInOut
{
	public List<int> Inputs { get; set; }
	public List<int> Outputs { get; }
	
	public VMInOut()
	{
		Outputs = new List<int>();
	}
	
	public int Read()
	{
		var val = Inputs[0];
		Inputs.RemoveAt(0);
		return val;
	}

	public void Write(int output)
	{
		Outputs.Add(output);
	}
}

public class IntCodeVM
{
	private IInOut io;
	private int[] origmem;
	private int[] mem;
	
	private int ip = 0;
	
	delegate int Op(ref int A, ref int B, ref int C, int ip);
	
	private Dictionary<int, (string mnem, Op op, int instrLen)> ops;

	public IntCodeVM(int[] mem, IInOut io)
	{
		this.mem = (int[])mem.Clone();
		this.origmem = (int[])mem.Clone();
		this.io = io;

		ops = new Dictionary<int, (string, Op, int)>
		{
			{ 1,  ("add",  (ref int A, ref int B, ref int C, int ip) => { C = A + B; return ip + 4; }, 4) },
			{ 2,  ("mul",  (ref int A, ref int B, ref int C, int ip) => { C = A * B; return ip + 4; }, 4) },
			{ 3,  ("in",   (ref int A, ref int B, ref int C, int ip) => { A = io.Read(); return ip + 2; }, 2) },
			{ 4,  ("out",  (ref int A, ref int B, ref int C, int ip) => { io.Write(A); return ip + 2; }, 2) },
			{ 5,  ("jnz",  (ref int A, ref int B, ref int C, int ip) => { if (A != 0) return B; else return ip + 3; }, 3) },
			{ 6,  ("jz",   (ref int A, ref int B, ref int C, int ip) => { if (A == 0) return B; else return ip + 3; }, 3) },
			{ 7,  ("lt",   (ref int A, ref int B, ref int C, int ip) => { if (A < B) C = 1; else C = 0; return ip + 4; }, 4) },
			{ 8,  ("eq",   (ref int A, ref int B, ref int C, int ip) => { if (A == B) C = 1; else C = 0; return ip + 4; }, 4) },
			{ 99, ("halt", (ref int A, ref int B, ref int C, int ip) => { return -1; }, 1) },
		};
	}
	
	public void Reset()
	{
		ip = 0;
		mem = (int[])origmem.Clone();
	}

	public bool ExecuteNext()
	{
		ref int locA = ref mem[0];
		ref int locB = ref mem[0];
		ref int locC = ref mem[0];

		var instr = mem[ip] % 100;
		var immA = (mem[ip] / 100) % 10 > 0;
		var immB = (mem[ip] / 1000) % 10 > 0;
		var immC = mem[ip] / 10000 > 0;

		var (mnem, op, instrLen) = ops[instr];

		if (instrLen > 1) if (immA) locA = ref mem[ip + 1]; else locA = ref mem[mem[ip + 1]];
		if (instrLen > 2) if (immB) locB = ref mem[ip + 2]; else locB = ref mem[mem[ip + 2]];
		if (instrLen > 3) if (immC) locC = ref mem[ip + 3]; else locC = ref mem[mem[ip + 3]];

		ip = op(ref locA, ref locB, ref locC, ip);
		
		return (ip > 0 && ip < mem.Length);
	}
}