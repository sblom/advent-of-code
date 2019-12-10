<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	var rx = new Regex(@"\w+:\s*\[(\d+), (\d+), (\d+), (\d+)\]");
	var ox = new Regex(@"(\d+) (\d+) (\d+) (\d+)");

	var lines = (await AoC.GetLinesWeb()).ToList();

	var possibles = new Dictionary<int, HashSet<string>>();

	int[] regs = new[] {0,0,0,0};
	
	var total = 0;

	for (int i = 0; lines[i].StartsWith("Before:"); i += 4)
	{
		var before = rx.Match(lines[i]);
		var beforevals = new[]{int.Parse(before.Groups[1].Value), int.Parse(before.Groups[2].Value), int.Parse(before.Groups[3].Value), int.Parse(before.Groups[4].Value)};
		var after = rx.Match(lines[i+2]);
		var aftervals = new[]{int.Parse(after.Groups[1].Value), int.Parse(after.Groups[2].Value), int.Parse(after.Groups[3].Value), int.Parse(after.Groups[4].Value)};
		var ops = ox.Match(lines[i+1]);
		var opsvals = new{op = int.Parse(ops.Groups[1].Value), a = int.Parse(ops.Groups[2].Value), b = int.Parse(ops.Groups[3].Value), c = int.Parse(ops.Groups[4].Value)};

		var candidates = new List<string>();
		foreach (var opcode in opcodes)
		{
			beforevals.CopyTo(regs,0);
			opcode.Value(regs,opsvals.a,opsvals.b,opsvals.c);
			if (regs[0] == aftervals[0] && regs[1] == aftervals[1] && regs[2] == aftervals[2] && regs[3] == aftervals[3])
			{
				candidates.Add(opcode.Key);
			}
		}
		if (!possibles.ContainsKey(opsvals.op))
		{
			possibles[opsvals.op] = new HashSet<string>(candidates);
		}
		else
		{
			if (possibles[opsvals.op].Count > candidates.Count())
			{
				possibles[opsvals.op].Dump();
				candidates.Dump();
			}
			possibles[opsvals.op].IntersectWith(candidates);
		}

		if (candidates.Count >= 3)
			total++;
	}
	total.Dump("Part 1");
	
	IEnumerable<string> resolved;
	
	do
	{
		resolved = possibles.Where(kv => kv.Value.Count() == 1).Select(kv => kv.Value.First()).ToList();
		foreach (var possible in possibles)
		{
			if (possible.Value.Count == 1) continue;
			possible.Value.ExceptWith(resolved);
		}
	} while (resolved.Count() < possibles.Count());
	
	// Find the first line in our input with two instructions in a row.
	var lineno = 0;
	while (!ox.IsMatch(lines[lineno]) || !ox.IsMatch(lines[lineno+1]))
	{
		lineno++;
	}
	
	regs = new[] {0,0,0,0};
	
	for (;lineno < lines.Count; lineno++)
	{
		var decode = ox.Match(lines[lineno]);
		var decvals = new{op = int.Parse(decode.Groups[1].Value), a = int.Parse(decode.Groups[2].Value), b = int.Parse(decode.Groups[3].Value), c = int.Parse(decode.Groups[4].Value)};
		opcodes[possibles[decvals.op].First()](regs, decvals.a, decvals.b, decvals.c);
	}
	
	regs[0].Dump("Part 2");
}

Dictionary<string, Action<int[], int, int, int>> opcodes = new Dictionary<string, Action<int[], int, int, int>>
	{
		{"addr", (regs,a,b,c)=>{regs[c] = regs[a] + regs[b];}},
		{"addi", (regs,a,b,c)=>{regs[c] = regs[a] + b;}},
		{"mulr", (regs,a,b,c)=>{regs[c] = regs[a] * regs[b];}},
		{"muli", (regs,a,b,c)=>{regs[c] = regs[a] * b;}},
		{"banr", (regs,a,b,c)=>{regs[c] = regs[a] & regs[b];}},
		{"bani", (regs,a,b,c)=>{regs[c] = regs[a] & b;}},
		{"borr", (regs,a,b,c)=>{regs[c] = regs[a] | regs[b];}},
		{"bori", (regs,a,b,c)=>{regs[c] = regs[a] | b;}},
		{"setr", (regs,a,b,c)=>{regs[c] = regs[a];}},
		{"seti", (regs,a,b,c)=>{regs[c] = a;}},
		{"gtir", (regs,a,b,c)=>{regs[c] = a > regs[b] ? 1 : 0;}},
		{"gtri", (regs,a,b,c)=>{regs[c] = regs[a] > b ? 1 : 0;}},
		{"gtrr", (regs,a,b,c)=>{regs[c] = regs[a] > regs[b] ? 1 : 0;}},
		{"eqir", (regs,a,b,c)=>{regs[c] = a == regs[b] ? 1 : 0;}},
		{"eqri", (regs,a,b,c)=>{regs[c] = regs[a] == b ? 1 : 0;}},
		{"eqrr", (regs,a,b,c)=>{regs[c] = regs[a] == regs[b] ? 1 : 0;}},
	};

// Define other methods and classes here