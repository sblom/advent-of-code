<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>static UserQuery</Namespace>
</Query>

//#define TEST

#region preamble
#load "..\Lib\Utils"
#load "..\Lib\BFS"
#endregion

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc".GetLines();
#endif

var rules = Enumerable.Range(0,27).Select(i => new char[26]).ToArray();

var groups = lines.GroupLines().ToArray();

var template = groups[0].Single().Dump();
var ruleLines = groups[1].Extract<(string pair, string ins)>(@"(..) -> (.)");

foreach (var rule in ruleLines)
{
	var (pair, ins) = rule;
	rules[pair[0] - 'A'][pair[1] - 'A'] = ins[0];
}

char[] line = template.ToCharArray();

for (int c = 0; c < 10; c++)
{
	var next = new List<char>(line.Length * 2);
	
	for (var i = 0; i < line.Length - 1; i++)
	{
		next.Add(line[i]);
		if (rules[line[i] - 'A'][line[i + 1] - 'A'] != default(char))
		{
			next.Add(rules[line[i] - 'A'][line[i+1] - 'A']);
		}
	}
	next.Add(line[line.Length - 1]);
	
	line = next.ToArray();
}

var chargroups = line.GroupBy(ch => ch).OrderByDescending(g => g.Count());

var maxch = chargroups.First().Count();
var minch = chargroups.Last().Count();

(maxch - minch).Dump();


var ruleSets = groups[1].Extract<(string pair, string ins)>(@"(..) -> (.)").ToDictionary(x => x.pair, x => new[] { x.pair[0] + x.ins, x.ins + x.pair[1] });
var pairs = ("[" + template).Zip(template + "[").Select(chs => $"{chs.First}{chs.Second}").ToDictionary(x => x, _ => 1L);

for (int c = 0; c < 40; c++)
{
	var newPairs = new Dictionary<string, long>();

	pairs = pairs.SelectMany(pair => ruleSets.ContainsKey(pair.Key) ? ruleSets[pair.Key].Select(x => (x, pair.Value)) : new[] { (pair.Key, pair.Value) }).GroupBy(g => g.x).ToDictionary(g => g.Key, g => g.Sum(v => v.Value));
}

var scores = pairs.SelectMany(kvp => new[] { (kvp.Key[0], kvp.Value), (kvp.Key[1], kvp.Value)}).GroupBy(ch => ch.Item1).Select(g => (g.Key, g.Sum(x => x.Value))).OrderBy(ch => ch.Item2);

((scores.Last().Item2 - scores.Skip(1).First().Item2) / 2).Dump();