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
//#define PART2

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"".GetLines();
#endif

foreach (var line in lines)
{
    (line.Replace(": ", " == ") + ", ").Dump();
}

Dictionary<string, rhs> monkeys = new();
Dictionary<string, string> strMonkeys = new();

foreach (var line in lines)
{
    
    
    var toks = line.Split(": ");
    
    strMonkeys[toks[0]] = toks[1];

    if (int.TryParse(toks[1], out var val))
    {
        monkeys[toks[0]] = new val(val);
    }
    else
    {
        var rhs = toks[1].Split(" ");
        monkeys[toks[0]] = new op(rhs[0], rhs[1][0], rhs[2]);
    }
}

long GetVal(rhs r)
{
checked{
    if (r is val v) return v.v;
    else
    {
        var o = r as op;
        return o.o switch
        {
            '*' => GetVal(monkeys[o.dep1]) * GetVal(monkeys[o.dep2]),
            '/' => GetVal(monkeys[o.dep1]) / GetVal(monkeys[o.dep2]),
            '+' => GetVal(monkeys[o.dep1]) + GetVal(monkeys[o.dep2]),
            '-' => GetVal(monkeys[o.dep1]) - GetVal(monkeys[o.dep2])
        };
    }
}
}

string GetStr(rhs r)
{
    if (r == monkeys["humn"]) return "humn";
    else if (r is val v) return v.v.ToString();
    {
        var o = r as op;
        return o.o switch
        {
            '*' => $"({GetStr(monkeys[o.dep1])}) * ({GetStr(monkeys[o.dep2])})",
            '/' => $"({GetStr(monkeys[o.dep1])}) / ({GetStr(monkeys[o.dep2])})",
            '+' => $"({GetStr(monkeys[o.dep1])}) + ({GetStr(monkeys[o.dep2])})",
            '-' => $"({GetStr(monkeys[o.dep1])}) - ({GetStr(monkeys[o.dep2])})"
        };
    }
}

GetVal(monkeys["root"]).Dump("Part 1");


var eq = strMonkeys["root"];

var eqsides = eq.Split(" ");
"Solve[".Dump();
GetStr(monkeys[eqsides[0]]).Dump();
"==".Dump();
GetStr(monkeys[eqsides[2]]).Dump();
",humn]".Dump();

record rhs;
record val(long v): rhs;
record op(string dep1, char o, string dep2): rhs;