<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#region Preamble

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST
#define CHECKED

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green".GetLines();
#endif

#if CHECKED
checked{
#endif

#endregion

var games = lines.Extract<game>(@"Game (\d+): (((\d+) (\w+),? ?)+;? ?)+");

int possible = 0;

foreach (var game in games)
{
    foreach (var draw in game.draws)
    {
        foreach (var color in draw.colors)
        {
            if (color.count > color.color switch {"red" => 12, "green" => 13, "blue" => 14}) goto impossible;
        }
    }
    possible += game.id;
impossible:;
}

possible.Dump("Part 1");

long total = 0;

foreach (var game in games)
{
    var colors = new Dictionary<string, int>
    {
        ["red"] = 0,
        ["green"] = 0,
        ["blue"] = 0
    };

    foreach (var draw in game.draws)
    {
        foreach (var color in draw.colors)
        {
            colors[color.color] = Math.Max(colors[color.color], color.count);
        }
    }
    int power = colors.Select(x => x.Value).Aggregate((x, y) => x * y);
    total += power;
}

total.Dump("Part 2");

record game(int id, List<draw> draws);
record draw(List<(int count, string color)> colors);

#if CHECKED
}
#endif
