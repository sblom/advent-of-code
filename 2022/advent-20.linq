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
//#define PART2

#if !TEST
var nums = (await AoC.GetLinesWeb()).Select(x => new DoublyLinkedList { value = 
#if PART2
811589153L *
#endif
long.Parse(x) }).ToArray();
#else
var nums = @"1
2
-3
3
-2
0
4".GetLines().Select(x => new DoublyLinkedList { value = 
#if PART2
811589153L *
#endif
long.Parse(x) }).ToArray();
#endif

//int[] next = new int[nums.Length];
//int[] prev = new int[nums.Length];

nums[0].prev = nums[nums.Length - 1];
nums[nums.Length - 1].next = nums[0];

for (int i = 0; i < nums.Length - 1; i++)
{
    nums[i].next = nums[i+1];
    nums[i+1].prev = nums[i];
}

#if PART2
for (int r = 0; r < 10; r++)
{
#endif
    for (int i = 0; i < nums.Length; i++)
    {
        var cur = nums[i];
        var val = cur.value % (nums.Length - 1);
        if (val == 0) continue;
        cur.prev.next = cur.next; cur.next.prev = cur.prev;
        for (int j = 0; j < Math.Abs(val); j++)
        {
            if (val > 0)
            {
                cur = cur.next;
            }
            else if (val < 0)
            {
                cur = cur.prev;
            }
        }

        if (val > 0)
        {
            nums[i].prev = cur;
            nums[i].next = cur.next;
            cur.next.prev = nums[i];
            cur.next = nums[i];
        }

        if (val < 0)
        {
            nums[i].next = cur;
            nums[i].prev = cur.prev;
            cur.prev.next = nums[i];
            cur.prev = nums[i];
        }
    }
#if PART2
        //DumpLL(nums[0]);
}
#endif

var zero = nums.ToList().Find(x => x.value == 0)!;

//DumpLL(zero);

long c = 0;

for (int i = 1; i <= 3000; i++)
{
    zero = zero.next;
    if (i % 1000 == 0) c += zero.value;
}

c.Dump("Part 1");

//DumpLL(nums[0]);

void DumpLL(DoublyLinkedList node)
{
    var cur = node;
    cur.value.Dump();
    cur = cur.next;
    while (cur != node)
    {
        cur.value.Dump();
        cur = cur.next;
    }
    "------------".Dump();
}

class DoublyLinkedList
{
    public DoublyLinkedList next;
    public DoublyLinkedList prev;
    public long value;
}

