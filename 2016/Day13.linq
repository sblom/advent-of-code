<Query Kind="Statements" />

var frontier = new List < (int, (int, int)) > { (0,(1, 1)) };
HashSet<(int, int)> visited = new HashSet<(int,int)>();

int count = 0;

bool isOpen(ulong x, ulong y)
{
	ulong num = x*x + 3*x + 2*x*y + y + y*y + 1358;
	ulong bits = 0;
	while (num > 0)
	{
		bits += num % 2;
		num /= 2;
	}
	
	return (bits % 2 == 0);
}

List<(int, int)> deltas = new List<(int, int)> { (-1, 0), (0, -1), (1, 0), (0, 1) };

while (true)
{
	var (c,(x, y)) = frontier[0];
	
	if (x == 31 && y == 39) c.Dump();
	
	frontier.RemoveAt(0);
	if (visited.Contains((x,y))) continue;	
	visited.Add((x,y));

	if (c <= 50)
	{
		count++;
	}
	
	if (c > 75) count.Dump();

	foreach (var (dx, dy) in deltas)
	{
		if (!(x + dx >= 0 && y + dy >= 0))
		{
			continue;
		}

		if (isOpen((ulong)(x + dx), (ulong)(y + dy)))
		{
			frontier.Add((c+1,(x+dx,y+dy)));
		}
	}
}