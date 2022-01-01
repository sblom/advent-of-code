<Query Kind="Statements" />

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var prog = File.ReadAllLines("advent23.txt");

var linecount = prog.Length;
if (string.IsNullOrEmpty(prog[linecount - 1])) linecount--;

int curline = 0;

int[] regs = new int[4];

// Part 2
// regs[2] = 1;

regs[0] = 12;

int m = 0, n = 0;

while (curline < linecount)
{
	// line 2 - line 10: a = a * b; b = b - 1;
	if (curline == 2)
	{
		regs[0] = regs[0] * regs[1];
		regs[1]--;
		curline = 11;
		continue;
	}
	var stmt = prog[curline].Split(' ');
	switch (stmt[0])
	{
		case "tgl":
			var loc = curline + Decode(stmt[1],ref m);
			if (loc < 0 || loc >= prog.Length)
				break;
			var stmt2 = prog[loc].Split(' ');
			if (stmt2.Length == 2)
			{
				if (stmt2[0] == "inc") stmt2[0] = "dec";
				else stmt2[0] = "inc";
			}
			else if (stmt2.Length == 3)
			{
				if (stmt2[0] == "jnz") stmt2[0] = "cpy";
				else stmt2[0] = "jnz";
			}			
			prog[loc] = string.Join(" ", stmt2);
			break;
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