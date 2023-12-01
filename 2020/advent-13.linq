<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

var lines = AoC.GetLines().ToArray();

var time = long.Parse(lines[0]);

var routes = lines[1].Split(',').Where(x => x != "x").Select(long.Parse).ToArray();

var wait = routes.Select(r => ((((time - 1) / r) + 1) * r) - time).Zip(routes).MinBy(x => x.First);

(wait.First * wait.Second).Dump("Part 1");

var schedules = lines[1].Split(',').Select((x,i) => (x,i)).Where(x => x.x != "x").Select(x => (x: long.Parse(x.x), x.i));

long b = 1;
long t = 0;

foreach (var sched in schedules)
{
	// Obnoxious workaround for C# treating (-i % x) as remainder instead of mod
	for (t = t % b; t % sched.x != (sched.x + (-sched.i % sched.x)) % sched.x; t += b);

	b *= sched.x;
	//($"{t:0,000}, {b:0,000}").Dump();
}

t.Dump("Part 2");