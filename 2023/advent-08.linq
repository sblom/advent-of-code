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
var lines = @"LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)".GetLines();
#endif

#if CHECKED
checked{
#endif

#endregion

var instructions = lines.First();

instructions.Length.Dump();

var map = lines.Skip(2).Extract<(string node, string left, string right)>(@"(...) = \((...), (...)\)");

var mapdict = map.ToDictionary(x => x.node, x => (x.left, x.right));

var loc = "AAA";

long i = 0;
for (i = 0; ; i++)
{
    if (loc == "ZZZ") break;
    var node = mapdict[loc];
    loc = instructions[(int)i % instructions.Length] switch {
        'L' => node.left,
        'R' => node.right
    };
}

i.Dump("Part 1");

var lengths = new List<long>();

foreach (var start in mapdict.Keys.Where(x => x[2] == 'A'))
{
    loc = start;
    i = 0;
    for (i = 0; ; i++)
    {
        if (loc[2] == 'Z') break;
        var node = mapdict[loc];
        loc = instructions[(int)i % instructions.Length] switch
        {
            'L' => node.left,
            'R' => node.right
        };
    }
    
    lengths.Add(i);
}

// Find the LCM of all of the numbers in lengths
var lcm = lengths.Aggregate((x, y) => (x * y) / Gcd(x, y));
lcm.Dump("Part 2");

// Implement the Gcd function
static long Gcd(long a, long b)
{
    while (b != 0)
    {
        var temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

#if CHECKED
}
#endif
