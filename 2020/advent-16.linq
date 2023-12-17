<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

var lines = AoC.GetLines().ToArray();

var rules = lines.GroupLines().First().Extract<(string name, int lo1, int hi1, int lo2, int hi2)>(@"(.*): (\d+)-(\d+) or (\d+)-(\d+)").ToArray();
var ticket = lines.GroupLines().Skip(1).First().Skip(1).First().Split(",").Select(long.Parse).ToArray();
var tickets = lines.GroupLines().Skip(2).First().Skip(1).Select(x => x.Split(",").Select(long.Parse).ToArray()).ToArray();

tickets.Select(ticket => ticket.Where(val => !rules.Any(rule => (rule.lo1 <= val && rule.hi1 >= val) || (rule.lo2 <= val && rule.hi2 >= val))).Sum()).Sum().Dump("Part 1");

var valid = tickets.Where(ticket => ticket.All(val => rules.Any(rule => (rule.lo1 <= val && rule.hi1 >= val) || (rule.lo2 <= val && rule.hi2 >= val)))).ToArray();

var colcounts = rules.ToDictionary(x => x.name, x => 0);

var colstats = Enumerable.Range(0,20).Select(i => (i, rules.Where(rule => valid.Select(x => x[i]).All(val => (rule.lo1 <= val && rule.hi1 >= val) || (rule.lo2 <= val && rule.hi2 >= val))).Select(rule => rule.name).ToHashSet())).OrderBy(x => x.Item2.Count).ToArray();

var colnums = new Dictionary<string,int>();

int c = 0;

foreach (var colstat in colstats)
{
	foreach (var candidate in colstat.Item2)
	{
		colcounts[candidate]++;
		if (!colnums.ContainsKey(candidate)) colnums[candidate] = colstat.i;
	}
}

var keycols = colnums.Where(x => x.Key.StartsWith("departure")).Select(x => x.Value).ToHashSet();

ticket.Where((v,i) => keycols.Contains(i)).Aggregate(1L, (x,y) => x*y).Dump("Part 2");