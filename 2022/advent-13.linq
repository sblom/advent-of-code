<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
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

#if !TEST
var lines = (await AoC.GetLinesWeb()).ToArray();
#else
var lines = @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]".GetLines().ToArray();
#endif

var groups = lines.Chunk(3);

int c = 0;

foreach (var (group,index) in groups.Select((g,i) => (g,i)))
{
    var tree1 = JsonSerializer.Deserialize<JsonArray>(group[0])!;
    var tree2 = JsonSerializer.Deserialize<JsonArray>(group[1])!;
    
    if (Compare(tree1,tree2) == 1) c += index + 1;
}

c.Dump("Part 1");

List<string> packets = new();

foreach (var line in lines.Concat(new[] { "[[2]]", "[[6]]" }))
{
    if (line == "") 
        continue;
    
    int i = 0;
    for (i = 0; i < packets.Count; i++)
    {
        if (Compare(JsonSerializer.Deserialize<JsonArray>(line)!, JsonSerializer.Deserialize<JsonArray>(packets[i])!) == 1)
        {
            packets.Insert(i,line);
            break;
        }
    }
    if (i == packets.Count)
    {
        packets.Add(line);
    }
}

var a = packets.FindIndex(x => x == "[[2]]") + 1;
var b = packets.FindIndex(x => x == "[[6]]") + 1;
(a * b).Dump("Part 2");

int Compare(JsonNode arr1, JsonNode arr2)
{
    if (arr1 is JsonArray && arr2 is JsonValue)
    {
        return Compare(arr1, new JsonArray(arr2.GetValue<int>()));
    }
    else if (arr2 is JsonArray && arr1 is JsonValue)
    {
        return Compare(new JsonArray(arr1.GetValue<int>()), arr2);
    }
    else if (arr1 is JsonArray a1 && arr2 is JsonArray a2)
    {
        int i = 0;
        for (i = 0; i < a1.Count && i < a2.Count; i++)
        {
            var comp = Compare(a1[i]!, a2[i]!);
            if (comp == 0) continue;
            else return comp;
        }
        if (i < a1.Count) return -1;
        if (i < a2.Count) return 1;
        return 0;
    }
    else if (arr1 is JsonValue && arr2 is JsonValue)
    {
        if (arr1.GetValue<int>() < arr2.GetValue<int>()) return 1;
        if (arr1.GetValue<int>() > arr2.GetValue<int>()) return -1;
        return 0;
    }
    else
    {
        arr1.Dump();
        arr2.Dump();
        throw new Exception();
    }
}