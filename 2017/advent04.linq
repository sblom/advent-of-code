<Query Kind="Statements" />

var curqp = Util.CurrentQueryPath;
var dirname = Path.GetDirectoryName(curqp);
Directory.SetCurrentDirectory(dirname);
var inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp) + ".txt");
var input = File.ReadLines(inputName);

input.Select(x => x.Split(' ')).Select(x => (x, new HashSet<string>(x))).Where(x => x.Item1.Count() == x.Item2.Count()).Count();

var c = 0;
foreach (var line in input)
{
	if (!line.Split(' ').Select(x => x.OrderBy(ch=> ch)).Select(x=>new string(x.ToArray())).GroupBy(x => x).Select(x => x.Count()).Where(x => x > 1).Any())
	 c++;
}



c.Dump();