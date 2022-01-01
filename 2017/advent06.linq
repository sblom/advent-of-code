<Query Kind="Statements" />

var curqp = Util.CurrentQueryPath;
var dirname = Path.GetDirectoryName(curqp);
Directory.SetCurrentDirectory(dirname);
var inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp) + ".txt");
var input = File.ReadLines(inputName).First();

var banks = input.Split('\t').Select(x => int.Parse(x)).ToArray();

var set = new Dictionary<string,int>();

int n = 1;

while (true)
{
	int max = -1, loc = 0;
	
	for (int i = 0; i < banks.Length; i++)
	{
		if (banks[i] > max)
		{
			max = banks[i];
			loc = i;
		}
	}
	
	int v = banks[loc], ii = (loc + 1) % banks.Length;
	banks[loc] = 0;
	
	while (v > 0)
	{
		banks[ii]++;
		v--;
		ii = (ii + 1) % banks.Length;
	}
	
	var str = string.Join(",",banks.Select(x => x.ToString()));
	
	if (set.ContainsKey(str))
	{
		n.Dump("Part 1");
		(n - set[str]).Dump("Part 2");
		break;
	}
	
	set[str] = n;
	
	n++;
}