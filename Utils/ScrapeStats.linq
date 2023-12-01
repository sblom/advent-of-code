<Query Kind="Statements">
  <NuGetReference>HtmlAgilityPack</NuGetReference>
  <Namespace>HtmlAgilityPack</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

var hc = new HttpClient();
hc.DefaultRequestHeaders.Add("Cookie", $"session={Util.LoadString("adventofcode: session")}");

for (int y = 2015; y <= 2021; y++)
{
	var stats = await hc.GetStringAsync($"https://adventofcode.com/{y}/stats");
	var statsfilename = @$"c:\src\personal\aocodex\scrape\{y}\stats\20211224.html";
	
	await File.WriteAllTextAsync(statsfilename, stats);	
	
	for (int d = 1; d <= 25; d++)
	{
		var filename = @$"c:\src\personal\aocodex\scrape\{y}\day\{d}.html";
		
		if (File.Exists(filename)) continue;

		var webstr = await hc.GetStringAsync($"https://adventofcode.com/{y}/day/{d}");
		
		Directory.CreateDirectory(@$"c:\src\personal\aocodex\scrape\{y}\day");
		await File.WriteAllTextAsync(filename, webstr);
		
		filename.Dump();
	}

	for (int d = 1; d <= 25; d++)
	{
		var filename = @$"c:\src\personal\aocodex\scrape\{y}\leaderboard\day\{d}.html";

		if (File.Exists(filename)) continue;

		var webstr = await hc.GetStringAsync($"https://adventofcode.com/{y}/leaderboard/day/{d}");

		Directory.CreateDirectory(@$"c:\src\personal\aocodex\scrape\{y}\leaderboard\day");
		await File.WriteAllTextAsync(filename, webstr);

		filename.Dump();
	}
}