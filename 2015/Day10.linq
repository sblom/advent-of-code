<Query Kind="Statements">
  <NuGetReference>System.Collections.Immutable</NuGetReference>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

var lines = await AoC.GetLinesWeb();
var line = lines.First();

LookAndSay(line).Dump();

string LookAndSay(string num)
{
	num = num + " ";
	var result = new StringBuilder();
	int i = 0;
	while (i < num.Length - 1)
	{
		int c = i;
		char ch = num[i];
		while (num[c] == ch)
			c++;
		result.Append((c - i).ToString());
		result.Append(ch);
		i = c;
	}
	return result.ToString();
}

for (int i = 0; i < 40; i++)
{
	line = LookAndSay(line);
}

line.Length.Dump("Part 1");

for (int i = 0; i < 10; i++)
{
	line = LookAndSay(line);
}

line.Length.Dump("Part 2");