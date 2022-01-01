<Query Kind="Statements" />

var lines = (await AoC.GetLinesWeb()).ToList();

int ip = 0, a = 0, b = 0;

for (int i = 0; i < 2; i++)
{
	ip = 0; a = i; b = 0;
	while (ip >= 0 && ip < lines.Count())
	{
		var instr = lines[ip].Split(' ');
		switch (instr[0])
		{
			case "hlf":
				if (instr[1] == "a"){
					a /= 2;
				}
				else
				{
					b /= 2;
				}
				ip++;
				break;
			case "tpl":
				if (instr[1] == "a")
				{
					a *= 3;
				}
				else
				{
					b *= 3;
				}
				ip++;
				break;
			case "inc":
				if (instr[1] == "a")
				{
					a += 1;
				}
				else
				{
					b += 1;
				}
				ip++;
				break;
			case "jmp":
				var o = int.Parse(instr[1]);
				ip += o;
				break;
			case "jie":
				bool iseven = false;
				if (instr[1] == "a,")
				{
					iseven = a % 2 == 0;
				}
				else
				{
					iseven = b % 2 == 0;
				}
				var o2 = int.Parse(instr[2]);
				if (iseven)
				{
					ip += o2;
				}
				else
				{
					ip++;
				}
				break;
			case "jio":
				bool isone = false;
				if (instr[1] == "a,")
				{
					isone = a == 1;
				}
				else
				{
					isone = b == 1;
				}
				var o3 = int.Parse(instr[2]);
				if (isone)
				{
					ip += o3;
				}
				else
				{
					ip++;
				}
				break;
		}
	}
	
	b.Dump($"Part {i+1}");
}