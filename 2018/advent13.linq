<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main(string[] args)
{
	var lines = await AoC.GetLinesWeb();

	var map = new List<List<char>>();

	var carts = new List<Cart>();

	var dirs = new[] { (1, 0), (0, 1), (-1, 0), (0, -1) }.ToList();

	foreach (var line in lines)
	{
		map.Add(line.ToList());
	}

	for (int i = 0; i < map.Count; i++)
	{
		for (int j = 0; j < map[0].Count; j++)
		{
			switch (map[i][j])
			{
				case '>':
					map[i][j] = '-';
					carts.Add(new Cart { x = j, y = i, dx = 1, dy = 0 });
					break;
				case '<':
					map[i][j] = '-';
					carts.Add(new Cart { x = j, y = i, dx = -1, dy = 0 });
					break;
				case 'v':
					map[i][j] = '|';
					carts.Add(new Cart { x = j, y = i, dx = 0, dy = 1 });
					break;
				case '^':
					map[i][j] = '|';
					carts.Add(new Cart { x = j, y = i, dx = 0, dy = -1 });
					break;
			}
		}
	}

	while (true)
	{
		HashSet<Cart> crash = new HashSet<Cart>();
		foreach (var cart in carts)
		{
			cart.x += cart.dx;
			cart.y += cart.dy;
			var atloc = carts.FindAll(x => x.x == cart.x && x.y == cart.y);
			if (atloc.Count > 1) crash.UnionWith(atloc);

			switch (map[cart.y][cart.x])
			{
				case '/':
					if (cart.dx == -1)
					{
						cart.dy = 1;
						cart.dx = 0;
					}
					else if (cart.dx == 1)
					{
						cart.dy = -1;
						cart.dx = 0;
					}
					else if (cart.dy == -1)
					{
						cart.dy = 0;
						cart.dx = 1;
					}
					else
					{
						cart.dy = 0;
						cart.dx = -1;
					}
					break;
				case '\\':
					if (cart.dx == -1)
					{
						cart.dy = -1;
						cart.dx = 0;
					}
					else if (cart.dx == 1)
					{
						cart.dy = 1;
						cart.dx = 0;
					}
					else if (cart.dy == -1)
					{
						cart.dy = 0;
						cart.dx = -1;
					}
					else
					{
						cart.dy = 0;
						cart.dx = 1;
					}
					break;
				case '+':
					(cart.dx, cart.dy) = dirs[(dirs.FindIndex(x => x.Item1 == cart.dx && x.Item2 == cart.dy) + cart.turnDir + 3) % 4];
					cart.turnDir += 1; cart.turnDir %= 3;
					break;
			}
		}
		foreach (var cart in crash)
		{
			cart.Dump("Removing");
			carts.Remove(cart);

		}
		if (carts.Count == 1)
		{
			carts.First().Dump("Part2");
		}
	}
done:
	return;
}


// Define other methods and classes here

public class Cart
{
	public int x;
	public int y;
	public int dx;
	public int dy;
	public int turnDir;
}