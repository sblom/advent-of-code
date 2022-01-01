<Query Kind="Statements" />

var lines = File.ReadLines(@"C:\users\sblom\desktop\advent07.txt");

int countAbba = 0;

foreach (var line in lines)
{
	bool isAbba = false;
	bool inHypernet = false;
	for (int i = 0; i < line.Length - 2; ++i)
	{
		char ch = line[i];
		if (ch == '[')
			inHypernet = true;
		else if (ch == ']')
			inHypernet = false;
		else
		{
			if (line[i] == line[i + 2] && line[i] != line[i + 1])
			{
				if (!inHypernet)
				{
					if ("[]".Contains(line[i]) || "[]".Contains(line[i+1])) continue;
					Regex regex = new Regex($@"\[[^\[\]]*{line[i + 1]}{line[i]}{line[i + 1]}[^\[\]]*\]");
					if (regex.IsMatch(line))
					{
						countAbba++;
						break;
					}
				}
			}
		}			
	}
}

countAbba.Dump();