<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#LINQPad checked+

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = $@"1
2
3
2024".Replace("\\", "").GetLines();
    //   2,4,1,4,7,5,4,1,1,4,5,5,0,3,3,0

#endif

var swaps = new Dictionary<string, string>
{
    ["vcf"] = "z10",
    ["z10"] = "vcf",
    ["z17"] = "fhg",
    ["fhg"] = "z17",
    ["fsq"] = "dvb",
    ["dvb"] = "fsq",
    ["z39"] = "tnc",
    ["tnc"] = "z39",
};

var dc = new DumpContainer().Dump();

var groups = lines.GroupLines();

var inits = groups.First().Extract<(string,int)>(@"(...): (\d)");

var gates = groups.Skip(1).First().Extract<(string,string,string,string)>(@"(...) (AND|OR|XOR) (...) -> (...)");

var vals = inits.ToDictionary(x => x.Item1, x => x.Item2);
var remgates = gates.Select(x => string.Compare(x.Item1, x.Item3) < 0 ? (x.Item1, x.Item3, swaps.ContainsKey(x.Item4) ? swaps[x.Item4] : x.Item4, x.Item2) : (x.Item3, x.Item1, swaps.ContainsKey(x.Item4) ? swaps[x.Item4] : x.Item4, x.Item2)).ToList();

while (remgates.Count > 0)
{
    var next = remgates.Where(g => vals.ContainsKey(g.Item1) && vals.ContainsKey(g.Item2)).First();

    switch (next.Item4)
    {
        case "AND":
            vals[next.Item3] = vals[next.Item1] & vals[next.Item2];
            break;
        case "OR":
            vals[next.Item3] = vals[next.Item1] | vals[next.Item2];
            break;
        case "XOR":
            vals[next.Item3] = vals[next.Item1] ^ vals[next.Item2];
            break;
    }
    remgates.Remove(next);
}

string.Join("", vals.Where(kv => kv.Key.StartsWith("z")).OrderByDescending(kv => kv.Key).Select(x => x.Value.ToString()));

string.Join("", gates.Select(x => $"{x.Item1}->{x.Item4}{x.Item2},{x.Item3}->{x.Item4}{x.Item2},"));

remgates = gates.Select(x => string.Compare(x.Item1, x.Item3) < 0 ? (x.Item1, x.Item3, swaps.ContainsKey(x.Item4) ? swaps[x.Item4] : x.Item4, x.Item2) : (x.Item3, x.Item1, swaps.ContainsKey(x.Item4) ? swaps[x.Item4] : x.Item4, x.Item2)).ToList();

var map = new Dictionary<string,string>();

for (int i = 0; i < 45; i++)
{
    string c,s;
    if (i == 0) map[$"c{i:00}"] = remgates.Where(x => x.Item1 == $"x{i:00}" && x.Item2 == $"y{i:00}" && x.Item4 == "AND").First().Item3;
    map[$"s{i:00}"] = remgates.Where(x => x.Item1 == $"x{i:00}" && x.Item2 == $"y{i:00}" && x.Item4 == "XOR").First().Item3;
    if (i > 0)
    {
        map[$"a{i:00}"] = remgates.Where(x => x.Item1 == $"x{i:00}" && x.Item2 == $"y{i:00}" && x.Item4 == "AND").First().Item3;
        dc.UpdateContent(map);
        map[$"b{i:00}"] = remgates.Where(x => ((x.Item1 == map[$"c{i - 1:00}"] && x.Item2 == map[$"s{i:00}"]) || (x.Item1 == map[$"s{i:00}"] && x.Item2 == map[$"c{i - 1:00}"])) && x.Item4 == "AND").First().Item3;
        dc.UpdateContent(map);
        map[$"c{i:00}"] = remgates.Where(x => ((x.Item1 == map[$"a{i:00}"] && x.Item2 == map[$"b{i:00}"]) || (x.Item1 == map[$"b{i:00}"] && x.Item2 == map[$"a{i:00}"])) && x.Item4 == "OR").First().Item3;
        dc.UpdateContent(map);
    }
    //map.Dump();
}

string.Join(",",swaps.Keys.Order()).Dump();
