<Query Kind="Statements" />

var curqp = Util.CurrentQueryPath;
var dirname = Path.GetDirectoryName(curqp);
Directory.SetCurrentDirectory(dirname);
var inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp) + ".txt");
var input = File.ReadLines(inputName);

var frontier = new List<int>();
var conns = new List<List<int>>(2000);
var set = new HashSet<int>();
var groups = 0;

var universe = new HashSet<int>(Enumerable.Range(0,2000));

foreach (var line in input)
{
	var others = line.Split(' ').Skip(2).Select(x => int.Parse(x.Trim(','))).ToList();
	conns.Add(others);
}

while (universe.Any())
{
	frontier.Add(universe.Min());
	
	while (frontier.Any())
	{
		var cur = frontier[0];
		frontier.RemoveAt(0);

		if (!set.Contains(cur))
		{
			set.Add(cur);
			frontier = frontier.Concat(conns[cur]).ToList();
		}
	}
	
	if (set.Contains(0))
		set.Count().Dump("part 1");
	
	universe.RemoveWhere(x => set.Contains(x));
	set.Clear();
	groups++;
}

groups.Dump("part 2");