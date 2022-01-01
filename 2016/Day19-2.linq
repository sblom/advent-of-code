<Query Kind="Statements" />

List<int> elves = Enumerable.Range(1, 3004953).ToList();

int n = 0;

while (elves.Count() > 1)
{
	int victim = (n + (elves.Count() / 2)) % elves.Count();
	elves.RemoveAt(victim);
	if (victim > n) n = n + 1;
	n = n % elves.Count();
}

elves[0].Dump("Part 2");