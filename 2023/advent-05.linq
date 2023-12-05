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
//#define CHECKED

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4".GetLines();
#endif

#if CHECKED
checked{
#endif

#endregion

List<List<(long to, long from, long len)>> maps = new();

var seeds = lines.First().Split(" ")[1..].Select(long.Parse).ToArray();

List<(long from, long len)> ranges = new();
for (int i = 0; i < seeds.Length / 2; i++)
{
    ranges.Add((seeds[i * 2], seeds[i * 2 + 1]));
}

$"{ranges.Select(x => x.len).Sum():0,000}".DumpTrace("Brute force total");

List<(long to, long from, long len)> section = null;

foreach (var line in lines.Skip(2).Where(x => !string.IsNullOrWhiteSpace(x)))
{    
    if (line[0] is not (>= '0' and <= '9'))
    {
        section = new();
        maps.Add(section);
        continue;
    }
    
    section.Add(line.Extract<(long,long,long)>(@"(\d+) (\d+) (\d+)"));
}

maps = maps.Select(map => map.OrderBy(x => x.from).ToList()).ToList();

foreach (var map in maps)
{
        seeds = seeds.Select(seed => { var diff = map.Where(m => seed >= m.from && seed < m.from + m.len).SingleOrDefault(); return diff != default ? seed - diff.from + diff.to : seed; }).ToArray();
}

seeds.Min().Dump("Part 1");

foreach (var map in maps)
{
    var nextranges = new List<(long,long)>();
    
    int i = 0;
    
    map[i].DumpTrace("map");
    
    foreach (var range in ranges.OrderBy(x => x.from))
    {
        var rem = range;
        
        while (i < map.Count)
        {
            rem.DumpTrace("rem");
            // We're entirely past the current map
            if (rem.from >= map[i].from + map[i].len)
            {
                i++;
                if (i < map.Count) map[i].DumpTrace("map");
                continue;
            }
            // We're entirely before the current map
            else if (rem.from + rem.len - 1 < map[i].from)
            {
                nextranges.Add((rem.from, rem.len));
                goto next_range;
            }
            // We're overlapping the current map
            else
            {
                // The part before the map
                if (rem.from < map[i].from)
                {
                    nextranges.Add((rem.from, map[i].from - rem.from));
                    rem = (map[i].from, rem.len - (map[i].from - rem.from));
                }
                // The part inside the map
                nextranges.Add((map[i].to + rem.from - map[i].from, Math.Min(rem.from + rem.len, map[i].from + map[i].len) - rem.from));
                // If we continue past the map
                if (rem.from + rem.len - 1 > map[i].from + map[i].len - 1)
                {
                    rem = (map[i].from + map[i].len, rem.from + rem.len - map[i].from - map[i].len);
                    continue;
                }
                else
                {
                    goto next_range;
                }
            }
        }
        
        nextranges.Add(rem);
        
next_range:;        
    }
    ranges = nextranges;
    nextranges.DumpTrace();
}

ranges.OrderBy(x => x.from).First().from.Dump("Part 2");

#if CHECKED
}
#endif
