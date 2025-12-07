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

#define TEST

    long[] reg = new long[3];
    var ip = 0;
var output = new List<long>();
var goal = new[] {0,3,5,5,3,0,3,1,0,4,5,7,3,1,4,2};
//long j = 0b11_000_100_101_001_011_000_001_100_101_111_000_111_000_110_101L;
//           0   3   5   5   3   0   3   1   0   4   5   7   3   1   4   2
//
//for (long n = 0; ; n++)
long j = 0b11_000_100_101_001_011_000_001_100_101_111_000_000_000_000_000L;
//long j = 9641154550269;
for (long k = j; ; k++)
{
    
    
    //1000001100101111001100111111101
#if !TEST
var lines = await AoC.GetLinesWeb();
#else
    var lines = $@"Register A: {k}
Register B: 0
Register C: 0

Program: 2,4,1,3,7,5,4,0,1,3,0,3,5,5,3,0".Replace("\\", "").GetLines();
    //   2,4,1,4,7,5,4,1,1,4,5,5,0,3,3,0

#endif

    var groups = lines.GroupLines();
    foreach (var (i, x) in groups.First().Extract<long>(@"(\d+)").Index())
    {
        reg[i] = x;
    }
    ip = 0;
    output.Clear();

    var instrs = groups.Skip(1).First().Extract<List<int>>(@"Program: ((\d+),?)+").First();

    while (ip < instrs.Count)
    {
        var res = instrs[ip] switch
        {
            0 => adv(instrs[++ip]),
            1 => bxl(instrs[++ip]),
            2 => bst(instrs[++ip]),
            3 => jnz(instrs[++ip]),
            4 => bxc(instrs[++ip]),
            5 => @out(instrs[++ip]),
            6 => bdv(instrs[++ip]),
            7 => cdv(instrs[++ip])
        };
        ip++;
    }

    if (output[0] == 2 && output[1] == 4 && output[2] == 1 && output[3] == 3 && output[4] == 7 && output[5] == 5 && output[6] == 4 && output[7] == 0 && output[8] == 1 && output[9] == 3 && output[10] == 0 /* && output[11] == 3 && output[12] == 5 && output[13] == 5 && output[14] == 3 && output[15] == 0*/)
    {
        output.Dump();
        k.Dump();
    }
    //if ("2,4,1,3,7,5,4,0,1,3,0,3,5,5,3,0".StartsWith(string.Join(",", output))) j.Dump();
    //output.Dump();
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
