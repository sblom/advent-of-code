<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Text.Json.Nodes</Namespace>
  <Namespace>System.Text.Json</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST
#define PART2

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"1=-0-2
12111
2=0=
21
2=01
111
20012
112
1=-1=
1-12
12
1=
122".Replace("\t", "    ").Replace("\r", "").Replace("\\","").Split("\n");
#endif

long tot = 0;
foreach (var line in lines)
{    
    tot += line.Aggregate(0L, (acc, ch) => acc * 5 + ch switch {'2' => 2,'1' => 1,'0' => 0,'-' => -1,'=' => -2});;
}

ImmutableList<sbyte> acc = ImmutableList<sbyte>.Empty;

sbyte carry = 0;
while (tot > 0 || carry > 0)
{
    sbyte digit = (sbyte)(tot % 5 + carry);
    if (digit >= 3)
    {
        digit -= 5;
        carry = 1;
    }
    else
    {
        carry = 0;
    }
    
    acc = acc.Add(digit);
    
    tot /= 5;
}

string.Join("", acc.Reverse().Select(x => x switch { -2 => '=', -1 => '-', 0 => '0', 1 => '1', 2 => '2' })).Dump();

