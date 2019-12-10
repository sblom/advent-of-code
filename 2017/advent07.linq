<Query Kind="Statements" />

var curqp = Util.CurrentQueryPath;
var dirname = Path.GetDirectoryName(curqp);
Directory.SetCurrentDirectory(dirname);
var inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp) + ".txt");
var input = File.ReadLines(inputName);

var all = new Dictionary<string,(int,List<string>)>();
var aloft = new HashSet<string>();

foreach (var line in input)
{
	var split = line.Split('>');
	var above = split.Length > 1 ? split[1].Split(',').Select(x=>x.Trim()) : new List<string>();
	var name = split[0].Split('(')[0].Trim();
	
	all[name] = (int.Parse(split[0].Split('(')[1].Split(')')[0]), above.ToList());
}

int Weight(string prog)
{
	var subprogs = all[prog].Item2.Select(x => Weight(x));
	
	if (subprogs.Any(x => x != subprogs.FirstOrDefault()))
		(prog, subprogs).Dump();
	
	return all[prog].Item1 + subprogs.Sum();
}

Weight("uownj");