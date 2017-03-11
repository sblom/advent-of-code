<Query Kind="Statements" />

var set = new HashSet<(int x, int y)> ();

var lines = File.ReadLines(@"c:\users\sblom\desktop\advent08.txt");
var regex = new Regex(@"(?<rect>rect (?<x>\d+)x(?<y>\d+))|(?<row>rotate row y=(?<y>\d+) by (?<x>\d+))|(?<column>rotate column x=(?<x>\d+) by (?<y>\d+))");

int X = 50;
int Y = 6;

foreach (var line in lines)
{
	var match = regex.Match(line);
	var newset = new HashSet<(int x, int y)> ();
	int x = int.Parse(match.Groups["x"].Value);
	int y = int.Parse(match.Groups["y"].Value);
	switch (match)
	{
		case Match m when m.Groups["rect"].Success:
			for (int i = 0; i < y; ++i)
			{
				for (int j = 0; j < x; ++j)
				{
					set.Add((j,i));
				}
			}
			break;
		case Match m when m.Groups["row"].Success:
			foreach (var point in set)
			{
				if (point.y == y)
				{
					newset.Add(((point.x + x) % X, point.y));
				}
				else
				{
					newset.Add(point);
				}
			}
			set = newset;
			break;
		case Match m when m.Groups["column"].Success:
			foreach (var point in set)
			{
				if (point.x == x)
				{
					newset.Add((point.x, (point.y + y) % Y));
				}
				else
				{
					newset.Add(point);
				}
			}
			set = newset;
			break;
	}
}

for (int i = 0; i < Y; ++i)
{
	for (int j = 0; j < X; ++j)
	{
		Console.Write(set.Contains((j, i)) ? "X" : " ");
	}
	Console.WriteLine();
}
