<Query Kind="Statements" />

var curqp = Util.CurrentQueryPath;
var dirname = Path.GetDirectoryName(curqp);
Directory.SetCurrentDirectory(dirname);
var inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp).Replace("-cs","") + ".txt");
var input = File.ReadLines(inputName);

int total = 0;

foreach (var line in input)
{
	int c;
	total += 2;
	for (c = 0; c < line.Length; ++c)
	{
		if (line[c] == '\\')
		{
			if (line[c + 1] == '\"' || line[c + 1] == '\\')
			{
				total += 1;
				c += 1;
			}
			else if (line[c + 1] == 'x')
			{
				total += 3;
				c += 3;
			}
		}
	}
}
total.Dump();

total = 0;

foreach (var line in input)
{
	total += 2;
	total += new Regex(@"\\").Matches(line).Count;
	total += new Regex(@"\""").Matches(line).Count;
}
total.Dump();