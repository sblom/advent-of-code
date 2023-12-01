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
var lines = @"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8".GetLines();
#endif

var stacks = new List<Stack<char>>(9);
for (int i = 0; i < 9; i++)
{
    stacks.Add(new Stack<char>());
}

var crates = lines.Take(8).ToList();

for (int i = 7; i >= 0; i--)
{
    for (int j = 0; j < 9; j++)
    {
        int ch = 4 * j + 1;
        if (crates[i][ch] != ' ') stacks[j].Push(crates[i][ch]);
    }
}

var instrs = lines.Skip(10).Extract<(int count, int from, int to)>(@"move (\d+) from (\d+) to (\d+)$").Dump();

foreach (var instr in instrs)
{
    for (int c = 0; c < instr.count; c++)
    {
        stacks[instr.to - 1].Push(stacks[instr.from - 1].Pop());
    }
}

for (int i = 0; i < 9; i++)
{
    Console.Write(stacks[i].Peek());
}
Console.WriteLine();

stacks.Clear();
for (int i = 0; i < 9; i++)
{
    stacks.Add(new Stack<char>());
}
for (int i = 7; i >= 0; i--)
{
    for (int j = 0; j < 9; j++)
    {
        int ch = 4 * j + 1;
        if (crates[i][ch] != ' ') stacks[j].Push(crates[i][ch]);
    }
}

foreach (var instr in instrs)
{
    var temp = new Stack<char>();
    for (int c = 0; c < instr.count; c++)
    {
        temp.Push(stacks[instr.from - 1].Pop());
    }
    
    while (temp.Any())
    {
        stacks[instr.to - 1].Push(temp.Pop());
    }
}

for (int i = 0; i < 9; i++)
{
    Console.Write(stacks[i].Peek());
}
