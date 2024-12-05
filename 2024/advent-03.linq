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
var lines = @"xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))".GetLines();
#endif

// Why the hell does it need that $ in order to find more than one match?!
// It's because of the lazy .*? It can match zero characters and, having found one complete mul(), it can quit.
lines.Extract<List<(int,int)>>(@"(mul\((\d+),(\d+)\).*?)+$").Sum(x => x.Sum(y => y.Item1 * y.Item2)).Dump();

// This is probably a RegExtract bug.
//lines.Extract<List<(int,int)>>(@"mul\((\d+),(\d+)\)").Dump();

int t1 = 0, t2 = 0;
bool enabled = true;
foreach (var line in lines)
{
    foreach (Match match in Regex.Matches(line, @"mul\((\d+),(\d+)\)|do\(\)|don't\(\)"))
    {
        if (match.Groups[0].Value.StartsWith("don't")) enabled = false;
        else if (match.Groups[0].Value.StartsWith("do(")) enabled = true;
        else
        {
            var val = int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
            t1 += val;
            if (enabled) t2 += val;
        }

    }
}

t1.Dump();
t2.Dump();