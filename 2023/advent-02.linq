<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green".GetLines();
#endif

List<(int n, List<List<(int,string)>>)> things = new();

foreach (var game in lines)
{
    var gamesplit = game.Split(": ");
    var num = int.Parse(gamesplit[0].Split(" ")[1]);

    var sets = gamesplit[1].Split("; ");
    var setlist = sets.Select(x => x.Split(", ").Select(y => y.Split(" ")).Select(z => (int.Parse(z[0]),z[1])).ToList()).ToList();
    
    things.Add((num, setlist));
}

int possible = 0;

foreach (var thing in things)
{
    foreach (var set in thing.Item2)
    {
        foreach (var game in set)
        {
            if (game.Item1 > game.Item2 switch {"red" => 12, "green" => 13, "blue" => 14}) goto impossible;
        }
    }
    possible += thing.n;
impossible:;
}

possible.Dump("Part 1");

long total = 0;

foreach (var thing in things)
{
    Dictionary<string, int> colors = new Dictionary<string, int>
    {
        ["red"] = 0,
        ["green"] = 0,
        ["blue"] = 0
    };

    foreach (var set in thing.Item2)
    {
        foreach (var game in set)
        {
            colors[game.Item2] = Math.Max(colors[game.Item2], game.Item1);
        }
    }
    int power = colors.Select(x => x.Value).Aggregate((x,y) => x * y);
    total += power;
}

total.Dump("Part 2");