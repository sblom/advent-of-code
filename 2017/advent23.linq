<Query Kind="Statements" />

var lines = AoC.GetLines().ToList();

var registers = Enumerable.Range(0,8).Select(x => (char)('a' + x)).Select(x => new KeyValuePair<string,long>(x.ToString(), 0)).ToDictionary(x => x.Key, x=>x.Value);

long eval(string str)
{
	if (int.TryParse(str, out int i))
	{
		return i;
	}
	else
	{
		try
		{
			return registers[str];
		}
		catch
		{
			str.Dump();
			return 0;
		}
	}
}

int count = 0;

int pc = 0;


while (pc >= 0 && pc < lines.Count){
var toks = lines[pc].Split(' ').Select(x => x.Trim()).ToArray();

switch (toks[0])
{
	//snd X plays a sound with a frequency equal to the value of X.
	//set X Y sets register X to the value of Y.
	//add X Y increases register X by the value of Y.
	//mul X Y sets register X to the result of multiplying the value contained in register X by the value of Y.
	//mod X Y sets register X to the remainder of dividing the value contained in register X by the value of Y(that is, it sets X to the result of X modulo Y).
	//rcv X recovers the frequency of the last sound played, but only when the value of X is not zero. (If it is zero, the command does nothing.)
	//jgz X Y

	case "set":
		registers[toks[1]] = eval(toks[2]);
		break;
	case "sub":
		registers[toks[1]] -= eval(toks[2]);
		if (toks[1] == "h") registers["h"].Dump();		
		break;
	case "mul":
		count++;
		registers[toks[1]] *= eval(toks[2]);
		break;
	case "mod":
		registers[toks[1]] %= eval(toks[2]);
		break;
	case "jnz":
		if (eval(toks[1]) != 0)
		{
			pc += (int)eval(toks[2]);
			pc--;
		}
		break;
	default:
		"D'oh!".Dump();
		break;
}
pc++;
}

count.Dump("Part 1");  // Note, equals (b - 2)(b - 2) 

var h = 0;
var a = 1;
for (int b = a == 1 ? 107900 : 79, c = a == 1 ? b + 17000 : b; b <= c; b += 17)
{
	for (var d = 2; d * d <= b; d++)
	{
		var e = b / d;
		
		if (e * d == b)
		{
			h++;
			break;
		}
	}
}

/* Super literal version

var h = 0;
var a = 1;
for (int b = a == 1 ? 107900 : 79, c = a == 1 ? b + 17000 : b; b <= c; b += 17)
{
	var f = 1;
	for (var d = 2; d < b; d++)
	for (var e = 2; e < b; e++)
	{
		if (e * d == b)
		{
			f = 0;
		}
	}
	if (f == 0) h++;
}*/

h.Dump("Part 2");