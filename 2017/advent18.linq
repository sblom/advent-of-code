<Query Kind="Statements" />

var lines = AoC.GetLines().ToArray();

//lines = @"snd 1
//snd 2
//snd p
//rcv a
//rcv b
//rcv c
//rcv d".Split('\n').Select(x => x.Trim()).ToArray();

var registers = new[] {Enumerable.Range(0,26).Select(x => (char)('a' + x)).Select(x => new KeyValuePair<string,long>(x.ToString(), 0)).ToDictionary(x => x.Key, x=>x.Value),
						Enumerable.Range(0,26).Select(x => (char)('a' + x)).Select(x => new KeyValuePair<string,long>(x.ToString(), 0)).ToDictionary(x => x.Key, x=>x.Value)};

var queue = new[] { new List<long>(), new List<long>() };

registers[0]["p"] = 0;
registers[1]["p"] = 1;
long[] pc = new long[]{0,0};


long eval(int pp, string str)
{
	if (int.TryParse(str, out int i))
	{
		return i;
	}
	else
	{
		try{
		return registers[pp][str];
		}
		catch {
		str.Dump();
		return 0;
		}
	}
}

int result = 0;
bool[] wait = new[] { false, false };

int p = 0;

while (true)
{
	while (pc[p] >= 0 && pc[p] < lines.Count())
	{
		wait[p] = false;
		var toks = lines[pc[p]].Split(' ').Select(x => x.Trim()).ToArray();
		switch (toks[0])
		{
			//snd X plays a sound with a frequency equal to the value of X.
			//set X Y sets register X to the value of Y.
			//add X Y increases register X by the value of Y.
			//mul X Y sets register X to the result of multiplying the value contained in register X by the value of Y.
			//mod X Y sets register X to the remainder of dividing the value contained in register X by the value of Y(that is, it sets X to the result of X modulo Y).
			//rcv X recovers the frequency of the last sound played, but only when the value of X is not zero. (If it is zero, the command does nothing.)
			//jgz X Y
			case "snd":
				if (p == 1) result++;
				queue[1 - p].Add(eval(p, toks[1]));
				if (wait[1 - p])
				{
					pc[p]++;
					goto sw;
				}
				break;
			case "set":
				registers[p][toks[1]] = eval(p, toks[2]);
				break;
			case "add":
				registers[p][toks[1]] += eval(p, toks[2]);
				break;
			case "mul":
				registers[p][toks[1]] *= eval(p, toks[2]);
				break;
			case "mod":
				registers[p][toks[1]] %= eval(p, toks[2]);
				break;
			case "rcv":
				if (queue[p].Count > 0)
				{
					registers[p][toks[1]] = queue[p][0];
					queue[p].RemoveAt(0);
				}
				else if (wait[1 - p])
				{
					result.Dump();
					break;
				}
				else
				{
					wait[p] = true;
					goto sw;
				}
				break;
			case "jgz":
				if (eval(p,toks[1]) > 0)
				{
					pc[p] += eval(p, toks[2]);
					pc[p]--;
				}
				break;
			default:
				"D'oh!".Dump();
				break;
		}
		pc[p]++;
	}
	wait[p] = true;
sw:
	p = 1 - p;
}