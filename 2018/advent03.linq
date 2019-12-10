<Query Kind="Statements" />

var lines = AoC.GetLines();

var rx = new Regex(@" (\d+),(\d+): (\d+)x(\d+)$");

int[,] cloth = new int[10000, 10000];

int tot = 0;

int L = 1;
var claims = new HashSet<int>();

foreach (var line in lines)
{
	var nums = rx.Match(line);
	var rect = (int.Parse(nums.Groups[1].Value), int.Parse(nums.Groups[2].Value), int.Parse(nums.Groups[3].Value), int.Parse(nums.Groups[4].Value));

	for (int x = rect.Item1; x < rect.Item1 + rect.Item3; x++)
	{
		for (int y = rect.Item2; y < rect.Item2 + rect.Item4; y++)
		{
			cloth[x, y]++;
			if (cloth[x, y] == 2) tot++;
		}
	}
}

cloth = new int[10000, 10000];

foreach (var line in lines)
{
	claims.Add(L);
	var nums = rx.Match(line);
	var rect = (int.Parse(nums.Groups[1].Value), int.Parse(nums.Groups[2].Value), int.Parse(nums.Groups[3].Value), int.Parse(nums.Groups[4].Value));

	for (int x = rect.Item1; x < rect.Item1 + rect.Item3; x++)
	{
		for (int y = rect.Item2; y < rect.Item2 + rect.Item4; y++)
		{
			if (cloth[x, y] != 0) { claims.Remove(cloth[x, y]); claims.Remove(L); }
			else cloth[x,y] = L;			
		}
	}
	L++;
}

tot.Dump();
claims.Dump();