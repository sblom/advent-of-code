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
var lines = @"".GetLines();
#endif

var machines = lines.GroupLines().Select(group => string.Join("\n",group)).Extract<((int x,int y) a,(int x,int y) b, (int x,int y) p)>(@"Button A: (X\+(\d+), Y\+(\d+))\s+Button B: (X\+(\d+), Y\+(\d+))\s+Prize: (X=(\d+), Y=(\d+))");

int t = 0;

foreach (var machine in machines)
{
    int min_tokens = int.MaxValue;
    for (int i = 0; i <= 100; i++)
    {
        for (int j = 0; j <= 100; j++)
        {
            if (machine.a.x * i + machine.b.x * j == machine.p.x && machine.a.y * i + machine.b.y * j == machine.p.y)
            {
                var tokens = i * 3 + j;
                if (tokens < min_tokens)
                {
                    if (min_tokens != int.MaxValue) "!!!".Dump();
                    min_tokens = tokens;
                }
            }
        }
    }
    
    if (min_tokens != int.MaxValue) t += min_tokens;
}

t.Dump();

long t2 = 0;

foreach (var machine in machines)
{
    var p = (x: 10000000000000L + machine.p.x, y: 10000000000000L + machine.p.y);
    var a = machine.a;
    var b = machine.b;

    // Cramer's rule
    long D = (a.x * b.y - a.y * b.x);
    long an = (p.x * b.y - p.y * b.x) / D;
    long bn = (a.x * p.y - a.y * p.x) / D;

    long min_tokens = long.MaxValue;
    
    if (a.x * an + b.x * bn == p.x && a.y * an + b.y * bn == p.y)
    {
        long tokens = (long)(an * 3 + bn);
        t2 += tokens;
    }
}

t2.Dump();