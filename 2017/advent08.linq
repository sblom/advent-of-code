<Query Kind="Statements" />

var curqp = Util.CurrentQueryPath;
var dirname = Path.GetDirectoryName(curqp);
Directory.SetCurrentDirectory(dirname);
var inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp) + ".txt");
var input = File.ReadLines(inputName);

var regs = new Dictionary<string,int>();

foreach (var line in input)
{
	var splits = line.Split(' ');
	
	regs[splits[0]] = 0;
	regs[splits[4]] = 0;
}

var max = 0;

void adjust(string reg, string op, int amt)
{
	if (op == "dec")
	  regs[reg] -= amt;
	else regs[reg] += amt;
	
	if (regs[reg] > max)
		max = regs[reg];
}

foreach (var line in input)
{
	var splits = line.Split(' ');

	switch(splits[5])
	{
		case "==":
			if (regs[splits[4]] == int.Parse(splits[6]))
			{
				adjust(splits[0],splits[1],int.Parse(splits[2]));
			}
		break;
		case ">":
			if (regs[splits[4]] > int.Parse(splits[6]))
			{
				adjust(splits[0], splits[1], int.Parse(splits[2]));
			}
			break;
		case ">=":
			if (regs[splits[4]] >= int.Parse(splits[6]))
			{
				adjust(splits[0], splits[1], int.Parse(splits[2]));
			}
			break;
		case "<":
			if (regs[splits[4]] < int.Parse(splits[6]))
			{
				adjust(splits[0], splits[1], int.Parse(splits[2]));
			}
			break;
		case "<=":
			if (regs[splits[4]] <= int.Parse(splits[6]))
			{
				adjust(splits[0], splits[1], int.Parse(splits[2]));
			}
			break;
		case "!=":
			if (regs[splits[4]] != int.Parse(splits[6]))
			{
				adjust(splits[0], splits[1], int.Parse(splits[2]));
			}
			break;
	}
}

regs.Values.Max().Dump("part 1");

max.Dump("part 2");