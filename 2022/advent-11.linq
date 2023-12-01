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
var lines = (await AoC.GetLinesWeb()).ToArray();
#else
var lines = @"".GetLines().ToArray();
#endif
checked {
    Monkey[] monkeys = new[] {
        new Monkey {
            items = new() { 98, 70, 75, 80, 84, 89, 55, 98 },
            op = (x) => x * 2,
            test = (x) => (x % 11 == 0) ? 1 : 4
        },
        new Monkey {
            items = new() { 59 },
            op = (x) => x * x,
            test = (x) => (x % 19 == 0) ? 7 : 3
        },
        new Monkey {
            items = new() { 77, 95, 54, 65, 89 },
            op = (x) => x + 6,
            test = (x) => (x % 7 == 0) ? 0 : 5
        },
        new Monkey {
            items = new() { 71, 64, 75 },
            op = (x) => x + 2,
            test = (x) => (x % 17 == 0) ? 6 : 2
        },
        new Monkey {
            items = new() { 74, 55, 87, 98 },
            op = (x) => x * 11,
            test = (x) => (x % 3 == 0) ? 1 : 7
        },
        new Monkey {
            items = new() { 90, 98, 85, 52, 91, 60 },
            op = (x) => x + 7,
            test = (x) => (x % 5 == 0) ? 0 : 4
        },
        new Monkey {
            items = new() { 99, 51 },
            op = (x) => x + 1,
            test = (x) => (x % 13 == 0) ? 5 : 2
        },
        new Monkey {
            items = new() { 98, 94, 59, 76, 51, 65, 75 },
            op = (x) => x + 5,
            test = (x) => (x % 2 == 0) ? 3 : 6
        },
    };

    var inspected = new long[8];

    for (int r = 0; r < 10_000; r++)
    {
        for (int m = 0; m < 8; m++)
        {
            while (monkeys[m].items.Any())
            {
                inspected[m]++;
                var item = monkeys[m].items[0];
                monkeys[m].items.RemoveAt(0);
                item = monkeys[m].op(item);
                //item /= 3;
                item = item % (2 * 3 * 5 * 7 * 11 * 13 * 17 * 19);
                monkeys[monkeys[m].test(item)].items.Add(item);
            }
        }
    }

    var top2 = inspected.OrderByDescending(x => x).Take(2).ToArray();
    (top2[0] * top2[1]).Dump("Part 1");
}
class Monkey
{
    public List<long> items { get;set; }
    public Func<long,long> op {get;set;}
    public Func<long,long> test {get;set;}
}