<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

var hc = new HttpClient();

hc.DefaultRequestHeaders.Add("cookie", "session=53616c7465645f5f4be8cd650c2114ef9e1eb510d1474a634aa241ec91bd1a2c2dee1504c3a933d0730f08ddbdd18dcc");

foreach (var i in (1..(25 + 1)))
{
	var url = $"https://adventofcode.com/2020/day/{i}/input";	
	var contents = await hc.GetStringAsync(url);
	
	File.WriteAllText(@$"c:\users\sblom\Documents\LINQPad Queries\advent\2020\advent-{i:00}.txt", contents);
}