<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationCore.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationFramework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\WindowsBase.dll</Reference>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

List<List<char>> map;
List<Unit> units;

int elfattack = 3;

async Task Main()
{
	IEnumerable<string> lines = (AoC.GetLinesWeb()).Result;

	//	var test = 
	//		@"#######
	//		  #.G...#
	//		  #...EG#
	//		  #.#.#G#
	//		  #..G#E#
	//		  #.....#
	//		  #######"; // 27730

	//		var test = @"#######
	//		#G..#E#
	//		#E#E.E#
	//		#G.##.#
	//		#...#E#
	//		#...E.#
	//		#######";  // 36334

	//	var test = @"#######
	//#E..EG#
	//#.#G.E#
	//#E.##E#
	//#G..#.#
	//#..E#.#
	//#######"; // 39514

	//var test = @"#######
	//#E.G#.#
	//#.#G..#
	//#G.#.G#
	//#G..#.#
	//#...E.#
	//#######";  // 27755

	//		var test = @"#######
	//#.E...#
	//#.#..G#
	//#.###.#
	//#E#G#G#
	//#...#G#
	//#######"; //28944


	//	var test =@"#########
	//	#G......#
	//	#.E.#...#
	//	#..##..G#
	//	#...##..#
	//	#...#...#
	//	#.G...G.#
	//	#.....G.#
	//	#########"; // 18740
	//
	//	lines = test.Split('\n').Select(x => x.Trim());


	for (; ; elfattack++)
	{
		map = lines.Select(x => x.ToList()).ToList();
		units = new List<Unit>();

		for (int i = 0; i < map.Count; i++)
		{
			for (int j = 0; j < map[0].Count; j++)
			{
				if (map[i][j] == 'E' || map[i][j] == 'G')
				{
					units.Add(new Unit { team = map[i][j], x = j, y = i, hp = 200 });
				}
			}
		}
		//units.Dump();

		PrintMap(0);

		for (int T = 0; ; T++)
		{
			foreach (var unit in units.OrderBy(u => u.y).ThenBy(u => u.x).ToList())
			{
				if (!units.Contains(unit)) continue;

				if (units.Select(u => u.team).Distinct().Count() < 2)
				{
					//				units.Dump();
					//				PrintMap(T);
					(T * units.Select(u => u.hp).Sum()).Dump("Part 1");
					
					units.Dump();
					
					elfattack.Dump("Part 2");
					
					return;
				}

				Unit neighbor = OpponentAdjacent(unit);
				if (neighbor == null)
				{
					var bestdir = FindBestStepTowardBestOpponentAdjacency((unit.x, unit.y), unit.team);
					var moveto = (x: unit.x + bestdir.dx, y: unit.y + bestdir.dy);
					map[unit.y][unit.x] = '.';
					map[moveto.y][moveto.x] = unit.team;
					(unit.x, unit.y) = moveto;
					neighbor = OpponentAdjacent(unit);
				}

				if (neighbor != null)
				{
					Attack(neighbor);
					if (neighbor.hp <= 0)
					{
						units.Remove(neighbor);
						map[neighbor.y][neighbor.x] = '.';
						if (neighbor.team == 'E')
							goto increase_elfattack;
						//PrintMap(T);
					}
				}
			}
			//Console.Clear();
			//PrintMap(T);
			
		}
	increase_elfattack: ;
	}
}

(int dx, int dy) FindBestStepTowardBestOpponentAdjacency((int x, int y) p, char team)
{
	var frontier = new List<((int x, int y) pos, int dist, (int dx, int dy) step)>();
	var visited = new HashSet<(int x, int y)> { (p.x, p.y) };
	var candidates = new List<((int x, int y) pos, (int dx, int dy) step)>();
	
	foreach (var (dx, dy) in dirs)
	{
		var newloc = (p.x + dx, p.y + dy);
		if (map[newloc.Item2][newloc.Item1] == '.')
			frontier.Add(((p.x + dx, p.y + dy), 1, (dx,dy)));
	}

	for (int g = 0; frontier.Count > 0; g++)
	{
		var f = frontier.ToList();
		frontier.Clear();
		foreach (var loc in f)
		{
			foreach (var (dx, dy) in dirs)
			{
				var newloc = (x: loc.pos.x + dx, y: loc.pos.y + dy);
				if (map[newloc.y][newloc.x] == '#' || map[newloc.y][newloc.x] == team) continue;
				if (map[newloc.y][newloc.x] == (team == 'E' ? 'G' : 'E'))
					candidates.Add((loc.pos, loc.step));
				if (!visited.Contains(newloc))
				{
					visited.Add(newloc);
					frontier.Add((newloc, loc.dist + 1, loc.step));
				}
			}
		}
		if (candidates.Count > 0)
		{
			return candidates.OrderBy(c => c.pos.y).ThenBy(c => c.pos.x).ThenBy(c => dirs.Find(d => d == c.step)).First().step;
		}
	}

	return (0,0);
}

void PrintMap(int T)
{
	$"*** {T} ***".DumpFixed();
	foreach (var row in map)
	{
		string.Join(" ", row).DumpFixed();
	}
	"".DumpFixed();
}

int FindBestOpponentAdjacencyDist((int x, int y) p, char team)
{
	var frontier = new List<((int x, int y) pos, int dist)> { ((p.x, p.y), 0) };
	var visited = new HashSet<(int x, int y)> { (p.x, p.y) };
	for (int g = 0; frontier.Count > 0; g++)
	{
		var f = frontier.ToList();
		frontier.Clear();
		foreach (var loc in f)
		{
			foreach (var (dx, dy) in dirs)
			{
				var newloc = (x: loc.pos.x + dx, y: loc.pos.y + dy);
				if (map[newloc.y][newloc.x] == '#' || map[newloc.y][newloc.x] == team) continue;
				if (!visited.Contains(newloc))
				{
					if (map[newloc.y][newloc.x] == (team == 'E' ? 'G' : 'E'))
						return loc.dist;
					visited.Add(newloc);
					frontier.Add((newloc,loc.dist + 1));
				}
			}
		}
	}
	return int.MaxValue;
}

// Define other methods and classes here

class Unit
{
	public char team;
	public int x, y;
	public int hp;
}

// In reading order
List<(int,int)> dirs = new List<(int,int)>{ (0,-1), (-1,0), (1,0), (0,1) };

void Attack(Unit unit)
{
	if (unit.team == 'G')
	{
		unit.hp -= elfattack;
	}
	else
	{
		unit.hp -= 3;
	}
}

Unit OpponentAdjacent(Unit unit)
{
	var (x0, y0) = (unit.x, unit.y);
	
	List<Unit> adjacent = new List<Unit>();

	foreach (var (dx, dy) in dirs)
	{
		var (x, y) = (x0 + dx, y0 + dy);
		
		var neighbor = units.Find(u => u.x == x && u.y == y && u.team != unit.team);
		
		if (neighbor != null)
			adjacent.Add(neighbor);
	}
	
	return adjacent.OrderBy(u => u.hp).FirstOrDefault();
}