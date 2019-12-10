<Query Kind="Statements" />

var lines = AoC.GetLinesWeb().Result;

var ip = int.Parse(lines.First().Split(' ')[1]);

var instrs = lines.Skip(1).Select(L => { var dec = L.Split(' '); return new { op = dec[0], a = int.Parse(dec[1]), b = int.Parse(dec[2]), c = int.Parse(dec[3]) };}).ToList();

var regs = new int[6];

ref int ipreg = ref regs[ip];

Dictionary<string, Action<int, int, int>> opcodes = new Dictionary<string, Action<int, int, int>>
	{
		{"addr", (a,b,c)=>{regs[c] = regs[a] + regs[b];}},
		{"addi", (a,b,c)=>{regs[c] = regs[a] + b;}},
		{"mulr", (a,b,c)=>{regs[c] = regs[a] * regs[b];}},
		{"muli", (a,b,c)=>{regs[c] = regs[a] * b;}},
		{"banr", (a,b,c)=>{regs[c] = regs[a] & regs[b];}},
		{"bani", (a,b,c)=>{regs[c] = regs[a] & b;}},
		{"borr", (a,b,c)=>{regs[c] = regs[a] | regs[b];}},
		{"bori", (a,b,c)=>{regs[c] = regs[a] | b;}},
		{"setr", (a,b,c)=>{regs[c] = regs[a];}},
		{"seti", (a,b,c)=>{regs[c] = a;}},
		{"gtir", (a,b,c)=>{regs[c] = a > regs[b] ? 1 : 0;}},
		{"gtri", (a,b,c)=>{regs[c] = regs[a] > b ? 1 : 0;}},
		{"gtrr", (a,b,c)=>{regs[c] = regs[a] > regs[b] ? 1 : 0;}},
		{"eqir", (a,b,c)=>{regs[c] = a == regs[b] ? 1 : 0;}},
		{"eqri", (a,b,c)=>{regs[c] = regs[a] == b ? 1 : 0;}},
		{"eqrr", (a,b,c)=>{regs[c] = regs[a] == regs[b] ? 1 : 0;}},
	};

//while (ipreg < lines.Count())
//{
//	var instr = instrs[ipreg];
//	$@"{instr.op} {instr.a} {instr.b} {instr.c}
//{string.Join(" ", regs)}".Dump();
//	
//	opcodes[instr.op](instr.a,instr.b,instr.c);	
//	$@"{string.Join(" ", regs)}".Dump();
//	
//	ipreg++;
//}
//
//regs[0].Dump("Part 1");

Enumerable.Range(1,875).Where(n => (875 / n) * n == 875).Sum().Dump("Part 1");
Enumerable.Range(1,10551275).Where(n => (10551275 / n) * n == 10551275).Sum().Dump("Part 2");


//					# ip 2
//							0  addi 2 16 2      jmp 17
//
//					top:
//							1  seti 1 1 1       for (b = 1
//							2  seti 1 4 3           for (c = 1
//							3  mulr 1 3 5
//							4  eqrr 5 4 5       if (b * c == d)
//							5  addr 5 2 2           a = a + b
//							6  addi 2 1 2
//							7  addr 1 0 0
//							8  addi 3 1 3       c = c + 1
//							9  gtrr 3 4 5       if (c > d)
//							10 addr 2 5 2           b = b + 1
//							11 seti 2 4 2       else 
//                          12 addi 1 1 1           jmp 3
//							13 gtrr 1 4 5       if (b > d)
//							14 addr 5 2 2           jmp 33
//							15 seti 1 0 2       else
//                          16 mulr 2 2 2           jmp 2
//
//					init:
//							17 addi 4 2 4       d = d + 2
//							18 mulr 4 4 4       d = d * *2
//							19 mulr 2 4 4       d = d * 19
//							20 muli 4 11 4      d = d * 11
//							21 addi 5 1 5       e = e + 1
//							22 mulr 5 2 5       e = e * 22
//							23 addi 5 17 5      e = e + 17
//							24 addr 4 5 4       d = e + d           d = 875
//							25 addr 2 0 2       if (!a)
//							26 seti 0 9 2           goto top
//							27 setr 2 3 5       e = 2(2 * 28 + 29) * 30 * 14 * 32
//							28 mulr 5 2 5       e = e * 28
//							29 addr 2 5 5       e = e + 29
//							30 mulr 2 5 5       e = e * 30
//							31 muli 5 14 5      e = e * 14
//							32 mulr 5 2 5       e = e * 32
//							33 addr 4 5 4       d = d + e           d = 10551275
//							34 seti 0 9 0       a = 0
//							35 seti 0 6 2       goto top