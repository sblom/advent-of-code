<Query Kind="Statements" />

//# ip 5
//seti 123 0 3			d = 123
//bani 3 456 3			d = d & 456
//eqri 3 72 3			if d == 72:
//addr 3 5 5				goto start
//seti 0 0 5			goto top
//	seti 0 9 3			d = 0

//start:
//	bori 3 65536 1		b = d | 0b1_0000_0000_0000_0000
//	seti 14906355 8 3	d = 0b1110_0011_0111_0011_1111_0011

var results = new HashSet<int>();

int prev = 0;

int a = 0, b = 0, c = 0, d = 0, e = 0;

d = 0;
start:
b = d | 65536;
d = 14906355;

restart:
e = b & 255;
d += e;
d &= 16777215;
d *= 65899;
d &= 16777215;
if (b < 256) goto end;

loop:
c = e + 1;
c *= 256;
if (c > b) goto bk;
e += 1;
goto loop;

//e = b / 256;
//c = (e + 1) * 256;

bk:
b = e;
goto restart;

end:
if (!results.Contains(d))
{
	results.Add(d);
	prev = d;
}
else
{
	prev.Dump("Part 2");
}
/*if (a != d)*/ goto start;

//restart:
//	bani 1 255 4		e = b & 0b1111
//	addr 3 4 3			d += e
//	bani 3 16777215 3	d = d & 0b1111_1111_1111_1111_1111_1111
//	muli 3 65899 3		d = d * 0b1_0000_0001_0110_1011
//	bani 3 16777215 3   d = d & 0b1111_1111_1111_1111_1111_1111
//	gtir 256 1 4		if b < 256:
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