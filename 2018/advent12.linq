<Query Kind="Statements" />

var pots = ".##..##..####..#.#.#.###....#...#..#.#.#..#...#....##.#.#.#.#.#..######.##....##.###....##..#.####.#";

var result = (50_000_000_000L - 123 - 2) * 58 + 9106;
result.Dump("Part 2");

var initpots = pots;

var cache = new Dictionary<string,List<(int iter,int left)>>();

var rules = new[] {( ".#...", "#" ),
( "#....", "." ),
( "#.###", "." ),
( "#.##.", "." ),
( "#...#", "." ),
( "...#.", "." ),
( ".#..#", "#" ),
( ".####", "#" ),
( ".###.", "." ),
( "###..", "#" ),
( "#####", "." ),
( "....#", "." ),
( ".#.##", "#" ),
( "####.", "." ),
( "##.#.", "#" ),
( "#.#.#", "#" ),
( "..#.#", "." ),
( ".#.#.", "#" ),
( "###.#", "#" ),
( "##.##", "." ),
( "..#..", "." ),
( ".....", "." ),
( "..###", "#" ),
( "#..##", "#" ),
( "##...", "#" ),
( "...##", "#" ),
( "##..#", "." ),
( ".##..", "#" ),
( "#..#.", "." ),
( "#.#..", "#" ),
( ".##.#", "." ),
( "..##.", "." ),};

var left = 0;

for (int i = 1; i <= 20000; i++)
{
	var sb = new StringBuilder();
	while (!pots.StartsWith("...."))
	{
		pots = "." + pots;
		left--;
	}
	pots = ".." + pots;
	while (!pots.EndsWith("..."))
	{
		pots = pots + ".";
	}
	
	for (int c = 0; c < pots.Length - 4; c++)
	{
		var sub = pots.Substring(c,5);
		sb.Append(rules.First(x => x.Item1 == sub).Item2);
	}

	pots = sb.ToString().Substring(2);
	left += 2;

	if (!cache.ContainsKey(pots))
	{
		cache[pots] = new List<(int, int)>();
	}
	else
	{
		(i, pots.Select((p, n) => p == '#' ? n + left : 0).Sum()).ToString().Dump();
	}
	cache[pots].Add((i, left));

	(left,pots).ToString().Dump();
	
	if (pots.Contains(initpots))
	{
		i.Dump("Whoa!");
	}
}

pots.Dump();
left.Dump();

pots.Select((p, n) => p == '#' ? n + left : 0).Sum().Dump();