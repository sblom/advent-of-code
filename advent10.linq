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
	

//	lines = @".#....#.###.........#..##.###.#.....##...
//...........##.......#.#...#...#..#....#..
//...#....##..##.......#..........###..#...
//....#....####......#..#.#........#.......
//...............##..#....#...##..#...#..#.
//..#....#....#..#.....#.#......#..#...#...
//.....#.#....#.#...##.........#...#.......
//#...##.#.#...#.......#....#........#.....
//....##........#....#..........#.......#..
//..##..........##.....#....#.........#....
//...#..##......#..#.#.#...#...............
//..#.##.........#...#.#.....#........#....
//#.#.#.#......#.#...##...#.........##....#
//.#....#..#.....#.#......##.##...#.......#
//..#..##.....#..#.........#...##.....#..#.
//##.#...#.#.#.#.#.#.........#..#...#.##...
//.#.....#......##..#.#..#....#....#####...
//........#...##...#.....#.......#....#.#.#
//#......#..#..#.#.#....##..#......###.....
//............#..#.#.#....#.....##..#......
//...#.#.....#..#.......#..#.#............#
//.#.#.....#..##.....#..#..............#...
//.#.#....##.....#......##..#...#......#...
//.......#..........#.###....#.#...##.#....
//.....##.#..#.....#.#.#......#...##..#.#..
//.#....#...#.#.#.......##.#.........#.#...
//##.........#............#.#......#....#..
//.#......#.............#.#......#.........
//.......#...##........#...##......#....#..
//#..#.....#.#...##.#.#......##...#.#..#...
//#....##...#.#........#..........##.......
//..#.#.....#.....###.#..#.........#......#
//......##.#...#.#..#..#.##..............#.
//.......##.#..#.#.............#..#.#......
//...#....##.##..#..#..#.....#...##.#......
//#....#..#.#....#...###...#.#.......#.....
//.#..#...#......##.#..#..#........#....#..
//..#.##.#...#......###.....#.#........##..
//#.##.###.........#...##.....#..#....#.#..
//..........#...#..##..#..##....#.........#
//..#..#....###..........##..#...#...#..#..".Split("\n").Select(l => l.Trim());

	var lineList = lines.ToList();

	var asteroids = new List<(int x, int y)>();

	for (int y = 0; y < lineList.Count; ++y)
	{
		for (int x = 0; x < lineList[0].Length; ++x)
		{
			if (lineList[y][x] == '#') asteroids.Add((x, y));
		}
	}

	var lc = new List<(int, (int, int))>();

	foreach (var A in asteroids)
	{
		var slopes = asteroids.Where(B => B != A).Select(B => (-(B.y - A.y), B.x - A.x));
		var simplified = slopes.GroupBy(N => N.Item1 == 0 ? (0, Math.Sign(N.Item2)) : N.Item2 == 0 ? (-Math.Sign(N.Item1), 0) : (N.Item1 / GCD(Math.Abs(N.Item1), Math.Abs(N.Item2)), N.Item2 / GCD(Math.Abs(N.Item1), Math.Abs(N.Item2))));

		//var atans = slopes.GroupBy(N => Math.Atan2(N.Item1,N.Item2));

		lc.Add((simplified.Count(), A));
	}

	var res1 = lc.Max().Dump();

	(int x, int y) A2 = (res1.Item2.Item1, res1.Item2.Item2);
	var slopes2 = asteroids.Where(B => B != A2).Select(B => (-(B.y - A2.y), B.x - A2.x));
	var simplified2 = slopes2.GroupBy(N => N.Item1 == 0 ? (0, Math.Sign(N.Item2)) : N.Item2 == 0 ? (-Math.Sign(N.Item1), 0) : (N.Item1 / GCD(Math.Abs(N.Item1), Math.Abs(N.Item2)), N.Item2 / GCD(Math.Abs(N.Item1), Math.Abs(N.Item2))));
	var atans = slopes2.GroupBy(N => Math.Atan2(N.Item1, N.Item2)).Select(x => (angle: x.Key, asteroids: x.OrderBy(z => z.Item1 * z.Item1 + z.Item2 * z.Item2).ToList())).OrderBy(x => x.angle).Reverse().ToList();

	var skip = atans.FindIndex(x => x.angle == atans.Where(a => a.angle < Math.PI / 2 + 0.0001).First().angle).Dump();

	var atansFixed = atans.Skip(skip).Concat(atans.Take(skip)).Skip(199).First().Dump();

	var res2 = (res1.Item2.Item1 + atansFixed.Item2.First().Item2, res1.Item2.Item2 - atansFixed.Item2.First().Item1).ToString().Dump();

	// Holy shit. I can't believe Part 2 worked on my first try.
}

int GCD(int a, int b)
{
	while (b > 0)
	{
		int rem = a % b;
		a = b;
		b = rem;
	}
	return a;
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