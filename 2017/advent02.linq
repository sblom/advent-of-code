<Query Kind="Statements" />

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var lines = File.ReadAllLines("advent02.txt");

int total1 = 0, total2 = 0;

foreach (var line in lines)
{
	var nums = line.Split('\t');
	
	var numnums = nums.Select(x => int.Parse(x)).ToList();

	for (int i = 0; i < numnums.Count(); ++i)
	{
		for (int j = 0; j < numnums.Count(); ++j)
		{
			if (i == j) continue;
			if (numnums[i] == (int)(numnums[i] / numnums[j]) * numnums[j])
			{
				total2 += numnums[i] / numnums[j];
			}
		}
	}
	
	var max = numnums.Max();
	var min = numnums.Min();
	
	total1 += max - min;
}

total1.Dump();
total2.Dump();