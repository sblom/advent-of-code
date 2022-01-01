<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();
var line = lines.First().ToCharArray().ToList();

var changed = true;

while (changed)
{
	changed = false;
	for (int i = 0; i < line.Count - 1; i++)
	{
		if ((char.IsLower(line[i]) && line[i + 1] == char.ToUpper(line[i])) || (char.IsUpper(line[i]) && line[i + 1] == char.ToLower(line[i])))
		{
			line.RemoveAt(i);
			line.RemoveAt(i);
			i -= 1;
			changed = true;
		}
	}
}

var linerem = line;

for (char ch = 'A'; ch <= 'Z'; ch++)
{
	changed = true;
	var lineremcopy = linerem.ToList();
	lineremcopy.RemoveAll(c => c == ch || c == char.ToLower(ch));

	while (changed)
	{
		changed = false;
		for (int i = 0; i < lineremcopy.Count - 1; i++)
		{
			if ((char.IsLower(lineremcopy[i]) && lineremcopy[i + 1] == char.ToUpper(lineremcopy[i])) || (char.IsUpper(lineremcopy[i]) && lineremcopy[i + 1] == char.ToLower(lineremcopy[i])))
			{
				lineremcopy.RemoveAt(i);
				lineremcopy.RemoveAt(i);
				i -= 1;
				changed = true;
			}
		}
	}
	(ch,lineremcopy.Count).ToString().Dump();

}