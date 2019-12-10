<Query Kind="Statements" />

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var lines = File.ReadAllLines("advent01.txt");

var line = lines[0];

var sum = 0;

for (int i = 1; i < line.Length; ++i)
{
	if (line[i] == line[(i + line.Length / 2) % line.Length])
		sum += line[i] - '0';
}

if (line[line.Length - 1] == line[0])
{
	sum += line[0] - '0';
}

sum.Dump();