<Query Kind="Statements" />

int x = 0, y = 0;

var lines = AoC.GetLines().ToArray();

for (; lines[0][x] != '|'; x++);

var (dx, dy) = (0, 1);

var dirs = new Dictionary<(int, int), char> {
	{(1,0), 'E'},
	{(-1,0), 'W'},
	{(0,1), 'S'},
	{(0,-1), 'N'},
};

int steps = 0;

while (true)
{
	(x, y) = (x + dx, y + dy);
	steps++;

	if (char.IsWhiteSpace(lines[y][x]))
	{
		Console.WriteLine();
		steps.Dump();
		return;
	}

	if (char.IsLetter(lines[y][x]))
	{
		Console.Write(lines[y][x]);
		continue;
	}

	if (lines[y][x] == '+')
	{
		foreach (var kv in dirs)
		{
			var (tx, ty) = kv.Key;
			if ((tx,ty).Equals((-dx,-dy)))
				continue;
			if (!char.IsWhiteSpace(lines[ty + y][tx + x]))
			{
				(dx, dy) = (tx,ty);
				//kv.Value.Dump();
				break;
			}
		}
	}
}