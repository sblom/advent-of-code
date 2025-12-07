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
var lines = $@"Register A: {k}
Register B: 0
Register C: 0

Program: 2,4,1,3,7,5,4,0,1,3,0,3,5,5,3,0".Replace("\\", "").GetLines();
#endif

var groups = lines.GroupLines();
var instrs = groups.Skip(1).First().Extract<List<int>>(@"Program: ((\d+),?)+").First();

long[] reg = new long[3];
var ip = 0;
var output = new List<long>();
var goal = new[] { 0, 3, 5, 5, 3, 0, 3, 1, 0, 4, 5, 7, 3, 1, 4, 2 };

foreach (var (i, x) in groups.First().Extract<long>(@"(\d+)").Index())
{
    reg[i] = x;
}


Solve(0,0);

void Solve(long partial, int depth)
{
    for (int i = 0; i < 8; i++)
    {
        long a = partial + i, b = 0, c = 0;

        b = (a % 8) ^ 3;
        c = a >> (int)b;
        if (goal[depth] == (a ^ c) % 8)
        {
            if (depth == goal.Length - 1)
            {
                (partial + i).Dump(); return;
            }
            else if (partial + i != 0)
            {
                Solve((partial + i) << 3, depth + 1);
            }
        }
    }
}

long combo(int op)
{
    return op switch {
        0 => 0,
        1 => 1,
        2 => 2,
        3 => 3,
        4 => reg[0],
        5 => reg[1],
        6 => reg[2],
        7 => throw new Exception()
    };
}

int adv(int op)
{
    $"adv {reg[0]} {combo(op)}".DumpTrace();
    reg[0] = (long)(reg[0] / (1L << (int)combo(op)));
    $"a = {reg[0]}".DumpTrace();    
    return 0;
}

int bxl(int op)
{
    $"bxl {reg[1]} {op}".DumpTrace();
    reg[1] = reg[1] ^ op;
    $"{reg[1]}".DumpTrace();
    return 0;
}

int bst(int op)
{
    $"bst {combo(op)}".DumpTrace();
    reg[1] = 0b111 & combo(op);
    $"{reg[1]}".DumpTrace();
    return 0;
}
int jnz(int op)
{
    $"jnx {reg[0]}".DumpTrace();
    if (reg[0] != 0) ip = op - 1;
    $"{ip}".DumpTrace();    
    return 0;
}
int bxc(int op)
{
    $"bxc {reg[1]} {reg[2]}".DumpTrace();
    reg[1] = reg[1] ^ reg[2];
    $"{reg[1]}".DumpTrace();
    return 0;
}
int @out(int op)
{
    $"out {combo(op)}".DumpTrace();
    output.Add(combo(op)%8);
    $"".DumpTrace();
    return 0;
}
int bdv(int op) { 
    $"bdv {reg[0]} {combo(op)}".DumpTrace();
    reg[1] = (int)(reg[0] / (1 << (int)combo(op)));
    $"{reg[1]}".DumpTrace();    
    return 0;
}
int cdv(int op)
{
    $"cdv {reg[0]} {combo(op)}".DumpTrace();
    reg[2] = (long)(reg[0] / (1L << (int)combo(op)));
    $"{reg[2]}".DumpTrace();    
    return 0;
}
