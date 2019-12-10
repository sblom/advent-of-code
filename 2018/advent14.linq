<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();
var line = lines.First();
var num = int.Parse(line);

var scores = new List<int>(new[] { 3, 7 } );

int[] elflocs = new[] {0,1};

bool part1done = false;

string shift = "";

for (int i = 0; ; i++)
{
	var newchars = elflocs.Select(ii => scores[ii]).Sum().ToString();
	var newvals = newchars.Select(ch => ch - '0');
	scores.AddRange(newvals);
	shift += string.Join("", newvals);
	
	if (shift.Contains(line))
	{
		(scores.Count - shift.Length + shift.IndexOf(line)).Dump("Part 2");
	}
	
	if (shift.Length > 5) shift = shift.Substring(shift.Length - 5, 5);
	
	for (int ei = 0; ei < elflocs.Length; ei++)
	{
		int jump = scores[elflocs[ei]] + 1;
		jump -= (jump / scores.Count) * scores.Count;
		elflocs[ei] = (elflocs[ei] + jump) % scores.Count;
	}
	
	if (scores.Count > num + 10 && !part1done)
	{
		string.Join("", (scores.Skip(num).Take(10)).Select(ch => (char)(ch + '0'))).Dump("Part 1");
		part1done = true;
	}
}