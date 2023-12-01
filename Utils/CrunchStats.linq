<Query Kind="Statements" />

var timeStats = new List<DayStats>();
var solveStats = new List<SolveStats>();

for (int y = 2015; y <= 2021; y++)
{
	var starcountsRaw = await File.ReadAllTextAsync(@$"c:\src\personal\aocodex\scrape\{y}\stats\20211224.html");
	var starcounts = Regex.Matches(starcountsRaw, @"<a href=""[^""]+"">\W*(\d+)\W*<span class=""stats-both"">\W*(\d+)</span>  <span class=""stats-firstonly"">\W*(\d+)</span>").ToDictionary(x => int.Parse(x.Groups[1].Value), x => (both: int.Parse(x.Groups[2].Value), firstonly: int.Parse(x.Groups[3].Value)));
	
	for (int d = 1; d <= 25; d++)
	{
		var filename = @$"c:\src\personal\aocodex\scrape\{y}\leaderboard\day\{d}.html";
		var board = await File.ReadAllTextAsync(filename);

		var g1 = Regex.Match(board, @"both stars</span> on Day \d+:</p><div class=""leaderboard-entry""><span class=""leaderboard-position"">  1\)</span> <span class=""leaderboard-time"">([^<]*)</span>").Groups[1].Value;
		var gs100 = Regex.Matches(board, @"<div class=""leaderboard-entry""><span class=""leaderboard-position"">100\)</span> <span class=""leaderboard-time"">([^<]*)</span>");
		var g100 = gs100[0].Groups[1].Value;
		var s100 = gs100[1].Groups[1].Value;
		var s1 = Regex.Match(board, @"first star</span> on Day \d+:</p><div class=""leaderboard-entry""><span class=""leaderboard-position"">  1\)</span> <span class=""leaderboard-time"">([^<]*)</span>").Groups[1].Value;
		
		timeStats.Add(new DayStats(y,d,TimeSpan.Parse(s1[7..]),TimeSpan.Parse(s100[7..]),TimeSpan.Parse(g1[7..]),TimeSpan.Parse(g100[7..])));
		solveStats.Add(new SolveStats(y,d,starcounts[d].both + starcounts[d].firstonly,starcounts[d].both));
	}
}

timeStats.Dump();
solveStats.Dump();

record DayStats(int year, int day, TimeSpan s1, TimeSpan s100, TimeSpan g1, TimeSpan g100);
record SolveStats(int year, int day, int silver, int gold);