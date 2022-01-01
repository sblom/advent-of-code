<Query Kind="Program">
  <Namespace>System.Collections.Specialized</Namespace>
</Query>

void Main()
{
	var lines = AoC.GetLinesWeb().Result;
	
	var line = lines.First().ToArray();
	
	int chloc = 1;
	
	var tree = BuildTree(line, ref chloc, (0,0));
	
	//WalkTree(tree);
	WalkMap();
	
	//count.Dump();
	
	//tree.Dump();
//
//	foreach (var ch in line)
//	{
//		if (char.IsLetter(ch))
//		{
//			locs[(x,y)].Add(ch);
//			var (dy, dx) = dirs[ch];
//			(x, y) = (x + dx, y + dy);
//			if (!locs.ContainsKey((x,y))) locs[(x,y)] = new HashSet<char>();
//		}
//		if (ch == '(')
//		{
//			stack.Append((x,y));
//		}
//		if (ch == '|')
//		{
//			(x,y) = stack.Last();
//		}
//		if (ch == ')')
//		{
//			
//		}
//	}
}

Dictionary<char, (int dy, int dx)> dirs = new Dictionary<char, (int dy, int dx)> {
	{'N', (-1, 0)},
	{'W', (0, -1)},
	{'E', (0,  1)},
	{'S', (1,  0)}
};

List<(int,int)> stack = new List<(int x, int y)>();

int count = 0;

int x = 0;
int y = 0;

Dictionary<(int y, int x), HashSet<char>> locs = new Dictionary<(int y, int x), HashSet<char>>();

Dictionary<(int,int),(bool,bool)> map = new Dictionary<(int x,int y),(bool east,bool south)>();

Node BuildTree(char[] line, ref int chloc, (int x, int y) loc)
{
	Node result = new Node();
	
	char ch;

	// Prefix phase
	while (char.IsLetter(ch = line[chloc++]))
	{
		//result.value += ch;
		switch (ch)
		{
			case 'N':
				{
					var newloc = (loc.x, loc.y - 1);
					if (map.ContainsKey(newloc))
					{
						var doors = map[newloc];
						doors.Item2 = true;
						map[newloc] = doors;
					}
					else map[newloc] = (false, true);
					
					loc = newloc;
				}
				break;
			case 'W':
				{
					var newloc = (loc.x - 1, loc.y);
					if (map.ContainsKey(newloc))
					{
						var doors = map[newloc];
						doors.Item1 = true;
						map[newloc] = doors;
					}
					else map[newloc] = (true, false);
					
					loc = newloc;
				}
				break;
			case 'S':
				{
					if (map.ContainsKey(loc))
					{
						var doors = map[loc];
						doors.Item2 = true;
						map[loc] = doors;
					}
					else map[loc] = (false, true);
					
					loc = (loc.x, loc.y + 1);
				}
				break;
			case 'E':
				{
					if (map.ContainsKey(loc))
					{
						var doors = map[loc];
						doors.Item1 = true;
						map[loc] = doors;
					}
					else map[loc] = (true, false);

					loc = (loc.x + 1, loc.y);
				}
				break;
		}
	}
	if (ch == '|' || ch == ')')
	{
		result.leaves.Add(result);
		result.leavelocs.Add(loc);
		return result;
	}

	chloc--;
	while ((ch = line[chloc++]) != '$')
	{
		if (ch == '(')
		{
			chloc--;
			while ((ch = line[chloc++]) != ')')
			{
				result.children.Add(BuildTree(line, ref chloc, loc));
				result.leaves.AddRange(result.children.Last().leaves);
				result.leavelocs.UnionWith(result.children.Last().leavelocs);
				chloc--;
			}

//			var tree = BuildTree(line, ref chloc, loc);
//			foreach (var leaf in result.leaves)
//			{
//				leaf.children.Add(tree);
//			}

			var tmpchloc = chloc;
			Node tree = new Node();
			foreach (var leaveloc in result.leavelocs)
			{
				chloc = tmpchloc;
				tree = BuildTree(line, ref chloc, leaveloc);
			}
//			result.leaves = tree.leaves.ToList();
			result.leavelocs = tree.leavelocs;

			return result;
		}
	}
	
	return result;
}

void WalkMap()
{
	var frontier = new List<((int x, int y), int dist)> { ((0, 0), 0) };
	var visited = new HashSet<(int x, int y)> { (0, 0) };
	
	int dist = 0;
	(int x, int y) loc;
	
	while (frontier.Count > 0)
	{
		(loc, dist) = frontier.First();
		frontier.RemoveAt(0);
		
		if (dist >= 1000)
			count++;
		
		(int x, int y) newloc;
		
		// N
		newloc = (loc.x, loc.y - 1);
		if (!visited.Contains(newloc))
		{
			if (map.ContainsKey(newloc) && map[newloc].Item2)
			{
				frontier.Add((newloc, dist + 1));
				visited.Add(newloc);
			}
		}
		// W
		newloc = (loc.x - 1, loc.y);
		if (!visited.Contains(newloc))
		{
			if (map.ContainsKey(newloc) && map[newloc].Item1)
			{
				frontier.Add((newloc, dist + 1));
				visited.Add(newloc);				
			}
		}
		// S
		newloc = (loc.x, loc.y + 1);
		if (!visited.Contains(newloc))
		{
			if (map.ContainsKey(loc) && map[loc].Item2)
			{
				frontier.Add((newloc, dist + 1));
				visited.Add(newloc);				
			}
		}
		// E
		newloc = (loc.x + 1, loc.y);
		if (!visited.Contains(newloc))
		{
			if (map.ContainsKey(loc) && map[loc].Item1)
			{
				frontier.Add((newloc, dist + 1));
				visited.Add(newloc);				
			}
		}
	}
	dist.Dump("Part 1");
	count.Dump("Part 2");
}

void WalkTree(Node tree, string acc = "")
{
	if (tree.children.Count == 0)
	{
		count++.Dump();
		//acc.Dump();
	}
	else{
	//acc += tree.value;
		foreach (var child in tree.children)
		{
			WalkTree(child, acc);
		}
	}
}

class Node
{
	public string value;
	public List<Node> children;
	public List<Node> leaves;
	public HashSet<(int x, int y)> leavelocs;
	
	public Node()
	{
		value = "";
		children = new List<Node>();
		leaves = new List<Node>();
		leavelocs = new HashSet<(int x, int y)>();
	}
}