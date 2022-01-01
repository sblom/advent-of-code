<Query Kind="Statements" />

var curqp = Util.CurrentQueryPath;
var dirname = Path.GetDirectoryName(curqp);
Directory.SetCurrentDirectory(dirname);
var inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp) + ".txt");
var input = File.ReadLines(inputName);

var dirs = input.First().Split(',');

int x = 0, y = 0;

int d = 0, dmax = 0;

foreach (var dir in dirs)
{
	switch (dir)
	{
		case "s":
			x = x + 1;
			break;
		case "n":
			x = x - 1;
			break;
		case "sw":
			x = x + 1;
			y = y - 1;
			break;
		case "se":
			x = x + 1;
			y = y + 1;
			break;
		case "nw":
			x = x - 1;
			y = y - 1;
			break;
		case "ne":
			x = x - 1;
			y = y + 1;
			break;
	}
	
	if (Math.Abs(y) > Math.Abs(x))
		d = y;
	else
		d = Math.Abs(x) + Math.Abs(Math.Abs(y)-Math.Abs(x)) / 2;
		
	if (d > dmax)
		dmax = d;
}

d.Dump("Part 1");
dmax.Dump("Part 2");