<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
</Query>

IEnumerable<int> Pattern(int pos)
{
	while (true)
	{
		for (int i = 0; i < pos; i++)
		{
			yield return 0;
		}
		for (int i = 0; i < pos; i++)
		{
			yield return 1;
		}
		for (int i = 0; i < pos; i++)
		{
			yield return 0;
		}
		for (int i = 0; i < pos; i++)
		{
			yield return -1;
		}
	}
}

var lines = await AoC.GetLinesWeb();
//lines = new[] { "69317163492948606335995924319873" };

var digits = lines.Select(L => L.Trim()).First().Select(c => c - '0');
var offset = int.Parse(string.Join("",digits.Take(7)));
var backlen = digits.Count() * 10_000 - offset;

for (int c = 0; c < 100; c++)
{
	digits = Enumerable.Range(1, digits.Count()).Select(d => digits.Zip(Pattern(d).Skip(1), (a, b) => a * b)).Select(d => d.Sum()).Select(d => Math.Abs(d) % 10).ToList();
}

string.Join("",digits.Take(8).Select(d => (char)(d + '0'))).Dump("Part 1");

// 75945609 is wrong

digits = lines.Select(L => L.Trim()).First().Select(c => c - '0');

var reverse = digits.Reverse().Repeat();

var suffix = reverse.Take(backlen);

for (int c = 0; c < 100; c++)
{
	suffix = suffix.Scan(0, (a, b) => a + b).Select(s => Math.Abs(s) % 10);
}

string.Join("", suffix.Reverse().Take(8)).Dump("Part 2");