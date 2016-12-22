<Query Kind="Statements" />

var line = File.ReadAllText(@"c:\users\sblom\desktop\advent09.txt");

line = line.Trim();

var regex = new Regex(@"(?<before>.*?)(?<marker>\((?<bytes>\d+)x(?<reps>\d+)\))(?<rest>.*)");

long DecompressLen(string str)
{
	long len = 0;

	while (regex.IsMatch(str))
	{
		var match = regex.Match(str);
		len += match.Groups["before"].Length;
		int bytes = int.Parse(match.Groups["bytes"].Value);
		int reps = int.Parse(match.Groups["reps"].Value);
		len += DecompressLen(match.Groups["rest"].Value.Substring(0,bytes)) * reps;
		str = match.Groups["rest"].Value.Substring(bytes);
	}

	len += str.Length;
	
	return len;
}

DecompressLen(line).Dump();