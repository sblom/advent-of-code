<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

//#define TEST

#region preamble
#load "..\Lib\Utils"
#load "..\Lib\BFS"
#endregion

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"3,4,3,1,2".GetLines();
#endif

var count = 0;

var lookup = new Dictionary<string, int>{
	{"abcefg", 0},
	{"cf", 1},
	{"acdeg", 2},
	{"acdfg", 3},
	{"bcdf", 4},
	{"abdfg", 5},
	{"abdefg", 6},
	{"acf", 7},
	{"abcdefg", 8},
	{"abcdfg", 9}
};

foreach (var line in lines)
{
	var parts = line.Split("|")[1];
	var segs = parts.Split(" ").Where(seg => new[] {2,4,3,7}.Contains(seg.Count())).Count();
	count += segs;
}

count.Dump();

var sum = 0;

foreach (var line in lines)
{
	var ans = line.Split("|")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
	var segs = line.Split("|")[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);

	bool dupe = false;

	foreach (var perm in Permutations("abcdefg"))
	{
		var map = perm.Zip("abcdefg").ToDictionary(x => x.First, x => x.Second);
		
		if (segs.All(seg => IsValid(seg, map)))
		{
			if (dupe) "dupe!".Dump();
			var N = ans.Select(a => lookup[Map(a, map)]).Aggregate(0, (n,d) => n * 10 + d);
			dupe = true;
			sum += N;
			break;
		}
	}
}

sum.Dump();


string Map(string seg, Dictionary<char,char> map)
{
	return string.Join("", seg.Select(c => map[c]).OrderBy(c => c).ToArray());
}

bool IsValid(string seg, Dictionary<char,char> map)
{
	return lookup.Keys.Contains(Map(seg,map));
}

static IEnumerable<string> Permutations(string word, string prefix = "")
{
	if (string.IsNullOrEmpty(word))
	{ yield return prefix; }

	// each charcater need to be permutated
	for (int i = 0; i < word.Length; i++)
	{
		// remove current char from original word, and append it to prefix, then permute recursively
		foreach (var item in Permutations(word.Remove(i, 1), prefix + word[i]))
		{
			yield return item;
		}
	}
}