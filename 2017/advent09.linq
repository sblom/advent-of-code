<Query Kind="Statements" />

var curqp = Util.CurrentQueryPath;
var dirname = Path.GetDirectoryName(curqp);
Directory.SetCurrentDirectory(dirname);
var inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp) + ".txt");
var input = File.ReadLines(inputName).First();

int i = 0, t = 0, d = 0, g = 0;
bool garbage = false;

while (i < input.Length)
{
	if (garbage)
	{
		if (input[i] == '!') i++;
		else if (input[i] == '>') garbage = false;
		else g++;
	}
	else if (input[i] == '{')
	{
		d++;
		t += d;
	}
	else if (input[i] == '}')
	{
		d--;
	}
	else if (input[i] == '<')
	{
		garbage = true;
	}
	
	i++;
}

t.Dump();
g.Dump();