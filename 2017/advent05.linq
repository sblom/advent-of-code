<Query Kind="Statements" />

var curqp = Util.CurrentQueryPath;
var dirname = Path.GetDirectoryName(curqp);
Directory.SetCurrentDirectory(dirname);
var inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp) + ".txt");
var input = File.ReadLines(inputName);

var arr = input.Select(x => int.Parse(x)).ToArray();

int c = 0, i = 0;

try
{
	while(true){
	var d = arr[i];
	if (d >= 3) arr[i]--;
	else arr[i]++;
	i += d;
	c++;
	}
}
catch{
	
}
c.Dump();