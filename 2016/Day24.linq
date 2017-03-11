<Query Kind="Statements" />

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var lines = File.ReadAllLines("advent24.txt");

(int x,int y)[] locs = new (int,int)[8];

char[][] grid = new char[lines.Count()][];
for (int i = 0; i < lines.Count(); ++i)
{
	grid[i] = lines[i].ToCharArray();
}

int[,] dists = new int[8,8];

for (int Y = 0; Y < grid.Length; ++Y)
{
	for (int X = 0; X < grid[0].Length; ++X)
	{
		if (grid[Y][X] >= '0' && grid[Y][X] <= '9')
		{
			locs[grid[Y][X] - '0'] = (X,Y);
		}
	}
}

var frontier = new List<(int dist,(int x,int y))>();
var visited = new HashSet<(int,int)>();

for (int i = 0; i < 8; ++i)
{
	frontier.Clear(); visited.Clear();
	frontier.Add((0,locs[i])); visited.Add(locs[i]);

	while (frontier.Any())
	{
		var cur = frontier[0]; frontier.RemoveAt(0);
		var (x, y) = cur.Item2;
		
		if (grid[y][x] >= '0' && grid[y][x] <= '9') dists[i,grid[y][x]-'0'] = cur.dist;

		if (x - 1 >= 0 && grid[y][x - 1] != '#' && !visited.Contains((x - 1, y))) { frontier.Add((cur.dist + 1, (x - 1, y))); visited.Add((x - 1, y)); }
		if (x + 1 < grid[0].Length && grid[y][x + 1] != '#' && !visited.Contains((x + 1, y))) { frontier.Add((cur.dist + 1, (x + 1, y))); visited.Add((x + 1, y)); }
		if (y - 1 >= 0 && grid[y - 1][x] != '#' && !visited.Contains((x, y - 1))) { frontier.Add((cur.dist + 1, (x, y - 1))); visited.Add((x, y - 1)); }
		if (y + 1 < grid.Length && grid[y + 1][x] != '#' && !visited.Contains((x, y + 1))) { frontier.Add((cur.dist + 1, (x, y + 1))); visited.Add((x, y + 1)); }
	}
}

dists.Dump();

var set = Enumerable.Range(0,8).ToList();

List<List<int>> Permutations = new List<List<int>>();

void RecCreatePermutation(List<int> current, List<int> elements)
{
	for (int i = 0; i < elements.Count; i++)
	{
		List<int> NewCurrent = current.ToList();
		List<int> NewElements = elements.ToList();
		NewCurrent.Add(elements[i]);
		NewElements.RemoveAt(i);
		if (NewElements.Count == 0)
		{
			Permutations.Add(NewCurrent);
		}
		else
			RecCreatePermutation(NewCurrent, NewElements);
	}
}

RecCreatePermutation(new List<int>(), set);

int mindist = int.MaxValue;

foreach (var perm in Permutations)
{
	perm.Insert(0,0); perm.Add(0);
	int dist = 0;
	for (int i = 1; i < perm.Count(); i++)
	{
		dist += dists[perm[i - 1],perm[i]];
	}
	if (dist < mindist) mindist = dist;
}

mindist.Dump("Part 2");


(40 + 30 + 58 + 150 + 48 + 26 + 76).Dump("Part 1");