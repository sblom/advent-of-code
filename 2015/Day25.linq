<Query Kind="Statements" />

var set = new HashSet<long>();

long val = 20151125;

var (row, col) = (3010, 3019);

// row, col determine which diagonal you're on
var diag = row + col - 1;
// a traingular sum for the numbers before diag plus which element you're on in your diag
var num = diag * (diag - 1) / 2 + col;

for (int i = 1; i < num; i++)
{
	val = (val * 252533) % 33554393;
}

val.Dump("Part 1");