<Query Kind="Statements">
  <NuGetReference>System.Collections.Immutable</NuGetReference>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

var lines = await AoC.GetLinesWeb();
var line = lines.First().ToArray();

void strinc(char[] pwd)
{
	var pos = pwd.Length - 1;

	while (pos >= 0)
	{
		if (pwd[pos] == 'z')
		{
			pwd[pos] = 'a';
			pos--;
			continue;
		}
		else
		{
			pwd[pos]++;
			if (pwd[pos] == 'i' || pwd[pos] == 'l' || pwd[pos] == 'o')
			{
				pwd[pos]++;
				for (pos++; pos < pwd.Length; pos++)
				{
					pwd[pos] = 'a';
				}
			}
			break;
		}
	}
	if (pos < 0)
	{
		pwd.Dump();
		throw new OverflowException();
	}
}

//line = "abcdefgh".ToCharArray();

int n = 0;

while (true)
{
	strinc(line);
	
	bool r1 = false, r2 = false, r3 = false;
	var pairs = new HashSet<char>();
	
	for (int i = 0; i < line.Length; i++)
	{
		if (i < line.Length - 2 && (int)line[i] + 1 == (int)line[i + 1] && (int)line[i + 1] + 1 == (int)line[i + 2])
		{
			r1 = true;
		}
		if (i < line.Length - 1 && (int)line[i] == (int)line[i + 1])
		{
			pairs.Add(line[i]);
		}
	}
	if (r1 && pairs.Count >= 2)
	{
		n++;
		string.Join("", line).Dump($"Part {n}");
		if (n == 2) break;
	}
}