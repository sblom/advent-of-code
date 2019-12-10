<Query Kind="Statements" />

var curqp = Util.CurrentQueryPath;
var dirname = Path.GetDirectoryName(curqp);
Directory.SetCurrentDirectory(dirname);
var inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp) + ".txt");
var input = File.ReadLines(inputName);

var andrx = new Regex(@"(\w+) AND (\w+) -> (\w+)");
var orrx = new Regex(@"(\w+) OR (\w+) -> (\w+)");
var rshiftrx = new Regex(@"(\w+) RSHIFT (\d+) -> (\w+)");
var lshiftrx = new Regex(@"(\w+) LSHIFT (\d+) -> (\w+)");
var notrx = new Regex(@"NOT (\w+) -> (\w+)");
var setrx = new Regex(@"(\w+) -> (\w+)");

var dict = new Dictionary<string, Func<ushort>>()
{
	{"1", () => 1 }
};

ushort get(string key)
{
	if (ushort.TryParse(key,out var num))
	{
		return num;
	}
	else
	{
		var result = dict[key]();
		dict[key] = () => result;
		return result;
	}
}

foreach (var line in input)
{
	if (andrx.IsMatch(line)){
		var match = andrx.Match(line);
		Console.WriteLine($"{match.Groups[1].Value} AND {match.Groups[2].Value} -> {match.Groups[3].Value}");
		dict[match.Groups[3].Value] = () => { Console.WriteLine($"Evaluating {match.Groups[1].Value} AND {match.Groups[2].Value} -> {match.Groups[3].Value}"); return (ushort)(get(match.Groups[1].Value) & get(match.Groups[2].Value));};

	}
	else if (orrx.IsMatch(line))
	{
		var match = orrx.Match(line);
		dict[match.Groups[3].Value] = () => { Console.WriteLine($"Evaluating {match.Groups[1].Value} OR {match.Groups[2].Value} -> {match.Groups[3].Value}"); return (ushort)(get(match.Groups[1].Value) | get(match.Groups[2].Value)); };
		Console.WriteLine($"{match.Groups[1].Value} OR {match.Groups[2].Value} -> {match.Groups[3].Value}");
	}
	else if (lshiftrx.IsMatch(line))
	{
		var match = lshiftrx.Match(line);
		dict[match.Groups[3].Value] = () => { Console.WriteLine($"Evaluating {match.Groups[1].Value} LSHIFT {match.Groups[2].Value} -> {match.Groups[3].Value}"); return (ushort)(get(match.Groups[1].Value) << ushort.Parse(match.Groups[2].Value)); };
		Console.WriteLine($"{match.Groups[1].Value} LSHIFT {match.Groups[2].Value} -> {match.Groups[3].Value}");		
	}
	else if (rshiftrx.IsMatch(line))
	{
		var match = rshiftrx.Match(line);
		dict[match.Groups[3].Value] = () => { Console.WriteLine($"Evaluating {match.Groups[1].Value} RSHIFT {match.Groups[2].Value} -> {match.Groups[3].Value}"); return (ushort)(get(match.Groups[1].Value) >> ushort.Parse(match.Groups[2].Value));};
		Console.WriteLine($"{match.Groups[1].Value} RSHIFT {match.Groups[2].Value} -> {match.Groups[3].Value}");
	}
	else if (notrx.IsMatch(line))
	{
		var match = notrx.Match(line);
		Console.WriteLine($"NOT {match.Groups[1].Value} -> {match.Groups[2].Value}");
		dict[match.Groups[2].Value] = () =>
		{
			Console.WriteLine($"Evaluating NOT {match.Groups[1].Value} -> {match.Groups[2].Value}"); return (ushort)(~get(match.Groups[1].Value));
		};
	}
	else if (setrx.IsMatch(line))
	{
		var match = setrx.Match(line);
		Console.WriteLine($"{match.Groups[1].Value} -> {match.Groups[2].Value}");
		dict[match.Groups[2].Value] = () => { Console.WriteLine($"Evaluating {match.Groups[1].Value} -> {match.Groups[2].Value}"); try { return ushort.Parse(match.Groups[1].Value); } catch { return get(match.Groups[1].Value); } };
	}
}

dict["a"]().Dump();