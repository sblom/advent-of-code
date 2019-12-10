<Query Kind="Program" />

void Main()
{
	var lines = AoC.GetLinesWeb().Result;

	var ip = int.Parse(lines.First().Split(' ')[1]);

	var instrs = lines.Skip(1).Select(L => { var dec = L.Split(' '); return new { op = dec[0], a = int.Parse(dec[1]), b = int.Parse(dec[2]), c = int.Parse(dec[3]) }; }).ToList();

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

	while (ipreg < lines.Count())
	{
		var instr = instrs[ipreg];
		$@"{ipreg}: {instr.op} {instr.a} {instr.b} {instr.c}
{string.Join(" ", regs)}".Dump();

		if (ipreg == 18)
		{
			regs[4] = regs[2] / 256;
			regs[2] = (regs[4] + 1) * 256;
			ipreg = 7;
		}
		else if (ipreg == 28)
		{
			$@"{string.Join(" ", regs)}".Dump("Part 1");
			return;
		}
		else
		{
			opcodes[instr.op](instr.a, instr.b, instr.c);
		}
		$@"{string.Join(" ", regs)}".Dump();

		ipreg++;
	}

	regs[0].Dump("Part 1");
}

//# ip 5
//seti 123 0 3			d = 123
//bani 3 456 3			d = d & 456
//eqri 3 72 3			if d == 72:
//addr 3 5 5				goto start
//seti 0 0 5			goto top

//start:
//	seti 0 9 3			d = 0
//	bori 3 65536 1		b = d | 0b1_0000_0000_0000_0000
//	seti 14906355 8 3	d = 0b1110_0011_0111_0011_1111_0011

//restart:
//	bani 1 255 4		e = b & 0b1111
//	addr 3 4 3			d += e
//	bani 3 16777215 3	d = d & 0b1111_1111_1111_1111_1111_1111
//	muli 3 65899 3		d = d * 0b1_0000_0001_0110_1011
//	bani 3 16777215 3   d = d & 0b1111_1111_1111_1111_1111_1111
//	gtir 256 1 4		if b <= 256:
//	addr 4 5 5          	goto end
//	addi 5 1 5
//	seti 27 8 5
//	seti 0 4 4			e = 0

//loop:
//	addi 4 1 2			c = e + 1
//	muli 2 256 2		c *= 256
//	gtrr 2 1 2			if c > b:
//	addr 2 5 5				goto break
//	addi 5 1 5
//	seti 25 1 5
//	addi 4 1 4			e += 1
//	seti 17 2 5			goto loop

//break:
//	setr 4 9 1			b = e
//	seti 7 0 5			goto restart

//end:
//	eqrr 3 0 4			if a != d:
//	addr 4 5 5				goto start
//	seti 5 3 5