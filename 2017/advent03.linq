<Query Kind="Statements" />

//var curqp = Util.CurrentQueryPath;
//var dirname = Path.GetDirectoryName(curqp);
//Directory.SetCurrentDirectory(dirname);
//var inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp) + ".txt");
//var input = File.ReadLines(inputName);

long[,] thing = new long[2000, 2000];

long findval(int x0, int y0)
{
	long total = 0;
	for (int ddx = -1; ddx <= 1; ddx++)
	{
		for (int ddy = -1; ddy <= 1; ddy++)
		{
			if (ddx == 0 && ddy == 0)
				continue;
			total += thing[x0 + ddx, y0 + ddy];		
		}
	}
	return total;
}

var x = 1000;
var y = 1000;

thing[x, y] = 1;

var (dx, dy) = (0, 1);

(x,y) = (x+dx, y+dy);
(dx, dy) = (-dy, dx);
(x,y) = (x-dx, y-dy);

for (int i = 1; i < 1000; i++)
{
	for (int n = 0; n < 4; n++)
	{
		for (int j = 0; j < i * 2; j++)
		{
			x = x + dx; y = y + dy;
			thing[x, y] = findval(x, y);
			$"({x},{y}) = {thing[x,y]}".Dump();
		}
		if (n == 3)
		{
			x = x + dx; y = y + dy;
			(dx, dy) = (-dy, dx);
			x = x - dx; y = y - dy;
		}
		else
		{
			(dx, dy) = (-dy, dx);
		}
	}
}