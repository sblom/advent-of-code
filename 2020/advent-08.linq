<Query Kind="Statements">
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>RegExtract</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

var lines = AoC.GetLines().Extract<(string, int)>(@"(...) ([-+]\d+)").ToArray();

var (_, res) = Run(lines);

res.Dump("Part 1");

for (int i = 0; i < lines.Length; i++)
{
	var instrs = ((string,int)[])lines.Clone();
	
	if (instrs[i].Item1 == "jmp")
	{
		instrs[i].Item1 = "nop";		
	}
	else if (instrs[i].Item1 == "nop")
	{
		instrs[i].Item1 = "jmp";
	}
	else
	{
		continue;
	}
	
	if (Run(instrs) is (true, var acc))
	{
		acc.Dump("Part 1");
		break;
	}
}

static (bool success, int acc) Run((string,int)[] instrs)
{
	var visited = new HashSet<int>();
	int pc = 0, acc = 0;

	while (true)
	{
		if (pc >= instrs.Length)
		{
			return (true, acc);
		}
		else if (pc < 0 || visited.Contains(pc))
		{
			return (false, acc);
		}
		
		visited.Add(pc);
		
		switch (instrs[pc])
		{
			case ("nop", _):
				break;
			case ("acc", int arg):
				acc += arg;
				break;
			case ("jmp", int arg):
				pc += arg - 1;
				break;
		}

		pc++;
	}
}