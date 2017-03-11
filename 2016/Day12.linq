<Query Kind="Statements" />

Directory.SetCurrentDirectory (Path.GetDirectoryName (Util.CurrentQueryPath));

var prog = File.ReadAllLines("advent12.txt");

var linecount = prog.Length;
if (string.IsNullOrEmpty(prog[linecount - 1])) linecount--;

int curline = 0;

int[] regs = new int[4];

// Part 2
// regs[2] = 1;

int m = 0, n = 0;

while (curline < linecount)
{
	var stmt = prog[curline].Split(' ');
	switch (stmt[0])
	{
		case "cpy":
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
	}
	curline++;
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