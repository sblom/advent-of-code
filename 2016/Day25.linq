<Query Kind="Statements" />

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var prog = File.ReadAllLines("advent25.txt");

var linecount = prog.Length;

if (string.IsNullOrEmpty(prog[linecount - 1])) linecount--;

int curline = 0;

int[] regs = new int[4];

int m = 0, n = 0;

int step = 0;
bool next = false;

for (int i = 0; i < int.MaxValue; ++i)
{
	curline = 0;
	regs[0] = regs[1] = regs[2] = regs[3] = 0;
	regs[0] = i;
	step = 0;
	next = false;
	
	while (curline < linecount)
	{
		var stmt = prog[curline].Split(' ');
		switch (stmt[0])
		{
			case "cpy":
				if (curline == 7) { regs[0] += 2548; regs[3] = regs[0]; break; }
				Decode(stmt[2], ref n) = Decode(stmt[1], ref m);
				break;
			case "inc":
				Decode(stmt[1], ref m)++;
				break;
			case "dec":
				Decode(stmt[1], ref m)--;
				break;
			case "jnz":
				if (0 != Decode(stmt[1], ref m))
				{
					curline += (Decode(stmt[2], ref n) - 1);
				}
				break;
			case "out":
				if (regs[1] != (step++) % 2)
				{
					goto next_i;
				}
				if (step >= 100)
					i.Dump();
				break;
		}
		curline++;
	}
next_i: ;
}

regs.Dump();

ref int Decode(string str, ref int slot)
{
	if ("abcd".Contains(str[0]))
	{
		return ref regs[str[0] - 'a'];
	}
	else
	{
		slot = int.Parse(str);
		return ref slot;
	}
}