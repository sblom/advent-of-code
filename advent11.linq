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

	var mem = lines.First().Split(',').Select(n => long.Parse(n)).ToArray();
	Array.Resize(ref mem, 1_000_000);

	int[] loc = new int[2];

	var panels = new Dictionary<(int,int), int>();
	var io = new VMInOut(panels, loc);
// Bug 2: Extra inputs on VMInOut from day 9
	var vm = new IntCodeVM(mem, io);

	var (x, y) = (0, 0);
	var (dx, dy) = (0, -1);
	
	(int, int) turn (int t, (int dx,int dy)ds)
	{
		return t switch {
			1 => (-dy, dx),
			0 => (dy, -dx)
// Bug 1: Missing neg ^ sign
		};
	}

	int c = 0;

	while (vm.ExecuteNext())
	{
		var color = 0;
		if (io.Outputs.Count > 0)
		{
			if (c++ % 2 == 0)
			{
				color = (int)io.Outputs[0];
				io.Outputs.RemoveAt(0);
				panels[(x,y)] = color;
			}
			else
			{
				(dx, dy) = turn((int)io.Outputs[0], (dx, dy));
				(x, y) = (x + dx, y + dy);
				io.Loc[0] = x; io.Loc[1] = y;
				//(dx,dy).ToString().Dump();
				//(x,y).ToString().Dump();
				io.Outputs.RemoveAt(0);
				//DispShip(panels);
			}
		}
	}
	
	panels.Count.Dump("Part 1");
	DispShip(panels);
}

public void DispShip(Dictionary<(int,int),int> panels)
{
	var minx = panels.Keys.Min(x => x.Item1);
	var miny = panels.Keys.Min(x => x.Item2);
	var maxx = panels.Keys.Max(x => x.Item1);
	var maxy = panels.Keys.Max(x => x.Item2);
	
	"".Dump("Next");
	
	for (int i = miny; i <= maxy; i++)
	{
		string line = "";
		for (int j = minx; j <= maxx; j++)
		{
			line += panels.ContainsKey((j,i)) ? (panels[(j,i)] == 1 ? '#' : '.') : '.';
		}
		line.DumpFixed();
	}
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
			stack = stack.Push((curitems, pos, acc));
			(curitems, pos, acc) = (curitems.RemoveAt(pos), 0, acc.Add(curitems[pos]));
		}
	}
}

public interface IInOut
{
	public long Read();
	public void Write(long output);
}

public class VMInOut : IInOut
{
	public List<long> Outputs { get; }
	public Dictionary<(int,int),int> Surface;
	public int[] Loc;
	int c = 0;

	public VMInOut(Dictionary<(int,int),int> surface, int[] loc)
	{
		Outputs = new List<long>();
		Surface = surface;
		Loc = loc;
	}

	public long Read()
	{
		if (c++ == 0) return 1;
		return Surface.ContainsKey((Loc[0],Loc[1])) ? Surface[(Loc[0],Loc[1])] : 0;
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
			if (!imms[i - 1]) sb.Append("*");
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