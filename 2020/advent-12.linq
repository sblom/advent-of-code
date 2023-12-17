<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

var instrs = AoC.GetLines().Extract<(char,int)>(@"(.)(\d+)").ToArray();

var (dx,dy) = (1,0);
var (x,y) = (0,0);

(int,int) right(int dx,int dy) {
	return (-dy, dx);
}

(int, int) left(int dx, int dy)
{
	return (dy, -dx);
}

foreach (var (ins, qty) in instrs)
{
	int _qty = qty;
	if (ins == 'N') (x,y) = (x, y - qty);
	if (ins == 'S') (x, y) = (x, y + qty);
	if (ins == 'E') (x, y) = (x + qty, y);
	if (ins == 'W') (x, y) = (x - qty, y);
	if (ins == 'F') (x, y) = (x + dx * qty, y + dy * qty);
	if (ins == 'L') while (_qty > 0) { (dx, dy) = left(dx, dy); _qty -= 90; }
	if (ins == 'R') while (_qty > 0) { (dx, dy) = right(dx, dy); _qty -= 90; }
}

(Math.Abs(x) + Math.Abs(y)).Dump("Part 1");

(dx, dy) = (10, -1);
(x, y) = (0, 0);

foreach (var (ins, qty) in instrs)
{
	int _qty = qty;
	if (ins == 'N') (dx,dy) = (dx, dy - qty);
	if (ins == 'S') (dx,dy) = (dx, dy + qty);
	if (ins == 'E') (dx, dy) = (dx + qty, dy);
	if (ins == 'W') (dx, dy) = (dx - qty, dy);
	if (ins == 'F') (x, y) = (x + dx * qty, y + dy * qty);
	if (ins == 'L') while (_qty > 0) { (dx, dy) = left(dx, dy); _qty -= 90; }
	if (ins == 'R') while (_qty > 0) { (dx, dy) = right(dx, dy); _qty -= 90; }
}

(Math.Abs(x) + Math.Abs(y)).Dump("Part 2");
