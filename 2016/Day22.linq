<Query Kind="Statements" />

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var lines = File.ReadLines("advent22.txt");

var rx = new Regex(@"/dev/grid/node-x(\d+)-y(\d+)\W+(\d+)T\W+(\d+)T\W+(\d+)T\W+(\d+)%");

lines = lines.Skip(2);

var nodes = (from L in lines
			let match = rx.Match(L).Groups
			select new
			{
				x = int.Parse(match[1].Value),
				y = int.Parse(match[2].Value),
				size = int.Parse(match[3].Value),
				used = int.Parse(match[4].Value),
				avail = int.Parse(match[5].Value),
				pct = int.Parse(match[6].Value)
			}).ToList();
			
nodes.Dump();

nodes.Select((n,i) => nodes.Where(m => n.x != m.x || n.y != m.y).Where(m => n.used > 0 && n.used <= m.avail).Count()).Sum().Dump("Part 1");

nodes.Where(n => n.used < 200).Select(x => x.size).Min().Dump();

char[,] grid = new char[31, 33];

for (int x = 0; x < 33; ++x)
{
	for (int y = 0; y < 31; ++y)
	{
		grid[y, x] = nodes.Where(n => n.x == x && n.y == y).Select(n =>
		 {
		 	switch (n.used)
			{
				case int m when m == 0:
				return '_';
				case int m when m < 200:
				return 'o';
				case int m when m > 200:
				return 'O';
				default:
				return 'X';
			}
		 }).Single();
	}
}

grid.Dump();

// 31 + 32 + 1 + 32 + 1 + 31 + 1 + 31 + 1 + 30 + 1 + 30 + 1 + .. + 2 + 1 + 2 + 1 + 1

int total = 31 + 1;
for (int i = 33; i >= 3; i--)
{
	total += i * 2;
}

total.Dump();

// 1148 Too High