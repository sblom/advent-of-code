<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();

var bots = new List<((int,int,int),int)>();

var (x0,y0,z0) = (-1,-1,-1);
var maxr = 0;

int Dist(int x, int y, int z)
{
	return Math.Abs(x - x0) + Math.Abs(y - y0) + Math.Abs(z - z0);
}

foreach (var line in lines)
{
	var r = int.Parse(line.Split('=')[2]);
	var coords = line.Split('<','>')[1].Split(',');
	var (x,y,z) = (int.Parse(coords[0]),int.Parse(coords[1]),int.Parse(coords[2]));

	if (r > maxr)
	{
		maxr = r;
		(x0, y0, z0) = (x, y, z);
	}

	bots.Add(((x,y,z),r));
}

int c = 0;

foreach (var bot in bots)
{
	var d = Dist(bot.Item1.Item1, bot.Item1.Item2, bot.Item1.Item3);
	if (d <= maxr /* && d <= bot.Item2 */)
		c++;
}

c.Dump("Part 1");  //433