<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
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
var lines = @"rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7".GetLines();
#endif

#if CHECKED
checked{
#endif

AoC._outputs = new DumpContainer[] {new(), new()};
Util.HorizontalRun("Part 1,Part 2",AoC._outputs).Dump();

#endregion

int Hash(string str)
{
    int acc = 0;
    foreach (var ch in str)
    {
        acc += ch;
        acc *= 17;
        acc %= 256;
    }
    
    return acc;
}

var total = 0;

foreach (var line in lines.Single().Split(","))
{
    int acc = 0;
    foreach (var ch in line)
    {
        acc += ch;
        acc *= 17;
        acc %= 256;
    }
    
    total += acc;
}

total.Dump1();

List<(string label,int power)>[] boxes = new List<(string,int)>[256];
for (int i = 0; i < 256; i++)
{
    boxes[i] = new();
}

var instructions = lines.Single().Extract<List<(string label,string op)>>(@"((.*?)([=-].?),?)+");
//var instructions = lines.Single().Split(",").Extract<(string label,string op)>(@"(.*?)([=-].?)");

foreach (var instruction in instructions)
{
    var box = Hash(instruction.label);
    
    if (instruction.op[0] == '-')
    {
        boxes[box].RemoveAll(x => x.label == instruction.label);
    }
    else
    {
        var pos = boxes[box].FindIndex(lens => lens.label == instruction.label);
        if (pos != -1)
        {
            boxes[box][pos] = (instruction.label,instruction.op[1] - '0');
        }
        else
        {
            boxes[box].Add((instruction.label,instruction.op[1] - '0'));
        }
    }    
}

boxes.Select((box, bn) => box.Select((lens, ln) => (bn + 1) * (ln + 1) * lens.power).Sum()).Sum().Dump2();

#if CHECKED
}
#endif
