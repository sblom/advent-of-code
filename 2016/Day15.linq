<Query Kind="Statements" />

var discs = new List<(int n, int p)> { (7, 0), (13, 0), (3, 2), (5, 2), (17, 0), (19, 7), /* Part 2 */ (11, 0) };

for (int t = 0; ; ++t)
{
	var success = true;
	for (int i = 0; i < discs.Count(); ++i)
	{
		if ((discs[i].p + (i + 1) + t) % discs[i].n != 0)
		{
			success = false;
			break;
		}
	}
	if (success)
	{
		t.Dump();
		break;
	}
}