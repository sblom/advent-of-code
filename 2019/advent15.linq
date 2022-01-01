<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

async Task Main()
{
	var lines = await AoC.GetLinesWeb();

	var mem = lines.First().Split(',').Select(n => long.Parse(n)).ToArray();
	Array.Resize(ref mem, 25_000);

	var prog = new FindAC(mem);
	
	prog.Solve();
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

public class FindAC : IntCodeVM.IInOut
{
	Dictionary<(int, int), int> map = new Dictionary<(int, int), int> { { (0, 0), 1 } };
	IntCodeVM vm;
	
	IImmutableStack<((int x, int y) loc, int nextdir)> stack = ImmutableStack<((int,int),int)>.Empty.Push(((0,0),0));
	
	(int x, int y)[] dirs = new[] {
		(0, 0),
		(0, -1),
		(0, 1),
		(-1, 0),
		(1, 0),
	};
	
	public FindAC(long[] mem)
	{
		vm = new IntCodeVM(mem, this);
	}
	
	public void Solve()
	{
		while (vm.ExecuteNext()) {
			if (stack.IsEmpty)
			{
				map.Dump("Part 1");
			}
		}
	}
	
	public long Read()
	{
		var cur = stack.Peek();
		// If we've checked everywhere from our current direction, go back to previous cell.
		for (int i = cur.nextdir + 1; i <= 4; i++)
		{
			var next = (cur.loc.x + dirs[i].x, cur.loc.y + dirs[i].y);
			if (map.ContainsKey(next))
			{
				continue;
			}
			else
			{
				stack = stack.Pop().Push((cur.loc, i));
				stack = stack.Push((next, 0));
				return i;
			}
		}
		stack = stack.Pop();
		if (stack.IsEmpty)
		{
			Render().DumpFixed();

			var visited = new HashSet<(int, int)> { (0, 0) };
			var frontier = new List<((int, int), int)> { ((0,0), 0) };
			
			while (frontier.Any())
			{
				var front = frontier[0];
				if (map[front.Item1] == 2)
					front.Item2.Dump("Part 1");
				frontier.RemoveAt(0);
				for (int i = 1; i <= 4; i++)
				{
					if (!visited.Contains((front.Item1.Item1 + dirs[i].x, front.Item1.Item2 + dirs[i].y)) && map[(front.Item1.Item1 + dirs[i].x, front.Item1.Item2 + dirs[i].y)] != 0)
					{
						frontier.Add(((front.Item1.Item1 + dirs[i].x, front.Item1.Item2 + dirs[i].y), front.Item2 + 1));
						visited.Add((front.Item1.Item1 + dirs[i].x, front.Item1.Item2 + dirs[i].y));
					}
				}
			}

			var oxygenLoc = map.Where(kv => kv.Value == 2).First().Key;
			visited = new HashSet<(int, int)> { oxygenLoc };
			frontier = new List<((int, int), int)> { (oxygenLoc, 0) };
			
			int max = 0;

			while (frontier.Any())
			{
				var front = frontier[0];
				max = front.Item2;
				//if (map[front.Item1] == 2)
				//	front.Item2.Dump("Part 1");
				frontier.RemoveAt(0);
				for (int i = 1; i <= 4; i++)
				{
					if (!visited.Contains((front.Item1.Item1 + dirs[i].x, front.Item1.Item2 + dirs[i].y)) && map[(front.Item1.Item1 + dirs[i].x, front.Item1.Item2 + dirs[i].y)] != 0)
					{
						frontier.Add(((front.Item1.Item1 + dirs[i].x, front.Item1.Item2 + dirs[i].y), front.Item2 + 1));
						visited.Add((front.Item1.Item1 + dirs[i].x, front.Item1.Item2 + dirs[i].y));
					}
				}
			}
			max.Dump("Part 2");
			Environment.Exit(0);
		}
		return stack.Peek().nextdir switch {
			1 => 2, 2 => 1, 3 => 4, 4 => 3
		};
	}
	
	public string Render()
	{
		var minx = map.Keys.Min(x => x.Item1);
		var maxx = map.Keys.Max(x => x.Item1);
		var miny = map.Keys.Min(x => x.Item1);				
		var maxy = map.Keys.Max(x => x.Item1);		
		
		var sb = new StringBuilder();
		
		for (int i = miny; i <= maxy; i++)
		{
			for (int j = minx; j <= maxx; j++)
			{
				if (!map.ContainsKey((j,i)))
				{
					sb.Append('+');
					continue;
				}
				sb.Append(map[(j,i)] switch {
					0 => '+',
					1 => ' ',
					2 => '#'
				});
			}
			sb.Append("\n");
		}
		return sb.ToString();
	}

	public void Write(long output)
	{
		var cur = stack.Peek();
		switch (output)
		{
			case 0: map[cur.loc] = 0;
				stack = stack.Pop();
				break;
			case 1: map[cur.loc] = 1;
				break;
			case 2: map[cur.loc] = 2;
				"found it!".Dump();
				break;
		}
	}
}

public class IntCodeVM
{
	public interface IInOut
	{
		public long Read();
		public void Write(long output);
	}
	
	private IInOut io;
	private long[] origmem;
	public long[] mem;

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

		return (ip >= 0 && ip < mem.Length);
	}
}