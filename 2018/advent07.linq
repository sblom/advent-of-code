<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();

var edge = new List<(string,string)>();

foreach (var line in lines)
{
	var sections = line.Split(' ');
	edge.Add((sections[1], sections[7]));
}

var set = new HashSet<string>();

set = new HashSet<string>(edge.Select(x=>x.Item1).Union(edge.Select(x=>x.Item2)));

var seq = "";

while(set.Count > 0)
{
	var next = set.Except(edge.Select(x=>x.Item2)).OrderBy(x=>x).First();
	seq += next;
	edge.RemoveAll(x => x.Item1 == next);
	set.Remove(next);
}

seq.Dump("Part 1");

edge.Clear();
set.Clear();

var NUM_WORKERS = 5;
var workers = new List<(string,int)>(NUM_WORKERS);

int total = 0;

foreach (var line in lines)
{
	var sections = line.Split(' ');
	edge.Add((sections[1], sections[7]));
}

set = new HashSet<string>(edge.Select(x => x.Item1).Union(edge.Select(x => x.Item2)));

while (set.Count > 0)
{
	while (workers.Count < NUM_WORKERS)
	{
		var next = set.Except(edge.Select(x=>x.Item2)).Except(workers.Select(x => x.Item1)).OrderBy(x=>x);
		if (next.Count() == 0) break;
		workers.Add((next.First(), 61 + next.First()[0] - 'A'));
	}

	var min = workers.OrderBy(x => x.Item2).First();
	total += min.Item2;
	workers = workers.Select(x => (x.Item1, x.Item2 - min.Item2)).ToList();
	var done = workers.Where(x => x.Item2 == 0).Select(x => x.Item1).ToList();
	workers.RemoveAll(x => done.Any(y => y == x.Item1));
	set.RemoveWhere(x => done.Any(y => x == y));
	edge.RemoveAll(x => done.Any(y => y == x.Item1));
}

total.Dump("Part 2");